namespace SCViewAndWindow.Client
{
	partial class ViewCreateUserControl
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
			this.buttonCreateTemp = new System.Windows.Forms.Button();
			this.textBoxVGNewName = new System.Windows.Forms.TextBox();
			this.buttonCreate2x1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxCount = new System.Windows.Forms.TextBox();
			this.textBoxLayout = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textBoxSelectedGroup = new System.Windows.Forms.TextBox();
			this.buttonSelectGroup = new System.Windows.Forms.Button();
			this.buttonCreateGroup = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxGroupName = new System.Windows.Forms.TextBox();
			this.groupBoxView = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.comboBoxViewItemType = new System.Windows.Forms.ComboBox();
			this.comboBoxViewItemIx = new System.Windows.Forms.ComboBox();
			this.buttonCreateView = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBoxView.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Temp VG Name:";
			// 
			// buttonCreateTemp
			// 
			this.buttonCreateTemp.Location = new System.Drawing.Point(229, 27);
			this.buttonCreateTemp.Name = "buttonCreateTemp";
			this.buttonCreateTemp.Size = new System.Drawing.Size(184, 23);
			this.buttonCreateTemp.TabIndex = 1;
			this.buttonCreateTemp.Text = "Create Temporary View Group";
			this.buttonCreateTemp.UseVisualStyleBackColor = true;
			this.buttonCreateTemp.Click += new System.EventHandler(this.OnCreateTempVG);
			// 
			// textBoxVGNewName
			// 
			this.textBoxVGNewName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxVGNewName.Location = new System.Drawing.Point(113, 29);
			this.textBoxVGNewName.Name = "textBoxVGNewName";
			this.textBoxVGNewName.Size = new System.Drawing.Size(111, 20);
			this.textBoxVGNewName.TabIndex = 0;
			// 
			// buttonCreate2x1
			// 
			this.buttonCreate2x1.Location = new System.Drawing.Point(229, 16);
			this.buttonCreate2x1.Name = "buttonCreate2x1";
			this.buttonCreate2x1.Size = new System.Drawing.Size(180, 23);
			this.buttonCreate2x1.TabIndex = 1;
			this.buttonCreate2x1.Text = "Create 2x1";
			this.buttonCreate2x1.UseVisualStyleBackColor = true;
			this.buttonCreate2x1.Click += new System.EventHandler(this.OnCreate2x1);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(168, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 13);
			this.label3.TabIndex = 18;
			this.label3.Text = "Count";
			// 
			// textBoxCount
			// 
			this.textBoxCount.Location = new System.Drawing.Point(113, 48);
			this.textBoxCount.Name = "textBoxCount";
			this.textBoxCount.ReadOnly = true;
			this.textBoxCount.Size = new System.Drawing.Size(49, 20);
			this.textBoxCount.TabIndex = 2;
			// 
			// textBoxLayout
			// 
			this.textBoxLayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxLayout.Location = new System.Drawing.Point(113, 19);
			this.textBoxLayout.Name = "textBoxLayout";
			this.textBoxLayout.Size = new System.Drawing.Size(111, 20);
			this.textBoxLayout.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 22);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(42, 13);
			this.label4.TabIndex = 15;
			this.label4.Text = "Layout:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textBoxVGNewName);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.buttonCreateTemp);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(419, 77);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Select or Create Top level View Group";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.textBoxSelectedGroup);
			this.groupBox2.Controls.Add(this.buttonSelectGroup);
			this.groupBox2.Controls.Add(this.buttonCreateGroup);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.textBoxGroupName);
			this.groupBox2.Location = new System.Drawing.Point(0, 83);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(419, 95);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Select or Create Group (Folder)";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 23);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(39, 13);
			this.label6.TabIndex = 6;
			this.label6.Text = "Group:";
			// 
			// textBoxSelectedGroup
			// 
			this.textBoxSelectedGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxSelectedGroup.Location = new System.Drawing.Point(113, 23);
			this.textBoxSelectedGroup.Name = "textBoxSelectedGroup";
			this.textBoxSelectedGroup.ReadOnly = true;
			this.textBoxSelectedGroup.Size = new System.Drawing.Size(111, 20);
			this.textBoxSelectedGroup.TabIndex = 0;
			// 
			// buttonSelectGroup
			// 
			this.buttonSelectGroup.Location = new System.Drawing.Point(229, 23);
			this.buttonSelectGroup.Name = "buttonSelectGroup";
			this.buttonSelectGroup.Size = new System.Drawing.Size(184, 23);
			this.buttonSelectGroup.TabIndex = 1;
			this.buttonSelectGroup.Text = "Select Group";
			this.buttonSelectGroup.UseVisualStyleBackColor = true;
			this.buttonSelectGroup.Click += new System.EventHandler(this.OnSelectGroup);
			// 
			// buttonCreateGroup
			// 
			this.buttonCreateGroup.Enabled = false;
			this.buttonCreateGroup.Location = new System.Drawing.Point(229, 49);
			this.buttonCreateGroup.Name = "buttonCreateGroup";
			this.buttonCreateGroup.Size = new System.Drawing.Size(184, 23);
			this.buttonCreateGroup.TabIndex = 3;
			this.buttonCreateGroup.Text = "Create Group (Folder)";
			this.buttonCreateGroup.UseVisualStyleBackColor = true;
			this.buttonCreateGroup.Click += new System.EventHandler(this.OnCreateGroup);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(9, 52);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(95, 13);
			this.label5.TabIndex = 23;
			this.label5.Text = "New Group Name:";
			// 
			// textBoxGroupName
			// 
			this.textBoxGroupName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxGroupName.Enabled = false;
			this.textBoxGroupName.Location = new System.Drawing.Point(113, 49);
			this.textBoxGroupName.Name = "textBoxGroupName";
			this.textBoxGroupName.Size = new System.Drawing.Size(111, 20);
			this.textBoxGroupName.TabIndex = 2;
			// 
			// groupBoxView
			// 
			this.groupBoxView.Controls.Add(this.textBoxLayout);
			this.groupBoxView.Controls.Add(this.label4);
			this.groupBoxView.Controls.Add(this.textBoxCount);
			this.groupBoxView.Controls.Add(this.label3);
			this.groupBoxView.Controls.Add(this.buttonCreate2x1);
			this.groupBoxView.Enabled = false;
			this.groupBoxView.Location = new System.Drawing.Point(2, 181);
			this.groupBoxView.Name = "groupBoxView";
			this.groupBoxView.Size = new System.Drawing.Size(415, 98);
			this.groupBoxView.TabIndex = 2;
			this.groupBoxView.TabStop = false;
			this.groupBoxView.Text = "Select or Create ViewLayout";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.comboBoxViewItemType);
			this.groupBox4.Controls.Add(this.comboBoxViewItemIx);
			this.groupBox4.Location = new System.Drawing.Point(4, 309);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(415, 66);
			this.groupBox4.TabIndex = 3;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Select ViewItems";
			// 
			// comboBoxViewItemType
			// 
			this.comboBoxViewItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxViewItemType.Enabled = false;
			this.comboBoxViewItemType.FormattingEnabled = true;
			this.comboBoxViewItemType.Location = new System.Drawing.Point(105, 19);
			this.comboBoxViewItemType.Name = "comboBoxViewItemType";
			this.comboBoxViewItemType.Size = new System.Drawing.Size(302, 21);
			this.comboBoxViewItemType.TabIndex = 1;
			this.comboBoxViewItemType.SelectedIndexChanged += new System.EventHandler(this.OnViewItemChanged);
			// 
			// comboBoxViewItemIx
			// 
			this.comboBoxViewItemIx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxViewItemIx.Enabled = false;
			this.comboBoxViewItemIx.FormattingEnabled = true;
			this.comboBoxViewItemIx.Items.AddRange(new object[] {
            "Shared",
            "Private",
            "Temporary",
            "SmartWall"});
			this.comboBoxViewItemIx.Location = new System.Drawing.Point(13, 19);
			this.comboBoxViewItemIx.Name = "comboBoxViewItemIx";
			this.comboBoxViewItemIx.Size = new System.Drawing.Size(55, 21);
			this.comboBoxViewItemIx.TabIndex = 0;
			this.comboBoxViewItemIx.SelectedIndexChanged += new System.EventHandler(this.OnIndexChanged);
			// 
			// buttonCreateView
			// 
			this.buttonCreateView.Enabled = false;
			this.buttonCreateView.Location = new System.Drawing.Point(235, 402);
			this.buttonCreateView.Name = "buttonCreateView";
			this.buttonCreateView.Size = new System.Drawing.Size(164, 23);
			this.buttonCreateView.TabIndex = 4;
			this.buttonCreateView.Text = "Create View";
			this.buttonCreateView.UseVisualStyleBackColor = true;
			this.buttonCreateView.Click += new System.EventHandler(this.OnCreate);
			// 
			// ViewCreateUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBoxView);
			this.Controls.Add(this.buttonCreateView);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "ViewCreateUserControl";
			this.Size = new System.Drawing.Size(422, 440);
			this.Load += new System.EventHandler(this.OnLoad);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBoxView.ResumeLayout(false);
			this.groupBoxView.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonCreateTemp;
		private System.Windows.Forms.TextBox textBoxVGNewName;
		private System.Windows.Forms.Button buttonCreate2x1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxCount;
		private System.Windows.Forms.TextBox textBoxLayout;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button buttonCreateGroup;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxGroupName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBoxSelectedGroup;
		private System.Windows.Forms.Button buttonSelectGroup;
		private System.Windows.Forms.GroupBox groupBoxView;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ComboBox comboBoxViewItemType;
		private System.Windows.Forms.ComboBox comboBoxViewItemIx;
		private System.Windows.Forms.Button buttonCreateView;
	}
}
