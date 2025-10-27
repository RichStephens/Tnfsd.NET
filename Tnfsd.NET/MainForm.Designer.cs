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
            buttonCreateService = new Button();
            lblExecFolder = new Label();
            lblTnfsShareFolder = new Label();
            textBoxExecFolder = new TextBox();
            textBoxShareFolder = new TextBox();
            buttonBrowseExecFolder = new Button();
            buttonBrowseShareFolder = new Button();
            buttonDeleteService = new Button();
            buttonStartService = new Button();
            buttonStopService = new Button();
            labelServiceIsRunning = new Label();
            labelServiceNotRunning = new Label();
            labelUser = new Label();
            labelPassword = new Label();
            textBoxUser = new TextBox();
            textBoxPassword = new TextBox();
            downloadButton = new Button();
            SuspendLayout();
            // 
            // buttonCreateService
            // 
            buttonCreateService.Location = new Point(116, 177);
            buttonCreateService.Name = "buttonCreateService";
            buttonCreateService.Size = new Size(153, 32);
            buttonCreateService.TabIndex = 6;
            buttonCreateService.Text = "Create/Update Service";
            buttonCreateService.UseVisualStyleBackColor = true;
            buttonCreateService.Click += buttonCreateService_Click;
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
            // buttonDeleteService
            // 
            buttonDeleteService.Location = new Point(275, 177);
            buttonDeleteService.Name = "buttonDeleteService";
            buttonDeleteService.Size = new Size(153, 32);
            buttonDeleteService.TabIndex = 7;
            buttonDeleteService.Text = "Delete Service";
            buttonDeleteService.UseVisualStyleBackColor = true;
            buttonDeleteService.Click += buttonDeleteService_Click;
            // 
            // buttonStartService
            // 
            buttonStartService.Location = new Point(434, 177);
            buttonStartService.Name = "buttonStartService";
            buttonStartService.Size = new Size(153, 32);
            buttonStartService.TabIndex = 8;
            buttonStartService.Text = "Start Service";
            buttonStartService.UseVisualStyleBackColor = true;
            buttonStartService.Click += buttonStartService_Click;
            // 
            // buttonStopService
            // 
            buttonStopService.Location = new Point(593, 177);
            buttonStopService.Name = "buttonStopService";
            buttonStopService.RightToLeft = RightToLeft.No;
            buttonStopService.Size = new Size(153, 32);
            buttonStopService.TabIndex = 9;
            buttonStopService.Text = "Stop Service";
            buttonStopService.UseVisualStyleBackColor = true;
            buttonStopService.Click += buttonStopService_Click;
            // 
            // labelServiceIsRunning
            // 
            labelServiceIsRunning.AutoSize = true;
            labelServiceIsRunning.BorderStyle = BorderStyle.Fixed3D;
            labelServiceIsRunning.Location = new Point(310, 148);
            labelServiceIsRunning.Name = "labelServiceIsRunning";
            labelServiceIsRunning.Size = new Size(232, 17);
            labelServiceIsRunning.TabIndex = 12;
            labelServiceIsRunning.Text = " *** Tnfsd service is currently running ***";
            // 
            // labelServiceNotRunning
            // 
            labelServiceNotRunning.AutoSize = true;
            labelServiceNotRunning.BorderStyle = BorderStyle.Fixed3D;
            labelServiceNotRunning.Location = new Point(296, 148);
            labelServiceNotRunning.Name = "labelServiceNotRunning";
            labelServiceNotRunning.Size = new Size(260, 17);
            labelServiceNotRunning.TabIndex = 13;
            labelServiceNotRunning.Text = " *** Tnfsd service is NOT currently running ***";
            // 
            // labelUser
            // 
            labelUser.AutoSize = true;
            labelUser.Location = new Point(114, 78);
            labelUser.Name = "labelUser";
            labelUser.Size = new Size(36, 15);
            labelUser.TabIndex = 14;
            labelUser.Text = "User:";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(88, 110);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(62, 15);
            labelPassword.TabIndex = 15;
            labelPassword.Text = "Password:";
            // 
            // textBoxUser
            // 
            textBoxUser.Location = new Point(156, 75);
            textBoxUser.Name = "textBoxUser";
            textBoxUser.Size = new Size(285, 23);
            textBoxUser.TabIndex = 16;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(156, 107);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(285, 23);
            textBoxPassword.TabIndex = 17;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // downloadButton
            // 
            downloadButton.Location = new Point(777, 15);
            downloadButton.Name = "downloadButton";
            downloadButton.Size = new Size(75, 23);
            downloadButton.TabIndex = 18;
            downloadButton.Text = "Download";
            downloadButton.UseVisualStyleBackColor = true;
            downloadButton.Click += downloadButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(863, 219);
            Controls.Add(downloadButton);
            Controls.Add(textBoxPassword);
            Controls.Add(textBoxUser);
            Controls.Add(labelPassword);
            Controls.Add(labelUser);
            Controls.Add(labelServiceNotRunning);
            Controls.Add(labelServiceIsRunning);
            Controls.Add(buttonStopService);
            Controls.Add(buttonStartService);
            Controls.Add(buttonDeleteService);
            Controls.Add(buttonCreateService);
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
        private Button buttonCreateService;
        private Button buttonDeleteService;
        private Button buttonStartService;
        private Button buttonStopService;
        private Label labelServiceIsRunning;
        private Label labelServiceNotRunning;
        private Label labelUser;
        private Label labelPassword;
        private TextBox textBoxUser;
        private TextBox textBoxPassword;
        private Button downloadButton;
    }
}
