using ChatWithWebsockets.Client;
using System.Windows.Forms.Integration;
using VideoOS.Platform.Admin;

namespace ChatWithWebsockets.Admin
{
    public partial class ChatWithWebsocketsItemNodeUserControl : ItemNodeUserControl
    {
        public ChatWithWebsocketsItemNodeUserControl()
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
            chatWpfUserControl.Init();
        }

        public override void Close()
        {
            chatWpfUserControl.Close();
        }

        private ElementHost host;

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
            this.host = new ElementHost();
            this.chatWpfUserControl = new ChatSidePanelWpfUserControl();
            this.SuspendLayout();
            // 
            // host
            // 
            this.host.Dock = System.Windows.Forms.DockStyle.Fill;
            this.host.Location = new System.Drawing.Point(0, 0);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(394, 657);
            this.host.TabIndex = 0;
            this.host.Child = this.chatWpfUserControl;
            // 
            // ChatItemNodeUserControl
            // 
            this.Controls.Add(this.host);
            this.Name = "ChatItemNodeUserControl";
            this.Size = new System.Drawing.Size(394, 657);
            this.ResumeLayout(false);
        }

        #endregion

        private Client.ChatSidePanelWpfUserControl chatWpfUserControl = new ChatSidePanelWpfUserControl();
    }
}
