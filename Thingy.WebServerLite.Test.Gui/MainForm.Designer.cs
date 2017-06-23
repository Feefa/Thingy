namespace Thingy.WebServerLite.Test.Gui
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
            this.MessagesTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // MessagesTextBox
            // 
            this.MessagesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessagesTextBox.Location = new System.Drawing.Point(0, 0);
            this.MessagesTextBox.Multiline = true;
            this.MessagesTextBox.Name = "MessagesTextBox";
            this.MessagesTextBox.Size = new System.Drawing.Size(946, 583);
            this.MessagesTextBox.TabIndex = 0;
            this.MessagesTextBox.DoubleClick += new System.EventHandler(this.MessagesTextBox_DoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 583);
            this.Controls.Add(this.MessagesTextBox);
            this.Name = "MainForm";
            this.Text = "WebServerLite Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MessagesTextBox;
    }
}

