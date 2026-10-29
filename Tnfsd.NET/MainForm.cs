using System.Drawing.Drawing2D;

namespace Tnfsd.NET
{
    public partial class MainForm : Form
    {
        private enum ServerState { NotInstalled, Running, Stopped, Error }

        private readonly System.Windows.Forms.Timer _uiTimer = new() { Interval = 2000 };
        private readonly Dictionary<ServerState, Icon> _trayIcons = new();
        private NotifyIcon trayIcon = null!;
        private ContextMenuStrip trayMenu = null!;
        private ToolStripMenuItem menuStartTask = null!, menuStopTask = null!, menuExit = null!;
        private TnfsdStatus? _status;
        private Color _stateColor = Color.Gray;
        private bool _busy;
        private bool _refreshing;

        public MainForm()
        {
            InitializeComponent();
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            foreach (ServerState state in Enum.GetValues<ServerState>())
                _trayIcons[state] = CreateBadgedIcon(this.Icon!, state);

            trayMenu = new ContextMenuStrip();

            var menuRestore = new ToolStripMenuItem("Restore Window", null, (s, e) => RestoreWindow());
            menuStartTask = new ToolStripMenuItem("Start", null, async (s, e) => await StartAsync());
            menuStopTask = new ToolStripMenuItem("Stop", null, async (s, e) => await StopAsync());
            menuExit = new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit());

            trayMenu.Items.AddRange(new ToolStripItem[]
            {
                menuRestore,
                new ToolStripSeparator(),
                menuStartTask,
                menuStopTask,
                new ToolStripSeparator(),
                menuExit
            });

            trayIcon = new NotifyIcon
            {
                Icon = _trayIcons[ServerState.NotInstalled],
                ContextMenuStrip = trayMenu,
                Visible = true,
                Text = "TNFSD"
            };

            trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    trayMenu.Show(Cursor.Position);
            };

            trayIcon.DoubleClick += (s, e) => RestoreWindow();
        }

        private static Color StateColor(ServerState state) => state switch
        {
            ServerState.Running => Color.FromArgb(16, 124, 16),
            ServerState.Stopped => Color.FromArgb(202, 128, 0),
            ServerState.Error => Color.FromArgb(196, 43, 28),
            _ => Color.Gray
        };

        // The app icon with a corner badge: ▶ running, ■ stopped, ✕ not installed or error.
        private static Icon CreateBadgedIcon(Icon baseIcon, ServerState state)
        {
            Size size = SystemInformation.SmallIconSize;
            using var bitmap = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(bitmap))
            using (var source = baseIcon.ToBitmap())
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(source, new Rectangle(Point.Empty, size));

                float d = size.Width * 0.5f;
                var badge = new RectangleF(size.Width - d, size.Height - d, d, d);
                using (var fill = new SolidBrush(StateColor(state)))
                    g.FillEllipse(fill, badge);
                using (var outline = new Pen(Color.White, 1f))
                    g.DrawEllipse(outline, badge);

                float cx = badge.X + d / 2, cy = badge.Y + d / 2, r = d * 0.22f;
                using var glyph = new SolidBrush(Color.White);
                using var glyphPen = new Pen(Color.White, Math.Max(1.5f, d * 0.14f));
                switch (state)
                {
                    case ServerState.Running:
                        g.FillPolygon(glyph, new[] { new PointF(cx - r * 0.8f, cy - r * 1.1f), new PointF(cx - r * 0.8f, cy + r * 1.1f), new PointF(cx + r * 1.2f, cy) });
                        break;
                    case ServerState.Stopped:
                        g.FillRectangle(glyph, cx - r, cy - r, r * 2, r * 2);
                        break;
                    default:
                        g.DrawLine(glyphPen, cx - r, cy - r, cx + r, cy + r);
                        g.DrawLine(glyphPen, cx - r, cy + r, cx + r, cy - r);
                        break;
                }
            }

            return Icon.FromHandle(bitmap.GetHicon());
        }

        private void RestoreWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private static ServerState GetState(TnfsdStatus status)
        {
            if (!status.TaskExists)
                return ServerState.NotInstalled;
            if (status.IsRunning)
                return ServerState.Running;
            return status.ExecutableFound ? ServerState.Stopped : ServerState.Error;
        }

        private void Render(TnfsdStatus status)
        {
            _status = status;
            ServerState state = GetState(status);

            string headline = state switch
            {
                ServerState.Running => "Running",
                ServerState.Stopped => "Stopped",
                ServerState.Error => "tnfsd.exe not found",
                _ => "Not installed"
            };

            string detail;
            if (state == ServerState.NotInstalled)
            {
                detail = "Choose the folders below, then click Create Task.";
            }
            else if (state == ServerState.Error)
            {
                detail = $"Expected in {status.ExecutableFolder}. Download it or choose another folder, then recreate the task.";
            }
            else
            {
                string firewall = status.FirewallRuleExists ? "allowed" : "not allowed";
                string share = string.IsNullOrEmpty(status.ShareFolder) ? "No share folder set" : $"Sharing {status.ShareFolder}";
                detail = $"Starts with Windows · Firewall: {firewall}\n{share}";
            }

            ShowStatus(state, headline, detail);

            buttonStartStop.Text = state == ServerState.Running ? "Stop" : "Start";
            buttonStartStop.Enabled = state is ServerState.Running or ServerState.Stopped;
            buttonCreateTask.Enabled = !status.TaskExists;
            buttonDeleteTask.Enabled = status.TaskExists;
            checkBoxAddFirewallException.Enabled = !status.TaskExists;

            menuStartTask.Enabled = state == ServerState.Stopped;
            menuStopTask.Enabled = state == ServerState.Running;
        }

        private void ShowStatus(ServerState state, string headline, string detail)
        {
            _stateColor = StateColor(state);
            panelStatus.Invalidate();
            labelStatusText.Text = headline;
            labelStatusDetail.Text = detail;

            trayIcon.Icon = _trayIcons[state];
            string tip = $"TNFSD: {headline}";
            trayIcon.Text = tip.Length > 63 ? tip[..63] : tip;
        }

        private void panelStatus_Paint(object sender, PaintEventArgs e)
        {
            int d = labelStatusText.Height * 2 / 3;
            int x = (labelStatusText.Left - d) / 2;
            int y = labelStatusText.Top + (labelStatusText.Height - d) / 2;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(_stateColor);
            e.Graphics.FillEllipse(brush, x, y, d, d);
        }

        private async Task RefreshStatusAsync()
        {
            if (_refreshing)
                return;

            _refreshing = true;
            try
            {
                var status = await TnfsdManager.GetStatusAsync();
                if (!_busy)
                    Render(status);
            }
            catch (Exception ex)
            {
                ShowStatus(ServerState.Error, "Status unavailable", ex.Message);
            }
            finally
            {
                _refreshing = false;
            }
        }

        private async Task RunAsync(Func<Task> operation, string failureMessage)
        {
            _busy = true;
            this.Enabled = false;
            this.UseWaitCursor = true;
            menuStartTask.Enabled = false;
            menuStopTask.Enabled = false;

            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{failureMessage}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _busy = false;
                this.UseWaitCursor = false;
                this.Enabled = true;
                await RefreshStatusAsync();
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                var status = await TnfsdManager.GetStatusAsync();
                textBoxExecFolder.Text = status.ExecutableFolder ?? string.Empty;
                textBoxShareFolder.Text = status.ShareFolder ?? string.Empty;
                Render(status);
            }
            catch (Exception ex)
            {
                ShowStatus(ServerState.Error, "Status unavailable", ex.Message);
            }

            _uiTimer.Tick += async (s, ev) => await RefreshStatusAsync();
            _uiTimer.Start();
        }

        private Task StartAsync() => RunAsync(TnfsdManager.StartAsync, "Failed to start tnfsd");

        private Task StopAsync() => RunAsync(TnfsdManager.StopAsync, "Failed to stop tnfsd");

        private async void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (_status?.IsRunning == true)
                await StopAsync();
            else
                await StartAsync();
        }

        private async void buttonCreateTask_Click(object sender, EventArgs e)
        {
            string exeFolder = textBoxExecFolder.Text.Trim();
            string shareFolder = textBoxShareFolder.Text.Trim();

            if (string.IsNullOrEmpty(exeFolder) || !Directory.Exists(exeFolder))
            {
                MessageBox.Show("Please specify a valid server folder.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TnfsdManager.ExecutableExists(exeFolder))
            {
                MessageBox.Show($"'{TaskSchedulerManager.ExecutableName}' was not found in the server folder. Use the Download link to get it.", "Missing Executable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool addFirewallRule = checkBoxAddFirewallException.Checked;
            await RunAsync(async () =>
            {
                await TnfsdManager.CreateAsync(exeFolder, shareFolder, addFirewallRule);
                MessageBox.Show("Scheduled task created.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }, "Failed to create scheduled task");
        }

        private async void buttonDeleteTask_Click(object sender, EventArgs e)
        {
            await RunAsync(async () =>
            {
                if (await TnfsdManager.DeleteAsync())
                    MessageBox.Show("Firewall exception removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MessageBox.Show("Scheduled task removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }, "Failed to remove scheduled task");
        }

        private async void linkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string folder = textBoxExecFolder.Text.Trim();

            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("Please specify a server folder first.", "Missing Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TnfsdManager.IsFolderEmpty(folder))
            {
                var result = MessageBox.Show(
                    "Folder already contains files. Continue download?",
                    "Folder Not Empty",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button3);

                if (result == DialogResult.No)
                    MessageBox.Show("Download skipped to avoid overwriting.", "Folder Not Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (result != DialogResult.Yes)
                    return;
            }

            await RunAsync(async () =>
            {
                await TnfsdManager.InstallAsync(folder);
                MessageBox.Show($"Download complete and extracted to:\n{folder}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }, "Failed to download or extract tnfsd");
        }

        private void buttonBrowseExecFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the tnfsd server folder";
                folderDialog.SelectedPath = textBoxExecFolder.Text;
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxExecFolder.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void buttonBrowseShareFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the tnfsd share folder";
                folderDialog.SelectedPath = textBoxShareFolder.Text;
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxShareFolder.Text = folderDialog.SelectedPath;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _uiTimer.Stop();
            trayIcon.Visible = false;
            base.OnFormClosing(e);
        }
    }
}
