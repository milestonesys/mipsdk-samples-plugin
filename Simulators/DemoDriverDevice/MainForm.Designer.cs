namespace DemoDriverDevice
{
	partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.buttonClose = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelLoginState = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonOutOfCpuHardwareEvent = new System.Windows.Forms.Button();
            this.buttonRebootHardwareEvent = new System.Windows.Forms.Button();
            this.maskedTextBoxMac = new System.Windows.Forms.MaskedTextBox();
            this.buttonStop = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxLoginData = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableDiscovery = new System.Windows.Forms.CheckBox();
            this.cbScheme = new System.Windows.Forms.ComboBox();
            this.lblScheme = new System.Windows.Forms.Label();
            this.checkBoxIncludeOverlay = new System.Windows.Forms.CheckBox();
            this.workPanel = new DemoDriverDevice.WorkPanel();
            this.groupBoxLoginData.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(725, 423);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonCloseClick);
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(85, 61);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(109, 20);
            this.textBoxUser.TabIndex = 8;
            this.textBoxUser.Text = "root";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(85, 87);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(109, 20);
            this.textBoxPassword.TabIndex = 9;
            this.textBoxPassword.Text = "pass";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Credentials:";
            // 
            // labelLoginState
            // 
            this.labelLoginState.AutoSize = true;
            this.labelLoginState.Location = new System.Drawing.Point(225, 47);
            this.labelLoginState.Name = "labelLoginState";
            this.labelLoginState.Size = new System.Drawing.Size(51, 13);
            this.labelLoginState.TabIndex = 11;
            this.labelLoginState.Text = "No logins";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Port:";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(85, 35);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(109, 20);
            this.textBoxPort.TabIndex = 12;
            this.textBoxPort.Text = "22222";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(228, 8);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(127, 23);
            this.buttonStart.TabIndex = 14;
            this.buttonStart.Text = "Start service";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonOutOfCpuHardwareEvent
            // 
            this.buttonOutOfCpuHardwareEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutOfCpuHardwareEvent.Enabled = false;
            this.buttonOutOfCpuHardwareEvent.Location = new System.Drawing.Point(536, 13);
            this.buttonOutOfCpuHardwareEvent.Name = "buttonOutOfCpuHardwareEvent";
            this.buttonOutOfCpuHardwareEvent.Size = new System.Drawing.Size(263, 23);
            this.buttonOutOfCpuHardwareEvent.TabIndex = 15;
            this.buttonOutOfCpuHardwareEvent.Text = "Send Out of CPU or Memory hardware event";
            this.buttonOutOfCpuHardwareEvent.UseVisualStyleBackColor = true;
            this.buttonOutOfCpuHardwareEvent.Click += new System.EventHandler(this.buttonOutOfCpuHardwareEvent_Click);
            // 
            // buttonRebootHardwareEvent
            // 
            this.buttonRebootHardwareEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRebootHardwareEvent.Enabled = false;
            this.buttonRebootHardwareEvent.Location = new System.Drawing.Point(536, 42);
            this.buttonRebootHardwareEvent.Name = "buttonRebootHardwareEvent";
            this.buttonRebootHardwareEvent.Size = new System.Drawing.Size(264, 23);
            this.buttonRebootHardwareEvent.TabIndex = 16;
            this.buttonRebootHardwareEvent.Text = "Send Reboot hardware event";
            this.buttonRebootHardwareEvent.UseVisualStyleBackColor = true;
            this.buttonRebootHardwareEvent.Click += new System.EventHandler(this.buttonRebootHardwareEvent_Click);
            // 
            // maskedTextBoxMac
            // 
            this.maskedTextBoxMac.Location = new System.Drawing.Point(85, 113);
            this.maskedTextBoxMac.Name = "maskedTextBoxMac";
            this.maskedTextBoxMac.Size = new System.Drawing.Size(109, 20);
            this.maskedTextBoxMac.TabIndex = 18;
            this.maskedTextBoxMac.Text = "DE:AD:C0:DE:56:78";
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(361, 8);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(127, 23);
            this.buttonStop.TabIndex = 17;
            this.buttonStop.Text = "Stop service";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "MAC address:";
            // 
            // groupBoxLoginData
            // 
            this.groupBoxLoginData.Controls.Add(this.checkBoxEnableDiscovery);
            this.groupBoxLoginData.Controls.Add(this.cbScheme);
            this.groupBoxLoginData.Controls.Add(this.lblScheme);
            this.groupBoxLoginData.Controls.Add(this.label2);
            this.groupBoxLoginData.Controls.Add(this.maskedTextBoxMac);
            this.groupBoxLoginData.Controls.Add(this.label3);
            this.groupBoxLoginData.Controls.Add(this.textBoxPort);
            this.groupBoxLoginData.Controls.Add(this.label1);
            this.groupBoxLoginData.Controls.Add(this.textBoxUser);
            this.groupBoxLoginData.Controls.Add(this.textBoxPassword);
            this.groupBoxLoginData.Location = new System.Drawing.Point(12, 3);
            this.groupBoxLoginData.Name = "groupBoxLoginData";
            this.groupBoxLoginData.Size = new System.Drawing.Size(200, 172);
            this.groupBoxLoginData.TabIndex = 20;
            this.groupBoxLoginData.TabStop = false;
            this.groupBoxLoginData.Text = "Login data";
            // 
            // checkBoxEnableDiscovery
            // 
            this.checkBoxEnableDiscovery.AutoSize = true;
            this.checkBoxEnableDiscovery.Checked = true;
            this.checkBoxEnableDiscovery.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableDiscovery.Location = new System.Drawing.Point(11, 140);
            this.checkBoxEnableDiscovery.Name = "checkBoxEnableDiscovery";
            this.checkBoxEnableDiscovery.Size = new System.Drawing.Size(146, 17);
            this.checkBoxEnableDiscovery.TabIndex = 23;
            this.checkBoxEnableDiscovery.Text = "Enable express discovery";
            this.checkBoxEnableDiscovery.UseVisualStyleBackColor = true;
            // 
            // cbScheme
            // 
            this.cbScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScheme.FormattingEnabled = true;
            this.cbScheme.Items.AddRange(new object[] {
            "http",
            "https"});
            this.cbScheme.Location = new System.Drawing.Point(85, 9);
            this.cbScheme.Name = "cbScheme";
            this.cbScheme.Size = new System.Drawing.Size(109, 21);
            this.cbScheme.TabIndex = 22;
            // 
            // lblScheme
            // 
            this.lblScheme.AutoSize = true;
            this.lblScheme.Location = new System.Drawing.Point(7, 15);
            this.lblScheme.Name = "lblScheme";
            this.lblScheme.Size = new System.Drawing.Size(49, 13);
            this.lblScheme.TabIndex = 21;
            this.lblScheme.Text = "Scheme:";
            // 
            // checkBoxIncludeOverlay
            // 
            this.checkBoxIncludeOverlay.AutoSize = true;
            this.checkBoxIncludeOverlay.Location = new System.Drawing.Point(533, 72);
            this.checkBoxIncludeOverlay.Name = "checkBoxIncludeOverlay";
            this.checkBoxIncludeOverlay.Size = new System.Drawing.Size(118, 17);
            this.checkBoxIncludeOverlay.TabIndex = 21;
            this.checkBoxIncludeOverlay.Text = "Include text overlay";
            this.checkBoxIncludeOverlay.UseVisualStyleBackColor = true;
            // 
            // workPanel
            // 
            this.workPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.workPanel.Enabled = false;
            this.workPanel.Location = new System.Drawing.Point(12, 99);
            this.workPanel.Name = "workPanel";
            this.workPanel.Size = new System.Drawing.Size(691, 347);
            this.workPanel.TabIndex = 17;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(812, 458);
            this.Controls.Add(this.checkBoxIncludeOverlay);
            this.Controls.Add(this.groupBoxLoginData);
            this.Controls.Add(this.workPanel);
            this.Controls.Add(this.buttonRebootHardwareEvent);
            this.Controls.Add(this.buttonOutOfCpuHardwareEvent);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.labelLoginState);
            this.Controls.Add(this.buttonClose);
            this.Name = "MainForm";
            this.Text = "Demo Driver Device";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.groupBoxLoginData.ResumeLayout(false);
            this.groupBoxLoginData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelLoginState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonOutOfCpuHardwareEvent;
        private System.Windows.Forms.Button buttonRebootHardwareEvent;
        private WorkPanel workPanel;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxMac;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.GroupBox groupBoxLoginData;
        private System.Windows.Forms.ComboBox cbScheme;
        private System.Windows.Forms.Label lblScheme;
        private System.Windows.Forms.CheckBox checkBoxIncludeOverlay;
        private System.Windows.Forms.CheckBox checkBoxEnableDiscovery;
    }
}

