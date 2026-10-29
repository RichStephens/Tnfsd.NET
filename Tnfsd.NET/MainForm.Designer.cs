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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            panelStatus = new Panel();
            labelStatusText = new Label();
            labelStatusDetail = new Label();
            buttonStartStop = new Button();
            groupSetup = new GroupBox();
            lblExecFolder = new Label();
            textBoxExecFolder = new TextBox();
            buttonBrowseExecFolder = new Button();
            linkDownload = new LinkLabel();
            lblShareFolder = new Label();
            textBoxShareFolder = new TextBox();
            buttonBrowseShareFolder = new Button();
            checkBoxAddFirewallException = new CheckBox();
            buttonCreateTask = new Button();
            buttonDeleteTask = new Button();
            panelStatus.SuspendLayout();
            groupSetup.SuspendLayout();
            SuspendLayout();
            //
            // panelStatus
            //
            panelStatus.BackColor = SystemColors.Window;
            panelStatus.BorderStyle = BorderStyle.FixedSingle;
            panelStatus.Controls.Add(labelStatusText);
            panelStatus.Controls.Add(labelStatusDetail);
            panelStatus.Controls.Add(buttonStartStop);
            panelStatus.Location = new Point(12, 12);
            panelStatus.Name = "panelStatus";
            panelStatus.Size = new Size(616, 88);
            panelStatus.TabIndex = 0;
            panelStatus.Paint += panelStatus_Paint;
            //
            // labelStatusText
            //
            labelStatusText.AutoSize = true;
            labelStatusText.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelStatusText.Location = new Point(40, 12);
            labelStatusText.Name = "labelStatusText";
            labelStatusText.Size = new Size(110, 21);
            labelStatusText.TabIndex = 1;
            labelStatusText.Text = "Checking...";
            //
            // labelStatusDetail
            //
            labelStatusDetail.ForeColor = SystemColors.GrayText;
            labelStatusDetail.Location = new Point(42, 40);
            labelStatusDetail.Name = "labelStatusDetail";
            labelStatusDetail.Size = new Size(448, 38);
            labelStatusDetail.TabIndex = 2;
            //
            // buttonStartStop
            //
            buttonStartStop.Enabled = false;
            buttonStartStop.Location = new Point(500, 26);
            buttonStartStop.Name = "buttonStartStop";
            buttonStartStop.Size = new Size(100, 32);
            buttonStartStop.TabIndex = 3;
            buttonStartStop.Text = "Start";
            buttonStartStop.UseVisualStyleBackColor = true;
            buttonStartStop.Click += buttonStartStop_Click;
            //
            // groupSetup
            //
            groupSetup.Controls.Add(lblExecFolder);
            groupSetup.Controls.Add(textBoxExecFolder);
            groupSetup.Controls.Add(buttonBrowseExecFolder);
            groupSetup.Controls.Add(linkDownload);
            groupSetup.Controls.Add(lblShareFolder);
            groupSetup.Controls.Add(textBoxShareFolder);
            groupSetup.Controls.Add(buttonBrowseShareFolder);
            groupSetup.Controls.Add(checkBoxAddFirewallException);
            groupSetup.Controls.Add(buttonCreateTask);
            groupSetup.Controls.Add(buttonDeleteTask);
            groupSetup.Location = new Point(12, 112);
            groupSetup.Name = "groupSetup";
            groupSetup.Size = new Size(616, 176);
            groupSetup.TabIndex = 1;
            groupSetup.TabStop = false;
            groupSetup.Text = "Setup";
            //
            // lblExecFolder
            //
            lblExecFolder.AutoSize = true;
            lblExecFolder.Location = new Point(12, 29);
            lblExecFolder.Name = "lblExecFolder";
            lblExecFolder.Size = new Size(78, 15);
            lblExecFolder.TabIndex = 0;
            lblExecFolder.Text = "Server folder:";
            //
            // textBoxExecFolder
            //
            textBoxExecFolder.Location = new Point(104, 26);
            textBoxExecFolder.Name = "textBoxExecFolder";
            textBoxExecFolder.Size = new Size(404, 23);
            textBoxExecFolder.TabIndex = 1;
            //
            // buttonBrowseExecFolder
            //
            buttonBrowseExecFolder.Location = new Point(516, 25);
            buttonBrowseExecFolder.Name = "buttonBrowseExecFolder";
            buttonBrowseExecFolder.Size = new Size(88, 25);
            buttonBrowseExecFolder.TabIndex = 2;
            buttonBrowseExecFolder.Text = "Browse...";
            buttonBrowseExecFolder.UseVisualStyleBackColor = true;
            buttonBrowseExecFolder.Click += buttonBrowseExecFolder_Click;
            //
            // linkDownload
            //
            linkDownload.AutoSize = true;
            linkDownload.Location = new Point(102, 53);
            linkDownload.Name = "linkDownload";
            linkDownload.Size = new Size(176, 15);
            linkDownload.TabIndex = 3;
            linkDownload.TabStop = true;
            linkDownload.Text = "Download tnfsd into this folder";
            linkDownload.LinkClicked += linkDownload_LinkClicked;
            //
            // lblShareFolder
            //
            lblShareFolder.AutoSize = true;
            lblShareFolder.Location = new Point(12, 83);
            lblShareFolder.Name = "lblShareFolder";
            lblShareFolder.Size = new Size(74, 15);
            lblShareFolder.TabIndex = 4;
            lblShareFolder.Text = "Share folder:";
            //
            // textBoxShareFolder
            //
            textBoxShareFolder.Location = new Point(104, 80);
            textBoxShareFolder.Name = "textBoxShareFolder";
            textBoxShareFolder.Size = new Size(404, 23);
            textBoxShareFolder.TabIndex = 5;
            //
            // buttonBrowseShareFolder
            //
            buttonBrowseShareFolder.Location = new Point(516, 79);
            buttonBrowseShareFolder.Name = "buttonBrowseShareFolder";
            buttonBrowseShareFolder.Size = new Size(88, 25);
            buttonBrowseShareFolder.TabIndex = 6;
            buttonBrowseShareFolder.Text = "Browse...";
            buttonBrowseShareFolder.UseVisualStyleBackColor = true;
            buttonBrowseShareFolder.Click += buttonBrowseShareFolder_Click;
            //
            // checkBoxAddFirewallException
            //
            checkBoxAddFirewallException.AutoSize = true;
            checkBoxAddFirewallException.Checked = true;
            checkBoxAddFirewallException.CheckState = CheckState.Checked;
            checkBoxAddFirewallException.Location = new Point(104, 112);
            checkBoxAddFirewallException.Name = "checkBoxAddFirewallException";
            checkBoxAddFirewallException.Size = new Size(197, 19);
            checkBoxAddFirewallException.TabIndex = 7;
            checkBoxAddFirewallException.Text = "Allow through Windows Firewall";
            checkBoxAddFirewallException.UseVisualStyleBackColor = true;
            //
            // buttonCreateTask
            //
            buttonCreateTask.Location = new Point(404, 136);
            buttonCreateTask.Name = "buttonCreateTask";
            buttonCreateTask.Size = new Size(96, 28);
            buttonCreateTask.TabIndex = 8;
            buttonCreateTask.Text = "Create Task";
            buttonCreateTask.UseVisualStyleBackColor = true;
            buttonCreateTask.Click += buttonCreateTask_Click;
            //
            // buttonDeleteTask
            //
            buttonDeleteTask.Location = new Point(508, 136);
            buttonDeleteTask.Name = "buttonDeleteTask";
            buttonDeleteTask.Size = new Size(96, 28);
            buttonDeleteTask.TabIndex = 9;
            buttonDeleteTask.Text = "Delete Task";
            buttonDeleteTask.UseVisualStyleBackColor = true;
            buttonDeleteTask.Click += buttonDeleteTask_Click;
            //
            // MainForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(640, 300);
            Controls.Add(panelStatus);
            Controls.Add(groupSetup);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Tnfsd.NET";
            Load += MainForm_Load;
            panelStatus.ResumeLayout(false);
            panelStatus.PerformLayout();
            groupSetup.ResumeLayout(false);
            groupSetup.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelStatus;
        private Label labelStatusText;
        private Label labelStatusDetail;
        private Button buttonStartStop;
        private GroupBox groupSetup;
        private Label lblExecFolder;
        private TextBox textBoxExecFolder;
        private Button buttonBrowseExecFolder;
        private LinkLabel linkDownload;
        private Label lblShareFolder;
        private TextBox textBoxShareFolder;
        private Button buttonBrowseShareFolder;
        private CheckBox checkBoxAddFirewallException;
        private Button buttonCreateTask;
        private Button buttonDeleteTask;
    }
}
