namespace ConfigDump.Admin
{
	partial class ConfigDumpUserControl
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
			this.dumpConfigurationUserControl1 = new ConfigDump.Admin.DumpConfigurationUserControl();
			this.SuspendLayout();
			// 
			// dumpConfigurationUserControl1
			// 
			this.dumpConfigurationUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dumpConfigurationUserControl1.Location = new System.Drawing.Point(16, 15);
			this.dumpConfigurationUserControl1.Name = "dumpConfigurationUserControl1";
			this.dumpConfigurationUserControl1.Size = new System.Drawing.Size(687, 516);
			this.dumpConfigurationUserControl1.TabIndex = 2;
			// 
			// ConfigDumpUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Controls.Add(this.dumpConfigurationUserControl1);
			this.Name = "ConfigDumpUserControl";
			this.Size = new System.Drawing.Size(718, 534);
			this.ResumeLayout(false);

		}

		#endregion

		private DumpConfigurationUserControl dumpConfigurationUserControl1;
	}
}
