namespace ServiceTest.Admin
{
	partial class ServiceTestUserControl
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
			this._listBoxServices = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonRegister = new System.Windows.Forms.Button();
			this.buttonUnregister = new System.Windows.Forms.Button();
			this.buttonRefresh = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _listBoxServices
			// 
			this._listBoxServices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._listBoxServices.HorizontalScrollbar = true;
			this._listBoxServices.Location = new System.Drawing.Point(142, 17);
			this._listBoxServices.Name = "_listBoxServices";
			this._listBoxServices.Size = new System.Drawing.Size(561, 121);
			this._listBoxServices.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 17);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Registrered Services:";
			// 
			// buttonRegister
			// 
			this.buttonRegister.Location = new System.Drawing.Point(16, 171);
			this.buttonRegister.Name = "buttonRegister";
			this.buttonRegister.Size = new System.Drawing.Size(222, 23);
			this.buttonRegister.TabIndex = 3;
			this.buttonRegister.Text = "Register test service";
			this.buttonRegister.UseVisualStyleBackColor = true;
			this.buttonRegister.Click += new System.EventHandler(this.OnRegister);
			// 
			// buttonUnregister
			// 
			this.buttonUnregister.Location = new System.Drawing.Point(16, 200);
			this.buttonUnregister.Name = "buttonUnregister";
			this.buttonUnregister.Size = new System.Drawing.Size(222, 23);
			this.buttonUnregister.TabIndex = 4;
			this.buttonUnregister.Text = "Unregister test service";
			this.buttonUnregister.UseVisualStyleBackColor = true;
			this.buttonUnregister.Click += new System.EventHandler(this.OnUnRegister);
			// 
			// buttonRefresh
			// 
			this.buttonRefresh.Location = new System.Drawing.Point(16, 117);
			this.buttonRefresh.Name = "buttonRefresh";
			this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
			this.buttonRefresh.TabIndex = 2;
			this.buttonRefresh.Text = "Refresh";
			this.buttonRefresh.UseVisualStyleBackColor = true;
			this.buttonRefresh.Click += new System.EventHandler(this.OnRefresh);
			// 
			// ServiceTestUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Controls.Add(this.buttonRefresh);
			this.Controls.Add(this.buttonUnregister);
			this.Controls.Add(this.buttonRegister);
			this.Controls.Add(this.label2);
			this.Controls.Add(this._listBoxServices);
			this.Name = "ServiceTestUserControl";
			this.Size = new System.Drawing.Size(718, 401);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox _listBoxServices;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonRegister;
		private System.Windows.Forms.Button buttonUnregister;
		private System.Windows.Forms.Button buttonRefresh;
	}
}
