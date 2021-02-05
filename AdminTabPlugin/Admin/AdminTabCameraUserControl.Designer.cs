namespace AdminTabPlugin.Admin
{
    partial class AdminTabCameraUserControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelItemName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ptzGroupBox = new System.Windows.Forms.GroupBox();
            this.upLeftButton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.downLeftButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.homeButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upRightButton = new System.Windows.Forms.Button();
            this.rightButton = new System.Windows.Forms.Button();
            this.downRightButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.tiltNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.zoomNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.getAbsolutePositionButton = new System.Windows.Forms.Button();
            this.setAbsolutePositionButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.ptzGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tiltNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Associated object:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Property-1:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Property-2:";
            // 
            // labelItemName
            // 
            this.labelItemName.AutoSize = true;
            this.labelItemName.Location = new System.Drawing.Point(121, 30);
            this.labelItemName.Name = "labelItemName";
            this.labelItemName.Size = new System.Drawing.Size(0, 13);
            this.labelItemName.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelItemName);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(509, 116);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sample admin tab";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(124, 78);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(222, 20);
            this.textBox2.TabIndex = 7;
            this.textBox2.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(124, 52);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(222, 20);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // ptzGroupBox
            // 
            this.ptzGroupBox.Controls.Add(this.setAbsolutePositionButton);
            this.ptzGroupBox.Controls.Add(this.getAbsolutePositionButton);
            this.ptzGroupBox.Controls.Add(this.zoomNumericUpDown);
            this.ptzGroupBox.Controls.Add(this.tiltNumericUpDown);
            this.ptzGroupBox.Controls.Add(this.panNumericUpDown);
            this.ptzGroupBox.Controls.Add(this.label6);
            this.ptzGroupBox.Controls.Add(this.label5);
            this.ptzGroupBox.Controls.Add(this.label4);
            this.ptzGroupBox.Controls.Add(this.downRightButton);
            this.ptzGroupBox.Controls.Add(this.rightButton);
            this.ptzGroupBox.Controls.Add(this.upRightButton);
            this.ptzGroupBox.Controls.Add(this.downButton);
            this.ptzGroupBox.Controls.Add(this.homeButton);
            this.ptzGroupBox.Controls.Add(this.upButton);
            this.ptzGroupBox.Controls.Add(this.downLeftButton);
            this.ptzGroupBox.Controls.Add(this.leftButton);
            this.ptzGroupBox.Controls.Add(this.upLeftButton);
            this.ptzGroupBox.Location = new System.Drawing.Point(4, 126);
            this.ptzGroupBox.Name = "ptzGroupBox";
            this.ptzGroupBox.Size = new System.Drawing.Size(508, 129);
            this.ptzGroupBox.TabIndex = 7;
            this.ptzGroupBox.TabStop = false;
            this.ptzGroupBox.Text = "PTZ control";
            this.ptzGroupBox.Visible = false;
            // 
            // upLeftButton
            // 
            this.upLeftButton.Location = new System.Drawing.Point(23, 30);
            this.upLeftButton.Name = "upLeftButton";
            this.upLeftButton.Size = new System.Drawing.Size(27, 23);
            this.upLeftButton.TabIndex = 0;
            this.upLeftButton.UseVisualStyleBackColor = true;
            this.upLeftButton.Click += new System.EventHandler(this.upLeftButton_Click);
            // 
            // leftButton
            // 
            this.leftButton.Location = new System.Drawing.Point(23, 60);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(27, 23);
            this.leftButton.TabIndex = 1;
            this.leftButton.Text = "<-";
            this.leftButton.UseVisualStyleBackColor = true;
            this.leftButton.Click += new System.EventHandler(this.leftButton_Click);
            // 
            // downLeftButton
            // 
            this.downLeftButton.Location = new System.Drawing.Point(23, 89);
            this.downLeftButton.Name = "downLeftButton";
            this.downLeftButton.Size = new System.Drawing.Size(27, 23);
            this.downLeftButton.TabIndex = 2;
            this.downLeftButton.UseVisualStyleBackColor = true;
            this.downLeftButton.Click += new System.EventHandler(this.downLeftButton_Click);
            // 
            // upButton
            // 
            this.upButton.Location = new System.Drawing.Point(56, 30);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(27, 23);
            this.upButton.TabIndex = 3;
            this.upButton.Text = "^";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // homeButton
            // 
            this.homeButton.Location = new System.Drawing.Point(56, 60);
            this.homeButton.Name = "homeButton";
            this.homeButton.Size = new System.Drawing.Size(27, 23);
            this.homeButton.TabIndex = 4;
            this.homeButton.Text = "H";
            this.homeButton.UseVisualStyleBackColor = true;
            this.homeButton.Click += new System.EventHandler(this.homeButton_Click);
            // 
            // downButton
            // 
            this.downButton.Location = new System.Drawing.Point(56, 89);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(27, 23);
            this.downButton.TabIndex = 5;
            this.downButton.Text = "v";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upRightButton
            // 
            this.upRightButton.Location = new System.Drawing.Point(89, 30);
            this.upRightButton.Name = "upRightButton";
            this.upRightButton.Size = new System.Drawing.Size(27, 23);
            this.upRightButton.TabIndex = 6;
            this.upRightButton.UseVisualStyleBackColor = true;
            this.upRightButton.Click += new System.EventHandler(this.upRightButton_Click);
            // 
            // rightButton
            // 
            this.rightButton.Location = new System.Drawing.Point(89, 60);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(27, 23);
            this.rightButton.TabIndex = 7;
            this.rightButton.Text = "->";
            this.rightButton.UseVisualStyleBackColor = true;
            this.rightButton.Click += new System.EventHandler(this.rightButton_Click);
            // 
            // downRightButton
            // 
            this.downRightButton.Location = new System.Drawing.Point(89, 89);
            this.downRightButton.Name = "downRightButton";
            this.downRightButton.Size = new System.Drawing.Size(27, 23);
            this.downRightButton.TabIndex = 8;
            this.downRightButton.UseVisualStyleBackColor = true;
            this.downRightButton.Click += new System.EventHandler(this.downRightButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(150, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Pan:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(150, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Tilt:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(150, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Zoom:";
            // 
            // panNumericUpDown
            // 
            this.panNumericUpDown.DecimalPlaces = 2;
            this.panNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.panNumericUpDown.Location = new System.Drawing.Point(201, 33);
            this.panNumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.panNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.panNumericUpDown.Name = "panNumericUpDown";
            this.panNumericUpDown.Size = new System.Drawing.Size(54, 20);
            this.panNumericUpDown.TabIndex = 12;
            // 
            // tiltNumericUpDown
            // 
            this.tiltNumericUpDown.DecimalPlaces = 2;
            this.tiltNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.tiltNumericUpDown.Location = new System.Drawing.Point(201, 63);
            this.tiltNumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tiltNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.tiltNumericUpDown.Name = "tiltNumericUpDown";
            this.tiltNumericUpDown.Size = new System.Drawing.Size(54, 20);
            this.tiltNumericUpDown.TabIndex = 13;
            // 
            // zoomNumericUpDown
            // 
            this.zoomNumericUpDown.DecimalPlaces = 2;
            this.zoomNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.zoomNumericUpDown.Location = new System.Drawing.Point(201, 92);
            this.zoomNumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.zoomNumericUpDown.Name = "zoomNumericUpDown";
            this.zoomNumericUpDown.Size = new System.Drawing.Size(54, 20);
            this.zoomNumericUpDown.TabIndex = 14;
            // 
            // getAbsolutePositionButton
            // 
            this.getAbsolutePositionButton.Location = new System.Drawing.Point(270, 30);
            this.getAbsolutePositionButton.Name = "getAbsolutePositionButton";
            this.getAbsolutePositionButton.Size = new System.Drawing.Size(75, 23);
            this.getAbsolutePositionButton.TabIndex = 15;
            this.getAbsolutePositionButton.Text = "Get position";
            this.getAbsolutePositionButton.UseVisualStyleBackColor = true;
            this.getAbsolutePositionButton.Click += new System.EventHandler(this.getAbsolutePositionButton_Click);
            // 
            // setAbsolutePositionButton
            // 
            this.setAbsolutePositionButton.Location = new System.Drawing.Point(270, 60);
            this.setAbsolutePositionButton.Name = "setAbsolutePositionButton";
            this.setAbsolutePositionButton.Size = new System.Drawing.Size(75, 23);
            this.setAbsolutePositionButton.TabIndex = 16;
            this.setAbsolutePositionButton.Text = "Set position";
            this.setAbsolutePositionButton.UseVisualStyleBackColor = true;
            this.setAbsolutePositionButton.Click += new System.EventHandler(this.setAbsolutePositionButton_Click);
            // 
            // AdminTabCameraUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ptzGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "AdminTabCameraUserControl";
            this.Size = new System.Drawing.Size(527, 364);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ptzGroupBox.ResumeLayout(false);
            this.ptzGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tiltNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelItemName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox ptzGroupBox;
        private System.Windows.Forms.Button downRightButton;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.Button upRightButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button homeButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downLeftButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Button upLeftButton;
        private System.Windows.Forms.Button setAbsolutePositionButton;
        private System.Windows.Forms.Button getAbsolutePositionButton;
        private System.Windows.Forms.NumericUpDown zoomNumericUpDown;
        private System.Windows.Forms.NumericUpDown tiltNumericUpDown;
        private System.Windows.Forms.NumericUpDown panNumericUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}
