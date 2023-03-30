namespace KeePassHackEdition
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bOpenDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.bCreateDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.bSaveDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.bExit = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bActivateButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.bAoutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.passList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bAddEntry = new System.Windows.Forms.ToolStripMenuItem();
            this.bShowPassword = new System.Windows.Forms.ToolStripMenuItem();
            this.bSaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.bAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.openLicenseDialog = new System.Windows.Forms.OpenFileDialog();
            this.openDbDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveDbDialog = new System.Windows.Forms.SaveFileDialog();
            this.addFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.exporFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.mainMenu.SuspendLayout();
            this.listContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(680, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bOpenDatabase,
            this.bCreateDatabase,
            this.bSaveDatabase,
            this.toolStripMenuItem1,
            this.bExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // bOpenDatabase
            // 
            this.bOpenDatabase.Enabled = false;
            this.bOpenDatabase.Name = "bOpenDatabase";
            this.bOpenDatabase.Size = new System.Drawing.Size(159, 22);
            this.bOpenDatabase.Text = "Open Database";
            this.bOpenDatabase.Click += new System.EventHandler(this.bOpenDatabase_Click);
            // 
            // bCreateDatabase
            // 
            this.bCreateDatabase.Enabled = false;
            this.bCreateDatabase.Name = "bCreateDatabase";
            this.bCreateDatabase.Size = new System.Drawing.Size(159, 22);
            this.bCreateDatabase.Text = "Create Database";
            this.bCreateDatabase.Click += new System.EventHandler(this.bCreateDatabase_Click);
            // 
            // bSaveDatabase
            // 
            this.bSaveDatabase.Enabled = false;
            this.bSaveDatabase.Name = "bSaveDatabase";
            this.bSaveDatabase.Size = new System.Drawing.Size(159, 22);
            this.bSaveDatabase.Text = "Save Database";
            this.bSaveDatabase.Click += new System.EventHandler(this.bSaveDatabase_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(156, 6);
            // 
            // bExit
            // 
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(159, 22);
            this.bExit.Text = "Exit";
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bActivateButton,
            this.toolStripMenuItem2,
            this.bAoutButton});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // bActivateButton
            // 
            this.bActivateButton.Name = "bActivateButton";
            this.bActivateButton.Size = new System.Drawing.Size(180, 22);
            this.bActivateButton.Text = "Activation";
            this.bActivateButton.Click += new System.EventHandler(this.bActivateButton_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // bAoutButton
            // 
            this.bAoutButton.Name = "bAoutButton";
            this.bAoutButton.Size = new System.Drawing.Size(180, 22);
            this.bAoutButton.Text = "About";
            this.bAoutButton.Click += new System.EventHandler(this.bAoutButton_Click);
            // 
            // passList
            // 
            this.passList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.passList.ContextMenuStrip = this.listContextMenu;
            this.passList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passList.FullRowSelect = true;
            this.passList.HideSelection = false;
            this.passList.Location = new System.Drawing.Point(0, 24);
            this.passList.Name = "passList";
            this.passList.Size = new System.Drawing.Size(680, 504);
            this.passList.TabIndex = 1;
            this.passList.UseCompatibleStateImageBehavior = false;
            this.passList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 168;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Label";
            this.columnHeader2.Width = 302;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Data";
            this.columnHeader3.Width = 202;
            // 
            // listContextMenu
            // 
            this.listContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bAddEntry,
            this.bShowPassword,
            this.bSaveFile,
            this.bAddFile});
            this.listContextMenu.Name = "listContextMenu";
            this.listContextMenu.Size = new System.Drawing.Size(150, 92);
            this.listContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.listContextMenu_Opening);
            // 
            // bAddEntry
            // 
            this.bAddEntry.Name = "bAddEntry";
            this.bAddEntry.Size = new System.Drawing.Size(149, 22);
            this.bAddEntry.Text = "Add Password";
            this.bAddEntry.Click += new System.EventHandler(this.bAddEntry_Click);
            // 
            // bShowPassword
            // 
            this.bShowPassword.Name = "bShowPassword";
            this.bShowPassword.Size = new System.Drawing.Size(149, 22);
            this.bShowPassword.Text = "Get Password";
            this.bShowPassword.Click += new System.EventHandler(this.bShowPassword_Click);
            // 
            // bSaveFile
            // 
            this.bSaveFile.Name = "bSaveFile";
            this.bSaveFile.Size = new System.Drawing.Size(149, 22);
            this.bSaveFile.Text = "Save File";
            this.bSaveFile.Click += new System.EventHandler(this.bSaveFile_Click);
            // 
            // bAddFile
            // 
            this.bAddFile.Name = "bAddFile";
            this.bAddFile.Size = new System.Drawing.Size(149, 22);
            this.bAddFile.Text = "Add File";
            this.bAddFile.Click += new System.EventHandler(this.bAddFile_Click);
            // 
            // openLicenseDialog
            // 
            this.openLicenseDialog.Filter = "Keepass license Files|*.kpdblic";
            // 
            // openDbDialog
            // 
            this.openDbDialog.Filter = "KeePass DB File|*.kpdb";
            // 
            // saveDbDialog
            // 
            this.saveDbDialog.Filter = "KeePass DB File|*.kpdb";
            // 
            // addFileDialog
            // 
            this.addFileDialog.Filter = "All Files|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 528);
            this.Controls.Add(this.passList);
            this.Controls.Add(this.mainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "KEEPASS HACK EDITION";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.listContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bOpenDatabase;
        private System.Windows.Forms.ToolStripMenuItem bCreateDatabase;
        private System.Windows.Forms.ToolStripMenuItem bSaveDatabase;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bExit;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bActivateButton;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem bAoutButton;
        private System.Windows.Forms.ListView passList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.OpenFileDialog openLicenseDialog;
        private System.Windows.Forms.OpenFileDialog openDbDialog;
        private System.Windows.Forms.SaveFileDialog saveDbDialog;
        private System.Windows.Forms.ContextMenuStrip listContextMenu;
        private System.Windows.Forms.ToolStripMenuItem bAddEntry;
        private System.Windows.Forms.ToolStripMenuItem bShowPassword;
        private System.Windows.Forms.ToolStripMenuItem bSaveFile;
        private System.Windows.Forms.ToolStripMenuItem bAddFile;
        private System.Windows.Forms.OpenFileDialog addFileDialog;
        private System.Windows.Forms.SaveFileDialog exporFileDialog;
    }
}

