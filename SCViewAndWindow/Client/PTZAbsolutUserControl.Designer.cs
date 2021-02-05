namespace SCViewAndWindow.Client
{
	partial class PTZAbsolutUserControl
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
            this.maskedTextBoxPan = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxTilt = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.maskedTextBoxZoom = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this._allowRepeatsCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Pan:";
            // 
            // maskedTextBoxPan
            // 
            this.maskedTextBoxPan.Location = new System.Drawing.Point(120, 12);
            this.maskedTextBoxPan.Name = "maskedTextBoxPan";
            this.maskedTextBoxPan.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBoxPan.TabIndex = 0;
            // 
            // maskedTextBoxTilt
            // 
            this.maskedTextBoxTilt.Location = new System.Drawing.Point(120, 38);
            this.maskedTextBoxTilt.Name = "maskedTextBoxTilt";
            this.maskedTextBoxTilt.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBoxTilt.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Enter Tilt:";
            // 
            // maskedTextBoxZoom
            // 
            this.maskedTextBoxZoom.Location = new System.Drawing.Point(120, 64);
            this.maskedTextBoxZoom.Name = "maskedTextBoxZoom";
            this.maskedTextBoxZoom.Size = new System.Drawing.Size(100, 20);
            this.maskedTextBoxZoom.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Enter Zoom:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(120, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Fire PTZ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnFirePTZ);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(253, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Get Current PTZ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnGetCurrent);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(120, 194);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Fire MoveStart";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnMoveStart);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(120, 223);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Fire MoveStop";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.OnMoveStop);
            // 
            // _allowRepeatsCheckBox
            // 
            this._allowRepeatsCheckBox.AutoSize = true;
            this._allowRepeatsCheckBox.Location = new System.Drawing.Point(120, 93);
            this._allowRepeatsCheckBox.Name = "_allowRepeatsCheckBox";
            this._allowRepeatsCheckBox.Size = new System.Drawing.Size(89, 17);
            this._allowRepeatsCheckBox.TabIndex = 8;
            this._allowRepeatsCheckBox.Text = "Allow repeats";
            this._allowRepeatsCheckBox.UseVisualStyleBackColor = true;
            // 
            // PTZAbsolutUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._allowRepeatsCheckBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.maskedTextBoxZoom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.maskedTextBoxTilt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.maskedTextBoxPan);
            this.Controls.Add(this.label1);
            this.Name = "PTZAbsolutUserControl";
            this.Size = new System.Drawing.Size(386, 296);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MaskedTextBox maskedTextBoxPan;
		private System.Windows.Forms.MaskedTextBox maskedTextBoxTilt;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.MaskedTextBox maskedTextBoxZoom;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox _allowRepeatsCheckBox;
	}
}
