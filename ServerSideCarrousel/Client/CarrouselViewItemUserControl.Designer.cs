using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ServerSideCarrousel.Client
{
	partial class CarrouselViewItemUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CarrouselViewItemUserControl));
            this._imageViewerControlPanel = new System.Windows.Forms.Panel();
            this._toolbarPanel = new System.Windows.Forms.Panel();
            this._nextButton = new System.Windows.Forms.Button();
            this._pauseCheckBox = new System.Windows.Forms.CheckBox();
            this._previousButton = new System.Windows.Forms.Button();
            this._hideToolPanelTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolbarPanel.SuspendLayout();
            this.contextMenuStripcopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // _imageViewerControlPanel
            // 
            resources.ApplyResources(this._imageViewerControlPanel, "_imageViewerControlPanel");
            this._imageViewerControlPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this._imageViewerControlPanel.Name = "_imageViewerControlPanel";
            this._imageViewerControlPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CarouselViewItemUserControl_MouseMove);
            // 
            // _toolbarPanel
            // 
            this._toolbarPanel.BackColor = System.Drawing.SystemColors.Control;
            this._toolbarPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._toolbarPanel.Controls.Add(this._nextButton);
            this._toolbarPanel.Controls.Add(this._pauseCheckBox);
            this._toolbarPanel.Controls.Add(this._previousButton);
            resources.ApplyResources(this._toolbarPanel, "_toolbarPanel");
            this._toolbarPanel.Name = "_toolbarPanel";
            this._toolbarPanel.MouseEnter += new System.EventHandler(this._toolbarPanel_MouseEnter);
            this._toolbarPanel.MouseLeave += new System.EventHandler(this._toolbarPanel_MouseLeave);
            // 
            // _nextButton
            // 
            resources.ApplyResources(this._nextButton, "_nextButton");
            this._nextButton.Name = "_nextButton";
            this._nextButton.UseVisualStyleBackColor = true;
            this._nextButton.Click += new System.EventHandler(this.CarouselViewItemUserControl_Next);
            this._nextButton.MouseEnter += new System.EventHandler(this._toolbarPanel_MouseEnter);
            this._nextButton.MouseLeave += new System.EventHandler(this._toolbarPanel_MouseLeave);
            // 
            // _pauseCheckBox
            // 
            resources.ApplyResources(this._pauseCheckBox, "_pauseCheckBox");
            this._pauseCheckBox.Name = "_pauseCheckBox";
            this._pauseCheckBox.UseVisualStyleBackColor = true;
            this._pauseCheckBox.CheckedChanged += new System.EventHandler(this.PauseCheckBox_CheckedChanged);
            this._pauseCheckBox.MouseEnter += new System.EventHandler(this._toolbarPanel_MouseEnter);
            this._pauseCheckBox.MouseLeave += new System.EventHandler(this._toolbarPanel_MouseLeave);
            // 
            // _previousButton
            // 
            resources.ApplyResources(this._previousButton, "_previousButton");
            this._previousButton.Name = "_previousButton";
            this._previousButton.UseVisualStyleBackColor = true;
            this._previousButton.Click += new System.EventHandler(this.CarouselViewItemUserControl_Previous);
            this._previousButton.MouseEnter += new System.EventHandler(this._toolbarPanel_MouseEnter);
            this._previousButton.MouseLeave += new System.EventHandler(this._toolbarPanel_MouseLeave);
            // 
            // _hideToolPanelTimer
            // 
            this._hideToolPanelTimer.Interval = 2000;
            this._hideToolPanelTimer.Tick += new System.EventHandler(this._hideToolPanelTimer_Tick);
            // 
            // contextMenuStripcopy
            // 
            this.contextMenuStripcopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStripcopy.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStripcopy, "contextMenuStripcopy");
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            // 
            // CarrouselViewItemUserControl
            // 
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.Controls.Add(this._toolbarPanel);
            this.Controls.Add(this._imageViewerControlPanel);
            this.Name = "CarrouselViewItemUserControl";
            resources.ApplyResources(this, "$this");
            this.MouseLeave += new System.EventHandler(this.MouseLeaveHandler);
            this.MouseHover += new System.EventHandler(this.MouseHoverHandler);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CarouselViewItemUserControl_MouseMove);
            this._toolbarPanel.ResumeLayout(false);
            this.contextMenuStripcopy.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		internal ContextMenuStrip contextMenuStripcopy;
		internal ToolStripMenuItem copyToolStripMenuItem;

	}
}
