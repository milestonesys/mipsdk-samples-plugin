using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{

    public partial class LensWpfUserControl : UserControl
    {
        public LensWpfUserControl()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            listBoxIndicator.Items.Clear();
            Type msgType = typeof(LensCommandData);
            FieldInfo[] info = msgType.GetFields();
            foreach (FieldInfo type in info)
            {
                if (type.IsLiteral)
                {
                    String name = type.ToString();
                    name = name.Substring(name.LastIndexOf(" ") + 1);
                    listBoxIndicator.Items.Add(name);
            	}
            }
        }

        private void OnFireIndicator(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new Message(MessageId.Control.LensCommand, listBoxIndicator.SelectedItem), null, null);
        }
    }
}

