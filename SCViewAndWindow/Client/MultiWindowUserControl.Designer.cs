using System.Windows.Forms;

namespace SCViewAndWindow.Client
{
	partial class MultiWindowUserControl
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStripcopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxScreens = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxWindows = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBoxPlayback = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // contextMenuStripcopy
            // 
            this.contextMenuStripcopy.Name = "contextMenuStripcopy";
            this.contextMenuStripcopy.Size = new System.Drawing.Size(61, 4);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(18, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Smart Client View:";
            // 
            // comboBoxScreens
            // 
            this.comboBoxScreens.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxScreens.FormattingEnabled = true;
            this.comboBoxScreens.Location = new System.Drawing.Point(167, 16);
            this.comboBoxScreens.Name = "comboBoxScreens";
            this.comboBoxScreens.Size = new System.Drawing.Size(166, 21);
            this.comboBoxScreens.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(18, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Physical Screens:";
            // 
            // comboBoxWindows
            // 
            this.comboBoxWindows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWindows.FormattingEnabled = true;
            this.comboBoxWindows.Location = new System.Drawing.Point(167, 48);
            this.comboBoxWindows.Name = "comboBoxWindows";
            this.comboBoxWindows.Size = new System.Drawing.Size(166, 21);
            this.comboBoxWindows.TabIndex = 1;
            this.comboBoxWindows.Enter += new System.EventHandler(this.OnEnterWindows);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Smart Client Window:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(167, 126);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(166, 95);
            this.listBox1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(167, 84);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Select a view ...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnSelectView);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(167, 236);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Fire Command";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnFireCommand);
            // 
            // checkBoxPlayback
            // 
            this.checkBoxPlayback.AutoSize = true;
            this.checkBoxPlayback.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxPlayback.Location = new System.Drawing.Point(21, 204);
            this.checkBoxPlayback.Name = "checkBoxPlayback";
            this.checkBoxPlayback.Size = new System.Drawing.Size(98, 17);
            this.checkBoxPlayback.TabIndex = 9;
            this.checkBoxPlayback.Text = "Allow Playback";
            this.checkBoxPlayback.UseVisualStyleBackColor = true;
            // 
            // MultiWindowUserControl
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.checkBoxPlayback);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxWindows);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxScreens);
            this.Name = "MultiWindowUserControl";
            this.Size = new System.Drawing.Size(457, 369);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		internal ContextMenuStrip contextMenuStripcopy;
		internal ToolStripMenuItem copyToolStripMenuItem;
		private Label label2;
		private ComboBox comboBoxScreens;
		private Label label3;
		private ComboBox comboBoxWindows;
		private Label label4;
		private ListBox listBox1;
		private Button button1;
		private Button button2;
        private CheckBox checkBoxPlayback;

	}
}
