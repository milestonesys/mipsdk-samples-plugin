using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
    public partial class WorkspaceUserControl : UserControl
    {
        public WorkspaceUserControl()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                List<Item> tempWSitems = ClientControl.Instance.GetWorkSpaceItems();
                foreach (var ws in tempWSitems)
                {
                    listBox1.Items.Add(ws);
                }
            }
        }

        private void OnSwitch(object sender, EventArgs e)
        {
            Item selected = listBox1.SelectedItem as Item;
            if (selected != null)
            {
                EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.ShowWorkSpaceCommand, selected.FQID), null, null);
            }
        }
    }
}
