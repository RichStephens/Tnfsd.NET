namespace Tnfsd.NET
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblExecFolder = new Label();
            lblTnfsShareFolder = new Label();
            textBoxExecFolder = new TextBox();
            textBoxShareFolder = new TextBox();
            buttonBrowseExecFolder = new Button();
            buttonBrowseShareFolder = new Button();
            buttonInstallTask = new Button();
            buttonDeleteTask = new Button();
            buttonStartTask = new Button();
            buttonStopTask = new Button();
            labelTaskIsRunning = new Label();
            labelTaskNotRunning = new Label();
            labelUser = new Label();
            labelPassword = new Label();
            textBoxUser = new TextBox();
            textBoxPassword = new TextBox();
            checkBoxRunWithHighestPriv = new CheckBox();
            SuspendLayout();
            // 
            // lblExecFolder
            // 
            lblExecFolder.AutoSize = true;
            lblExecFolder.Location = new Point(25, 19);
            lblExecFolder.Name = "lblExecFolder";
            lblExecFolder.Size = new Size(142, 15);
            lblExecFolder.TabIndex = 0;
            lblExecFolder.Text = "Tnfsd executable folder:";
            // 
            // lblTnfsShareFolder
            // 
            lblTnfsShareFolder.AutoSize = true;
            lblTnfsShareFolder.Location = new Point(57, 48);
            lblTnfsShareFolder.Name = "lblTnfsShareFolder";
            lblTnfsShareFolder.Size = new Size(110, 15);
            lblTnfsShareFolder.TabIndex = 1;
            lblTnfsShareFolder.Text = "Tnfsd share folder:";
            // 
            // textBoxExecFolder
            // 
            textBoxExecFolder.Location = new Point(173, 16);
            textBoxExecFolder.Name = "textBoxExecFolder";
            textBoxExecFolder.Size = new Size(534, 23);
            textBoxExecFolder.TabIndex = 2;
            // 
            // textBoxShareFolder
            // 
            textBoxShareFolder.Location = new Point(173, 44);
            textBoxShareFolder.Name = "textBoxShareFolder";
            textBoxShareFolder.Size = new Size(534, 23);
            textBoxShareFolder.TabIndex = 3;
            // 
            // buttonBrowseExecFolder
            // 
            buttonBrowseExecFolder.Location = new Point(713, 15);
            buttonBrowseExecFolder.Name = "buttonBrowseExecFolder";
            buttonBrowseExecFolder.Size = new Size(75, 23);
            buttonBrowseExecFolder.TabIndex = 4;
            buttonBrowseExecFolder.Text = "Browse";
            buttonBrowseExecFolder.UseVisualStyleBackColor = true;
            buttonBrowseExecFolder.Click += buttonBrowseExecFolder_Click;
            // 
            // buttonBrowseShareFolder
            // 
            buttonBrowseShareFolder.Location = new Point(713, 44);
            buttonBrowseShareFolder.Name = "buttonBrowseShareFolder";
            buttonBrowseShareFolder.Size = new Size(75, 23);
            buttonBrowseShareFolder.TabIndex = 5;
            buttonBrowseShareFolder.Text = "Browse";
            buttonBrowseShareFolder.UseVisualStyleBackColor = true;
            buttonBrowseShareFolder.Click += buttonBrowseShareFolder_Click;
            // 
            // buttonInstallTask
            // 
            buttonInstallTask.Location = new Point(91, 218);
            buttonInstallTask.Name = "buttonInstallTask";
            buttonInstallTask.Size = new Size(153, 32);
            buttonInstallTask.TabIndex = 6;
            buttonInstallTask.Text = "Install/Update Task";
            buttonInstallTask.UseVisualStyleBackColor = true;
            buttonInstallTask.Click += buttonInstallTask_Click;
            // 
            // buttonDeleteTask
            // 
            buttonDeleteTask.Location = new Point(250, 218);
            buttonDeleteTask.Name = "buttonDeleteTask";
            buttonDeleteTask.Size = new Size(153, 32);
            buttonDeleteTask.TabIndex = 7;
            buttonDeleteTask.Text = "Delete Task";
            buttonDeleteTask.UseVisualStyleBackColor = true;
            buttonDeleteTask.Click += buttonDeleteTask_Click;
            // 
            // buttonStartTask
            // 
            buttonStartTask.Location = new Point(409, 218);
            buttonStartTask.Name = "buttonStartTask";
            buttonStartTask.Size = new Size(153, 32);
            buttonStartTask.TabIndex = 8;
            buttonStartTask.Text = "Start Task";
            buttonStartTask.UseVisualStyleBackColor = true;
            buttonStartTask.Click += buttonStartTask_Click;
            // 
            // buttonStopTask
            // 
            buttonStopTask.Location = new Point(568, 218);
            buttonStopTask.Name = "buttonStopTask";
            buttonStopTask.Size = new Size(153, 32);
            buttonStopTask.TabIndex = 9;
            buttonStopTask.Text = "Stop Task";
            buttonStopTask.UseVisualStyleBackColor = true;
            buttonStopTask.Click += buttonStopTask_Click;
            // 
            // labelTaskIsRunning
            // 
            labelTaskIsRunning.AutoSize = true;
            labelTaskIsRunning.BorderStyle = BorderStyle.Fixed3D;
            labelTaskIsRunning.Location = new Point(285, 189);
            labelTaskIsRunning.Name = "labelTaskIsRunning";
            labelTaskIsRunning.Size = new Size(215, 17);
            labelTaskIsRunning.TabIndex = 12;
            labelTaskIsRunning.Text = " *** Tnfsd task is currently running ***";
            // 
            // labelTaskNotRunning
            // 
            labelTaskNotRunning.AutoSize = true;
            labelTaskNotRunning.BorderStyle = BorderStyle.Fixed3D;
            labelTaskNotRunning.Location = new Point(271, 189);
            labelTaskNotRunning.Name = "labelTaskNotRunning";
            labelTaskNotRunning.Size = new Size(243, 17);
            labelTaskNotRunning.TabIndex = 13;
            labelTaskNotRunning.Text = " *** Tnfsd task is NOT currently running ***";
            // 
            // labelUser
            // 
            labelUser.AutoSize = true;
            labelUser.Location = new Point(131, 123);
            labelUser.Name = "labelUser";
            labelUser.Size = new Size(36, 15);
            labelUser.TabIndex = 14;
            labelUser.Text = "User:";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(105, 155);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(62, 15);
            labelPassword.TabIndex = 15;
            labelPassword.Text = "Password:";
            // 
            // textBoxUser
            // 
            textBoxUser.Location = new Point(173, 120);
            textBoxUser.Name = "textBoxUser";
            textBoxUser.Size = new Size(285, 23);
            textBoxUser.TabIndex = 16;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(173, 152);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(285, 23);
            textBoxPassword.TabIndex = 17;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // checkBoxRunWithHighestPriv
            // 
            checkBoxRunWithHighestPriv.AutoSize = true;
            checkBoxRunWithHighestPriv.Location = new Point(173, 84);
            checkBoxRunWithHighestPriv.Name = "checkBoxRunWithHighestPriv";
            checkBoxRunWithHighestPriv.Size = new Size(177, 19);
            checkBoxRunWithHighestPriv.TabIndex = 18;
            checkBoxRunWithHighestPriv.Text = "Run with highest privileges";
            checkBoxRunWithHighestPriv.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 262);
            Controls.Add(checkBoxRunWithHighestPriv);
            Controls.Add(textBoxPassword);
            Controls.Add(textBoxUser);
            Controls.Add(labelPassword);
            Controls.Add(labelUser);
            Controls.Add(labelTaskNotRunning);
            Controls.Add(labelTaskIsRunning);
            Controls.Add(buttonStopTask);
            Controls.Add(buttonStartTask);
            Controls.Add(buttonDeleteTask);
            Controls.Add(buttonInstallTask);
            Controls.Add(buttonBrowseShareFolder);
            Controls.Add(buttonBrowseExecFolder);
            Controls.Add(textBoxShareFolder);
            Controls.Add(textBoxExecFolder);
            Controls.Add(lblTnfsShareFolder);
            Controls.Add(lblExecFolder);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            Text = "Tnfsd.NET";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblExecFolder;
        private Label lblTnfsShareFolder;
        private TextBox textBoxExecFolder;
        private TextBox textBoxShareFolder;
        private Button buttonBrowseExecFolder;
        private Button buttonBrowseShareFolder;
        private Button buttonInstallTask;
        private Button buttonDeleteTask;
        private Button buttonStartTask;
        private Button buttonStopTask;
        private Label labelTaskIsRunning;
        private Label labelTaskNotRunning;
        private Label labelUser;
        private Label labelPassword;
        private TextBox textBoxUser;
        private TextBox textBoxPassword;
        private CheckBox checkBoxRunWithHighestPriv;
    }
}
