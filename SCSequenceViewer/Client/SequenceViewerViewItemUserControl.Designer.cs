using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SequenceViewer.Client
{
	partial class SequenceViewerViewItemUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SequenceViewerViewItemUserControl));
            this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.buttonSelectCamera = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonShowSeq = new System.Windows.Forms.Button();
            this.buttonShowSeqAsync = new System.Windows.Forms.Button();
            this.buttonShowTypes = new System.Windows.Forms.Button();
            this.buttonShowMDAsync = new System.Windows.Forms.Button();
            this.buttonShowMD = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
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
            // buttonSelectCamera
            // 
            resources.ApplyResources(this.buttonSelectCamera, "buttonSelectCamera");
            this.buttonSelectCamera.Name = "buttonSelectCamera";
            this.buttonSelectCamera.UseVisualStyleBackColor = true;
            this.buttonSelectCamera.Click += new System.EventHandler(this.OnSelectCamera);
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            this.listBox1.TabStop = false;
            // 
            // buttonShowSeq
            // 
            resources.ApplyResources(this.buttonShowSeq, "buttonShowSeq");
            this.buttonShowSeq.Name = "buttonShowSeq";
            this.buttonShowSeq.UseVisualStyleBackColor = true;
            this.buttonShowSeq.Click += new System.EventHandler(this.OnShowSeq);
            // 
            // buttonShowSeqAsync
            // 
            resources.ApplyResources(this.buttonShowSeqAsync, "buttonShowSeqAsync");
            this.buttonShowSeqAsync.Name = "buttonShowSeqAsync";
            this.buttonShowSeqAsync.UseVisualStyleBackColor = true;
            this.buttonShowSeqAsync.Click += new System.EventHandler(this.OnRefreshSeqAsync);
            // 
            // buttonShowTypes
            // 
            resources.ApplyResources(this.buttonShowTypes, "buttonShowTypes");
            this.buttonShowTypes.Name = "buttonShowTypes";
            this.buttonShowTypes.UseVisualStyleBackColor = true;
            this.buttonShowTypes.Click += new System.EventHandler(this.OnGetSeqType);
            // 
            // buttonShowMDAsync
            // 
            resources.ApplyResources(this.buttonShowMDAsync, "buttonShowMDAsync");
            this.buttonShowMDAsync.Name = "buttonShowMDAsync";
            this.buttonShowMDAsync.UseVisualStyleBackColor = true;
            this.buttonShowMDAsync.Click += new System.EventHandler(this.OnRefreshMDAsync);
            // 
            // buttonShowMD
            // 
            resources.ApplyResources(this.buttonShowMD, "buttonShowMD");
            this.buttonShowMD.Name = "buttonShowMD";
            this.buttonShowMD.UseVisualStyleBackColor = true;
            this.buttonShowMD.Click += new System.EventHandler(this.OnRefreshMD);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.buttonSelectCamera);
            this.panelMain.Controls.Add(this.listBox1);
            this.panelMain.Controls.Add(this.buttonShowMDAsync);
            this.panelMain.Controls.Add(this.buttonShowSeq);
            this.panelMain.Controls.Add(this.buttonShowMD);
            this.panelMain.Controls.Add(this.buttonShowTypes);
            this.panelMain.Controls.Add(this.buttonShowSeqAsync);
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            this.panelMain.Click += new System.EventHandler(this.OnClick);
            this.panelMain.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // SequenceViewerViewItemUserControl
            // 
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.Name = "SequenceViewerViewItemUserControl";
            resources.ApplyResources(this, "$this");
            this.Click += new System.EventHandler(this.OnClick);
            this.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		internal ContextMenuStrip contextMenuStripcopy;
		internal ToolStripMenuItem copyToolStripMenuItem;
		private Label label1;
		private Label labelName;
		private Panel panelHeader;
		private Button buttonSelectCamera;
		private ListBox listBox1;
		private Button buttonShowSeq;
		private Button buttonShowSeqAsync;
		private Button buttonShowTypes;
		private Button buttonShowMDAsync;
		private Button buttonShowMD;
		private Panel panelMain;

	}
}
