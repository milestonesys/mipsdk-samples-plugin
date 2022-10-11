using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;

namespace Chat.Client
{
    public partial class ChatSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        private object _obj1, _obj2, _obj3, _obj4;
        private string _myName = "";

        internal static string MessageIdChatLine = "Chat.Line";
        internal Collection<EndPointIdentityData> AllEndPoints = null;
        private MessageCommunication _messageCommunication;

        public ChatSidePanelWpfUserControl()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            try
            {
                MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
                _messageCommunication = MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);
                _messageCommunication.ConnectionStateChangedEvent += new EventHandler(_messageCommunication_ConnectionStateChangedEvent);
                _obj1 = _messageCommunication.RegisterCommunicationFilter(WhoAreOnlineResponseHandler, new CommunicationIdFilter(MessageCommunication.WhoAreOnlineResponse));
                _obj2 = _messageCommunication.RegisterCommunicationFilter(NewEndPointHandler, new CommunicationIdFilter(MessageCommunication.NewEndPointIndication));
                _obj3 = _messageCommunication.RegisterCommunicationFilter(EndPointTableChangedHandler, new CommunicationIdFilter(MessageCommunication.EndPointTableChangedIndication));
                _obj4 = _messageCommunication.RegisterCommunicationFilter(ChatLineHandler, new CommunicationIdFilter(MessageIdChatLine));

                LoginSettings ls = LoginSettingsCache.GetLoginSettings(EnvironmentManager.Instance.MasterSite);
                _myName = ls.UserName;
                if (_myName.Contains("\\"))
                {
                    _myName = _myName.Split('\\')[1];
                }
                if (_messageCommunication.IsConnected)
                {
                    _messageCommunication.TransmitMessage(
                        new Message(MessageCommunication.WhoAreOnlineRequest), null, null,
                        null);
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("Chat.Init", ex);
            }
        }

        void _messageCommunication_ConnectionStateChangedEvent(object sender, EventArgs e)
        {
            if (_messageCommunication.IsConnected)
            {
                try
                {
                    _messageCommunication.TransmitMessage(
                        new Message(MessageCommunication.WhoAreOnlineRequest), null, null,
                        null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chat: " + ex.Message);
                }
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    this.IsEnabled = false; }));
            }
        }

        public override void Close()
        {
            _messageCommunication.ConnectionStateChangedEvent -= new EventHandler(_messageCommunication_ConnectionStateChangedEvent);
            _messageCommunication.UnRegisterCommunicationFilter(_obj1);
            _messageCommunication.UnRegisterCommunicationFilter(_obj2);
            _messageCommunication.UnRegisterCommunicationFilter(_obj3);
            _messageCommunication.UnRegisterCommunicationFilter(_obj4);
            _obj1 = _obj2 = _obj3 = _obj4 = null;
        }

        private object ChatLineHandler(Message message, FQID dest, FQID source)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new MessageReceiver(ChatLineHandler), message, dest, source);
            }
            else
            {
                ChatData chatData = message.Data as ChatData;
                if (chatData != null)
                {
                    listBoxChat.Items.Add(chatData.Source.IdentityName + ": " + chatData.Entry);
                }
            }
            return null;
        }

        private object WhoAreOnlineResponseHandler(Message message, FQID dest, FQID source)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new MessageReceiver(WhoAreOnlineResponseHandler), message, dest, source);
            }
            else
            {
                this.IsEnabled = true;
                Collection<EndPointIdentityData> result = message.Data as Collection<EndPointIdentityData>;
                AllEndPoints = result;
                if (result != null)
                {
                    listBoxUsers.Items.Clear();
                    foreach (EndPointIdentityData user in result)
                    {
                        AddUser(user);
                    }
                }
            }
            return null;
        }

        private void AddUser(EndPointIdentityData endPointIdentityData)
        {
            string shortName = endPointIdentityData.IdentityName;
            if (shortName.Contains("\\"))
            {
                shortName = shortName.Split('\\')[1];
            }
            shortName += " (" + endPointIdentityData.EndPointFQID.ServerId.ServerType + ") ";
            listBoxUsers.Items.Add(shortName);
        }

        private object NewEndPointHandler(Message message, FQID dest, FQID source)
        {
            EndPointIdentityData user = message.Data as EndPointIdentityData;
            if (user != null)
            {
                if (!Dispatcher.CheckAccess())
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        AddUser(user); }));
                else
                    AddUser(user);
            }
            return null;
        }

        private object EndPointTableChangedHandler(Message message, FQID dest, FQID source)
        {
            _messageCommunication.TransmitMessage(
                new Message(MessageCommunication.WhoAreOnlineRequest), null, null, null);
            return null;
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
            ChatData data = new ChatData()
            {
                Source = new EndPointIdentityData()
                {
                    IdentityName = _myName,
                    EndPointFQID =MessageCommunicationManager.EndPointFQID
                },
                Entry = textBoxEntry.Text
            };

            if (!IsEmptyOrAllSpaces(textBoxEntry.Text))
            {

                try
                {
                    _messageCommunication.TransmitMessage(new Message(MessageIdChatLine, data), null, null, null);
                }
                catch (Exception)
                {
                    textBoxEntry.Text += " (unable to send)";
                }
            }
            textBoxEntry.Text = "";
        }
    }

    [Serializable]
    public class ChatData
    {
        public EndPointIdentityData Source;
        public String Entry;
    }
}
