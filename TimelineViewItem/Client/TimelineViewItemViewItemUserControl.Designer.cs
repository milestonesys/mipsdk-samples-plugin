using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TimelineViewItem.Client
{
    partial class TimelineViewItemViewItemUserControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimelineViewItemViewItemUserControl));
            this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripcopy
            // 
            this.contextMenuStripcopy.Name = "contextMenuStripcopy";
            resources.ApplyResources(this.contextMenuStripcopy, "contextMenuStripcopy");
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.OnClick);
            this.label1.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // labelName
            // 
            resources.ApplyResources(this.labelName, "labelName");
            this.labelName.Name = "labelName";
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.Silver;
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.labelName);
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
            this.panelHeader.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            this.panelMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
            this.panelMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
            // 
            // TimelineViewItemViewItemUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            resources.ApplyResources(this, "$this");
            this.Name = "TimelineViewItemViewItemUserControl";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal ContextMenuStrip contextMenuStripcopy;
        internal ToolStripMenuItem copyToolStripMenuItem;
        private Label label1;
        private Label labelName;
        private Panel panelHeader;
        private Panel panelMain;

    }
}
