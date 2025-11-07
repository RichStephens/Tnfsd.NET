using System.IO.Compression;

namespace Tnfsd.NET
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.Timer _uiTimer;
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private ToolStripMenuItem menuStartTask, menuStopTask, menuExit;

        public MainForm()
        {
            InitializeComponent();
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            // Create tray menu and items
            trayMenu = new ContextMenuStrip();

            // New "Restore Window" option
            var menuRestore = new ToolStripMenuItem("Restore Window", null, (s, e) =>
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.BringToFront();
            });

            // Start/Stop task options
            menuStartTask = new ToolStripMenuItem("Start Task", null, (s, e) =>
            {
                try
                {
                    // Mirror what the start button does, but avoid coupling to the button control
                    this.UseWaitCursor = true;
                    menuStartTask.Enabled = false;

                    TaskSchedulerManager.StartTask(TaskSchedulerManager.TaskName);
                    setTaskRunningState();
                }
                finally
                {
                    menuStartTask.Enabled = true;
                    this.UseWaitCursor = false;
                }
            });

            menuStopTask = new ToolStripMenuItem("Stop Task", null, (s, e) =>
            {
                try
                {
                    this.UseWaitCursor = true;
                    menuStopTask.Enabled = false;

                    TaskSchedulerManager.StopTask(TaskSchedulerManager.TaskName);
                    setTaskRunningState();
                }
                finally
                {
                    menuStopTask.Enabled = true;
                    this.UseWaitCursor = false;
                }
            });

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

            trayIcon.DoubleClick += (s, e) =>
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.BringToFront();
            };
        }
        
        private void setTaskRunningState()
        {
            bool exists = TaskSchedulerManager.TaskExists(TaskSchedulerManager.TaskName);
            bool running = exists && TaskSchedulerManager.IsTaskRunning(TaskSchedulerManager.TaskName);
            bool firewallExists = FirewallManager.FirewallExceptionExists(TaskSchedulerManager.TaskName);

            if (exists)
            {
                if (running)
                {
                    labelTaskStatus.Text = "***   TNFSD task is running   ***";
                }
                else
                {
                    labelTaskStatus.Text = "*** TNFSD task is NOT running ***";
                }
            }
            else
            {
                labelTaskStatus.Text = "***   TNFSD task not created  ***";
            }

            buttonStartTask.Enabled = exists && !running;
            buttonStopTask.Enabled = exists && running;
            buttonCreateTask.Enabled = !exists;
            buttonDeleteTask.Enabled = exists;
            checkBoxAddFirewallException.Enabled = !firewallExists;

            // Sync tray tooltip and menu
            trayIcon.Text = running ? "TNFSD task is running" : "TNFSD task is not running";
            menuStartTask.Enabled = buttonStartTask.Enabled;
            menuStopTask.Enabled = buttonStopTask.Enabled;

            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            var props = TaskSchedulerManager.GetTaskProperties(TaskSchedulerManager.TaskName);
            if (props != null)
            {
                // Set executable folder field
                textBoxExecFolder.Text = props.ExecutableFolder ?? string.Empty;

                // Set share folder field (if present)
                if (!string.IsNullOrWhiteSpace(props.ShareFolder))
                    textBoxShareFolder.Text = props.ShareFolder;
            }
            else
            {
                // Default executable folder or username if no task yet
                textBoxExecFolder.Text = string.Empty;
                textBoxShareFolder.Text = string.Empty;
            }

            checkBoxAddFirewallException.Checked = !(FirewallManager.FirewallExceptionExists(TaskSchedulerManager.TaskName));

            setTaskRunningState();

            _uiTimer = new System.Windows.Forms.Timer();
            _uiTimer.Interval = 2000;
            _uiTimer.Tick += (s, ev) => setTaskRunningState();
            _uiTimer.Start();
        }

        private void buttonStartTask_Click(object sender, EventArgs e)
        {
            TaskSchedulerManager.StartTask(TaskSchedulerManager.TaskName);
            setTaskRunningState();
        }

        private void buttonStopTask_Click(object sender, EventArgs e)
        {
            TaskSchedulerManager.StopTask(TaskSchedulerManager.TaskName);
            setTaskRunningState();
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

            string exePath = Path.Combine(exeFolder, TaskSchedulerManager.ExecutableName);
            if (!File.Exists(exePath))
            {
                MessageBox.Show($"The executable '{TaskSchedulerManager.ExecutableName}' was not found in the specified folder.", "Missing Executable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.UseWaitCursor = true;
                buttonCreateTask.Enabled = false;

                TaskSchedulerManager.CreateOrUpdateTask(TaskSchedulerManager.TaskName, exePath, shareFolder);

                if (!FirewallManager.FirewallExceptionExists(TaskSchedulerManager.TaskName))
                {
                    if (checkBoxAddFirewallException.Checked)
                    {
                        FirewallManager.AddFirewallException(exePath, TaskSchedulerManager.TaskName);

                        if (FirewallManager.FirewallExceptionExists(TaskSchedulerManager.TaskName))
                        {
                            MessageBox.Show("Firewall exception created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                if (TaskSchedulerManager.TaskExists(TaskSchedulerManager.TaskName))
                {
                    if (!TaskSchedulerManager.IsTaskRunning(TaskSchedulerManager.TaskName))
                    {
                        TaskSchedulerManager.StartTask(TaskSchedulerManager.TaskName);
                    }

                    MessageBox.Show("Scheduled task created/updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create scheduled task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonCreateTask.Enabled = true;
                this.UseWaitCursor = false;
                setTaskRunningState();
            }
        }

        private async void buttonDeleteTask_Click(object sender, EventArgs e)
        {
            try
            {
                this.UseWaitCursor = true;
                buttonDeleteTask.Enabled = false;

                TaskSchedulerManager.DeleteTask(TaskSchedulerManager.TaskName);

                // Remove the firewall exception if the checkbox is checked
                if (checkBoxAddFirewallException.Checked)
                {
                    if (FirewallManager.FirewallExceptionExists(TaskSchedulerManager.TaskName))
                    {
                        FirewallManager.RemoveFirewallException(TaskSchedulerManager.TaskName);
                        MessageBox.Show("Firewall exception removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                MessageBox.Show("Scheduled task removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to remove scheduled task: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                buttonDeleteTask.Enabled = true;
                this.UseWaitCursor = false;
                setTaskRunningState();
            }
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            string folderPath = textBoxExecFolder.Text.Trim();
            string downloadUrl = AppSettings.TNFSDownloadURL;

            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("Please specify an executable folder first.", "Missing Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            else if (Directory.GetFiles(folderPath).Length > 0 || Directory.GetDirectories(folderPath).Length > 0)
            {
                var result = MessageBox.Show(
                    "Folder already contains files. Continue download?",
                    "Folder Not Empty",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Yes)
                {
                    // Continue with download and extraction
                    buttonDownload.Enabled = false;
                    this.UseWaitCursor = true;

                    try
                    {
                        using (var httpClient = new HttpClient())
                        using (var response = await httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();

                            string zipFileName = Path.GetFileName(new Uri(downloadUrl).LocalPath);
                            string tempZip = Path.Combine(Path.GetTempPath(), zipFileName);

                            await using (var stream = await response.Content.ReadAsStreamAsync())
                            await using (var fileStream = new FileStream(tempZip, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                await stream.CopyToAsync(fileStream);
                            }

                            await Task.Run(() => ZipFile.ExtractToDirectory(tempZip, folderPath));
                            File.Delete(tempZip);

                            MessageBox.Show($"Download complete and extracted to:\n{folderPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to download or extract tnfsd: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        buttonDownload.Enabled = true;
                        this.UseWaitCursor = false;
                    }
                }
                else if (result == DialogResult.No)
                {
                    MessageBox.Show("Download skipped to avoid overwriting.", "Folder Not Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else // Cancel
                {
                    return;
                }
            } 
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
            trayIcon.Visible = false;
            base.OnFormClosing(e);
        }
    }
}
