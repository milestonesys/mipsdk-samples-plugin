using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace VideoPreview.Client
{
	partial class VideoPreviewViewItemUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoPreviewViewItemUserControl));
            this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.button10 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.button11 = new System.Windows.Forms.Button();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBoxOverlayKeepAspect = new System.Windows.Forms.CheckBox();
            this.checkBoxScaleOverlay = new System.Windows.Forms.CheckBox();
            this.comboBoxVertical = new System.Windows.Forms.ComboBox();
            this.comboBoxHorizontal = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.hScrollBarSmoothBuffer = new System.Windows.Forms.HScrollBar();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLineInfo = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBoxOverlay = new System.Windows.Forms.GroupBox();
            this.comboBoxStreams = new System.Windows.Forms.ComboBox();
            this.checkBoxAspectRatio = new System.Windows.Forms.CheckBox();
            this.checkBoxHeader = new System.Windows.Forms.CheckBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.checkBoxLiveIndicator = new System.Windows.Forms.CheckBox();
            this.button12 = new System.Windows.Forms.Button();
            this.textBoxDisplayTime = new System.Windows.Forms.TextBox();
            this.checkBoxDigitalPtz = new System.Windows.Forms.CheckBox();
            this.numericUpDownScaleFactor = new System.Windows.Forms.NumericUpDown();
            this.panelHeader.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBoxOverlay.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScaleFactor)).BeginInit();
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
            this.panelHeader.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.labelName);
            this.panelHeader.Controls.Add(this.button7);
            this.panelHeader.Controls.Add(this.button8);
            this.panelHeader.Controls.Add(this.label5);
            this.panelHeader.Controls.Add(this.hScrollBar1);
            this.panelHeader.Controls.Add(this.button10);
            this.panelHeader.Controls.Add(this.label4);
            this.panelHeader.Controls.Add(this.checkBox2);
            this.panelHeader.Controls.Add(this.textBox1);
            this.panelHeader.Controls.Add(this.checkBox3);
            this.panelHeader.Controls.Add(this.button11);
            this.panelHeader.Controls.Add(this.checkBox4);
            this.panelHeader.Controls.Add(this.comboBox2);
            this.panelHeader.Controls.Add(this.comboBox1);
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Click += new System.EventHandler(this.OnClick);
            this.panelHeader.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.panelHeader.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnClick);
            // 
            // button7
            // 
            resources.ApplyResources(this.button7, "button7");
            this.button7.Name = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.OnSelectCamera);
            // 
            // button8
            // 
            resources.ApplyResources(this.button8, "button8");
            this.button8.Name = "button8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.OnMakeSquare);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // hScrollBar1
            // 
            resources.ApplyResources(this.hScrollBar1, "hScrollBar1");
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.SmallChange = 10;
            this.hScrollBar1.Value = 100;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnBufferChange);
            // 
            // button10
            // 
            resources.ApplyResources(this.button10, "button10");
            this.button10.Name = "button10";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.OnClear);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            resources.ApplyResources(this.button11, "button11");
            this.button11.Name = "button11";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.OnSelectPosition);
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // comboBox2
            // 
            resources.ApplyResources(this.comboBox2, "comboBox2");
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            resources.GetString("comboBox2.Items"),
            resources.GetString("comboBox2.Items1"),
            resources.GetString("comboBox2.Items2")});
            this.comboBox2.Name = "comboBox2";
            // 
            // comboBox1
            // 
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            resources.GetString("comboBox1.Items"),
            resources.GetString("comboBox1.Items1"),
            resources.GetString("comboBox1.Items2")});
            this.comboBox1.Name = "comboBox1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Name = "panel2";
            this.panel2.Click += new System.EventHandler(this.OnImageClick);
            this.panel2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnClick);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnSelectCamera);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnMakeSquare);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.OnClear);
            // 
            // checkBoxOverlayKeepAspect
            // 
            resources.ApplyResources(this.checkBoxOverlayKeepAspect, "checkBoxOverlayKeepAspect");
            this.checkBoxOverlayKeepAspect.Name = "checkBoxOverlayKeepAspect";
            this.checkBoxOverlayKeepAspect.UseVisualStyleBackColor = true;
            // 
            // checkBoxScaleOverlay
            // 
            resources.ApplyResources(this.checkBoxScaleOverlay, "checkBoxScaleOverlay");
            this.checkBoxScaleOverlay.Checked = true;
            this.checkBoxScaleOverlay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxScaleOverlay.Name = "checkBoxScaleOverlay";
            this.checkBoxScaleOverlay.UseVisualStyleBackColor = true;
            // 
            // comboBoxVertical
            // 
            this.comboBoxVertical.FormattingEnabled = true;
            this.comboBoxVertical.Items.AddRange(new object[] {
            resources.GetString("comboBoxVertical.Items"),
            resources.GetString("comboBoxVertical.Items1"),
            resources.GetString("comboBoxVertical.Items2")});
            resources.ApplyResources(this.comboBoxVertical, "comboBoxVertical");
            this.comboBoxVertical.Name = "comboBoxVertical";
            // 
            // comboBoxHorizontal
            // 
            this.comboBoxHorizontal.FormattingEnabled = true;
            this.comboBoxHorizontal.Items.AddRange(new object[] {
            resources.GetString("comboBoxHorizontal.Items"),
            resources.GetString("comboBoxHorizontal.Items1"),
            resources.GetString("comboBoxHorizontal.Items2")});
            resources.ApplyResources(this.comboBoxHorizontal, "comboBoxHorizontal");
            this.comboBoxHorizontal.Name = "comboBoxHorizontal";
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.OnSelectPosition);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // hScrollBarSmoothBuffer
            // 
            resources.ApplyResources(this.hScrollBarSmoothBuffer, "hScrollBarSmoothBuffer");
            this.hScrollBarSmoothBuffer.Name = "hScrollBarSmoothBuffer";
            this.hScrollBarSmoothBuffer.SmallChange = 10;
            this.hScrollBarSmoothBuffer.Value = 100;
            this.hScrollBarSmoothBuffer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnBufferChange);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBoxLineInfo
            // 
            resources.ApplyResources(this.textBoxLineInfo, "textBoxLineInfo");
            this.textBoxLineInfo.Name = "textBoxLineInfo";
            this.textBoxLineInfo.ReadOnly = true;
            this.textBoxLineInfo.TabStop = false;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.OnLiveTickChanged);
            // 
            // button6
            // 
            resources.ApplyResources(this.button6, "button6");
            this.button6.Name = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.OnShowBitmap);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.groupBoxOverlay);
            this.panel3.Controls.Add(this.comboBoxStreams);
            this.panel3.Controls.Add(this.checkBoxAspectRatio);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.hScrollBarSmoothBuffer);
            this.panel3.Name = "panel3";
            this.panel3.Click += new System.EventHandler(this.OnClick);
            this.panel3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnClick);
            this.panel3.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnDoubleClick);
            // 
            // groupBoxOverlay
            // 
            resources.ApplyResources(this.groupBoxOverlay, "groupBoxOverlay");
            this.groupBoxOverlay.Controls.Add(this.numericUpDownScaleFactor);
            this.groupBoxOverlay.Controls.Add(this.checkBoxOverlayKeepAspect);
            this.groupBoxOverlay.Controls.Add(this.label2);
            this.groupBoxOverlay.Controls.Add(this.checkBoxScaleOverlay);
            this.groupBoxOverlay.Controls.Add(this.button2);
            this.groupBoxOverlay.Controls.Add(this.comboBoxVertical);
            this.groupBoxOverlay.Controls.Add(this.button5);
            this.groupBoxOverlay.Controls.Add(this.comboBoxHorizontal);
            this.groupBoxOverlay.Controls.Add(this.button4);
            this.groupBoxOverlay.Name = "groupBoxOverlay";
            this.groupBoxOverlay.TabStop = false;
            // 
            // comboBoxStreams
            // 
            this.comboBoxStreams.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxStreams, "comboBoxStreams");
            this.comboBoxStreams.Name = "comboBoxStreams";
            this.comboBoxStreams.SelectedIndexChanged += new System.EventHandler(this.comboBoxStreams_SelectedIndexChanged);
            // 
            // checkBoxAspectRatio
            // 
            resources.ApplyResources(this.checkBoxAspectRatio, "checkBoxAspectRatio");
            this.checkBoxAspectRatio.Checked = true;
            this.checkBoxAspectRatio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAspectRatio.Name = "checkBoxAspectRatio";
            this.checkBoxAspectRatio.UseVisualStyleBackColor = true;
            this.checkBoxAspectRatio.CheckedChanged += new System.EventHandler(this.OnAspectChange);
            // 
            // checkBoxHeader
            // 
            resources.ApplyResources(this.checkBoxHeader, "checkBoxHeader");
            this.checkBoxHeader.Checked = true;
            this.checkBoxHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHeader.Name = "checkBoxHeader";
            this.checkBoxHeader.UseVisualStyleBackColor = true;
            this.checkBoxHeader.CheckedChanged += new System.EventHandler(this.OnShowHeaderChanged);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.checkBoxLiveIndicator);
            this.panelMain.Controls.Add(this.button12);
            this.panelMain.Controls.Add(this.textBoxDisplayTime);
            this.panelMain.Controls.Add(this.checkBoxDigitalPtz);
            this.panelMain.Controls.Add(this.panel2);
            this.panelMain.Controls.Add(this.textBoxLineInfo);
            this.panelMain.Controls.Add(this.checkBoxHeader);
            this.panelMain.Controls.Add(this.checkBox1);
            this.panelMain.Controls.Add(this.panel3);
            this.panelMain.Controls.Add(this.button6);
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            this.panelMain.Click += new System.EventHandler(this.OnClick);
            this.panelMain.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // checkBoxLiveIndicator
            // 
            resources.ApplyResources(this.checkBoxLiveIndicator, "checkBoxLiveIndicator");
            this.checkBoxLiveIndicator.Checked = true;
            this.checkBoxLiveIndicator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLiveIndicator.Name = "checkBoxLiveIndicator";
            this.checkBoxLiveIndicator.UseVisualStyleBackColor = true;
            this.checkBoxLiveIndicator.CheckedChanged += new System.EventHandler(this.checkBoxLiveIndicator_CheckedChanged);
            // 
            // button12
            // 
            resources.ApplyResources(this.button12, "button12");
            this.button12.Name = "button12";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // textBoxDisplayTime
            // 
            resources.ApplyResources(this.textBoxDisplayTime, "textBoxDisplayTime");
            this.textBoxDisplayTime.Name = "textBoxDisplayTime";
            this.textBoxDisplayTime.ReadOnly = true;
            this.textBoxDisplayTime.TabStop = false;
            // 
            // checkBoxDigitalPtz
            // 
            resources.ApplyResources(this.checkBoxDigitalPtz, "checkBoxDigitalPtz");
            this.checkBoxDigitalPtz.Name = "checkBoxDigitalPtz";
            this.checkBoxDigitalPtz.UseVisualStyleBackColor = true;
            this.checkBoxDigitalPtz.CheckedChanged += new System.EventHandler(this.OnDigitalZoomChanged);
            // 
            // numericUpDownScaleFactor
            // 
            this.numericUpDownScaleFactor.DecimalPlaces = 1;
            this.numericUpDownScaleFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            resources.ApplyResources(this.numericUpDownScaleFactor, "numericUpDownScaleFactor");
            this.numericUpDownScaleFactor.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownScaleFactor.Name = "numericUpDownScaleFactor";
            this.numericUpDownScaleFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // VideoPreviewViewItemUserControl
            // 
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.Name = "VideoPreviewViewItemUserControl";
            resources.ApplyResources(this, "$this");
            this.Click += new System.EventHandler(this.OnClick);
            this.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnDoubleClick);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBoxOverlay.ResumeLayout(false);
            this.groupBoxOverlay.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownScaleFactor)).EndInit();
            this.ResumeLayout(false);

		}

        #endregion

        internal ContextMenuStrip contextMenuStripcopy;
		internal ToolStripMenuItem copyToolStripMenuItem;
		private Label label1;
		private Label labelName;
		private Panel panelHeader;
		private Panel panel2;
		private Button button1;
		private Button button2;
		private Button button4;
		private CheckBox checkBoxOverlayKeepAspect;
		private CheckBox checkBoxScaleOverlay;
		private ComboBox comboBoxVertical;
		private ComboBox comboBoxHorizontal;
		private Button button5;
		private Label label2;
		private HScrollBar hScrollBarSmoothBuffer;
		private Label label3;
		private TextBox textBoxLineInfo;
		private CheckBox checkBox1;
		private Button button6;
		private Button button7;
		private Button button8;
		private Label label5;
		private HScrollBar hScrollBar1;
		private Button button10;
		private Label label4;
		private CheckBox checkBox2;
		private TextBox textBox1;
		private CheckBox checkBox3;
		private Button button11;
		private CheckBox checkBox4;
		private ComboBox comboBox2;
		private ComboBox comboBox1;
		private Panel panel3;
		private CheckBox checkBoxHeader;
		private CheckBox checkBoxAspectRatio;
		private Panel panelMain;
        private CheckBox checkBoxDigitalPtz;
        private TextBox textBoxDisplayTime;
        private Button button12;
        private CheckBox checkBoxLiveIndicator;
        private ComboBox comboBoxStreams;
        private GroupBox groupBoxOverlay;
        private NumericUpDown numericUpDownScaleFactor;
    }
}
