using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace LocationView.Client
{
	partial class LocationViewViewItemUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationViewViewItemUserControl));
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMain = new System.Windows.Forms.Panel();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.panelConfiguration = new System.Windows.Forms.Panel();
            this.labelTimeout = new System.Windows.Forms.Label();
            this.numericUpDownTimeout = new System.Windows.Forms.NumericUpDown();
            this.groupBoxToolTip = new System.Windows.Forms.GroupBox();
            this.labelToolTipText = new System.Windows.Forms.Label();
            this.labelToolTipAppearance = new System.Windows.Forms.Label();
            this.comboBoxToolTipText = new System.Windows.Forms.ComboBox();
            this.comboBoxToolTipAppearance = new System.Windows.Forms.ComboBox();
            this.labelMapType = new System.Windows.Forms.Label();
            this.groupBoxMarkers = new System.Windows.Forms.GroupBox();
            this.buttonRemoveMarker = new System.Windows.Forms.Button();
            this.buttonEditMarker = new System.Windows.Forms.Button();
            this.buttonAddMarker = new System.Windows.Forms.Button();
            this.dataGridViewMarkers = new System.Windows.Forms.DataGridView();
            this.deiceNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.markerTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSourceMarkers = new System.Windows.Forms.BindingSource(this.components);
            this.checkBoxZoomPanel = new System.Windows.Forms.CheckBox();
            this.comboBoxMapType = new System.Windows.Forms.ComboBox();
            this.trackBarZoom = new System.Windows.Forms.TrackBar();
            this.panelMain.SuspendLayout();
            this.panelConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).BeginInit();
            this.groupBoxToolTip.SuspendLayout();
            this.groupBoxMarkers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarkers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMarkers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            // 
            // panelMain
            // 
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Controls.Add(this.buttonCenter);
            this.panelMain.Controls.Add(this.panelConfiguration);
            this.panelMain.Controls.Add(this.trackBarZoom);
            this.panelMain.Name = "panelMain";
            // 
            // buttonCenter
            // 
            resources.ApplyResources(this.buttonCenter, "buttonCenter");
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.ButtonCenterClick);
            // 
            // panelConfiguration
            // 
            resources.ApplyResources(this.panelConfiguration, "panelConfiguration");
            this.panelConfiguration.Controls.Add(this.labelTimeout);
            this.panelConfiguration.Controls.Add(this.numericUpDownTimeout);
            this.panelConfiguration.Controls.Add(this.groupBoxToolTip);
            this.panelConfiguration.Controls.Add(this.labelMapType);
            this.panelConfiguration.Controls.Add(this.groupBoxMarkers);
            this.panelConfiguration.Controls.Add(this.checkBoxZoomPanel);
            this.panelConfiguration.Controls.Add(this.comboBoxMapType);
            this.panelConfiguration.Name = "panelConfiguration";
            // 
            // labelTimeout
            // 
            resources.ApplyResources(this.labelTimeout, "labelTimeout");
            this.labelTimeout.Name = "labelTimeout";
            // 
            // numericUpDownTimeout
            // 
            resources.ApplyResources(this.numericUpDownTimeout, "numericUpDownTimeout");
            this.numericUpDownTimeout.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownTimeout.Name = "numericUpDownTimeout";
            this.numericUpDownTimeout.ValueChanged += new System.EventHandler(this.NumericUpDownTimeoutValueChanged);
            // 
            // groupBoxToolTip
            // 
            resources.ApplyResources(this.groupBoxToolTip, "groupBoxToolTip");
            this.groupBoxToolTip.Controls.Add(this.labelToolTipText);
            this.groupBoxToolTip.Controls.Add(this.labelToolTipAppearance);
            this.groupBoxToolTip.Controls.Add(this.comboBoxToolTipText);
            this.groupBoxToolTip.Controls.Add(this.comboBoxToolTipAppearance);
            this.groupBoxToolTip.Name = "groupBoxToolTip";
            this.groupBoxToolTip.TabStop = false;
            // 
            // labelToolTipText
            // 
            resources.ApplyResources(this.labelToolTipText, "labelToolTipText");
            this.labelToolTipText.Name = "labelToolTipText";
            // 
            // labelToolTipAppearance
            // 
            resources.ApplyResources(this.labelToolTipAppearance, "labelToolTipAppearance");
            this.labelToolTipAppearance.Name = "labelToolTipAppearance";
            // 
            // comboBoxToolTipText
            // 
            resources.ApplyResources(this.comboBoxToolTipText, "comboBoxToolTipText");
            this.comboBoxToolTipText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxToolTipText.FormattingEnabled = true;
            this.comboBoxToolTipText.Name = "comboBoxToolTipText";
            this.comboBoxToolTipText.SelectedIndexChanged += new System.EventHandler(this.ComboBoxToolTipTextSelectedIndexChanged);
            // 
            // comboBoxToolTipAppearance
            // 
            this.comboBoxToolTipAppearance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxToolTipAppearance.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxToolTipAppearance, "comboBoxToolTipAppearance");
            this.comboBoxToolTipAppearance.Name = "comboBoxToolTipAppearance";
            this.comboBoxToolTipAppearance.SelectedIndexChanged += new System.EventHandler(this.ComboBoxToolTipAppearanceSelectedIndexChanged);
            // 
            // labelMapType
            // 
            resources.ApplyResources(this.labelMapType, "labelMapType");
            this.labelMapType.Name = "labelMapType";
            // 
            // groupBoxMarkers
            // 
            resources.ApplyResources(this.groupBoxMarkers, "groupBoxMarkers");
            this.groupBoxMarkers.Controls.Add(this.buttonRemoveMarker);
            this.groupBoxMarkers.Controls.Add(this.buttonEditMarker);
            this.groupBoxMarkers.Controls.Add(this.buttonAddMarker);
            this.groupBoxMarkers.Controls.Add(this.dataGridViewMarkers);
            this.groupBoxMarkers.Name = "groupBoxMarkers";
            this.groupBoxMarkers.TabStop = false;
            // 
            // buttonRemoveMarker
            // 
            resources.ApplyResources(this.buttonRemoveMarker, "buttonRemoveMarker");
            this.buttonRemoveMarker.Name = "buttonRemoveMarker";
            this.buttonRemoveMarker.UseVisualStyleBackColor = true;
            this.buttonRemoveMarker.Click += new System.EventHandler(this.ButtonRemoveMarkerClick);
            // 
            // buttonEditMarker
            // 
            resources.ApplyResources(this.buttonEditMarker, "buttonEditMarker");
            this.buttonEditMarker.Name = "buttonEditMarker";
            this.buttonEditMarker.UseVisualStyleBackColor = true;
            this.buttonEditMarker.Click += new System.EventHandler(this.ButtonEditMarkerClick);
            // 
            // buttonAddMarker
            // 
            resources.ApplyResources(this.buttonAddMarker, "buttonAddMarker");
            this.buttonAddMarker.Name = "buttonAddMarker";
            this.buttonAddMarker.UseVisualStyleBackColor = true;
            this.buttonAddMarker.Click += new System.EventHandler(this.ButtonAddMarkerClick);
            // 
            // dataGridViewMarkers
            // 
            resources.ApplyResources(this.dataGridViewMarkers, "dataGridViewMarkers");
            this.dataGridViewMarkers.AutoGenerateColumns = false;
            this.dataGridViewMarkers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMarkers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deiceNameDataGridViewTextBoxColumn,
            this.markerTypeDataGridViewTextBoxColumn});
            this.dataGridViewMarkers.DataSource = this.bindingSourceMarkers;
            this.dataGridViewMarkers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewMarkers.Name = "dataGridViewMarkers";
            this.dataGridViewMarkers.RowTemplate.Height = 23;
            this.dataGridViewMarkers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMarkers.SelectionChanged += new System.EventHandler(this.DataGridViewMarkersSelectionChanged);
            // 
            // deiceNameDataGridViewTextBoxColumn
            // 
            this.deiceNameDataGridViewTextBoxColumn.DataPropertyName = "DeviceName";
            resources.ApplyResources(this.deiceNameDataGridViewTextBoxColumn, "deiceNameDataGridViewTextBoxColumn");
            this.deiceNameDataGridViewTextBoxColumn.Name = "deiceNameDataGridViewTextBoxColumn";
            // 
            // markerTypeDataGridViewTextBoxColumn
            // 
            this.markerTypeDataGridViewTextBoxColumn.DataPropertyName = "MarkerType";
            resources.ApplyResources(this.markerTypeDataGridViewTextBoxColumn, "markerTypeDataGridViewTextBoxColumn");
            this.markerTypeDataGridViewTextBoxColumn.Name = "markerTypeDataGridViewTextBoxColumn";
            // 
            // bindingSourceMarkers
            // 
            this.bindingSourceMarkers.DataSource = typeof(LocationView.Client.Config.Marker);
            // 
            // checkBoxZoomPanel
            // 
            resources.ApplyResources(this.checkBoxZoomPanel, "checkBoxZoomPanel");
            this.checkBoxZoomPanel.Name = "checkBoxZoomPanel";
            this.checkBoxZoomPanel.UseVisualStyleBackColor = true;
            this.checkBoxZoomPanel.CheckedChanged += new System.EventHandler(this.CheckBoxZoomPanelCheckedChanged);
            // 
            // comboBoxMapType
            // 
            resources.ApplyResources(this.comboBoxMapType, "comboBoxMapType");
            this.comboBoxMapType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMapType.FormattingEnabled = true;
            this.comboBoxMapType.Name = "comboBoxMapType";
            this.comboBoxMapType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMapTypeSelectedIndexChanged);
            // 
            // trackBarZoom
            // 
            resources.ApplyResources(this.trackBarZoom, "trackBarZoom");
            this.trackBarZoom.Maximum = 20;
            this.trackBarZoom.Minimum = 1;
            this.trackBarZoom.Name = "trackBarZoom";
            this.trackBarZoom.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarZoom.Value = 17;
            this.trackBarZoom.Scroll += new System.EventHandler(this.TrackBarZoomScroll);
            // 
            // LocationViewViewItemUserControl
            // 
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.Controls.Add(this.panelMain);
            this.Name = "LocationViewViewItemUserControl";
            resources.ApplyResources(this, "$this");
            this.Load += new System.EventHandler(this.OnLoad);
            this.Click += new System.EventHandler(this.OnClick);
            this.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelConfiguration.ResumeLayout(false);
            this.panelConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimeout)).EndInit();
            this.groupBoxToolTip.ResumeLayout(false);
            this.groupBoxToolTip.PerformLayout();
            this.groupBoxMarkers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarkers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMarkers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        internal ToolStripMenuItem copyToolStripMenuItem;
        private Panel panelMain;
        private TrackBar trackBarZoom;
        private ComboBox comboBoxMapType;
        private CheckBox checkBoxZoomPanel;
        private Panel panelConfiguration;
        private Button buttonCenter;
        private GroupBox groupBoxMarkers;
        private DataGridView dataGridViewMarkers;
        private BindingSource bindingSourceMarkers;
        private GroupBox groupBoxToolTip;
        private Label labelToolTipText;
        private Label labelToolTipAppearance;
        private ComboBox comboBoxToolTipText;
        private ComboBox comboBoxToolTipAppearance;
        private Label labelMapType;
        private Button buttonRemoveMarker;
        private Button buttonEditMarker;
        private Button buttonAddMarker;
        private DataGridViewTextBoxColumn deiceNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn markerTypeDataGridViewTextBoxColumn;
        private Label labelTimeout;
        private NumericUpDown numericUpDownTimeout;
	}
}
