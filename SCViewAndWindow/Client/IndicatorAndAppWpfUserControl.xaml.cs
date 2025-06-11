using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
    public partial class IndicatorAndAppWpfUserControl : UserControl
    {
        public IndicatorAndAppWpfUserControl()
        {
            InitializeComponent();
            Setup();
        }

        /// <summary>
        /// Gets indicator and application commands.
        /// </summary>
        private void Setup()
        {
            Type msgType = typeof(ClearIndicatorCommandData);
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
            msgType = typeof(ApplicationControlCommandData);
            info = msgType.GetFields();
            foreach (FieldInfo type in info)
            {
                if (type.IsLiteral)
                {
                    String name = type.ToString();
                    name = name.Substring(name.LastIndexOf(" ") + 1);
                    listBoxApp.Items.Add(name);
                }
            }
        }

        private void OnFireIndicator(object sender, System.Windows.RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new Message(MessageId.SmartClient.ClearIndicatorCommand, listBoxIndicator.SelectedItem),
                null,
                null);
        }

        private void OnFireApp(object sender, System.Windows.RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new Message(MessageId.SmartClient.ApplicationControlCommand, listBoxApp.SelectedItem),
                null,
                null);
        }

        private void OnLiveMode(object sender, System.Windows.RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new Message(MessageId.SmartClient.ChangeModeCommand, Mode.ClientLive),
                GetWindow());
        }

        private void OnPlaybackMode(object sender, System.Windows.RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new Message(MessageId.SmartClient.ChangeModeCommand, Mode.ClientPlayback),
                GetWindow());
        }

        /// <summary>
        /// Gets selected Smart Client window.
        /// </summary>
        /// <returns></returns>
        private FQID GetWindow()
        {
            ConfigItem windowItem = (ConfigItem)comboBoxWindows.SelectedItem;
            if (windowItem != null)
                return windowItem.FQID;
            return null;
        }

        /// <summary>
        /// Creates a message with specified priority.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMakeMessage(object sender, System.Windows.RoutedEventArgs e)
        {
            SmartClientMessageData data = new SmartClientMessageData();
            data.Message = "Test message with " + ((Button)sender).Content.ToString().ToLower() + " priority";
            data.Priority = SmartClientMessageDataPriority.Normal;
            if ((string)(((Button)sender).Content) == "High")
                data.Priority = SmartClientMessageDataPriority.High;
            if ((string)(((Button)sender).Content) == "Low")
                data.Priority = SmartClientMessageDataPriority.Low;
            data.MessageId = Guid.NewGuid();
            data.IsClosable = true;
            EnvironmentManager.Instance.SendMessage(
                new Message(MessageId.SmartClient.SmartClientMessageCommand, data));
        }

        /// <summary>
        /// Gets all Smart Client Windows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadWindows(object sender, EventArgs e)
        {
            comboBoxWindows.Items.Clear();
            List<Item> list = Configuration.Instance.GetItemsByKind(Kind.Window);
            foreach (Item item in list)
            {
                comboBoxWindows.Items.Add(item);
            }
        }
    }
}
