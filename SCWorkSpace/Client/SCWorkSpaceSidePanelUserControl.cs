using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCWorkSpace.Client
{
    public partial class SCWorkSpaceSidePanelUserControl : SidePanelUserControl
    {
        public SCWorkSpaceSidePanelUserControl()
        {
            InitializeComponent();
        }

        private void _shuffleWorkSpaceCamerasButton_Click(object sender, EventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new Message(SCWorkSpaceDefinition.ShuffleCamerasMessage));
        }

        private void _maxCameraCountComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMaxCameraCountString = _maxCameraCountComboBox.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedMaxCameraCountString))
            {
                int maxCameras = int.Parse(selectedMaxCameraCountString);
                if(SCWorkSpaceDefinition.MaxCameras != maxCameras)
                {
                    SCWorkSpaceDefinition.MaxCameras = maxCameras;
                    EnvironmentManager.Instance.SendMessage(new Message(SCWorkSpaceDefinition.MaxCamerasChangedMessage));
                }
            }
        }
    }
}
