using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCWorkSpace.Client
{
    public partial class SCWorkSpaceViewItemUserControl : ViewItemUserControl
    {
        private List<object> _messageRegistrationObjects = new List<object>();

        public SCWorkSpaceViewItemUserControl()
        {
            InitializeComponent();

			ClientControl.Instance.RegisterUIControlForAutoTheming(this);
        }

        public override void Init()
        {
            base.Init();

            //add message listeners
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(ShownWorkSpaceChangedReceiver, new MessageIdFilter(MessageId.SmartClient.ShownWorkSpaceChangedIndication)));
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(WorkSpaceStateChangedReceiver, new MessageIdFilter(MessageId.SmartClient.WorkSpaceStateChangedIndication)));

            //init _selectWorkSpaceComboBox
            _selectWorkSpaceComboBox.Items.Clear();
            foreach (Item workSpaceItem in ClientControl.Instance.GetWorkSpaceItems())
            {
                _selectWorkSpaceComboBox.Items.Add(workSpaceItem);
            }

            //init _selectWorkSpaceStateComboBox
            _selectWorkSpaceStateComboBox.Items.Clear();
            foreach (string workSpaceState in Enum.GetNames(typeof (WorkSpaceState)))
            {
                _selectWorkSpaceStateComboBox.Items.Add(workSpaceState);
            }

            //init _selectedWorkSpaceLabel
            Item wsName = ClientControl.Instance.ShownWorkSpace;
            _selectedWorkSpaceLabel.Text = wsName!=null?wsName.Name : "";

            //init _selectedWorkSpaceStateLabel
            _selectedWorkSpaceStateLabel.Text = ClientControl.Instance.WorkSpaceState.ToString();
        }

        public override void Close()
        {
            base.Close();

            foreach (object messageRegistrationObject in _messageRegistrationObjects)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(messageRegistrationObject);
            }
            _messageRegistrationObjects.Clear();

        }

		private void ViewItemUserControlDoubleClick(object sender, EventArgs e)
		{
			FireDoubleClickEvent();
		}

		/// <summary>
		/// Do not show the sliding toolbar!
		/// </summary>
		public override bool ShowToolbar
		{
			get { return false; }
		}

        private object ShownWorkSpaceChangedReceiver(Message message, FQID sender, FQID related)
        {
	        if (message.Data != null)
	        {
		        _selectedWorkSpaceLabel.Text = ((Item) message.Data).Name;
	        }
	        return null;
        }

        private object WorkSpaceStateChangedReceiver(Message message, FQID sender, FQID related)
        {
            _selectedWorkSpaceStateLabel.Text = ((WorkSpaceState)message.Data).ToString();
            return null;
        }

        private void _shuffleWorkSpaceCamerasButton_Click(object sender, EventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new Message(SCWorkSpaceDefinition.ShuffleCamerasMessage));
        }

        private void _selectWorkSpaceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item workSpaceItem = _selectWorkSpaceComboBox.SelectedItem as Item;
            if (workSpaceItem != null)
            {
                EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.ShowWorkSpaceCommand, workSpaceItem.FQID));
                _selectWorkSpaceComboBox.SelectedItem = null;
            }
        }

        private void _selectWorkSpaceStateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedWorkSpaceStateString = _selectWorkSpaceStateComboBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedWorkSpaceStateString))
            {
                WorkSpaceState workSpaceState = (WorkSpaceState)Enum.Parse(typeof(WorkSpaceState), selectedWorkSpaceStateString);
                EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.ChangeWorkSpaceStateCommand, null, workSpaceState));
                _selectWorkSpaceStateComboBox.SelectedItem = null;
            }
        }

        private void SCWorkSpaceViewItemUserControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                FireClickEvent();
            }
        }

    }
}
