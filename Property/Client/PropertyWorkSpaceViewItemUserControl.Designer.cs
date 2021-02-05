namespace Property.Client
{
    partial class PropertyWorkSpaceViewItemUserControl
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxSharedUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxShareGlobal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(19, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(283, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Property workSpace View Item";
            // 
            // textBoxSharedUser
            // 
            this.textBoxSharedUser.Location = new System.Drawing.Point(19, 141);
            this.textBoxSharedUser.Name = "textBoxSharedUser";
            this.textBoxSharedUser.Size = new System.Drawing.Size(161, 20);
            this.textBoxSharedUser.TabIndex = 17;
            this.textBoxSharedUser.TextChanged += new System.EventHandler(this.textBoxSharedUser_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Shared user-private";
            // 
            // textBoxShareGlobal
            // 
            this.textBoxShareGlobal.Location = new System.Drawing.Point(19, 102);
            this.textBoxShareGlobal.Name = "textBoxShareGlobal";
            this.textBoxShareGlobal.Size = new System.Drawing.Size(161, 20);
            this.textBoxShareGlobal.TabIndex = 15;
            this.textBoxShareGlobal.TextChanged += new System.EventHandler(this.textBoxShareGlobal_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Shared globally";
            // 
            // PropertyWorkSpaceViewItemUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.textBoxSharedUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxShareGlobal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "PropertyWorkSpaceViewItemUserControl";
            this.Size = new System.Drawing.Size(378, 307);
            this.Click += new System.EventHandler(this.ViewItemUserControlClick);
            this.DoubleClick += new System.EventHandler(this.ViewItemUserControlDoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxSharedUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxShareGlobal;
        private System.Windows.Forms.Label label1;
    }
}
