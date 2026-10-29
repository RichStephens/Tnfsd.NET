namespace Tnfsd.NET
{
    public partial class MainForm : Form
    {
        private readonly System.Windows.Forms.Timer _uiTimer = new() { Interval = 2000 };
        private NotifyIcon trayIcon = null!;
        private ContextMenuStrip trayMenu = null!;
        private ToolStripMenuItem menuStartTask = null!, menuStopTask = null!, menuExit = null!;
        private bool _busy;
        private bool _refreshing;

        public MainForm()
        {
            InitializeComponent();
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            trayMenu = new ContextMenuStrip();

            var menuRestore = new ToolStripMenuItem("Restore Window", null, (s, e) => RestoreWindow());
            menuStartTask = new ToolStripMenuItem("Start Task", null, buttonStartTask_Click);
            menuStopTask = new ToolStripMenuItem("Stop Task", null, buttonStopTask_Click);
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
                Icon = this.Icon,
                ContextMenuStrip = trayMenu,
                Visible = true,
                Text = "TNFSD Status"
            };

            trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    trayMenu.Show(Cursor.Position);
            };

            trayIcon.DoubleClick += (s, e) => RestoreWindow();
        }

        private void RestoreWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void Render(TnfsdStatus status)
        {
            if (!status.TaskExists)
                labelTaskStatus.Text = "***   TNFSD task not created  ***";
            else if (status.IsRunning)
                labelTaskStatus.Text = "***   TNFSD task is running   ***";
            else
                labelTaskStatus.Text = "*** TNFSD task is NOT running ***";

            buttonStartTask.Enabled = status.TaskExists && !status.IsRunning;
            buttonStopTask.Enabled = status.TaskExists && status.IsRunning;
            buttonCreateTask.Enabled = !status.TaskExists;
            buttonDeleteTask.Enabled = status.TaskExists;
            checkBoxAddFirewallException.Enabled = !status.TaskExists;

            trayIcon.Text = status.IsRunning ? "TNFSD task is running" : "TNFSD task is not running";
            menuStartTask.Enabled = buttonStartTask.Enabled;
            menuStopTask.Enabled = buttonStopTask.Enabled;
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
                labelTaskStatus.Text = $"*** Status check failed: {ex.Message} ***";
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
                checkBoxAddFirewallException.Checked = true;
                Render(status);
            }
            catch (Exception ex)
            {
                labelTaskStatus.Text = $"*** Status check failed: {ex.Message} ***";
            }

            _uiTimer.Tick += async (s, ev) => await RefreshStatusAsync();
            _uiTimer.Start();
        }

        private async void buttonStartTask_Click(object? sender, EventArgs e)
        {
            await RunAsync(TnfsdManager.StartAsync, "Failed to start task");
        }

        private async void buttonStopTask_Click(object? sender, EventArgs e)
        {
            await RunAsync(TnfsdManager.StopAsync, "Failed to stop task");
        }

        private async void buttonCreateTask_Click(object sender, EventArgs e)
        {
            string exeFolder = textBoxExecFolder.Text.Trim();
            string shareFolder = textBoxShareFolder.Text.Trim();

            if (string.IsNullOrEmpty(exeFolder) || !Directory.Exists(exeFolder))
            {
                MessageBox.Show("Please specify a valid executable folder.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TnfsdManager.ExecutableExists(exeFolder))
            {
                MessageBox.Show($"The executable '{TaskSchedulerManager.ExecutableName}' was not found in the specified folder.", "Missing Executable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool addFirewallRule = checkBoxAddFirewallException.Checked;
            await RunAsync(async () =>
            {
                await TnfsdManager.CreateAsync(exeFolder, shareFolder, addFirewallRule);
                MessageBox.Show("Scheduled task created/updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            string folder = textBoxExecFolder.Text.Trim();

            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("Please specify an executable folder first.", "Missing Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                folderDialog.Description = "Select the tnfs executable folder";
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
                folderDialog.Description = "Select the tnfs share folder";
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
