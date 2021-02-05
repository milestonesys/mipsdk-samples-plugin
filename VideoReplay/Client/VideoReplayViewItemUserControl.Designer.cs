using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace VideoReplay.Client
{
	partial class VideoReplayViewItemUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoReplayViewItemUserControl));
            this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panelMain.SuspendLayout();
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
            this.panelHeader.BackColor = System.Drawing.Color.Goldenrod;
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.labelName);
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Click += new System.EventHandler(this.OnClick);
            this.panelHeader.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnReplay);
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.OnClick);
            this.pictureBox2.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.button1);
            this.panelMain.Controls.Add(this.button3);
            this.panelMain.Controls.Add(this.pictureBox2);
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            this.panelMain.Click += new System.EventHandler(this.OnClick);
            this.panelMain.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // VideoReplayViewItemUserControl
            // 
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.label2);
            this.Name = "VideoReplayViewItemUserControl";
            resources.ApplyResources(this, "$this");
            this.Click += new System.EventHandler(this.OnClick);
            this.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		internal ContextMenuStrip contextMenuStripcopy;
		internal ToolStripMenuItem copyToolStripMenuItem;
		private Label label1;
		private Label labelName;
		private Panel panelHeader;
		private Button button3;
		private PictureBox pictureBox2;
		private Label label2;
		private Panel panelMain;
		private Button button1;

	}
}
