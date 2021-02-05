namespace VideoPreview.Admin
{
	partial class VideoPreviewUserControl
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
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonCameraSelect = new System.Windows.Forms.Button();
            this.textBoxCameraName = new System.Windows.Forms.TextBox();
            this.textBoxStat = new System.Windows.Forms.TextBox();
            this.buttonPlayback = new System.Windows.Forms.Button();
            this.buttonLive = new System.Windows.Forms.Button();
            this.textBoxHeader = new System.Windows.Forms.TextBox();
            this.buttonLine = new System.Windows.Forms.Button();
            this.buttonPreviewImage = new System.Windows.Forms.Button();
            this.panelPlayback = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.comboBoxStream = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(139, 17);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(279, 20);
            this.textBoxName.TabIndex = 0;
            this.textBoxName.TextChanged += new System.EventHandler(this.OnUserChange);
            // 
            // buttonStart
            // 
            this.buttonStart.Enabled = false;
            this.buttonStart.Location = new System.Drawing.Point(16, 132);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(89, 23);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.OnStart);
            // 
            // buttonCameraSelect
            // 
            this.buttonCameraSelect.Location = new System.Drawing.Point(16, 53);
            this.buttonCameraSelect.Name = "buttonCameraSelect";
            this.buttonCameraSelect.Size = new System.Drawing.Size(89, 23);
            this.buttonCameraSelect.TabIndex = 1;
            this.buttonCameraSelect.Text = "Camera:";
            this.buttonCameraSelect.UseVisualStyleBackColor = true;
            this.buttonCameraSelect.Click += new System.EventHandler(this.OnSelectCamera);
            // 
            // textBoxCameraName
            // 
            this.textBoxCameraName.Location = new System.Drawing.Point(139, 56);
            this.textBoxCameraName.Name = "textBoxCameraName";
            this.textBoxCameraName.ReadOnly = true;
            this.textBoxCameraName.Size = new System.Drawing.Size(279, 20);
            this.textBoxCameraName.TabIndex = 7;
            this.textBoxCameraName.TabStop = false;
            // 
            // textBoxStat
            // 
            this.textBoxStat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStat.Location = new System.Drawing.Point(139, 505);
            this.textBoxStat.Name = "textBoxStat";
            this.textBoxStat.ReadOnly = true;
            this.textBoxStat.Size = new System.Drawing.Size(530, 20);
            this.textBoxStat.TabIndex = 8;
            this.textBoxStat.TabStop = false;
            // 
            // buttonPlayback
            // 
            this.buttonPlayback.Enabled = false;
            this.buttonPlayback.Location = new System.Drawing.Point(16, 277);
            this.buttonPlayback.Name = "buttonPlayback";
            this.buttonPlayback.Size = new System.Drawing.Size(89, 23);
            this.buttonPlayback.TabIndex = 4;
            this.buttonPlayback.Text = "Playback";
            this.buttonPlayback.UseVisualStyleBackColor = true;
            this.buttonPlayback.Click += new System.EventHandler(this.OnPlayback);
            // 
            // buttonLive
            // 
            this.buttonLive.Enabled = false;
            this.buttonLive.Location = new System.Drawing.Point(16, 306);
            this.buttonLive.Name = "buttonLive";
            this.buttonLive.Size = new System.Drawing.Size(89, 23);
            this.buttonLive.TabIndex = 5;
            this.buttonLive.Text = "Live";
            this.buttonLive.UseVisualStyleBackColor = true;
            this.buttonLive.Click += new System.EventHandler(this.OnLive);
            // 
            // textBoxHeader
            // 
            this.textBoxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxHeader.Location = new System.Drawing.Point(139, 106);
            this.textBoxHeader.Name = "textBoxHeader";
            this.textBoxHeader.ReadOnly = true;
            this.textBoxHeader.Size = new System.Drawing.Size(530, 20);
            this.textBoxHeader.TabIndex = 11;
            this.textBoxHeader.TabStop = false;
            // 
            // buttonLine
            // 
            this.buttonLine.Enabled = false;
            this.buttonLine.Location = new System.Drawing.Point(16, 204);
            this.buttonLine.Name = "buttonLine";
            this.buttonLine.Size = new System.Drawing.Size(89, 23);
            this.buttonLine.TabIndex = 3;
            this.buttonLine.Text = "Draw Line";
            this.buttonLine.UseVisualStyleBackColor = true;
            this.buttonLine.Click += new System.EventHandler(this.OnLine);
            // 
            // buttonPreviewImage
            // 
            this.buttonPreviewImage.Enabled = false;
            this.buttonPreviewImage.Location = new System.Drawing.Point(16, 346);
            this.buttonPreviewImage.Name = "buttonPreviewImage";
            this.buttonPreviewImage.Size = new System.Drawing.Size(89, 39);
            this.buttonPreviewImage.TabIndex = 6;
            this.buttonPreviewImage.Text = "Show Single Image";
            this.buttonPreviewImage.UseVisualStyleBackColor = true;
            this.buttonPreviewImage.Click += new System.EventHandler(this.OnShowSingleImage);
            // 
            // panelPlayback
            // 
            this.panelPlayback.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPlayback.Location = new System.Drawing.Point(139, 392);
            this.panelPlayback.Name = "panelPlayback";
            this.panelPlayback.Size = new System.Drawing.Size(530, 100);
            this.panelPlayback.TabIndex = 12;
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(139, 147);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(530, 225);
            this.pictureBox.TabIndex = 13;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageUserControl_MouseDown);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageUserControl_MouseUp);
            // 
            // comboBoxStream
            // 
            this.comboBoxStream.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStream.Enabled = false;
            this.comboBoxStream.FormattingEnabled = true;
            this.comboBoxStream.Location = new System.Drawing.Point(442, 54);
            this.comboBoxStream.Name = "comboBoxStream";
            this.comboBoxStream.Size = new System.Drawing.Size(227, 21);
            this.comboBoxStream.TabIndex = 14;
            this.comboBoxStream.SelectedIndexChanged += new System.EventHandler(this.OnStreamChanged);
            // 
            // VideoPreviewUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.comboBoxStream);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.panelPlayback);
            this.Controls.Add(this.buttonPreviewImage);
            this.Controls.Add(this.buttonLine);
            this.Controls.Add(this.textBoxHeader);
            this.Controls.Add(this.buttonLive);
            this.Controls.Add(this.buttonPlayback);
            this.Controls.Add(this.textBoxStat);
            this.Controls.Add(this.textBoxCameraName);
            this.Controls.Add(this.buttonCameraSelect);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.Name = "VideoPreviewUserControl";
            this.Size = new System.Drawing.Size(692, 580);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion


		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxName;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Button buttonCameraSelect;
		private System.Windows.Forms.TextBox textBoxCameraName;
		private System.Windows.Forms.TextBox textBoxStat;
		private System.Windows.Forms.Button buttonPlayback;
		private System.Windows.Forms.Button buttonLive;
		private System.Windows.Forms.TextBox textBoxHeader;
		private System.Windows.Forms.Button buttonLine;
		private System.Windows.Forms.Button buttonPreviewImage;
		private System.Windows.Forms.Panel panelPlayback;
		private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox comboBoxStream;
    }
}
