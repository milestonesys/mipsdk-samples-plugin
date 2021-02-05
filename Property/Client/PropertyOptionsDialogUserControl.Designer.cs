namespace Property.Client
{
    partial class PropertyOptionsDialogUserControl
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
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPropValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxShareGlobal = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSharedUser = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Property Value";
            // 
            // textBoxPropValue
            // 
            this.textBoxPropValue.Location = new System.Drawing.Point(6, 20);
            this.textBoxPropValue.Name = "textBoxPropValue";
            this.textBoxPropValue.Size = new System.Drawing.Size(158, 22);
            this.textBoxPropValue.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Shared globally";
            // 
            // textBoxShareGlobal
            // 
            this.textBoxShareGlobal.Location = new System.Drawing.Point(6, 64);
            this.textBoxShareGlobal.Name = "textBoxShareGlobal";
            this.textBoxShareGlobal.Size = new System.Drawing.Size(158, 22);
            this.textBoxShareGlobal.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Shared user-private";
            // 
            // textBoxSharedUser
            // 
            this.textBoxSharedUser.Location = new System.Drawing.Point(3, 108);
            this.textBoxSharedUser.Name = "textBoxSharedUser";
            this.textBoxSharedUser.Size = new System.Drawing.Size(161, 22);
            this.textBoxSharedUser.TabIndex = 8;
            // 
            // PropertyOptionsDialogUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.textBoxSharedUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxShareGlobal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPropValue);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PropertyOptionsDialogUserControl";
            this.Size = new System.Drawing.Size(269, 287);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPropValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxShareGlobal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSharedUser;
    }
}
