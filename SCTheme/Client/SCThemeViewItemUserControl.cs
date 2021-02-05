using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCTheme.Client
{
    public partial class SCThemeViewItemUserControl : ViewItemUserControl
    {
        private List<object> _messageRegistrationObjects = new List<object>();

        public SCThemeViewItemUserControl()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            base.Init();

            //add message listeners
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(ThemeChangedReceiver, new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication)));

            //init _selectThemeComboBox
            _selectThemeComboBox.Items.Clear();
            foreach (string themeType in Enum.GetNames(typeof(ThemeType)))
            {
                int ix = _selectThemeComboBox.Items.Add(themeType);

				if (themeType == ClientControl.Instance.Theme.ThemeType.ToString())
					_selectThemeComboBox.SelectedIndex = ix;
            }

            //init _selectedThemeLabel
            _selectedThemeLabel.Text = ClientControl.Instance.Theme.ThemeType.ToString();

            //add _windowsFormControlsPanel to smart client auto theming
            ClientControl.Instance.RegisterUIControlForAutoTheming(_windowsFormControlsPanel); 
            ClientControl.Instance.RegisterUIControlForAutoTheming(elementHost1.Child);
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

		public override bool ShowToolbar
		{
			get { return false; }
		}

        private object ThemeChangedReceiver(Message message, FQID sender, FQID related)
        {
            _selectedThemeLabel.Text = ((ThemeType)message.Data).ToString();
            return null;
        }

        private void _selectThemeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedThemeString = _selectThemeComboBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedThemeString))
            {
                ThemeType themeType = (ThemeType)Enum.Parse(typeof(ThemeType), selectedThemeString);
                EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.ChangeThemeCommand, null, themeType));
            }
        }

        private void On_Click(object sender, EventArgs e)
        {
            FireClickEvent();
        }

        private void On_DoubleClick(object sender, EventArgs e)
        {
            FireDoubleClickEvent();
        }
    }
}
