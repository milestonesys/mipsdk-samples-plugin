using System.Windows.Controls;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for DoorView.xaml
    /// </summary>
    public partial class DoorView : UserControl
    {
        public DoorView()
        {
            InitializeComponent();
        }

        private void tamperToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tamperToggleButtonPopup.IsOpen = true;
        }

        private void tamperButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tamperToggleButtonPopup.IsOpen = false;
        }

        private void clearTamperButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tamperToggleButtonPopup.IsOpen = false;
        }

        private void forcedOpenToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            forcedOpenToggleButtonPopup.IsOpen = true;
        }

        private void forcedOpenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            forcedOpenToggleButtonPopup.IsOpen = false;
        }

        private void clearForcedOpenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            forcedOpenToggleButtonPopup.IsOpen = false;
        }

        private void powerFailureToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            powerFailureToggleButtonPopup.IsOpen = true;
        }

        private void powerFailureButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            powerFailureToggleButtonPopup.IsOpen = false;
        }

        private void clearPowerFailureButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            powerFailureToggleButtonPopup.IsOpen = false;
        }
    }
}
