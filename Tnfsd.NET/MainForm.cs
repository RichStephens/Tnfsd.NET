using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Tnfsd.NET
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.Timer _uiTimer;
        private bool _passwordVisible = false;

        public MainForm()
        {
            InitializeComponent();
            textBoxPassword.UseSystemPasswordChar = true; // Mask password input

            // Create Show/Hide Password button (placed next to textBoxPassword)
            Button buttonTogglePassword = new Button();
            buttonTogglePassword.Text = "Show";
            buttonTogglePassword.Width = 60;
            buttonTogglePassword.Height = textBoxPassword.Height;
            buttonTogglePassword.Left = textBoxPassword.Right + 5;
            buttonTogglePassword.Top = textBoxPassword.Top;
            buttonTogglePassword.Click += (s, e) => TogglePasswordVisibility(buttonTogglePassword);
            this.Controls.Add(buttonTogglePassword);
        }

        private void TogglePasswordVisibility(Button toggleButton)
        {
            _passwordVisible = !_passwordVisible;
            textBoxPassword.UseSystemPasswordChar = !_passwordVisible;
            toggleButton.Text = _passwordVisible ? "Hide" : "Show";
        }

        private void setServiceRunningState()
        {
            if (!ServiceManager.ServiceExists(ServiceManager.ServiceName))
            {
                labelServiceIsRunning.Visible = false;
                labelServiceNotRunning.Visible = true;
                buttonDeleteService.Enabled = false;
                buttonStopService.Enabled = false;
                buttonStartService.Enabled = false;
            }
            else
            {
                if (ServiceManager.ServiceIsRunning(ServiceManager.ServiceName))
                {
                    labelServiceIsRunning.Visible = true;
                    labelServiceNotRunning.Visible = false;
                    buttonDeleteService.Enabled = true;
                    buttonStopService.Enabled = true;
                    buttonStartService.Enabled = false;
                }
                else
                {
                    labelServiceIsRunning.Visible = false;
                    labelServiceNotRunning.Visible = true;
                    buttonDeleteService.Enabled = true;
                    buttonStopService.Enabled = false;
                    buttonStartService.Enabled = true;

                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        this.Invoke(new System.Action(() =>
                        {
                            this.WindowState = FormWindowState.Normal;
                            this.Activate();
                            this.BringToFront();
                        }));
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ServiceProperties serviceProperties = ServiceManager.GetServiceProperties(ServiceManager.ServiceName);

            // Default username to current user if not loaded
            if (string.IsNullOrWhiteSpace(serviceProperties.UserId))
                textBoxUser.Text = Environment.UserName;
            else
                textBoxUser.Text = serviceProperties.UserId;

            _uiTimer = new System.Windows.Forms.Timer();
            _uiTimer.Interval = 2000;
            _uiTimer.Tick += (s, ev) => setServiceRunningState();
            _uiTimer.Start();
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

        private void buttonInstallService_Click(object sender, EventArgs e)
        {
            string execPath;

            if (string.IsNullOrEmpty(textBoxExecFolder.Text))
            {
                MessageBox.Show("tnfsd executable path is blank!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!Directory.Exists(textBoxExecFolder.Text))
            {
                MessageBox.Show("tnfsd executable path does not exist!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (textBoxExecFolder.Text.EndsWith("\\"))
            {
                execPath = textBoxExecFolder + ServiceManager.ServiceExecutable;
            }
            else
            {
                execPath = textBoxExecFolder + "\\" + ServiceManager.ServiceExecutable;
            }

            if (!File.Exists(execPath))
            {
                MessageBox.Show("Tnfsd executable does not exist in specified executable path!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(textBoxShareFolder.Text))
            {
                MessageBox.Show("Tnfsd share path is blank!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!Directory.Exists(textBoxShareFolder.Text))
            {
                MessageBox.Show("Tnfsd share path does not exist!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string shareFolder = textBoxShareFolder.Text;

            // Ensure that the share folder starts and ends with quotation marks
            if (!shareFolder.StartsWith('"') && !shareFolder.EndsWith('"'))
            {
                shareFolder = "\"" + shareFolder + "\"";
            }

            //TODO: Check for user and password filled in.

            try
            {
                ServiceManager.CreateOrUpdateService(ServiceManager.ServiceName, "Tnfsd Service", textBoxExecFolder.Text, textBoxShareFolder.Text, textBoxUser.Text, textBoxPassword.Text);
                setServiceRunningState();
                MessageBox.Show("Tnfsd Service created.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void buttonStartService_Click(object sender, EventArgs e)
        {
            ServiceManager.StartService(ServiceManager.ServiceName);
            setServiceRunningState();
        }

        private void buttonStopService_Click(object sender, EventArgs e)
        {
            ServiceManager.StopService(ServiceManager.ServiceName);
            setServiceRunningState();
        }

        private void buttonCreateService_Click(object sender, EventArgs e)
        {
            string exeFolder = textBoxExecFolder.Text.Trim();
            if (string.IsNullOrEmpty(exeFolder) || !Directory.Exists(exeFolder))
            {
                MessageBox.Show("Please specify a valid executable folder.", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string exePath = Path.Combine(exeFolder, ServiceManager.ServiceExecutable);
            if (!File.Exists(exePath))
            {
                MessageBox.Show($"The executable '{ServiceManager.ServiceExecutable}' was not found in the specified folder.", "Missing Executable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userId = textBoxUser.Text.Trim();
            if (string.IsNullOrWhiteSpace(userId))
                userId = Environment.UserName;

            string password = textBoxPassword.Text.Trim();

            try
            {
                ServiceManager.CreateOrUpdateService(ServiceManager.ServiceName, "TNFSD Service", exePath, userId, password);
                FirewallManager.AddFirewallException(exePath, "TNFSD Service");
                MessageBox.Show("Service created/updated and firewall exception added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create service: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            textBoxPassword.Text = string.Empty; // Clear password immediately
            setServiceRunningState();
        }

        private void buttonDeleteService_Click(object sender, EventArgs e)
        {
            try
            {
                FirewallManager.RemoveFirewallException(Path.Combine(textBoxExecFolder.Text.Trim(), ServiceManager.ServiceExecutable));
                ServiceManager.UninstallService(ServiceManager.ServiceName);
                MessageBox.Show("Service and firewall exception removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to remove service or firewall exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            setServiceRunningState();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ensure password is cleared when form closes
            textBoxPassword.Text = string.Empty;
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {

        }
    }
}
