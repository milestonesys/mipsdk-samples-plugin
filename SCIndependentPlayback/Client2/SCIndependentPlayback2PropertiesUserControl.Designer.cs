namespace SCIndependentPlayback.Client2
{
	partial class SCIndependentPlayback2PropertiesUserControl
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
			this.buttonSelect = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonSelect
			// 
			this.buttonSelect.Location = new System.Drawing.Point(18, 14);
			this.buttonSelect.Name = "buttonSelect";
			this.buttonSelect.Size = new System.Drawing.Size(214, 23);
			this.buttonSelect.TabIndex = 5;
			this.buttonSelect.Text = "Select Camera...";
			this.buttonSelect.UseVisualStyleBackColor = true;
			this.buttonSelect.Click += new System.EventHandler(this.OnSourceSelected);
			// 
			// SCIndependentPlaybackPropertiesUserControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.buttonSelect);
			this.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "SCIndependentPlaybackPropertiesUserControl";
			this.Size = new System.Drawing.Size(251, 89);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonSelect;


	}
}
