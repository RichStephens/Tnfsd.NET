using Microsoft.Win32.TaskScheduler;
using System.Diagnostics.Eventing.Reader;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Tnfsd.NET
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void setTaskRunningLabel()
        {
            if (TaskSchedulerManager.TaskExists("tnfsd") && TaskSchedulerManager.TaskIsRunning("tnfsd"))
            {
                labelTaskIsRunning.Visible = true;
                labelTaskNotRunning.Visible = false;
            }
            else
            {
                labelTaskIsRunning.Visible = false;
                labelTaskNotRunning.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string execPath = TaskSchedulerManager.GetTaskExecPath("tnfsd");
            if (!string.IsNullOrEmpty(execPath))
            {
                execPath = execPath.Replace("\\tnfsd.exe", "");
                textBoxExecFolder.Text = execPath;
            }
            else
            {
                textBoxExecFolder.Text = Application.StartupPath;
            }

            textBoxShareFolder.Text = TaskSchedulerManager.GetTaskArguments("tnfsd");
            setTaskRunningLabel();
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
        private void buttonInstallTask_Click(object sender, EventArgs e)
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
                execPath = textBoxExecFolder + "tnfsd.exe";
            }
            else
            {
                execPath = textBoxExecFolder + "\\tnfsd.exe";
            }

            if (!File.Exists(execPath))
            {
                MessageBox.Show("Tnfsd executable does not exist in specified executable path!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(textBoxShareFolder.Text))
            {
                MessageBox.Show("tnfsd share path is blank!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!Directory.Exists(textBoxShareFolder.Text))
            {
                MessageBox.Show("tnfsd share path does not exist!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string shareFolder = textBoxShareFolder.Text;

            // Ensure that the share folder starts and ends with quotation marks
            if (!shareFolder.StartsWith('"') && !shareFolder.EndsWith('"'))
            {
                shareFolder = "\"" + shareFolder + "\"";
            }

            try
            {
                TaskSchedulerManager.CreateOrUpdateTask("tnfsd", "Tnfs Server Daemon", execPath, shareFolder);
                setTaskRunningLabel();
                MessageBox.Show("Tnfsd task created.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void buttonDeleteTask_Click(object sender, EventArgs e)
        {
            if (TaskSchedulerManager.TaskExists("tnfsd") != true)
            {
                MessageBox.Show("tnfsd task does not exist!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TaskSchedulerManager.DeleteTask("tnfsd");
            setTaskRunningLabel();
            MessageBox.Show("Tnfsd task deleted.", "Information");
        }

        private void buttonStartTask_Click(object sender, EventArgs e)
        {
            if (TaskSchedulerManager.TaskExists("tnfsd") != true)
            {
                MessageBox.Show("tnfsd task does not exist!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (TaskSchedulerManager.TaskIsRunning("tnfsd") == true)
            {
                MessageBox.Show("tnfsd task is already running!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TaskSchedulerManager.StartTask("tnfsd");
            setTaskRunningLabel();
            MessageBox.Show("Tnfsd task started.", "Information");
        }

        private void buttonStopTask_Click(object sender, EventArgs e)
        {
            if (TaskSchedulerManager.TaskExists("tnfsd") != true)
            {
                MessageBox.Show("tnfsd task does not exist!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (TaskSchedulerManager.TaskIsRunning("tnfsd") != true)
            {
                MessageBox.Show("tnfsd task is not currently running!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TaskSchedulerManager.StopTask("tnfsd");
            setTaskRunningLabel();
            MessageBox.Show("Tnfsd task stopped.", "Information");
        }

    }
}
