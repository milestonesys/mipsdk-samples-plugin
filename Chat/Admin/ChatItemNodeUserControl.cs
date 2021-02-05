using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chat.Client;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Messaging;

namespace Chat.Admin
{
	public partial class ChatItemNodeUserControl : ItemNodeUserControl
	{

		public ChatItemNodeUserControl()
		{
			InitializeComponent();

			if (this.Site != null && this.Site.DesignMode)
			{
				// Avoid Init during design time in Visual Studio
			}
			else
			{
				Init(null);
			}
		}

		public override void Init(VideoOS.Platform.Item item)
		{
			chatSidePanelUserControl1.Init();
		}

		public override void Close()
		{
			chatSidePanelUserControl1.Close();
		}


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
			this.chatSidePanelUserControl1 = new Chat.Client.ChatSidePanelUserControl();
			this.SuspendLayout();
			// 
			// chatSidePanelUserControl1
			// 
			this.chatSidePanelUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chatSidePanelUserControl1.BackColor = System.Drawing.Color.Transparent;
			this.chatSidePanelUserControl1.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chatSidePanelUserControl1.Location = new System.Drawing.Point(20, 25);
			this.chatSidePanelUserControl1.Name = "chatSidePanelUserControl1";
			this.chatSidePanelUserControl1.Size = new System.Drawing.Size(522, 390);
			this.chatSidePanelUserControl1.TabIndex = 1;
			// 
			// ChatItemNodeUserControl
			// 
			this.Controls.Add(this.chatSidePanelUserControl1);
			this.Name = "ChatItemNodeUserControl";
			this.Size = new System.Drawing.Size(565, 470);
			this.ResumeLayout(false);

		}

		#endregion

		private Client.ChatSidePanelUserControl chatSidePanelUserControl1;


	}
}
