namespace DemoDriverDevice
{
    partial class WorkPanel
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
            this.panelTimeDisplay = new System.Windows.Forms.Panel();
            this.deviceLog = new System.Windows.Forms.ListBox();
            this.labelLog = new System.Windows.Forms.Label();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSessionCount = new System.Windows.Forms.TextBox();
            this.checkBoxInput1 = new System.Windows.Forms.CheckBox();
            this.textBoxLPR = new System.Windows.Forms.TextBox();
            this.checkBoxInput2 = new System.Windows.Forms.CheckBox();
            this.buttonMotionDetectStart = new System.Windows.Forms.Button();
            this.buttonMotionDetectStop = new System.Windows.Forms.Button();
            this.buttonLprEvent = new System.Windows.Forms.Button();
            this._firmwareUpgradeProgressBar = new System.Windows.Forms.ProgressBar();
            this._firmwareUpgradeLabel = new System.Windows.Forms.Label();
            this.panelTimeDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTimeDisplay
            // 
            this.panelTimeDisplay.BackColor = System.Drawing.Color.Azure;
            this.panelTimeDisplay.Controls.Add(this.deviceLog);
            this.panelTimeDisplay.Controls.Add(this.labelLog);
            this.panelTimeDisplay.Controls.Add(this.textBoxTime);
            this.panelTimeDisplay.Location = new System.Drawing.Point(213, 37);
            this.panelTimeDisplay.Name = "panelTimeDisplay";
            this.panelTimeDisplay.Size = new System.Drawing.Size(475, 305);
            this.panelTimeDisplay.TabIndex = 1;
            // 
            // deviceLog
            // 
            this.deviceLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceLog.FormattingEnabled = true;
            this.deviceLog.HorizontalScrollbar = true;
            this.deviceLog.Location = new System.Drawing.Point(6, 55);
            this.deviceLog.Name = "deviceLog";
            this.deviceLog.Size = new System.Drawing.Size(466, 238);
            this.deviceLog.TabIndex = 14;
            this.deviceLog.TabStop = false;
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(3, 39);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(78, 13);
            this.labelLog.TabIndex = 12;
            this.labelLog.Text = "Device actions";
            // 
            // textBoxTime
            // 
            this.textBoxTime.Location = new System.Drawing.Point(6, 16);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.ReadOnly = true;
            this.textBoxTime.Size = new System.Drawing.Size(143, 20);
            this.textBoxTime.TabIndex = 0;
            this.textBoxTime.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Sessions:";
            // 
            // textBoxSessionCount
            // 
            this.textBoxSessionCount.Location = new System.Drawing.Point(101, 140);
            this.textBoxSessionCount.Name = "textBoxSessionCount";
            this.textBoxSessionCount.ReadOnly = true;
            this.textBoxSessionCount.Size = new System.Drawing.Size(44, 20);
            this.textBoxSessionCount.TabIndex = 1;
            this.textBoxSessionCount.TabStop = false;
            this.textBoxSessionCount.Text = "0";
            // 
            // checkBoxInput1
            // 
            this.checkBoxInput1.AutoSize = true;
            this.checkBoxInput1.Location = new System.Drawing.Point(15, 166);
            this.checkBoxInput1.Name = "checkBoxInput1";
            this.checkBoxInput1.Size = new System.Drawing.Size(101, 17);
            this.checkBoxInput1.TabIndex = 9;
            this.checkBoxInput1.Text = "Activate Input 1";
            this.checkBoxInput1.UseVisualStyleBackColor = true;
            this.checkBoxInput1.CheckedChanged += new System.EventHandler(this.checkBoxInput1_CheckedChanged);
            // 
            // textBoxLPR
            // 
            this.textBoxLPR.Location = new System.Drawing.Point(95, 274);
            this.textBoxLPR.Name = "textBoxLPR";
            this.textBoxLPR.Size = new System.Drawing.Size(65, 20);
            this.textBoxLPR.TabIndex = 12;
            // 
            // checkBoxInput2
            // 
            this.checkBoxInput2.AutoSize = true;
            this.checkBoxInput2.Location = new System.Drawing.Point(15, 189);
            this.checkBoxInput2.Name = "checkBoxInput2";
            this.checkBoxInput2.Size = new System.Drawing.Size(101, 17);
            this.checkBoxInput2.TabIndex = 13;
            this.checkBoxInput2.Text = "Activate Input 2";
            this.checkBoxInput2.UseVisualStyleBackColor = true;
            this.checkBoxInput2.CheckedChanged += new System.EventHandler(this.checkBoxInput2_CheckedChanged);
            // 
            // buttonMotionDetectStart
            // 
            this.buttonMotionDetectStart.Location = new System.Drawing.Point(15, 213);
            this.buttonMotionDetectStart.Name = "buttonMotionDetectStart";
            this.buttonMotionDetectStart.Size = new System.Drawing.Size(146, 23);
            this.buttonMotionDetectStart.TabIndex = 14;
            this.buttonMotionDetectStart.Text = "Motion detect start event";
            this.buttonMotionDetectStart.UseVisualStyleBackColor = true;
            this.buttonMotionDetectStart.Click += new System.EventHandler(this.buttonMotionDetectStart_Click);
            // 
            // buttonMotionDetectStop
            // 
            this.buttonMotionDetectStop.Location = new System.Drawing.Point(14, 242);
            this.buttonMotionDetectStop.Name = "buttonMotionDetectStop";
            this.buttonMotionDetectStop.Size = new System.Drawing.Size(147, 23);
            this.buttonMotionDetectStop.TabIndex = 15;
            this.buttonMotionDetectStop.Text = "Motion detect stop event";
            this.buttonMotionDetectStop.UseVisualStyleBackColor = true;
            this.buttonMotionDetectStop.Click += new System.EventHandler(this.buttonMotionDetectStop_Click);
            // 
            // buttonLprEvent
            // 
            this.buttonLprEvent.Location = new System.Drawing.Point(14, 271);
            this.buttonLprEvent.Name = "buttonLprEvent";
            this.buttonLprEvent.Size = new System.Drawing.Size(75, 23);
            this.buttonLprEvent.TabIndex = 16;
            this.buttonLprEvent.Text = "LPR event";
            this.buttonLprEvent.UseVisualStyleBackColor = true;
            this.buttonLprEvent.Click += new System.EventHandler(this.buttonLprEvent_Click);
            // 
            // _firmwareUpgradeProgressBar
            // 
            this._firmwareUpgradeProgressBar.Location = new System.Drawing.Point(356, 8);
            this._firmwareUpgradeProgressBar.Name = "_firmwareUpgradeProgressBar";
            this._firmwareUpgradeProgressBar.Size = new System.Drawing.Size(332, 23);
            this._firmwareUpgradeProgressBar.Step = 5;
            this._firmwareUpgradeProgressBar.TabIndex = 25;
            this._firmwareUpgradeProgressBar.Visible = false;
            // 
            // _firmwareUpgradeLabel
            // 
            this._firmwareUpgradeLabel.AutoSize = true;
            this._firmwareUpgradeLabel.Location = new System.Drawing.Point(213, 12);
            this._firmwareUpgradeLabel.Name = "_firmwareUpgradeLabel";
            this._firmwareUpgradeLabel.Size = new System.Drawing.Size(137, 13);
            this._firmwareUpgradeLabel.TabIndex = 24;
            this._firmwareUpgradeLabel.Text = "Firmware upgrade progress:";
            this._firmwareUpgradeLabel.Visible = false;
            // 
            // WorkPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this._firmwareUpgradeProgressBar);
            this.Controls.Add(this._firmwareUpgradeLabel);
            this.Controls.Add(this.buttonLprEvent);
            this.Controls.Add(this.buttonMotionDetectStop);
            this.Controls.Add(this.buttonMotionDetectStart);
            this.Controls.Add(this.checkBoxInput2);
            this.Controls.Add(this.textBoxLPR);
            this.Controls.Add(this.checkBoxInput1);
            this.Controls.Add(this.textBoxSessionCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelTimeDisplay);
            this.Name = "WorkPanel";
            this.Size = new System.Drawing.Size(691, 348);
            this.panelTimeDisplay.ResumeLayout(false);
            this.panelTimeDisplay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelTimeDisplay;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSessionCount;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.ListBox deviceLog;
        private System.Windows.Forms.CheckBox checkBoxInput1;
        private System.Windows.Forms.TextBox textBoxLPR;
        private System.Windows.Forms.CheckBox checkBoxInput2;
        private System.Windows.Forms.Button buttonMotionDetectStart;
        private System.Windows.Forms.Button buttonMotionDetectStop;
        private System.Windows.Forms.Button buttonLprEvent;
        private System.Windows.Forms.Label _firmwareUpgradeLabel;
        private System.Windows.Forms.ProgressBar _firmwareUpgradeProgressBar;
    }
}
