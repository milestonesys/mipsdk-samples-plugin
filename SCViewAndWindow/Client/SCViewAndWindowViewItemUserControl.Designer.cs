using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SCViewAndWindow.Client
{
	partial class SCViewAndWindowViewItemUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SCViewAndWindowViewItemUserControl));
            this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.multiWindowUserControl1 = new SCViewAndWindow.Client.MultiWindowUserControl();
            this.ptzUserControl1 = new SCViewAndWindow.Client.PTZUserControl();
            this.ptzAbsolutUserControl1 = new SCViewAndWindow.Client.PTZAbsolutUserControl();
            this.indicatorAndAppUserControl1 = new SCViewAndWindow.Client.IndicatorAndAppUserControl();
            this.lensUserControl1 = new SCViewAndWindow.Client.LensUserControl();
            this.playbackUControl1 = new SCViewAndWindow.Client.PlaybackUControl();
            this.auxUserControl1 = new SCViewAndWindow.Client.AUXUserControl();
            this.viewCreateUserControl21 = new SCViewAndWindow.Client.ViewCreateUserControl();
            this.viewCommandUserControl1 = new SCViewAndWindow.Client.ViewCommandUserControl();
            this.viewEditUserControl21 = new SCViewAndWindow.Client.ViewEditUserControl();
            this.viewDumpUserControl1 = new SCViewAndWindow.Client.ViewDumpUserControl();
            this.workspaceUserControl1 = new SCViewAndWindow.Client.WorkspaceUserControl();
            this.panelHeader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.tabPage12.SuspendLayout();
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
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.labelName);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Click += new System.EventHandler(this.OnClick);
            this.panelHeader.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage8);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage5);
            this.tabControl.Controls.Add(this.tabPage6);
            this.tabControl.Controls.Add(this.tabPage7);
            this.tabControl.Controls.Add(this.tabPage9);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Controls.Add(this.tabPage10);
            this.tabControl.Controls.Add(this.tabPage11);
            this.tabControl.Controls.Add(this.tabPage12);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.multiWindowUserControl1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ptzUserControl1);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.ptzAbsolutUserControl1);
            resources.ApplyResources(this.tabPage8, "tabPage8");
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.indicatorAndAppUserControl1);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.lensUserControl1);
            resources.ApplyResources(this.tabPage5, "tabPage5");
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.playbackUControl1);
            resources.ApplyResources(this.tabPage6, "tabPage6");
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.auxUserControl1);
            resources.ApplyResources(this.tabPage7, "tabPage7");
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.viewCreateUserControl21);
            resources.ApplyResources(this.tabPage9, "tabPage9");
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.viewCommandUserControl1);
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.viewEditUserControl21);
            resources.ApplyResources(this.tabPage10, "tabPage10");
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.viewDumpUserControl1);
            resources.ApplyResources(this.tabPage11, "tabPage11");
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.workspaceUserControl1);
            resources.ApplyResources(this.tabPage12, "tabPage12");
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // multiWindowUserControl1
            // 
            this.multiWindowUserControl1.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.multiWindowUserControl1, "multiWindowUserControl1");
            this.multiWindowUserControl1.Name = "multiWindowUserControl1";
            this.multiWindowUserControl1.Click += new System.EventHandler(this.OnClick);
            // 
            // ptzUserControl1
            // 
            resources.ApplyResources(this.ptzUserControl1, "ptzUserControl1");
            this.ptzUserControl1.Name = "ptzUserControl1";
            // 
            // ptzAbsolutUserControl1
            // 
            resources.ApplyResources(this.ptzAbsolutUserControl1, "ptzAbsolutUserControl1");
            this.ptzAbsolutUserControl1.Name = "ptzAbsolutUserControl1";
            // 
            // indicatorAndAppUserControl1
            // 
            resources.ApplyResources(this.indicatorAndAppUserControl1, "indicatorAndAppUserControl1");
            this.indicatorAndAppUserControl1.Name = "indicatorAndAppUserControl1";
            // 
            // lensUserControl1
            // 
            resources.ApplyResources(this.lensUserControl1, "lensUserControl1");
            this.lensUserControl1.Name = "lensUserControl1";
            // 
            // playbackUControl1
            // 
            resources.ApplyResources(this.playbackUControl1, "playbackUControl1");
            this.playbackUControl1.Name = "playbackUControl1";
            // 
            // auxUserControl1
            // 
            resources.ApplyResources(this.auxUserControl1, "auxUserControl1");
            this.auxUserControl1.Name = "auxUserControl1";
            // 
            // viewCreateUserControl21
            // 
            resources.ApplyResources(this.viewCreateUserControl21, "viewCreateUserControl21");
            this.viewCreateUserControl21.Name = "viewCreateUserControl21";
            // 
            // viewCommandUserControl1
            // 
            resources.ApplyResources(this.viewCommandUserControl1, "viewCommandUserControl1");
            this.viewCommandUserControl1.Name = "viewCommandUserControl1";
            // 
            // viewEditUserControl21
            // 
            resources.ApplyResources(this.viewEditUserControl21, "viewEditUserControl21");
            this.viewEditUserControl21.Name = "viewEditUserControl21";
            // 
            // viewDumpUserControl1
            // 
            resources.ApplyResources(this.viewDumpUserControl1, "viewDumpUserControl1");
            this.viewDumpUserControl1.Name = "viewDumpUserControl1";
            // 
            // workspaceUserControl1
            // 
            resources.ApplyResources(this.workspaceUserControl1, "workspaceUserControl1");
            this.workspaceUserControl1.Name = "workspaceUserControl1";
            // 
            // SCViewAndWindowViewItemUserControl
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelHeader);
            this.Name = "SCViewAndWindowViewItemUserControl";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.OnLoad);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControl_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewItemUserControl_MouseDoubleClick);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage11.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		internal ContextMenuStrip contextMenuStripcopy;
		internal ToolStripMenuItem copyToolStripMenuItem;
		private Label label1;
		private Label labelName;
		private Panel panelHeader;
		private TabControl tabControl;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private MultiWindowUserControl multiWindowUserControl1;
		private PTZUserControl ptzUserControl1;
		private TabPage tabPage3;
		private IndicatorAndAppUserControl indicatorAndAppUserControl1;
		private TabPage tabPage4;
		private ViewCommandUserControl viewCommandUserControl1;
		private TabPage tabPage5;
		private LensUserControl lensUserControl1;
		private TabPage tabPage6;
		private PlaybackUControl playbackUControl1;
		private TabPage tabPage7;
		private AUXUserControl auxUserControl1;
		private TabPage tabPage8;
		private PTZAbsolutUserControl ptzAbsolutUserControl1;
		private TabPage tabPage9;
		private TabPage tabPage10;
		private ViewCreateUserControl viewCreateUserControl21;
		private ViewEditUserControl viewEditUserControl21;
		private TabPage tabPage11;
		private ViewDumpUserControl viewDumpUserControl1;
        private TabPage tabPage12;
        private WorkspaceUserControl workspaceUserControl1;
    }
}
