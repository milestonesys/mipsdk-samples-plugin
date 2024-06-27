using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCTheme.Client
{
    /// <summary>
    /// Interaction logic for SCThemeViewItemWpfUserControl.xaml
    /// </summary>
    public partial class SCThemeViewItemWpfUserControl : ViewItemWpfUserControl
    {

        public SCThemeViewItemWpfUserControl()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            base.Init();

            //init _selectThemeComboBox
            _selectThemeComboBox.Items.Clear();
            foreach (string themeType in Enum.GetNames(typeof(ThemeType)))
            {
                int ix = _selectThemeComboBox.Items.Add(themeType);

                if (themeType == ClientControl.Instance.Theme.ThemeType.ToString())
                    _selectThemeComboBox.SelectedIndex = ix;
            }

            //init labelTheme
            labelTheme.Content = "Current Theme Type: " + ClientControl.Instance.Theme.ThemeType.ToString();  
        }

        public override void Close()
        {
            base.Close();
}

        public override bool ShowToolbar
        {
            get { return false; }
        }


        private void _selectThemeComboBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedThemeString = _selectThemeComboBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedThemeString))
            {
                ThemeType themeType = (ThemeType)Enum.Parse(typeof(ThemeType), selectedThemeString);
                EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.ChangeThemeCommand, null, themeType));
                labelTheme.Content = "Current Theme Type: " + themeType;

            }
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }

        private void OnMouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            FireClickEvent();
        }
    }
}
