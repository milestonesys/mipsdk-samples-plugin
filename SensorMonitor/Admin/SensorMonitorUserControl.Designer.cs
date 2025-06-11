namespace SensorMonitor.Admin
{
	partial class SensorMonitorUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensorMonitorUserControl));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCameraSelect = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonEventSelect = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBoxDisabled = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Controller Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(109, 14);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(279, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.OnUserChange);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Related camera:";
            // 
            // buttonCameraSelect
            // 
            this.buttonCameraSelect.Location = new System.Drawing.Point(104, 65);
            this.buttonCameraSelect.Name = "buttonCameraSelect";
            this.buttonCameraSelect.Size = new System.Drawing.Size(279, 23);
            this.buttonCameraSelect.TabIndex = 3;
            this.buttonCameraSelect.Text = "Select camera...";
            this.buttonCameraSelect.UseVisualStyleBackColor = true;
            this.buttonCameraSelect.Click += new System.EventHandler(this.OnCameraSelect);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(109, 94);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(279, 93);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Related event:";
            // 
            // buttonEventSelect
            // 
            this.buttonEventSelect.Location = new System.Drawing.Point(104, 198);
            this.buttonEventSelect.Name = "buttonEventSelect";
            this.buttonEventSelect.Size = new System.Drawing.Size(279, 23);
            this.buttonEventSelect.TabIndex = 3;
            this.buttonEventSelect.Text = "Select event...";
            this.buttonEventSelect.UseVisualStyleBackColor = true;
            this.buttonEventSelect.Click += new System.EventHandler(this.OnEventSelect);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(104, 227);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(279, 93);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // checkBoxDisabled
            // 
            this.checkBoxDisabled.AutoSize = true;
            this.checkBoxDisabled.Location = new System.Drawing.Point(109, 41);
            this.checkBoxDisabled.Name = "checkBoxDisabled";
            this.checkBoxDisabled.Size = new System.Drawing.Size(67, 17);
            this.checkBoxDisabled.TabIndex = 6;
            this.checkBoxDisabled.Text = "Disabled";
            this.checkBoxDisabled.UseVisualStyleBackColor = true;
            this.checkBoxDisabled.CheckedChanged += new System.EventHandler(this.OnUserChange);
            // 
            // SensorMonitorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.checkBoxDisabled);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonEventSelect);
            this.Controls.Add(this.buttonCameraSelect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.Name = "SensorMonitorUserControl";
            this.Size = new System.Drawing.Size(403, 331);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion


		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonCameraSelect;
		private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonEventSelect;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox checkBoxDisabled;
	}
}
