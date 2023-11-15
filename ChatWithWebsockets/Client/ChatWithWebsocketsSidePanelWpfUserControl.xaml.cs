using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace ChatWithWebsockets.Client
{
    public partial class ChatSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        private Guid _subscriptionId;
        private string _myName = "";

        internal static string ChatSampleTopic = "samples.chat";
        private IMessageClient _messageClient;

        private string _myMessage = string.Empty;

        public ChatSidePanelWpfUserControl()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            try
            {
                MessageClientManager.Instance.GetClientAsync(EnvironmentManager.Instance.MasterSite.ServerId, CancellationToken.None)
                    .ContinueWith(OnMessageClientCreatedAsync);
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("Chat.Init", ex);
            }
        }

        private async Task OnMessageClientCreatedAsync(Task<IMessageClient> clientGetTask)
        {
            _messageClient = clientGetTask.Result;

            _subscriptionId = await _messageClient.SubscribeAsync<ChatData>(ChatSampleTopic, MessageReceiverAsync, CancellationToken.None);

            SendHelloWordMessage();
        }

        private void SendHelloWordMessage()
        {
            LoginSettings ls = LoginSettingsCache.GetLoginSettings(EnvironmentManager.Instance.MasterSite);
            _myName = ls.UserName;
            if (_myName.Contains("\\"))
            {
                _myName = _myName.Split('\\')[1];
            }

            PublishMessage("Hello word! from " + _myName);
        }

        public Task MessageReceiverAsync(IMessageReceiverArgs message)
        {
            var chatData = message?.Data as ChatData;
            if (chatData != null)
            {
                // As we are subscribed to the same topic that we publish on, should implement
                // some (very naive) filtering (separation) on our and other's messages
                if (_myMessage == chatData.textMessage)
                {
                    _myMessage = string.Empty;
                }
                else
                {
                    AddMessageToUi(chatData.textMessage, false);
                }
            }
            
            return Task.CompletedTask;
        }

        private void AddMessageToUi(string message, bool isMyMessage)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action<string, bool>(AddMessageToUi), message, isMyMessage);
            }
            else
            {
                var item = new TextBlock
                {
                    Text = message,
                    TextAlignment = isMyMessage ? TextAlignment.Right : TextAlignment.Left,
                };
                listBoxChat.Items.Add(item);
            }
        }

        public override void Close()
        {
            _messageClient?.UnsubscribeAsync(_subscriptionId, CancellationToken.None);
            _messageClient = null;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnClickSend(sender, e);
                e.Handled = true;
            }
        }

        private bool IsEmptyOrAllSpaces(string str)
        {
            return null != str && str.All(c => c.Equals(' '));
        }

        private void OnClickSend(object sender, RoutedEventArgs e)
        {
            if (!IsEmptyOrAllSpaces(textBoxEntry.Text))
            {
                textBoxEntry.Text = PublishMessage(textBoxEntry.Text);
            }
        }

        private string PublishMessage(string message)
        {
            ChatData data = new ChatData()
            {
                textMessage = message
            };

            try
            {
                _myMessage = message;
                _messageClient.PublishAsync(ChatSampleTopic, data, CancellationToken.None);
                AddMessageToUi(message, true);
                message = "";
            }
            catch (Exception)
            {
                message += " (unable to send)";
            }

            return message;
        }
    }

    public class ChatData
    {
        public string textMessage { get; set; }
    }
}
