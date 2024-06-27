namespace LicenseRegistration.Admin
{
	partial class LicenseRegistrationUserControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseRegistrationUserControl));
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxPlugins = new System.Windows.Forms.ComboBox();
			this.textBoxXML = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select plug-in to Register:";
			// 
			// comboBoxPlugins
			// 
			this.comboBoxPlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxPlugins.FormattingEnabled = true;
			this.comboBoxPlugins.Location = new System.Drawing.Point(169, 14);
			this.comboBoxPlugins.Name = "comboBoxPlugins";
			this.comboBoxPlugins.Size = new System.Drawing.Size(285, 21);
			this.comboBoxPlugins.TabIndex = 1;
			this.comboBoxPlugins.SelectedIndexChanged += new System.EventHandler(this.OnSelect);
			// 
			// textBoxXML
			// 
			this.textBoxXML.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxXML.Location = new System.Drawing.Point(16, 68);
			this.textBoxXML.Multiline = true;
			this.textBoxXML.Name = "textBoxXML";
			this.textBoxXML.ReadOnly = true;
			this.textBoxXML.Size = new System.Drawing.Size(667, 200);
			this.textBoxXML.TabIndex = 2;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(16, 286);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(140, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Copy to Clipboard";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnCopy);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox1.Location = new System.Drawing.Point(16, 338);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(667, 181);
			this.richTextBox1.TabIndex = 6;
			this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
			// 
			// LicenseRegistrationUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBoxXML);
			this.Controls.Add(this.comboBoxPlugins);
			this.Controls.Add(this.label1);
			this.Name = "LicenseRegistrationUserControl";
			this.Size = new System.Drawing.Size(718, 534);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion


		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxPlugins;
		private System.Windows.Forms.TextBox textBoxXML;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RichTextBox richTextBox1;
	}
}
