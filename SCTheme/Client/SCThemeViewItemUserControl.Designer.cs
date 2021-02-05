namespace SCTheme.Client
{
    partial class SCThemeViewItemUserControl
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
            this.label2 = new System.Windows.Forms.Label();
            this._selectThemeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this._selectedThemeLabel = new System.Windows.Forms.Label();
            this._windowsFormControlsPanel = new System.Windows.Forms.Panel();
            this.windowsFormsUserControl1 = new SCTheme.Client.WindowsFormsUserControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.wpfUserControl1 = new SCTheme.Client.WpfUserControl();
            this._windowsFormControlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select ThemeType:";
            this.label2.Click += new System.EventHandler(this.On_Click);
            this.label2.DoubleClick += new System.EventHandler(this.On_DoubleClick);
            // 
            // _selectThemeComboBox
            // 
            this._selectThemeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._selectThemeComboBox.FormattingEnabled = true;
            this._selectThemeComboBox.Location = new System.Drawing.Point(130, 39);
            this._selectThemeComboBox.Name = "_selectThemeComboBox";
            this._selectThemeComboBox.Size = new System.Drawing.Size(116, 21);
            this._selectThemeComboBox.TabIndex = 3;
            this._selectThemeComboBox.SelectedIndexChanged += new System.EventHandler(this._selectThemeComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(14, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Current ThemeType:";
            this.label3.Click += new System.EventHandler(this.On_Click);
            this.label3.DoubleClick += new System.EventHandler(this.On_DoubleClick);
            // 
            // _selectedThemeLabel
            // 
            this._selectedThemeLabel.AutoSize = true;
            this._selectedThemeLabel.Location = new System.Drawing.Point(127, 14);
            this._selectedThemeLabel.Name = "_selectedThemeLabel";
            this._selectedThemeLabel.Size = new System.Drawing.Size(0, 13);
            this._selectedThemeLabel.TabIndex = 6;
            // 
            // _windowsFormControlsPanel
            // 
            this._windowsFormControlsPanel.Controls.Add(this.windowsFormsUserControl1);
            this._windowsFormControlsPanel.Location = new System.Drawing.Point(17, 93);
            this._windowsFormControlsPanel.Name = "_windowsFormControlsPanel";
            this._windowsFormControlsPanel.Size = new System.Drawing.Size(172, 217);
            this._windowsFormControlsPanel.TabIndex = 7;
            this._windowsFormControlsPanel.Click += new System.EventHandler(this.On_Click);
            this._windowsFormControlsPanel.DoubleClick += new System.EventHandler(this.On_DoubleClick);
            // 
            // windowsFormsUserControl1
            // 
            this.windowsFormsUserControl1.Location = new System.Drawing.Point(4, 4);
            this.windowsFormsUserControl1.Name = "windowsFormsUserControl1";
            this.windowsFormsUserControl1.Size = new System.Drawing.Size(150, 150);
            this.windowsFormsUserControl1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(18, 326);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Area that is not themed.";
            this.label1.Click += new System.EventHandler(this.On_Click);
            this.label1.DoubleClick += new System.EventHandler(this.On_DoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Windows.Form UserControls";
            this.label4.Click += new System.EventHandler(this.On_Click);
            this.label4.DoubleClick += new System.EventHandler(this.On_DoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(222, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "WPF Elements";
            this.label5.Click += new System.EventHandler(this.On_Click);
            this.label5.DoubleClick += new System.EventHandler(this.On_DoubleClick);
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(210, 93);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(169, 217);
            this.elementHost1.TabIndex = 8;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.wpfUserControl1;
            // 
            // SCThemeViewItemUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSeaGreen;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this._windowsFormControlsPanel);
            this.Controls.Add(this._selectedThemeLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._selectThemeComboBox);
            this.Controls.Add(this.label2);
            this.Name = "SCThemeViewItemUserControl";
            this.Size = new System.Drawing.Size(797, 597);
            this.Click += new System.EventHandler(this.On_Click);
            this.DoubleClick += new System.EventHandler(this.On_DoubleClick);
            this._windowsFormControlsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _selectThemeComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label _selectedThemeLabel;
        private System.Windows.Forms.Panel _windowsFormControlsPanel;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private WpfUserControl wpfUserControl1;
        private WindowsFormsUserControl windowsFormsUserControl1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
    }
}
