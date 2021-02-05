using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace RGBVideoEnhancement.Client
{
	partial class RGBVideoEnhancementViewItemUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RGBVideoEnhancementViewItemUserControl));
            this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.hScrollBarExpose = new System.Windows.Forms.HScrollBar();
            this.hScrollBarOffset = new System.Windows.Forms.HScrollBar();
            this.label2 = new System.Windows.Forms.Label();
            this.vScrollBarB = new System.Windows.Forms.VScrollBar();
            this.vScrollBarG = new System.Windows.Forms.VScrollBar();
            this.vScrollBarR = new System.Windows.Forms.VScrollBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.labelDecode = new System.Windows.Forms.Label();
            this.panelTitleBar.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripcopy
            // 
            this.contextMenuStripcopy.ImageScalingSize = new System.Drawing.Size(20, 20);
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
            this.labelName.Click += new System.EventHandler(this.OnClick);
            this.labelName.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // panelTitleBar
            // 
            this.panelTitleBar.BackColor = System.Drawing.Color.ForestGreen;
            this.panelTitleBar.Controls.Add(this.label1);
            this.panelTitleBar.Controls.Add(this.labelName);
            resources.ApplyResources(this.panelTitleBar, "panelTitleBar");
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
            this.panelTitleBar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // hScrollBarExpose
            // 
            resources.ApplyResources(this.hScrollBarExpose, "hScrollBarExpose");
            this.hScrollBarExpose.LargeChange = 1;
            this.hScrollBarExpose.Maximum = 10;
            this.hScrollBarExpose.Minimum = 1;
            this.hScrollBarExpose.Name = "hScrollBarExpose";
            this.hScrollBarExpose.Value = 1;
            this.hScrollBarExpose.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnScrollChange);
            // 
            // hScrollBarOffset
            // 
            resources.ApplyResources(this.hScrollBarOffset, "hScrollBarOffset");
            this.hScrollBarOffset.Maximum = 255;
            this.hScrollBarOffset.Minimum = -255;
            this.hScrollBarOffset.Name = "hScrollBarOffset";
            this.hScrollBarOffset.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnScrollChange);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // vScrollBarB
            // 
            resources.ApplyResources(this.vScrollBarB, "vScrollBarB");
            this.vScrollBarB.Name = "vScrollBarB";
            this.vScrollBarB.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnScrollChange);
            // 
            // vScrollBarG
            // 
            resources.ApplyResources(this.vScrollBarG, "vScrollBarG");
            this.vScrollBarG.Name = "vScrollBarG";
            this.vScrollBarG.Value = 50;
            this.vScrollBarG.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnScrollChange);
            // 
            // vScrollBarR
            // 
            resources.ApplyResources(this.vScrollBarR, "vScrollBarR");
            this.vScrollBarR.Name = "vScrollBarR";
            this.vScrollBarR.Value = 100;
            this.vScrollBarR.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnScrollChange);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.pictureBox);
            this.panel2.Name = "panel2";
            // 
            // pictureBox
            // 
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.OnClick);
            this.pictureBox.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.pictureBox.Resize += new System.EventHandler(this.OnResizePictureBox);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.labelDecode);
            this.panelMain.Controls.Add(this.panel2);
            this.panelMain.Controls.Add(this.vScrollBarR);
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Controls.Add(this.vScrollBarG);
            this.panelMain.Controls.Add(this.label3);
            this.panelMain.Controls.Add(this.vScrollBarB);
            this.panelMain.Controls.Add(this.hScrollBarExpose);
            this.panelMain.Controls.Add(this.label2);
            this.panelMain.Controls.Add(this.hScrollBarOffset);
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            this.panelMain.Click += new System.EventHandler(this.OnClick);
            this.panelMain.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // labelDecode
            // 
            resources.ApplyResources(this.labelDecode, "labelDecode");
            this.labelDecode.Name = "labelDecode";
            // 
            // RGBVideoEnhancementViewItemUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelTitleBar);
            resources.ApplyResources(this, "$this");
            this.Name = "RGBVideoEnhancementViewItemUserControl";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControlMouseDoubleClick);
            this.Resize += new System.EventHandler(this.OnResize);
            this.panelTitleBar.ResumeLayout(false);
            this.panelTitleBar.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		internal ContextMenuStrip contextMenuStripcopy;
		internal ToolStripMenuItem copyToolStripMenuItem;
		private Label label1;
		private Label labelName;
		private Panel panelTitleBar;
		private Label label4;
		private Label label3;
		private HScrollBar hScrollBarExpose;
		private HScrollBar hScrollBarOffset;
		private Label label2;
		private VScrollBar vScrollBarB;
		private VScrollBar vScrollBarG;
		private VScrollBar vScrollBarR;
		private Panel panel2;
		private PictureBox pictureBox;
		private Panel panelMain;
        private Label labelDecode;

	}
}
