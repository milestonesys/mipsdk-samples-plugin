namespace SCWorkSpace.Client
{
    partial class SCWorkSpaceViewItemUserControl
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this._selectWorkSpaceComboBox = new System.Windows.Forms.ComboBox();
			this._shuffleWorkSpaceCamerasButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this._selectedWorkSpaceLabel = new System.Windows.Forms.Label();
			this._selectedWorkSpaceStateLabel = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this._selectWorkSpaceStateComboBox = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(7, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(177, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Shuffle WorkSpace plugin cameras:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(7, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Select WorkSpace:";
			// 
			// _selectWorkSpaceComboBox
			// 
			this._selectWorkSpaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._selectWorkSpaceComboBox.FormattingEnabled = true;
			this._selectWorkSpaceComboBox.Location = new System.Drawing.Point(119, 62);
			this._selectWorkSpaceComboBox.Name = "_selectWorkSpaceComboBox";
			this._selectWorkSpaceComboBox.Size = new System.Drawing.Size(116, 21);
			this._selectWorkSpaceComboBox.TabIndex = 3;
			this._selectWorkSpaceComboBox.SelectedIndexChanged += new System.EventHandler(this._selectWorkSpaceComboBox_SelectedIndexChanged);
			// 
			// _shuffleWorkSpaceCamerasButton
			// 
			this._shuffleWorkSpaceCamerasButton.Location = new System.Drawing.Point(201, 7);
			this._shuffleWorkSpaceCamerasButton.Name = "_shuffleWorkSpaceCamerasButton";
			this._shuffleWorkSpaceCamerasButton.Size = new System.Drawing.Size(34, 23);
			this._shuffleWorkSpaceCamerasButton.TabIndex = 4;
			this._shuffleWorkSpaceCamerasButton.Text = "Go";
			this._shuffleWorkSpaceCamerasButton.UseVisualStyleBackColor = true;
			this._shuffleWorkSpaceCamerasButton.Click += new System.EventHandler(this._shuffleWorkSpaceCamerasButton_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(7, 38);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Selected WorkSpace:";
			// 
			// _selectedWorkSpaceLabel
			// 
			this._selectedWorkSpaceLabel.AutoSize = true;
			this._selectedWorkSpaceLabel.Location = new System.Drawing.Point(123, 38);
			this._selectedWorkSpaceLabel.Name = "_selectedWorkSpaceLabel";
			this._selectedWorkSpaceLabel.Size = new System.Drawing.Size(0, 13);
			this._selectedWorkSpaceLabel.TabIndex = 6;
			// 
			// _selectedWorkSpaceStateLabel
			// 
			this._selectedWorkSpaceStateLabel.AutoSize = true;
			this._selectedWorkSpaceStateLabel.Location = new System.Drawing.Point(151, 92);
			this._selectedWorkSpaceStateLabel.Name = "_selectedWorkSpaceStateLabel";
			this._selectedWorkSpaceStateLabel.Size = new System.Drawing.Size(0, 13);
			this._selectedWorkSpaceStateLabel.TabIndex = 10;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Location = new System.Drawing.Point(7, 92);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(137, 13);
			this.label5.TabIndex = 9;
			this.label5.Text = "Selected WorkSpaceState:";
			// 
			// _selectWorkSpaceStateComboBox
			// 
			this._selectWorkSpaceStateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._selectWorkSpaceStateComboBox.FormattingEnabled = true;
			this._selectWorkSpaceStateComboBox.Location = new System.Drawing.Point(148, 114);
			this._selectWorkSpaceStateComboBox.Name = "_selectWorkSpaceStateComboBox";
			this._selectWorkSpaceStateComboBox.Size = new System.Drawing.Size(86, 21);
			this._selectWorkSpaceStateComboBox.TabIndex = 8;
			this._selectWorkSpaceStateComboBox.SelectedIndexChanged += new System.EventHandler(this._selectWorkSpaceStateComboBox_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Location = new System.Drawing.Point(7, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(125, 13);
			this.label6.TabIndex = 7;
			this.label6.Text = "Select WorkSpaceState:";
			// 
			// SCWorkSpaceViewItemUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._selectedWorkSpaceStateLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this._selectWorkSpaceStateComboBox);
			this.Controls.Add(this.label6);
			this.Controls.Add(this._selectedWorkSpaceLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this._shuffleWorkSpaceCamerasButton);
			this.Controls.Add(this._selectWorkSpaceComboBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "SCWorkSpaceViewItemUserControl";
			this.Size = new System.Drawing.Size(251, 202);
			this.DoubleClick += new System.EventHandler(this.ViewItemUserControlDoubleClick);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SCWorkSpaceViewItemUserControl_MouseDown);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _selectWorkSpaceComboBox;
        private System.Windows.Forms.Button _shuffleWorkSpaceCamerasButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label _selectedWorkSpaceLabel;
        private System.Windows.Forms.Label _selectedWorkSpaceStateLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox _selectWorkSpaceStateComboBox;
        private System.Windows.Forms.Label label6;
    }
}
