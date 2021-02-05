namespace SCWorkSpace.Client
{
    partial class SCWorkSpaceSidePanelUserControl
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
            this._shuffleWorkSpaceCamerasButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._maxCameraCountComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // _shuffleWorkSpaceCamerasButton
            // 
            this._shuffleWorkSpaceCamerasButton.Location = new System.Drawing.Point(11, 29);
            this._shuffleWorkSpaceCamerasButton.Name = "_shuffleWorkSpaceCamerasButton";
            this._shuffleWorkSpaceCamerasButton.Size = new System.Drawing.Size(70, 23);
            this._shuffleWorkSpaceCamerasButton.TabIndex = 6;
            this._shuffleWorkSpaceCamerasButton.Text = "Go";
            this._shuffleWorkSpaceCamerasButton.UseVisualStyleBackColor = true;
            this._shuffleWorkSpaceCamerasButton.Click += new System.EventHandler(this._shuffleWorkSpaceCamerasButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Shuffle WorkSpace plugin cameras";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Max Camera count:";
            // 
            // _maxCameraCountComboBox
            // 
            this._maxCameraCountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._maxCameraCountComboBox.Items.AddRange(new object[] {
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this._maxCameraCountComboBox.Location = new System.Drawing.Point(11, 96);
            this._maxCameraCountComboBox.Name = "_maxCameraCountComboBox";
            this._maxCameraCountComboBox.Size = new System.Drawing.Size(121, 21);
            this._maxCameraCountComboBox.TabIndex = 8;
            this._maxCameraCountComboBox.SelectedIndexChanged += new System.EventHandler(this._maxCameraCountComboBox_SelectedIndexChanged);
            // 
            // SCWorkSpaceSidePanelUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._maxCameraCountComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._shuffleWorkSpaceCamerasButton);
            this.Controls.Add(this.label1);
            this.Name = "SCWorkSpaceSidePanelUserControl";
            this.Size = new System.Drawing.Size(236, 144);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _shuffleWorkSpaceCamerasButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _maxCameraCountComboBox;
    }
}
