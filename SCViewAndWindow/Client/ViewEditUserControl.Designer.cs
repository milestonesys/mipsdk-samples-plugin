namespace SCViewAndWindow.Client
{
	partial class ViewEditUserControl
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
            this.groupBoxView = new System.Windows.Forms.GroupBox();
            this.textBoxLayout = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxCount = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSelectedGroup = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxNavigationBar = new System.Windows.Forms.CheckBox();
            this.checkBoxAddScript = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSelectCamera = new System.Windows.Forms.Button();
            this.comboBoxViewItemType = new System.Windows.Forms.ComboBox();
            this.comboBoxViewItemIx = new System.Windows.Forms.ComboBox();
            this.buttonSelectMap = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBoxView.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxView
            // 
            this.groupBoxView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxView.Controls.Add(this.textBoxLayout);
            this.groupBoxView.Controls.Add(this.label4);
            this.groupBoxView.Controls.Add(this.textBoxCount);
            this.groupBoxView.Controls.Add(this.button3);
            this.groupBoxView.Controls.Add(this.label3);
            this.groupBoxView.Controls.Add(this.button4);
            this.groupBoxView.Enabled = false;
            this.groupBoxView.Location = new System.Drawing.Point(3, 104);
            this.groupBoxView.Name = "groupBoxView";
            this.groupBoxView.Size = new System.Drawing.Size(507, 98);
            this.groupBoxView.TabIndex = 1;
            this.groupBoxView.TabStop = false;
            this.groupBoxView.Text = "Select and Delete ViewAndLayout";
            // 
            // textBoxLayout
            // 
            this.textBoxLayout.Location = new System.Drawing.Point(106, 19);
            this.textBoxLayout.Name = "textBoxLayout";
            this.textBoxLayout.ReadOnly = true;
            this.textBoxLayout.Size = new System.Drawing.Size(111, 20);
            this.textBoxLayout.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Select View:";
            // 
            // textBoxCount
            // 
            this.textBoxCount.Location = new System.Drawing.Point(106, 48);
            this.textBoxCount.Name = "textBoxCount";
            this.textBoxCount.ReadOnly = true;
            this.textBoxCount.Size = new System.Drawing.Size(49, 20);
            this.textBoxCount.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(229, 45);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(167, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Delete View";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnDeleteView);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Count";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(229, 16);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(167, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = "Select View";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.OnSelectView);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Group:";
            // 
            // textBoxSelectedGroup
            // 
            this.textBoxSelectedGroup.Location = new System.Drawing.Point(106, 23);
            this.textBoxSelectedGroup.Name = "textBoxSelectedGroup";
            this.textBoxSelectedGroup.ReadOnly = true;
            this.textBoxSelectedGroup.Size = new System.Drawing.Size(111, 20);
            this.textBoxSelectedGroup.TabIndex = 0;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(229, 23);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(167, 23);
            this.button6.TabIndex = 1;
            this.button6.Text = "Select Group";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.OnSelectGroup);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.textBoxText);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.checkBoxNavigationBar);
            this.groupBox4.Controls.Add(this.checkBoxAddScript);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.textBoxURL);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.buttonSelectCamera);
            this.groupBox4.Controls.Add(this.comboBoxViewItemType);
            this.groupBox4.Controls.Add(this.comboBoxViewItemIx);
            this.groupBox4.Controls.Add(this.buttonSelectMap);
            this.groupBox4.Location = new System.Drawing.Point(3, 208);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(507, 240);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Select ViewItem to insert in View";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 178);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Position:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 206);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Select ViewItem:";
            // 
            // textBoxText
            // 
            this.textBoxText.Location = new System.Drawing.Point(116, 151);
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.Size = new System.Drawing.Size(276, 20);
            this.textBoxText.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 154);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "TextViewItem:";
            // 
            // checkBoxNavigationBar
            // 
            this.checkBoxNavigationBar.AutoSize = true;
            this.checkBoxNavigationBar.Checked = true;
            this.checkBoxNavigationBar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxNavigationBar.Location = new System.Drawing.Point(266, 89);
            this.checkBoxNavigationBar.Name = "checkBoxNavigationBar";
            this.checkBoxNavigationBar.Size = new System.Drawing.Size(93, 17);
            this.checkBoxNavigationBar.TabIndex = 5;
            this.checkBoxNavigationBar.Text = "NavigationBar";
            this.checkBoxNavigationBar.UseVisualStyleBackColor = true;
            this.checkBoxNavigationBar.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // checkBoxAddScript
            // 
            this.checkBoxAddScript.AutoSize = true;
            this.checkBoxAddScript.Location = new System.Drawing.Point(190, 89);
            this.checkBoxAddScript.Name = "checkBoxAddScript";
            this.checkBoxAddScript.Size = new System.Drawing.Size(70, 17);
            this.checkBoxAddScript.TabIndex = 4;
            this.checkBoxAddScript.Text = "Addscript";
            this.checkBoxAddScript.UseVisualStyleBackColor = true;
            this.checkBoxAddScript.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(113, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "URL:";
            // 
            // textBoxURL
            // 
            this.textBoxURL.Location = new System.Drawing.Point(190, 60);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(202, 20);
            this.textBoxURL.TabIndex = 3;
            this.textBoxURL.TextChanged += new System.EventHandler(this.OnURLChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "HTMLViewItem:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "CameraViewItem:";
            // 
            // buttonSelectCamera
            // 
            this.buttonSelectCamera.Location = new System.Drawing.Point(116, 25);
            this.buttonSelectCamera.Name = "buttonSelectCamera";
            this.buttonSelectCamera.Size = new System.Drawing.Size(276, 23);
            this.buttonSelectCamera.TabIndex = 2;
            this.buttonSelectCamera.Text = "Select Camera...";
            this.buttonSelectCamera.UseVisualStyleBackColor = true;
            this.buttonSelectCamera.Click += new System.EventHandler(this.OnSelectCamera);
            // 
            // comboBoxViewItemType
            // 
            this.comboBoxViewItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxViewItemType.FormattingEnabled = true;
            this.comboBoxViewItemType.Location = new System.Drawing.Point(116, 203);
            this.comboBoxViewItemType.Name = "comboBoxViewItemType";
            this.comboBoxViewItemType.Size = new System.Drawing.Size(276, 21);
            this.comboBoxViewItemType.TabIndex = 1;
            this.comboBoxViewItemType.SelectedIndexChanged += new System.EventHandler(this.OnSelectedViewItemChanged);
            // 
            // comboBoxViewItemIx
            // 
            this.comboBoxViewItemIx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxViewItemIx.FormattingEnabled = true;
            this.comboBoxViewItemIx.Items.AddRange(new object[] {
            "Shared",
            "Private",
            "Temporary",
            "SmartWall"});
            this.comboBoxViewItemIx.Location = new System.Drawing.Point(116, 177);
            this.comboBoxViewItemIx.Name = "comboBoxViewItemIx";
            this.comboBoxViewItemIx.Size = new System.Drawing.Size(55, 21);
            this.comboBoxViewItemIx.TabIndex = 0;
            this.comboBoxViewItemIx.SelectedIndexChanged += new System.EventHandler(this.OnIndexChanged);
            // 
            // buttonSelectMap
            // 
            this.buttonSelectMap.Location = new System.Drawing.Point(116, 113);
            this.buttonSelectMap.Name = "buttonSelectMap";
            this.buttonSelectMap.Size = new System.Drawing.Size(276, 23);
            this.buttonSelectMap.TabIndex = 26;
            this.buttonSelectMap.Text = "Select Map...";
            this.buttonSelectMap.UseVisualStyleBackColor = true;
            this.buttonSelectMap.Click += new System.EventHandler(this.buttonSelectMap_Click);
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(350, 454);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(164, 23);
            this.button7.TabIndex = 3;
            this.button7.Text = "Save";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.OnSave);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBoxSelectedGroup);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(511, 95);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select and Delete Group (Folder)";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(229, 52);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Delete Group";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnDeleteGroup);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(410, 206);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "to insert";
            // 
            // ViewEditUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxView);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.groupBox2);
            this.Name = "ViewEditUserControl";
            this.Size = new System.Drawing.Size(522, 485);
            this.Load += new System.EventHandler(this.OnLoad);
            this.groupBoxView.ResumeLayout(false);
            this.groupBoxView.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBoxView;
		private System.Windows.Forms.TextBox textBoxLayout;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxCount;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBoxSelectedGroup;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ComboBox comboBoxViewItemType;
		private System.Windows.Forms.ComboBox comboBoxViewItemIx;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button buttonSelectCamera;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkBoxNavigationBar;
		private System.Windows.Forms.CheckBox checkBoxAddScript;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.Button buttonSelectMap;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}
