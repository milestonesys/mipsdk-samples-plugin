using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCExport.Client
{
	partial class SCExportSidePanelUserControl
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
            this.textBoxCurrent = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.textBoxProgressText = new System.Windows.Forms.TextBox();
            this.buttonExportDB = new System.Windows.Forms.Button();
            this.buttonMkv = new System.Windows.Forms.Button();
            this.buttonAVIexport = new System.Windows.Forms.Button();
            this.checkBoxIncludeOverlayImage = new System.Windows.Forms.CheckBox();
            this.buttonOverlayImage = new System.Windows.Forms.Button();
            this.checkBoxPreventReExport = new System.Windows.Forms.CheckBox();
            this.checkBoxSignExport = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxCurrent
            // 
            this.textBoxCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCurrent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCurrent.Location = new System.Drawing.Point(15, 3);
            this.textBoxCurrent.Name = "textBoxCurrent";
            this.textBoxCurrent.ReadOnly = true;
            this.textBoxCurrent.Size = new System.Drawing.Size(211, 15);
            this.textBoxCurrent.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(15, 199);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(211, 23);
            this.progressBar.TabIndex = 3;
            this.progressBar.Tag = "";
            // 
            // textBoxProgressText
            // 
            this.textBoxProgressText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProgressText.Location = new System.Drawing.Point(15, 228);
            this.textBoxProgressText.Name = "textBoxProgressText";
            this.textBoxProgressText.ReadOnly = true;
            this.textBoxProgressText.Size = new System.Drawing.Size(211, 22);
            this.textBoxProgressText.TabIndex = 4;
            // 
            // buttonExportDB
            // 
            this.buttonExportDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExportDB.Location = new System.Drawing.Point(117, 24);
            this.buttonExportDB.Name = "buttonExportDB";
            this.buttonExportDB.Size = new System.Drawing.Size(99, 23);
            this.buttonExportDB.TabIndex = 5;
            this.buttonExportDB.Text = "Export DB";
            this.buttonExportDB.UseVisualStyleBackColor = true;
            this.buttonExportDB.Click += new System.EventHandler(this.OnDBExport);
            // 
            // buttonMkv
            // 
            this.buttonMkv.Location = new System.Drawing.Point(15, 24);
            this.buttonMkv.Name = "buttonMkv";
            this.buttonMkv.Size = new System.Drawing.Size(96, 23);
            this.buttonMkv.TabIndex = 6;
            this.buttonMkv.Text = "Export MKV";
            this.buttonMkv.UseVisualStyleBackColor = true;
            this.buttonMkv.Click += new System.EventHandler(this.buttonMkv_Click);
            // 
            // buttonAVIexport
            // 
            this.buttonAVIexport.Location = new System.Drawing.Point(15, 53);
            this.buttonAVIexport.Name = "buttonAVIexport";
            this.buttonAVIexport.Size = new System.Drawing.Size(96, 23);
            this.buttonAVIexport.TabIndex = 0;
            this.buttonAVIexport.Text = "Export AVI";
            this.buttonAVIexport.UseVisualStyleBackColor = true;
            this.buttonAVIexport.Click += new System.EventHandler(this.OnAVIExport);
            // 
            // checkBoxIncludeOverlayImage
            // 
            this.checkBoxIncludeOverlayImage.AutoSize = true;
            this.checkBoxIncludeOverlayImage.Location = new System.Drawing.Point(15, 136);
            this.checkBoxIncludeOverlayImage.Name = "checkBoxIncludeOverlayImage";
            this.checkBoxIncludeOverlayImage.Size = new System.Drawing.Size(211, 20);
            this.checkBoxIncludeOverlayImage.TabIndex = 12;
            this.checkBoxIncludeOverlayImage.Text = "Include overlay image (AVI only)";
            this.checkBoxIncludeOverlayImage.UseVisualStyleBackColor = true;
            // 
            // buttonOverlayImage
            // 
            this.buttonOverlayImage.Location = new System.Drawing.Point(35, 165);
            this.buttonOverlayImage.Name = "buttonOverlayImage";
            this.buttonOverlayImage.Size = new System.Drawing.Size(191, 23);
            this.buttonOverlayImage.TabIndex = 13;
            this.buttonOverlayImage.Text = "Select...";
            this.buttonOverlayImage.UseVisualStyleBackColor = true;
            this.buttonOverlayImage.Click += new System.EventHandler(this.buttonOverlayImage_Click);
            // 
            // checkBoxPreventReExport
            // 
            this.checkBoxPreventReExport.AutoSize = true;
            this.checkBoxPreventReExport.Location = new System.Drawing.Point(15, 83);
            this.checkBoxPreventReExport.Name = "checkBoxPreventReExport";
            this.checkBoxPreventReExport.Size = new System.Drawing.Size(183, 20);
            this.checkBoxPreventReExport.TabIndex = 14;
            this.checkBoxPreventReExport.Text = "Prevent re-export (DB only)";
            this.checkBoxPreventReExport.UseVisualStyleBackColor = true;
            // 
            // checkBoxSignExport
            // 
            this.checkBoxSignExport.AutoSize = true;
            this.checkBoxSignExport.Location = new System.Drawing.Point(15, 110);
            this.checkBoxSignExport.Name = "checkBoxSignExport";
            this.checkBoxSignExport.Size = new System.Drawing.Size(151, 20);
            this.checkBoxSignExport.TabIndex = 15;
            this.checkBoxSignExport.Text = "Sign export (DB only)";
            this.checkBoxSignExport.UseVisualStyleBackColor = true;
            // 
            // SCExportSidePanelUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.checkBoxSignExport);
            this.Controls.Add(this.checkBoxPreventReExport);
            this.Controls.Add(this.buttonOverlayImage);
            this.Controls.Add(this.checkBoxIncludeOverlayImage);
            this.Controls.Add(this.buttonMkv);
            this.Controls.Add(this.buttonExportDB);
            this.Controls.Add(this.textBoxProgressText);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.textBoxCurrent);
            this.Controls.Add(this.buttonAVIexport);
            this.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SCExportSidePanelUserControl";
            this.Size = new System.Drawing.Size(248, 264);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		private System.Windows.Forms.TextBox textBoxCurrent;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.TextBox textBoxProgressText;
		private System.Windows.Forms.Button buttonExportDB;
        private System.Windows.Forms.Button buttonMkv;
        private System.Windows.Forms.Button buttonAVIexport;
        private System.Windows.Forms.CheckBox checkBoxIncludeOverlayImage;
        private System.Windows.Forms.Button buttonOverlayImage;
        private System.Windows.Forms.CheckBox checkBoxPreventReExport;
        private System.Windows.Forms.CheckBox checkBoxSignExport;
    }
}
