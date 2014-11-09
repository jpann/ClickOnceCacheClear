namespace mClickOnceCacheClear
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpFiles = new System.Windows.Forms.GroupBox();
            this.btnFilesBackup = new System.Windows.Forms.Button();
            this.btnFilesDelete = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListView();
            this.colFilesFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFilesFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpRegistry = new System.Windows.Forms.GroupBox();
            this.btnRegistryBackup = new System.Windows.Forms.Button();
            this.btnRegistryDelete = new System.Windows.Forms.Button();
            this.lstRegistry = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpFiles.SuspendLayout();
            this.grpRegistry.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFiles
            // 
            this.grpFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFiles.Controls.Add(this.btnFilesBackup);
            this.grpFiles.Controls.Add(this.btnFilesDelete);
            this.grpFiles.Controls.Add(this.lstFiles);
            this.grpFiles.Location = new System.Drawing.Point(12, 12);
            this.grpFiles.Name = "grpFiles";
            this.grpFiles.Size = new System.Drawing.Size(1019, 250);
            this.grpFiles.TabIndex = 0;
            this.grpFiles.TabStop = false;
            this.grpFiles.Text = "Files / Directories";
            // 
            // btnFilesBackup
            // 
            this.btnFilesBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilesBackup.Location = new System.Drawing.Point(847, 221);
            this.btnFilesBackup.Name = "btnFilesBackup";
            this.btnFilesBackup.Size = new System.Drawing.Size(75, 23);
            this.btnFilesBackup.TabIndex = 2;
            this.btnFilesBackup.Text = "Backup";
            this.btnFilesBackup.UseVisualStyleBackColor = true;
            this.btnFilesBackup.Click += new System.EventHandler(this.btnFilesBackup_Click);
            // 
            // btnFilesDelete
            // 
            this.btnFilesDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilesDelete.Enabled = false;
            this.btnFilesDelete.Location = new System.Drawing.Point(928, 221);
            this.btnFilesDelete.Name = "btnFilesDelete";
            this.btnFilesDelete.Size = new System.Drawing.Size(75, 23);
            this.btnFilesDelete.TabIndex = 1;
            this.btnFilesDelete.Text = "Delete";
            this.btnFilesDelete.UseVisualStyleBackColor = true;
            this.btnFilesDelete.Click += new System.EventHandler(this.btnFilesDelete_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.CheckBoxes = true;
            this.lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFilesFileName,
            this.colFilesFilePath});
            this.lstFiles.FullRowSelect = true;
            this.lstFiles.GridLines = true;
            this.lstFiles.Location = new System.Drawing.Point(16, 19);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(987, 196);
            this.lstFiles.TabIndex = 0;
            this.lstFiles.UseCompatibleStateImageBehavior = false;
            this.lstFiles.View = System.Windows.Forms.View.Details;
            // 
            // colFilesFileName
            // 
            this.colFilesFileName.Text = "Name";
            this.colFilesFileName.Width = 292;
            // 
            // colFilesFilePath
            // 
            this.colFilesFilePath.Text = "Full Path";
            this.colFilesFilePath.Width = 689;
            // 
            // grpRegistry
            // 
            this.grpRegistry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRegistry.Controls.Add(this.btnRegistryBackup);
            this.grpRegistry.Controls.Add(this.btnRegistryDelete);
            this.grpRegistry.Controls.Add(this.lstRegistry);
            this.grpRegistry.Location = new System.Drawing.Point(12, 268);
            this.grpRegistry.Name = "grpRegistry";
            this.grpRegistry.Size = new System.Drawing.Size(1019, 250);
            this.grpRegistry.TabIndex = 1;
            this.grpRegistry.TabStop = false;
            this.grpRegistry.Text = "Registry Keys";
            // 
            // btnRegistryBackup
            // 
            this.btnRegistryBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegistryBackup.Location = new System.Drawing.Point(847, 218);
            this.btnRegistryBackup.Name = "btnRegistryBackup";
            this.btnRegistryBackup.Size = new System.Drawing.Size(75, 23);
            this.btnRegistryBackup.TabIndex = 3;
            this.btnRegistryBackup.Text = "Backup";
            this.btnRegistryBackup.UseVisualStyleBackColor = true;
            this.btnRegistryBackup.Click += new System.EventHandler(this.btnRegistryBackup_Click);
            // 
            // btnRegistryDelete
            // 
            this.btnRegistryDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegistryDelete.Enabled = false;
            this.btnRegistryDelete.Location = new System.Drawing.Point(928, 218);
            this.btnRegistryDelete.Name = "btnRegistryDelete";
            this.btnRegistryDelete.Size = new System.Drawing.Size(75, 23);
            this.btnRegistryDelete.TabIndex = 1;
            this.btnRegistryDelete.Text = "Delete";
            this.btnRegistryDelete.UseVisualStyleBackColor = true;
            this.btnRegistryDelete.Click += new System.EventHandler(this.btnRegistryDelete_Click);
            // 
            // lstRegistry
            // 
            this.lstRegistry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRegistry.CheckBoxes = true;
            this.lstRegistry.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstRegistry.FullRowSelect = true;
            this.lstRegistry.GridLines = true;
            this.lstRegistry.Location = new System.Drawing.Point(16, 19);
            this.lstRegistry.Name = "lstRegistry";
            this.lstRegistry.Size = new System.Drawing.Size(987, 193);
            this.lstRegistry.TabIndex = 0;
            this.lstRegistry.UseCompatibleStateImageBehavior = false;
            this.lstRegistry.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 292;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Full Path";
            this.columnHeader2.Width = 689;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 528);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1043, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 550);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.grpRegistry);
            this.Controls.Add(this.grpFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Main";
            this.ShowIcon = false;
            this.Text = "mClickOnceCacheClear";
            this.Load += new System.EventHandler(this.Main_Load);
            this.grpFiles.ResumeLayout(false);
            this.grpRegistry.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFiles;
        private System.Windows.Forms.Button btnFilesDelete;
        private System.Windows.Forms.ListView lstFiles;
        private System.Windows.Forms.ColumnHeader colFilesFileName;
        private System.Windows.Forms.ColumnHeader colFilesFilePath;
        private System.Windows.Forms.GroupBox grpRegistry;
        private System.Windows.Forms.Button btnRegistryDelete;
        private System.Windows.Forms.ListView lstRegistry;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnFilesBackup;
        private System.Windows.Forms.Button btnRegistryBackup;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}

