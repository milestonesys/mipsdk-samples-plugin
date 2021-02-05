using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SCIndependentPlayback.Client2
{
	partial class SCIndependentPlayback2ViewItemUserControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SCIndependentPlayback2ViewItemUserControl));
			this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panelVideo = new System.Windows.Forms.Panel();
			this.panelPlaybackControl = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
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
			// 
			// labelName
			// 
			resources.ApplyResources(this.labelName, "labelName");
			this.labelName.Name = "labelName";
			// 
			// panel1
			// 
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.BackColor = System.Drawing.Color.DarkGray;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.labelName);
			this.panel1.Name = "panel1";
			// 
			// panelVideo
			// 
			resources.ApplyResources(this.panelVideo, "panelVideo");
			this.panelVideo.Name = "panelVideo";
			// 
			// panelPlaybackControl
			// 
			resources.ApplyResources(this.panelPlaybackControl, "panelPlaybackControl");
			this.panelPlaybackControl.BackColor = System.Drawing.Color.White;
			this.panelPlaybackControl.Name = "panelPlaybackControl";
			// 
			// SCIndependentPlayback2ViewItemUserControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.panelPlaybackControl);
			this.Controls.Add(this.panelVideo);
			this.Controls.Add(this.panel1);
			resources.ApplyResources(this, "$this");
			this.Name = "SCIndependentPlayback2ViewItemUserControl";
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		internal ContextMenuStrip contextMenuStripcopy;
		internal ToolStripMenuItem copyToolStripMenuItem;
		private Label label1;
		private Label labelName;
		private Panel panel1;
		private Panel panelVideo;
		private Panel panelPlaybackControl;

	}
}
