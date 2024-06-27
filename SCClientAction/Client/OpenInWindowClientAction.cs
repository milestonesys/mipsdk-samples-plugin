using System;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI.Controls;
using static VideoOS.Platform.Messaging.MessageId;

namespace SCClientAction.Client
{
    internal class OpenInWindowClientAction : ClientAction
    {
        public override Guid Id { get; } = new Guid("F836103F-741A-4EC5-962D-C17A4D1D80B6");

        public override string Name { get; } = "Open in window"; //Note that in a production plug-in the action name should be localized.

        public override VideoOSIconSourceBase Icon { get; } = new VideoOSIconBuiltInSource() { Icon = VideoOSIconBuiltInSource.Icons.Plus };

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        public override void Activated()
        {
            ImageViewerAddOn imageViewerAddOn = ImageViewerHelper.GetGlobalSelectedImageViewer();
            if (imageViewerAddOn != null)
            {
                ShowCamerasInFloatingWindowData showCamerasInFloatingWindowData = new ShowCamerasInFloatingWindowData();
                showCamerasInFloatingWindowData.Cameras = new Item[] { Configuration.Instance.GetItem(imageViewerAddOn.CameraFQID) };

                EnvironmentManager.Instance.SendMessage(new Message(SmartClient.ShowCamerasInFloatingWindowCommand) { Data = showCamerasInFloatingWindowData });
            }
        }
    }
}
