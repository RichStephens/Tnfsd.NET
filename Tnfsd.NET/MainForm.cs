using Microsoft.Win32.TaskScheduler;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Tnfsd.NET
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxExecFolder.Text = Application.StartupPath;
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

            if (textBoxExecFolder.Text.EndsWith("\\"))
            {
                execPath = textBoxExecFolder + "tnfsd.exe";
            }
            else
            {
                execPath = textBoxExecFolder + "\\tnfsd.exe";
            }

            try
            {
                TaskSchedulerManager.CreateOrUpdateTask("tnfsd", "Tnfs Server Daemon", execPath, textBoxShareFolder.Text + "\"");
                MessageBox.Show("Tnfsd Task Created.");
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
            }

            TaskSchedulerManager.StartTask("tnfsd");

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
            }

            TaskSchedulerManager.StopTask("tnfsd");
        }
    }
}
