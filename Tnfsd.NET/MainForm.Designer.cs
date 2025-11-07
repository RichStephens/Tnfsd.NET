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
            buttonCreateTask = new Button();
            lblExecFolder = new Label();
            lblTnfsShareFolder = new Label();
            textBoxExecFolder = new TextBox();
            textBoxShareFolder = new TextBox();
            buttonBrowseExecFolder = new Button();
            buttonBrowseShareFolder = new Button();
            buttonDeleteTask = new Button();
            buttonStartTask = new Button();
            buttonStopTask = new Button();
            labelTaskStatus = new Label();
            buttonDownload = new Button();
            checkBoxAddFirewallException = new CheckBox();
            SuspendLayout();
            // 
            // buttonCreateTask
            // 
            buttonCreateTask.Location = new Point(116, 105);
            buttonCreateTask.Name = "buttonCreateTask";
            buttonCreateTask.Size = new Size(153, 32);
            buttonCreateTask.TabIndex = 6;
            buttonCreateTask.Text = "Create/Update Task";
            buttonCreateTask.UseVisualStyleBackColor = true;
            buttonCreateTask.Click += buttonCreateTask_Click;
            // 
            // lblExecFolder
            // 
            lblExecFolder.AutoSize = true;
            lblExecFolder.Location = new Point(8, 19);
            lblExecFolder.Name = "lblExecFolder";
            lblExecFolder.Size = new Size(142, 15);
            lblExecFolder.TabIndex = 0;
            lblExecFolder.Text = "Tnfsd executable folder:";
            // 
            // lblTnfsShareFolder
            // 
            lblTnfsShareFolder.AutoSize = true;
            lblTnfsShareFolder.Location = new Point(40, 48);
            lblTnfsShareFolder.Name = "lblTnfsShareFolder";
            lblTnfsShareFolder.Size = new Size(110, 15);
            lblTnfsShareFolder.TabIndex = 1;
            lblTnfsShareFolder.Text = "Tnfsd share folder:";
            // 
            // textBoxExecFolder
            // 
            textBoxExecFolder.Location = new Point(156, 16);
            textBoxExecFolder.Name = "textBoxExecFolder";
            textBoxExecFolder.Size = new Size(534, 23);
            textBoxExecFolder.TabIndex = 2;
            // 
            // textBoxShareFolder
            // 
            textBoxShareFolder.Location = new Point(156, 44);
            textBoxShareFolder.Name = "textBoxShareFolder";
            textBoxShareFolder.Size = new Size(534, 23);
            textBoxShareFolder.TabIndex = 3;
            // 
            // buttonBrowseExecFolder
            // 
            buttonBrowseExecFolder.Location = new Point(696, 15);
            buttonBrowseExecFolder.Name = "buttonBrowseExecFolder";
            buttonBrowseExecFolder.Size = new Size(75, 23);
            buttonBrowseExecFolder.TabIndex = 4;
            buttonBrowseExecFolder.Text = "Browse";
            buttonBrowseExecFolder.UseVisualStyleBackColor = true;
            buttonBrowseExecFolder.Click += buttonBrowseExecFolder_Click;
            // 
            // buttonBrowseShareFolder
            // 
            buttonBrowseShareFolder.Location = new Point(696, 44);
            buttonBrowseShareFolder.Name = "buttonBrowseShareFolder";
            buttonBrowseShareFolder.Size = new Size(75, 23);
            buttonBrowseShareFolder.TabIndex = 5;
            buttonBrowseShareFolder.Text = "Browse";
            buttonBrowseShareFolder.UseVisualStyleBackColor = true;
            buttonBrowseShareFolder.Click += buttonBrowseShareFolder_Click;
            // 
            // buttonDeleteTask
            // 
            buttonDeleteTask.Location = new Point(275, 105);
            buttonDeleteTask.Name = "buttonDeleteTask";
            buttonDeleteTask.Size = new Size(153, 32);
            buttonDeleteTask.TabIndex = 7;
            buttonDeleteTask.Text = "Delete Task";
            buttonDeleteTask.UseVisualStyleBackColor = true;
            buttonDeleteTask.Click += buttonDeleteTask_Click;
            // 
            // buttonStartTask
            // 
            buttonStartTask.Location = new Point(434, 105);
            buttonStartTask.Name = "buttonStartTask";
            buttonStartTask.Size = new Size(153, 32);
            buttonStartTask.TabIndex = 8;
            buttonStartTask.Text = "Start Task";
            buttonStartTask.UseVisualStyleBackColor = true;
            buttonStartTask.Click += buttonStartTask_Click;
            // 
            // buttonStopTask
            // 
            buttonStopTask.Location = new Point(593, 105);
            buttonStopTask.Name = "buttonStopTask";
            buttonStopTask.RightToLeft = RightToLeft.No;
            buttonStopTask.Size = new Size(153, 32);
            buttonStopTask.TabIndex = 9;
            buttonStopTask.Text = "Stop Task";
            buttonStopTask.UseVisualStyleBackColor = true;
            buttonStopTask.Click += buttonStopTask_Click;
            // 
            // labelTaskStatus
            // 
            labelTaskStatus.AutoSize = true;
            labelTaskStatus.BorderStyle = BorderStyle.Fixed3D;
            labelTaskStatus.Location = new Point(337, 81);
            labelTaskStatus.Name = "labelTaskStatus";
            labelTaskStatus.Size = new Size(189, 17);
            labelTaskStatus.TabIndex = 13;
            labelTaskStatus.Text = " *** Tnfsd task is NOT running ***";
            // 
            // buttonDownload
            // 
            buttonDownload.Location = new Point(777, 15);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(75, 23);
            buttonDownload.TabIndex = 18;
            buttonDownload.Text = "Download";
            buttonDownload.UseVisualStyleBackColor = true;
            buttonDownload.Click += buttonDownload_Click;
            // 
            // checkBoxAddFirewallException
            // 
            checkBoxAddFirewallException.AutoSize = true;
            checkBoxAddFirewallException.Location = new Point(156, 81);
            checkBoxAddFirewallException.Name = "checkBoxAddFirewallException";
            checkBoxAddFirewallException.Size = new Size(152, 19);
            checkBoxAddFirewallException.TabIndex = 19;
            checkBoxAddFirewallException.Text = "Add Firewall Exception";
            checkBoxAddFirewallException.TextImageRelation = TextImageRelation.TextBeforeImage;
            checkBoxAddFirewallException.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(863, 150);
            Controls.Add(checkBoxAddFirewallException);
            Controls.Add(buttonDownload);
            Controls.Add(labelTaskStatus);
            Controls.Add(buttonStopTask);
            Controls.Add(buttonStartTask);
            Controls.Add(buttonDeleteTask);
            Controls.Add(buttonCreateTask);
            Controls.Add(buttonBrowseShareFolder);
            Controls.Add(buttonBrowseExecFolder);
            Controls.Add(textBoxShareFolder);
            Controls.Add(textBoxExecFolder);
            Controls.Add(lblTnfsShareFolder);
            Controls.Add(lblExecFolder);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Tnfsd.NET";
            Load += MainForm_Load;
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
        private Button buttonCreateTask;
        private Button buttonDeleteTask;
        private Button buttonStartTask;
        private Button buttonStopTask;
        private Label labelTaskStatus;
        private Button buttonDownload;
        private CheckBox checkBoxAddFirewallException;
    }
}
