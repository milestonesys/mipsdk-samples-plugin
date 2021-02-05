namespace SCViewAndWindow.Client
{
	partial class SCViewAndWindowPropertiesUserControl
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
			this.comboBoxID = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// comboBoxID
			// 
			this.comboBoxID.FormattingEnabled = true;
			this.comboBoxID.Location = new System.Drawing.Point(13, 34);
			this.comboBoxID.Name = "comboBoxID";
			this.comboBoxID.Size = new System.Drawing.Size(219, 21);
			this.comboBoxID.TabIndex = 1;
			this.comboBoxID.SelectedIndexChanged += new System.EventHandler(this.OnSourceSelected);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 11);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Select an ID:";
			// 
			// SCViewAndWindowPropertiesUserControl
			// 
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboBoxID);
			this.Name = "SCViewAndWindowPropertiesUserControl";
			this.Size = new System.Drawing.Size(251, 89);
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label2;

	}
}
