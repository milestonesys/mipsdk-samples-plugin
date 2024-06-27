using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{

    public partial class ViewCommandWpfUserControl : UserControl
    {

        public ViewCommandWpfUserControl()
        {
            InitializeComponent();
            GetCommands();
        }

        /// <summary>
        /// Gets ViewItem commands.
        /// </summary>
        private void GetCommands()
        {
            Type msgType = typeof(ViewItemControlCommandData);
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

        private void ButtonUpClick(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand,
                    new SetSelectedViewItemData() { MoveCommand=MoveCommand.MoveUp }));
        }

        private void ButtonLeftClick(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
               new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand,
                   new SetSelectedViewItemData() { MoveCommand = MoveCommand.MoveLeft }));
        }

        private void ButtonRightClick(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
               new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand,
                   new SetSelectedViewItemData() { MoveCommand = MoveCommand.MoveRight }));
        }

        private void ButtonDownClick(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
               new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand,
                   new SetSelectedViewItemData() { MoveCommand = MoveCommand.MoveDown }));
        }

        private void SelectIndexClick(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand,
                    new SetSelectedViewItemData() { MoveCommand = MoveCommand.MoveIndex, LayoutIndex=1 }));
        }

        private void FireIndicatorClick(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
               new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.ViewItemControlCommand, listBoxIndicator.SelectedItem),
               null,
               null);
        }
    }
}
