using System;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCWorkSpace.Client
{
    /// <summary>
    /// Interaction logic for SCWorkSpaceSidePanelWpfUserControl.xaml
    /// </summary>
    public partial class SCWorkSpaceSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        public SCWorkSpaceSidePanelWpfUserControl()
        {
            InitializeComponent();
        }

        private void _shuffleWorkSpaceCamerasButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new Message(SCWorkSpaceDefinition.ShuffleCamerasMessage));
        }

        private void _maxCameraCountComboBox_SelectedIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedMaxCameraCountString = (_maxCameraCountComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            Console.WriteLine(selectedMaxCameraCountString);
            if (!string.IsNullOrEmpty(selectedMaxCameraCountString))
            {
                int maxCameras = int.Parse(selectedMaxCameraCountString);
                if (SCWorkSpaceDefinition.MaxCameras != maxCameras)
                {
                    SCWorkSpaceDefinition.MaxCameras = maxCameras;
                    EnvironmentManager.Instance.SendMessage(new Message(SCWorkSpaceDefinition.MaxCamerasChangedMessage));
                }
            }
        }
    }
}