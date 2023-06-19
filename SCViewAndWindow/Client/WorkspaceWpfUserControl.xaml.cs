using System.Collections.Generic;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
    public partial class WorkspaceWpfUserControl : UserControl
    {
        public WorkspaceWpfUserControl()
        {
            InitializeComponent();
            PopulateWorkspaces();
        }

        private void PopulateWorkspaces()
        {
            List<Item> tempWSitems = ClientControl.Instance.GetWorkSpaceItems();
            foreach (var ws in tempWSitems)
            {
                listBoxWorkspaces.Items.Add(ws);
            }
         }

        private void OnSwitch(object sender, System.Windows.RoutedEventArgs e)
        {
            Item selected = listBoxWorkspaces.SelectedItem as Item;
            if (selected != null)
            {
                EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.ShowWorkSpaceCommand, selected.FQID), null, null);
            }
        }
    }
}
