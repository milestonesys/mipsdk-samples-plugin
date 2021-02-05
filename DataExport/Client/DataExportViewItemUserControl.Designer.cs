using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DataExport.Client
{
    partial class DataExportViewItemUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataExportViewItemUserControl));
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._noteTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            // 
            // _noteTextBox
            // 
            resources.ApplyResources(this._noteTextBox, "_noteTextBox");
            this._noteTextBox.Name = "_noteTextBox";
            this._noteTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this._noteTextBox_MouseClick);
            this._noteTextBox.TextChanged += new System.EventHandler(this._noteTextBox_TextChanged);
            this._noteTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._noteTextBox_MouseDoubleClick);
            // 
            // DataExportViewItemUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._noteTextBox);
            resources.ApplyResources(this, "$this");
            this.Name = "DataExportViewItemUserControl";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal ToolStripMenuItem copyToolStripMenuItem;
        private TextBox _noteTextBox;
    }
}
