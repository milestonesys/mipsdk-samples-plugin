namespace AdminMultiTab.Admin
{
    partial class AdminFirstTab
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
            this.Var1Textbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Var1Textbox
            // 
            this.Var1Textbox.Location = new System.Drawing.Point(63, 7);
            this.Var1Textbox.Name = "Var1Textbox";
            this.Var1Textbox.Size = new System.Drawing.Size(150, 20);
            this.Var1Textbox.TabIndex = 0;
            this.Var1Textbox.TextChanged += new System.EventHandler(this.Var1Textbox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Variable 1";
            // 
            // AdminFirstTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Var1Textbox);
            this.Name = "AdminFirstTab";
            this.Size = new System.Drawing.Size(304, 231);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Var1Textbox;
        private System.Windows.Forms.Label label1;
    }
}
