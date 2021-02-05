using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCInsertCamera.Client
{
	partial class SCInsertCameraSidePanelUserControl
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


		private void InitializeComponent()
		{
			this.buttonCreate = new System.Windows.Forms.Button();
			this.buttonSelect = new System.Windows.Forms.Button();
			this.buttonInsert = new System.Windows.Forms.Button();
			this.comboBoxIndex = new System.Windows.Forms.ComboBox();
			this.labelIndex = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxLayoutName = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonClear = new System.Windows.Forms.Button();
			this._temporaryInsertCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.comboBoxStream = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonCreate
			// 
			this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCreate.Location = new System.Drawing.Point(53, 26);
			this.buttonCreate.Name = "buttonCreate";
			this.buttonCreate.Size = new System.Drawing.Size(189, 23);
			this.buttonCreate.TabIndex = 0;
			this.buttonCreate.Text = "Create 1,5,1 layout";
			this.buttonCreate.UseVisualStyleBackColor = true;
			this.buttonCreate.Click += new System.EventHandler(this.OnCreateClick);
			// 
			// buttonSelect
			// 
			this.buttonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSelect.Location = new System.Drawing.Point(53, 64);
			this.buttonSelect.Name = "buttonSelect";
			this.buttonSelect.Size = new System.Drawing.Size(189, 23);
			this.buttonSelect.TabIndex = 1;
			this.buttonSelect.Text = "Select camera...";
			this.buttonSelect.UseVisualStyleBackColor = true;
			this.buttonSelect.Click += new System.EventHandler(this.OnSelectCameraClick);
			// 
			// buttonInsert
			// 
			this.buttonInsert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonInsert.Enabled = false;
			this.buttonInsert.Location = new System.Drawing.Point(53, 173);
			this.buttonInsert.Name = "buttonInsert";
			this.buttonInsert.Size = new System.Drawing.Size(189, 23);
			this.buttonInsert.TabIndex = 2;
			this.buttonInsert.Text = "Insert camera on index";
			this.buttonInsert.UseVisualStyleBackColor = true;
			this.buttonInsert.Click += new System.EventHandler(this.OnInsert);
			// 
			// comboBoxIndex
			// 
			this.comboBoxIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxIndex.FormattingEnabled = true;
			this.comboBoxIndex.Location = new System.Drawing.Point(53, 145);
			this.comboBoxIndex.Name = "comboBoxIndex";
			this.comboBoxIndex.Size = new System.Drawing.Size(189, 23);
			this.comboBoxIndex.TabIndex = 3;
			// 
			// labelIndex
			// 
			this.labelIndex.AutoSize = true;
			this.labelIndex.Location = new System.Drawing.Point(4, 148);
			this.labelIndex.Name = "labelIndex";
			this.labelIndex.Size = new System.Drawing.Size(43, 16);
			this.labelIndex.TabIndex = 4;
			this.labelIndex.Text = "Index:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 81);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Current:";
			// 
			// textBoxLayoutName
			// 
			this.textBoxLayoutName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLayoutName.Location = new System.Drawing.Point(53, 34);
			this.textBoxLayoutName.Name = "textBoxLayoutName";
			this.textBoxLayoutName.ReadOnly = true;
			this.textBoxLayoutName.Size = new System.Drawing.Size(189, 22);
			this.textBoxLayoutName.TabIndex = 6;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.comboBoxStream);
			this.groupBox1.Controls.Add(this.buttonClear);
			this.groupBox1.Controls.Add(this._temporaryInsertCheckBox);
			this.groupBox1.Controls.Add(this.textBoxLayoutName);
			this.groupBox1.Controls.Add(this.buttonSelect);
			this.groupBox1.Controls.Add(this.buttonInsert);
			this.groupBox1.Controls.Add(this.labelIndex);
			this.groupBox1.Controls.Add(this.comboBoxIndex);
			this.groupBox1.Location = new System.Drawing.Point(0, 67);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(248, 233);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Modify view layout";
			// 
			// buttonClear
			// 
			this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClear.Location = new System.Drawing.Point(53, 200);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(189, 23);
			this.buttonClear.TabIndex = 8;
			this.buttonClear.Text = "Clear camera on index";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// _temporaryInsertCheckBox
			// 
			this._temporaryInsertCheckBox.AutoSize = true;
			this._temporaryInsertCheckBox.Location = new System.Drawing.Point(53, 121);
			this._temporaryInsertCheckBox.Name = "_temporaryInsertCheckBox";
			this._temporaryInsertCheckBox.Size = new System.Drawing.Size(123, 20);
			this._temporaryInsertCheckBox.TabIndex = 7;
			this._temporaryInsertCheckBox.Text = "Temporary insert";
			this._temporaryInsertCheckBox.UseVisualStyleBackColor = true;
			this._temporaryInsertCheckBox.CheckedChanged += new System.EventHandler(this._temporaryInsertCheckBox_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.buttonCreate);
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(248, 61);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Create view layout";
			// 
			// comboBoxStream
			// 
			this.comboBoxStream.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxStream.Enabled = false;
			this.comboBoxStream.FormattingEnabled = true;
			this.comboBoxStream.Location = new System.Drawing.Point(53, 92);
			this.comboBoxStream.Name = "comboBoxStream";
			this.comboBoxStream.Size = new System.Drawing.Size(189, 23);
			this.comboBoxStream.TabIndex = 9;
			// 
			// SCInsertCameraSidePanelUserControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "SCInsertCameraSidePanelUserControl";
			this.Size = new System.Drawing.Size(248, 303);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.Button buttonCreate;
		private System.Windows.Forms.Button buttonInsert;
		private System.Windows.Forms.ComboBox comboBoxIndex;
		private System.Windows.Forms.Label labelIndex;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxLayoutName;
		private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.CheckBox _temporaryInsertCheckBox;
		private System.Windows.Forms.Button buttonClear;
		private System.Windows.Forms.ComboBox comboBoxStream;

	}
}
