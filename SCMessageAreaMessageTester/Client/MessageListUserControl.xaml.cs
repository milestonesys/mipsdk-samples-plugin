using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using UserControl = System.Windows.Controls.UserControl;

namespace SCMessageAreaMessageTester.Client
{
    public partial class MessageListUserControl : UserControl
    {
        private readonly List<object> _messageRegistrationObjects = new List<object>();
        private readonly ObservableCollection<MessageModel> _messageModels = new ObservableCollection<MessageModel>();

        public MessageListUserControl()
        {
            InitializeComponent();

            _messageDataGrid.ItemsSource = _messageModels;
        }

        public void Init()
        {
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(SmartClientMessageButtonClickedIndicationReceiver, new MessageIdFilter(MessageId.SmartClient.SmartClientMessageButtonClickedIndication)));
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(SmartClientMessageRemovedIndicationReceiver, new MessageIdFilter(MessageId.SmartClient.SmartClientMessageRemovedIndication)));
        }

        public void Close()
        {
            foreach (object messageRegistrationObject in _messageRegistrationObjects)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(messageRegistrationObject);
            }
            _messageRegistrationObjects.Clear();
        }

        private void _messageDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _deleteButton.IsEnabled = _messageDataGrid.SelectedItem != null;
        }

        private void _addButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.PropertyChanged += messageModel_PropertyChanged;
            _messageModels.Add(messageModel);
            SendSmartClientMessageCommand(messageModel.MessageId);
        }

        private void _deleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageModel messageModel = _messageDataGrid.SelectedItem as MessageModel;
            if (messageModel != null)
            {
                _messageModels.Remove(messageModel);
                messageModel.PropertyChanged -= messageModel_PropertyChanged;
                SendSmartClientMessageCommand(messageModel.MessageId);
            }
        }

        void messageModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MessageModel messageModel = _messageModels.SingleOrDefault(x => x == sender);
            if (messageModel != null)
            {
                SendSmartClientMessageCommand(messageModel.MessageId);
            }
        }

        private object SmartClientMessageButtonClickedIndicationReceiver(Message message, FQID sender, FQID related)
        {
            foreach (MessageModel messageModel in _messageModels)
            {
                if (messageModel.MessageId == message.Data)
                {
                    MessageBox.Show("The button on '" + messageModel.Message + "' was clicked!");
                    break;
                }
            }
            return null;
        }

        private object SmartClientMessageRemovedIndicationReceiver(Message message, FQID sender, FQID related)
        {
            foreach (MessageModel messageModel in _messageModels)
            {
                if (messageModel.MessageId == message.Data)
                {
                    _messageModels.Remove(messageModel);
                    break;
                }
            }
            return null;
        }

        private void SendSmartClientMessageCommand(object messageId)
        {
            MessageModel messageModel = _messageModels.SingleOrDefault(x => x.MessageId == messageId);
            if (messageModel != null)
            {
                SmartClientMessageData smartClientMessageData = new SmartClientMessageData();
                smartClientMessageData.MessageId = messageModel.MessageId;
                smartClientMessageData.Message = string.IsNullOrWhiteSpace(messageModel.Message) ? "[Empty]" : messageModel.Message;
                smartClientMessageData.MessageType = messageModel.MessageType;
                smartClientMessageData.Priority = messageModel.Priority;        // not in use by the Smart Client
                smartClientMessageData.IsClosable = messageModel.IsClosable;
                smartClientMessageData.ButtonText = messageModel.ButtonText;
                smartClientMessageData.TaskState = messageModel.TaskState;
                smartClientMessageData.TaskProgress = messageModel.TaskProgress;
                smartClientMessageData.TaskText = messageModel.TaskText;

                Message message = new Message(MessageId.SmartClient.SmartClientMessageCommand, smartClientMessageData);
                EnvironmentManager.Instance.SendMessage(message);
            }
            else
            {
                SmartClientMessageData smartClientMessageData = new SmartClientMessageData();
                smartClientMessageData.MessageId = messageId;
                smartClientMessageData.Message = string.Empty;

                Message message = new Message(MessageId.SmartClient.SmartClientMessageCommand, smartClientMessageData);
                EnvironmentManager.Instance.SendMessage(message);
            }
        }
    }
}
