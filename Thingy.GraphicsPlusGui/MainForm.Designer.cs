namespace Thingy.GraphicsPlusGui
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
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.EditorTabPage = new System.Windows.Forms.TabPage();
            this.SmdButton = new System.Windows.Forms.Button();
            this.PreviewSizeGroupBox = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.OenFolderButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.StructureTextBox = new System.Windows.Forms.TextBox();
            this.AddSettingsButton = new System.Windows.Forms.Button();
            this.SetValueButton = new System.Windows.Forms.Button();
            this.EqualsLabel = new System.Windows.Forms.Label();
            this.ValueTextBox = new System.Windows.Forms.TextBox();
            this.KeyTextBox = new System.Windows.Forms.TextBox();
            this.SettingsListBox = new System.Windows.Forms.ListBox();
            this.CreateButton = new System.Windows.Forms.Button();
            this.CopyButton = new System.Windows.Forms.Button();
            this.RenameButton = new System.Windows.Forms.Button();
            this.StructureNameTextBox = new System.Windows.Forms.TextBox();
            this.ObjectTreeView = new System.Windows.Forms.TreeView();
            this.FileDirectoryRefreshButton = new System.Windows.Forms.Button();
            this.StructuresListBox = new System.Windows.Forms.ListBox();
            this.FilesListBox = new System.Windows.Forms.ListBox();
            this.PreviewTabPage = new System.Windows.Forms.TabPage();
            this.OpenSmdFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.MainTabControl.SuspendLayout();
            this.EditorTabPage.SuspendLayout();
            this.PreviewSizeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.EditorTabPage);
            this.MainTabControl.Controls.Add(this.PreviewTabPage);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1128, 604);
            this.MainTabControl.TabIndex = 4;
            // 
            // EditorTabPage
            // 
            this.EditorTabPage.Controls.Add(this.SmdButton);
            this.EditorTabPage.Controls.Add(this.PreviewSizeGroupBox);
            this.EditorTabPage.Controls.Add(this.OenFolderButton);
            this.EditorTabPage.Controls.Add(this.RemoveButton);
            this.EditorTabPage.Controls.Add(this.StructureTextBox);
            this.EditorTabPage.Controls.Add(this.AddSettingsButton);
            this.EditorTabPage.Controls.Add(this.SetValueButton);
            this.EditorTabPage.Controls.Add(this.EqualsLabel);
            this.EditorTabPage.Controls.Add(this.ValueTextBox);
            this.EditorTabPage.Controls.Add(this.KeyTextBox);
            this.EditorTabPage.Controls.Add(this.SettingsListBox);
            this.EditorTabPage.Controls.Add(this.CreateButton);
            this.EditorTabPage.Controls.Add(this.CopyButton);
            this.EditorTabPage.Controls.Add(this.RenameButton);
            this.EditorTabPage.Controls.Add(this.StructureNameTextBox);
            this.EditorTabPage.Controls.Add(this.ObjectTreeView);
            this.EditorTabPage.Controls.Add(this.FileDirectoryRefreshButton);
            this.EditorTabPage.Controls.Add(this.StructuresListBox);
            this.EditorTabPage.Controls.Add(this.FilesListBox);
            this.EditorTabPage.Location = new System.Drawing.Point(4, 22);
            this.EditorTabPage.Name = "EditorTabPage";
            this.EditorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.EditorTabPage.Size = new System.Drawing.Size(1120, 578);
            this.EditorTabPage.TabIndex = 0;
            this.EditorTabPage.Text = "Editor";
            this.EditorTabPage.UseVisualStyleBackColor = true;
            // 
            // SmdButton
            // 
            this.SmdButton.Location = new System.Drawing.Point(6, 130);
            this.SmdButton.Name = "SmdButton";
            this.SmdButton.Size = new System.Drawing.Size(75, 23);
            this.SmdButton.TabIndex = 5;
            this.SmdButton.Text = "SMD->STR";
            this.SmdButton.UseVisualStyleBackColor = true;
            this.SmdButton.Click += new System.EventHandler(this.SmdButton_Click);
            // 
            // PreviewSizeGroupBox
            // 
            this.PreviewSizeGroupBox.Controls.Add(this.radioButton1);
            this.PreviewSizeGroupBox.Controls.Add(this.radioButton2);
            this.PreviewSizeGroupBox.Controls.Add(this.radioButton3);
            this.PreviewSizeGroupBox.Controls.Add(this.radioButton4);
            this.PreviewSizeGroupBox.Location = new System.Drawing.Point(6, 8);
            this.PreviewSizeGroupBox.Name = "PreviewSizeGroupBox";
            this.PreviewSizeGroupBox.Size = new System.Drawing.Size(85, 116);
            this.PreviewSizeGroupBox.TabIndex = 26;
            this.PreviewSizeGroupBox.TabStop = false;
            this.PreviewSizeGroupBox.Text = "Preview Size";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(66, 17);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "800x600";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.ResizePreviewOption_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(72, 17);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "1024x768";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.ResizePreviewOption_Click);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 65);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(72, 17);
            this.radioButton3.TabIndex = 6;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "1280x720";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.ResizePreviewOption_Click);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(6, 88);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(72, 17);
            this.radioButton4.TabIndex = 7;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "1440x810";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.ResizePreviewOption_Click);
            // 
            // OenFolderButton
            // 
            this.OenFolderButton.Location = new System.Drawing.Point(178, 538);
            this.OenFolderButton.Name = "OenFolderButton";
            this.OenFolderButton.Size = new System.Drawing.Size(75, 23);
            this.OenFolderButton.TabIndex = 25;
            this.OenFolderButton.Text = "Folder";
            this.OenFolderButton.UseVisualStyleBackColor = true;
            this.OenFolderButton.Click += new System.EventHandler(this.OenFolderButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(1037, 538);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 24;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // StructureTextBox
            // 
            this.StructureTextBox.Location = new System.Drawing.Point(350, 460);
            this.StructureTextBox.Name = "StructureTextBox";
            this.StructureTextBox.Size = new System.Drawing.Size(247, 20);
            this.StructureTextBox.TabIndex = 23;
            // 
            // AddSettingsButton
            // 
            this.AddSettingsButton.Location = new System.Drawing.Point(431, 509);
            this.AddSettingsButton.Name = "AddSettingsButton";
            this.AddSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.AddSettingsButton.TabIndex = 22;
            this.AddSettingsButton.Text = "+Setttings";
            this.AddSettingsButton.UseVisualStyleBackColor = true;
            this.AddSettingsButton.Click += new System.EventHandler(this.AddSettingsButton_Click);
            // 
            // SetValueButton
            // 
            this.SetValueButton.Location = new System.Drawing.Point(775, 512);
            this.SetValueButton.Name = "SetValueButton";
            this.SetValueButton.Size = new System.Drawing.Size(75, 23);
            this.SetValueButton.TabIndex = 21;
            this.SetValueButton.Text = "Set Value";
            this.SetValueButton.UseVisualStyleBackColor = true;
            this.SetValueButton.Click += new System.EventHandler(this.SetValueButton_Click);
            // 
            // EqualsLabel
            // 
            this.EqualsLabel.AutoSize = true;
            this.EqualsLabel.Location = new System.Drawing.Point(716, 489);
            this.EqualsLabel.Name = "EqualsLabel";
            this.EqualsLabel.Size = new System.Drawing.Size(13, 13);
            this.EqualsLabel.TabIndex = 20;
            this.EqualsLabel.Text = "=";
            // 
            // ValueTextBox
            // 
            this.ValueTextBox.Location = new System.Drawing.Point(735, 486);
            this.ValueTextBox.Name = "ValueTextBox";
            this.ValueTextBox.Size = new System.Drawing.Size(115, 20);
            this.ValueTextBox.TabIndex = 19;
            // 
            // KeyTextBox
            // 
            this.KeyTextBox.Location = new System.Drawing.Point(603, 486);
            this.KeyTextBox.Name = "KeyTextBox";
            this.KeyTextBox.Size = new System.Drawing.Size(107, 20);
            this.KeyTextBox.TabIndex = 18;
            // 
            // SettingsListBox
            // 
            this.SettingsListBox.FormattingEnabled = true;
            this.SettingsListBox.Location = new System.Drawing.Point(603, 8);
            this.SettingsListBox.Name = "SettingsListBox";
            this.SettingsListBox.Size = new System.Drawing.Size(247, 472);
            this.SettingsListBox.TabIndex = 17;
            this.SettingsListBox.Click += new System.EventHandler(this.SettingsListBox_Click);
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(350, 509);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 16;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // CopyButton
            // 
            this.CopyButton.Location = new System.Drawing.Point(522, 509);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(75, 23);
            this.CopyButton.TabIndex = 15;
            this.CopyButton.Text = "Copy";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // RenameButton
            // 
            this.RenameButton.Location = new System.Drawing.Point(522, 486);
            this.RenameButton.Name = "RenameButton";
            this.RenameButton.Size = new System.Drawing.Size(75, 23);
            this.RenameButton.TabIndex = 14;
            this.RenameButton.Text = "Rename";
            this.RenameButton.UseVisualStyleBackColor = true;
            this.RenameButton.Click += new System.EventHandler(this.RenameButton_Click);
            // 
            // StructureNameTextBox
            // 
            this.StructureNameTextBox.Location = new System.Drawing.Point(350, 486);
            this.StructureNameTextBox.Name = "StructureNameTextBox";
            this.StructureNameTextBox.Size = new System.Drawing.Size(166, 20);
            this.StructureNameTextBox.TabIndex = 13;
            // 
            // ObjectTreeView
            // 
            this.ObjectTreeView.LabelEdit = true;
            this.ObjectTreeView.Location = new System.Drawing.Point(856, 8);
            this.ObjectTreeView.Name = "ObjectTreeView";
            this.ObjectTreeView.Size = new System.Drawing.Size(256, 524);
            this.ObjectTreeView.TabIndex = 12;
            this.ObjectTreeView.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ObjectTreeView_BeforeLabelEdit);
            this.ObjectTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ObjectTreeView_AfterLabelEdit);
            // 
            // FileDirectoryRefreshButton
            // 
            this.FileDirectoryRefreshButton.Location = new System.Drawing.Point(97, 538);
            this.FileDirectoryRefreshButton.Name = "FileDirectoryRefreshButton";
            this.FileDirectoryRefreshButton.Size = new System.Drawing.Size(75, 23);
            this.FileDirectoryRefreshButton.TabIndex = 11;
            this.FileDirectoryRefreshButton.Text = "Refresh";
            this.FileDirectoryRefreshButton.UseVisualStyleBackColor = true;
            this.FileDirectoryRefreshButton.Click += new System.EventHandler(this.FileDirectoryRefreshButton_Click);
            // 
            // StructuresListBox
            // 
            this.StructuresListBox.FormattingEnabled = true;
            this.StructuresListBox.Location = new System.Drawing.Point(350, 8);
            this.StructuresListBox.Name = "StructuresListBox";
            this.StructuresListBox.Size = new System.Drawing.Size(247, 446);
            this.StructuresListBox.TabIndex = 9;
            this.StructuresListBox.Click += new System.EventHandler(this.StructuresListBox_Click);
            // 
            // FilesListBox
            // 
            this.FilesListBox.FormattingEnabled = true;
            this.FilesListBox.Location = new System.Drawing.Point(97, 8);
            this.FilesListBox.Name = "FilesListBox";
            this.FilesListBox.Size = new System.Drawing.Size(247, 524);
            this.FilesListBox.TabIndex = 8;
            this.FilesListBox.DoubleClick += new System.EventHandler(this.FilesListBox_DoubleClick);
            // 
            // PreviewTabPage
            // 
            this.PreviewTabPage.Location = new System.Drawing.Point(4, 22);
            this.PreviewTabPage.Name = "PreviewTabPage";
            this.PreviewTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PreviewTabPage.Size = new System.Drawing.Size(1120, 578);
            this.PreviewTabPage.TabIndex = 1;
            this.PreviewTabPage.Text = "Preview";
            this.PreviewTabPage.UseVisualStyleBackColor = true;
            this.PreviewTabPage.Paint += new System.Windows.Forms.PaintEventHandler(this.tabPage2_Paint);
            // 
            // OpenSmdFileDialog
            // 
            this.OpenSmdFileDialog.FileName = "openFileDialog1";
            this.OpenSmdFileDialog.Filter = "SMD Files (*.smd)|*.smd";
            this.OpenSmdFileDialog.Title = "Select SMD File";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 604);
            this.Controls.Add(this.MainTabControl);
            this.Name = "MainForm";
            this.Text = "Thingy Editor";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.MainTabControl.ResumeLayout(false);
            this.EditorTabPage.ResumeLayout(false);
            this.EditorTabPage.PerformLayout();
            this.PreviewSizeGroupBox.ResumeLayout(false);
            this.PreviewSizeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage EditorTabPage;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TabPage PreviewTabPage;
        private System.Windows.Forms.Button FileDirectoryRefreshButton;
        private System.Windows.Forms.ListBox StructuresListBox;
        private System.Windows.Forms.ListBox FilesListBox;
        private System.Windows.Forms.TreeView ObjectTreeView;
        private System.Windows.Forms.TextBox StructureNameTextBox;
        private System.Windows.Forms.Button RenameButton;
        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button SetValueButton;
        private System.Windows.Forms.Label EqualsLabel;
        private System.Windows.Forms.TextBox ValueTextBox;
        private System.Windows.Forms.TextBox KeyTextBox;
        private System.Windows.Forms.ListBox SettingsListBox;
        private System.Windows.Forms.Button AddSettingsButton;
        private System.Windows.Forms.TextBox StructureTextBox;
        private System.Windows.Forms.Button OenFolderButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.GroupBox PreviewSizeGroupBox;
        private System.Windows.Forms.Button SmdButton;
        private System.Windows.Forms.OpenFileDialog OpenSmdFileDialog;
    }
}

