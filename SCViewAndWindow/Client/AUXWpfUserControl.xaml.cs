using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
    public partial class AUXWpfUserControl : UserControl
    {
        public AUXWpfUserControl()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            comboBoxAUX.Items.Clear();
            for (int ix = 1; ix < 9; ix++)
                comboBoxAUX.Items.Add("" + ix);

            comboBoxOnOff.Items.Clear();
            comboBoxOnOff.Items.Add("On");
            comboBoxOnOff.Items.Add("Off");
        }

        private void OnFireIndicator(object sender, RoutedEventArgs e)
        {
            if (comboBoxOnOff.SelectedIndex==-1 || comboBoxAUX.SelectedIndex==-1)
            {
                MessageBox.Show("Please select above...", "Missing selection(s)");
                return;
            }
            PTZAUXCommandData data = new PTZAUXCommandData();
            data.AuxNumber = comboBoxAUX.SelectedIndex+1;
            data.On = (string)comboBoxOnOff.SelectedItem == "On";
            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZAUXCommand, data),
                null,
                null);
        }
    }
}
