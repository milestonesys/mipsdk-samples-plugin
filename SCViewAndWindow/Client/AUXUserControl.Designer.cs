namespace SCViewAndWindow.Client
{
	partial class AUXUserControl
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
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxAUX = new System.Windows.Forms.ComboBox();
			this.comboBoxOnOff = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(152, 93);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(203, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Fire Command";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnFireIndicator);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "AUX number:";
			// 
			// comboBoxAUX
			// 
			this.comboBoxAUX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxAUX.FormattingEnabled = true;
			this.comboBoxAUX.Location = new System.Drawing.Point(152, 11);
			this.comboBoxAUX.Name = "comboBoxAUX";
			this.comboBoxAUX.Size = new System.Drawing.Size(203, 21);
			this.comboBoxAUX.TabIndex = 0;
			// 
			// comboBoxOnOff
			// 
			this.comboBoxOnOff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxOnOff.FormattingEnabled = true;
			this.comboBoxOnOff.Location = new System.Drawing.Point(152, 45);
			this.comboBoxOnOff.Name = "comboBoxOnOff";
			this.comboBoxOnOff.Size = new System.Drawing.Size(203, 21);
			this.comboBoxOnOff.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "On or Off:";
			// 
			// AUXUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.comboBoxOnOff);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboBoxAUX);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Name = "AUXUserControl";
			this.Size = new System.Drawing.Size(368, 151);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxAUX;
		private System.Windows.Forms.ComboBox comboBoxOnOff;
		private System.Windows.Forms.Label label2;
	}
}
