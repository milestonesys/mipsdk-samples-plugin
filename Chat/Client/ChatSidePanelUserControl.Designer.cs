using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Client
{
	partial class ChatSidePanelUserControl
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
			this.textBoxEntry = new System.Windows.Forms.TextBox();
			this.listBoxUsers = new System.Windows.Forms.ListBox();
			this.listBoxChat = new System.Windows.Forms.ListBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBoxEntry
			// 
			this.textBoxEntry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBoxEntry.Location = new System.Drawing.Point(4, 363);
			this.textBoxEntry.Name = "textBoxEntry";
			this.textBoxEntry.Size = new System.Drawing.Size(205, 22);
			this.textBoxEntry.TabIndex = 1;
			this.textBoxEntry.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
			this.textBoxEntry.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OnPreviewKeyDown);
			// 
			// listBoxUsers
			// 
			this.listBoxUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBoxUsers.FormattingEnabled = true;
			this.listBoxUsers.ItemHeight = 15;
			this.listBoxUsers.Location = new System.Drawing.Point(6, 17);
			this.listBoxUsers.Name = "listBoxUsers";
			this.listBoxUsers.ScrollAlwaysVisible = true;
			this.listBoxUsers.Size = new System.Drawing.Size(229, 105);
			this.listBoxUsers.TabIndex = 3;
			// 
			// listBoxChat
			// 
			this.listBoxChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBoxChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBoxChat.FormattingEnabled = true;
			this.listBoxChat.HorizontalScrollbar = true;
			this.listBoxChat.ItemHeight = 15;
			this.listBoxChat.Location = new System.Drawing.Point(6, 15);
			this.listBoxChat.Name = "listBoxChat";
			this.listBoxChat.ScrollAlwaysVisible = true;
			this.listBoxChat.Size = new System.Drawing.Size(229, 180);
			this.listBoxChat.TabIndex = 4;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.listBoxChat);
			this.groupBox1.Location = new System.Drawing.Point(4, 149);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(241, 208);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Chat activity";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.listBoxUsers);
			this.groupBox2.Location = new System.Drawing.Point(4, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(241, 139);
			this.groupBox2.TabIndex = 6;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Users online";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Font = new System.Drawing.Font("Wingdings 3", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.button1.Location = new System.Drawing.Point(217, 362);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(23, 23);
			this.button1.TabIndex = 7;
			this.button1.Text = "8";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnClickSend);
			// 
			// ChatSidePanelUserControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.textBoxEntry);
			this.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "ChatSidePanelUserControl";
			this.Size = new System.Drawing.Size(248, 389);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.TextBox textBoxEntry;
		private System.Windows.Forms.ListBox listBoxUsers;
		private System.Windows.Forms.ListBox listBoxChat;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button1;

	}
}
