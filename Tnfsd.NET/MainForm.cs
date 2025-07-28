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

        private void setTaskRunningState()
        {
            if (!TaskSchedulerManager.TaskExists(TaskSchedulerManager.TaskName))
            {
                labelTaskIsRunning.Visible = false;
                labelTaskNotRunning.Visible = true;
                buttonStopTask.Enabled = false;
                buttonStartTask.Enabled = false;
            }
            else
            {
                if (TaskSchedulerManager.TaskIsRunning(TaskSchedulerManager.TaskName))
                {
                    labelTaskIsRunning.Visible = true;
                    labelTaskNotRunning.Visible = false;
                    buttonStopTask.Enabled = true;
                    buttonStartTask.Enabled = false;
                }
                else
                {
                    labelTaskIsRunning.Visible = false;
                    labelTaskNotRunning.Visible = true;
                    buttonStopTask.Enabled = false;
                    buttonStartTask.Enabled = true;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TaskProperties taskProperties = TaskSchedulerManager.GetTaskProperties(TaskSchedulerManager.TaskName);

            if (!string.IsNullOrEmpty(taskProperties.ExecutableFolder))
            {
                textBoxExecFolder.Text = taskProperties.ExecutableFolder;
            }
            else
            {
                textBoxExecFolder.Text = Application.StartupPath;
            }

            if (!string.IsNullOrEmpty(taskProperties.ShareFolder))
            {
                textBoxShareFolder.Text = taskProperties.ShareFolder;
            }

            checkBoxRunWithHighestPriv.Checked = taskProperties.RunWithHighestPrivilege;

            if (!string.IsNullOrEmpty(taskProperties.UserId))
            {
                textBoxUser.Text = taskProperties.UserId;
            }

            setTaskRunningState();
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
                execPath = textBoxExecFolder + TaskSchedulerManager.TaskExecutable;
            }
            else
            {
                execPath = textBoxExecFolder + "\\" + TaskSchedulerManager.TaskExecutable;
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

            try
            {
                TaskSchedulerManager.CreateOrUpdateTask(TaskSchedulerManager.TaskName, "Tnfs Server Daemon", checkBoxRunWithHighestPriv.Checked, textBoxUser.Text, textBoxPassword.Text, execPath, shareFolder);
                setTaskRunningState();
                MessageBox.Show("Tnfsd task created.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void buttonDeleteTask_Click(object sender, EventArgs e)
        {
            if (TaskSchedulerManager.TaskExists(TaskSchedulerManager.TaskName) != true)
            {
                MessageBox.Show("tnfsd task does not exist!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TaskSchedulerManager.DeleteTask(TaskSchedulerManager.TaskName);
            setTaskRunningState();
            MessageBox.Show("Tnfsd task deleted.", "Information");
        }

        private void buttonStartTask_Click(object sender, EventArgs e)
        {
            if (TaskSchedulerManager.TaskExists(TaskSchedulerManager.TaskName) != true)
            {
                MessageBox.Show("tnfsd task does not exist!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (TaskSchedulerManager.TaskIsRunning(TaskSchedulerManager.TaskName) == true)
            {
                MessageBox.Show("tnfsd task is already running!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TaskSchedulerManager.StartTask(TaskSchedulerManager.TaskName);
            setTaskRunningState();
            MessageBox.Show("Tnfsd task started.", "Information");
        }

        private void buttonStopTask_Click(object sender, EventArgs e)
        {
            if (TaskSchedulerManager.TaskExists(TaskSchedulerManager.TaskName) != true)
            {
                MessageBox.Show("Tnfsd task does not exist!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (TaskSchedulerManager.TaskIsRunning(TaskSchedulerManager.TaskName) != true)
            {
                MessageBox.Show("Tnfsd task is not currently running!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TaskSchedulerManager.StopTask(TaskSchedulerManager.TaskName);
            setTaskRunningState();
            MessageBox.Show("Tnfsd task stopped.", "Information");
        }
    }
}
