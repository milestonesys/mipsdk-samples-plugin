namespace LocationView.Client.Config
{
    partial class MarkerChangeForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelDevice = new System.Windows.Forms.Label();
            this.buttonDevice = new System.Windows.Forms.Button();
            this.labelMarker = new System.Windows.Forms.Label();
            this.comboBoxMarker = new System.Windows.Forms.ComboBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelDevice
            // 
            this.labelDevice.AutoSize = true;
            this.labelDevice.Location = new System.Drawing.Point(12, 17);
            this.labelDevice.Name = "labelDevice";
            this.labelDevice.Size = new System.Drawing.Size(43, 13);
            this.labelDevice.TabIndex = 0;
            this.labelDevice.Text = "Device:";
            // 
            // buttonDevice
            // 
            this.buttonDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDevice.Location = new System.Drawing.Point(62, 12);
            this.buttonDevice.Name = "buttonDevice";
            this.buttonDevice.Size = new System.Drawing.Size(155, 23);
            this.buttonDevice.TabIndex = 1;
            this.buttonDevice.Text = "Select device...";
            this.buttonDevice.UseVisualStyleBackColor = true;
            this.buttonDevice.Click += new System.EventHandler(this.ButtonDeviceClick);
            // 
            // labelMarker
            // 
            this.labelMarker.AutoSize = true;
            this.labelMarker.Location = new System.Drawing.Point(12, 44);
            this.labelMarker.Name = "labelMarker";
            this.labelMarker.Size = new System.Drawing.Size(44, 13);
            this.labelMarker.TabIndex = 2;
            this.labelMarker.Text = "Marker:";
            // 
            // comboBoxMarker
            // 
            this.comboBoxMarker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMarker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMarker.FormattingEnabled = true;
            this.comboBoxMarker.Location = new System.Drawing.Point(62, 41);
            this.comboBoxMarker.Name = "comboBoxMarker";
            this.comboBoxMarker.Size = new System.Drawing.Size(155, 21);
            this.comboBoxMarker.TabIndex = 3;
            this.comboBoxMarker.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMarkerSelectedIndexChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(61, 68);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(142, 68);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // MarkerChangeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 101);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.comboBoxMarker);
            this.Controls.Add(this.labelMarker);
            this.Controls.Add(this.buttonDevice);
            this.Controls.Add(this.labelDevice);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(444, 140);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(244, 140);
            this.Name = "MarkerChangeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MarkerChangeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDevice;
        private System.Windows.Forms.Button buttonDevice;
        private System.Windows.Forms.Label labelMarker;
        private System.Windows.Forms.ComboBox comboBoxMarker;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}