namespace SCViewAndWindow.Client
{
	partial class PTZUserControl
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
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			this.button10 = new System.Windows.Forms.Button();
			this.button11 = new System.Windows.Forms.Button();
			this.button12 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(127, 17);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(156, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Current";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnSelect);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(19, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Select camera:";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(68, 75);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(51, 43);
			this.button2.TabIndex = 2;
			this.button2.Text = "Up Left";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.OnUpLeft);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(151, 75);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(51, 43);
			this.button3.TabIndex = 3;
			this.button3.Text = "Up ";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.OnUp);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(232, 75);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(51, 43);
			this.button4.TabIndex = 4;
			this.button4.Text = "Up Right";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.OnUpRight);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(232, 148);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(51, 43);
			this.button5.TabIndex = 7;
			this.button5.Text = "Right";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.OnRight);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(151, 148);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(51, 43);
			this.button6.TabIndex = 6;
			this.button6.Text = "Home";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.OnHome);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(68, 148);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(51, 43);
			this.button7.TabIndex = 5;
			this.button7.Text = "Left";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.OnLeft);
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(232, 217);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(51, 43);
			this.button8.TabIndex = 10;
			this.button8.Text = "Down Right";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.OnDownRight);
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(151, 217);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(51, 43);
			this.button9.TabIndex = 9;
			this.button9.Text = "Down";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new System.EventHandler(this.OnDown);
			// 
			// button10
			// 
			this.button10.Location = new System.Drawing.Point(68, 217);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(51, 43);
			this.button10.TabIndex = 8;
			this.button10.Text = "Down Left";
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += new System.EventHandler(this.OnDownLeft);
			// 
			// button11
			// 
			this.button11.Location = new System.Drawing.Point(109, 277);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(51, 43);
			this.button11.TabIndex = 11;
			this.button11.Text = "Zoom In";
			this.button11.UseVisualStyleBackColor = true;
			this.button11.Click += new System.EventHandler(this.OnZoomIn);
			// 
			// button12
			// 
			this.button12.Location = new System.Drawing.Point(187, 277);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(51, 43);
			this.button12.TabIndex = 12;
			this.button12.Text = "Zoom Out";
			this.button12.UseVisualStyleBackColor = true;
			this.button12.Click += new System.EventHandler(this.OnZoomOut);
			// 
			// PTZUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.button12);
			this.Controls.Add(this.button11);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.button10);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Name = "PTZUserControl";
			this.Size = new System.Drawing.Size(408, 375);
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button11;
		private System.Windows.Forms.Button button12;
	}
}
