using System;
using System.Windows.Controls;
using VideoOS.Platform.Messaging;

namespace MessageTester.MessageDataControls
{
    /// <summary>
    /// Interaction logic for SmartClientMessageCommandUserControl.xaml
    /// </summary>
    public partial class SmartClientMessageCommandUserControl : MessageDataSuper
    {

        public override object Data { get { return GetData(); } }
        
        public SmartClientMessageCommandUserControl()
        {
            InitializeComponent();

            foreach (object value in Enum.GetValues(typeof(SmartClientMessageDataPriority)))
            {
                _priority.Items.Add(value);
            }
            _priority.SelectedIndex = 0;

            foreach (object value in Enum.GetValues(typeof(SmartClientMessageDataType)))
            {
                _type.Items.Add(value);
            }
            _type.SelectedIndex = 0;
        }

        private SmartClientMessageData GetData()
        {
            SmartClientMessageData data = new SmartClientMessageData();
            data.MessageId = _id.Text;
            data.Message = _text.Text;
            data.Priority = (SmartClientMessageDataPriority) _priority.SelectedItem;
            data.MessageType = (SmartClientMessageDataType) _type.SelectedItem;
            data.IsClosable = _isClosable.IsChecked.Value;
            
            return data;
        }

        private void _id_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_id.Text != string.Empty)
            {
                IsReadyToSend = true;
            }
            else
            {
                IsReadyToSend = false;
            }
        }
    }
}
