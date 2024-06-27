using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCToolbarPlugin.Client
{
    class ShowCameraNameToggleViewItemToolbarPluginInstance : ViewItemToolbarPluginInstance
    {
        private Item _viewItemInstance;
        private Item _window;

        public override void Init(Item viewItemInstance, Item window)
        {
            _viewItemInstance = viewItemInstance;
            _window = window;

            Title = "Show camera name in message bar (Toggle)";
            Tooltip = "Select to show current camera name in message bar.";
        }

        public override void OnIsCheckedChanged()
        {
            if (IsChecked)
            {
                ShowMessage();
            }
            else
            {
                HideMessage();
            }
        }

        public override void Close()
        {
            HideMessage();
        }

        private void ShowMessage()
        {
            Message message = new Message(MessageId.SmartClient.SmartClientMessageCommand);
            message.Data = new SmartClientMessageData() { MessageId = this, Message = CameraNameResolver.ResolveCameraName(_viewItemInstance, _window)};
            EnvironmentManager.Instance.SendMessage(message);
        }

        private void HideMessage()
        {
            Message message = new Message(MessageId.SmartClient.SmartClientMessageCommand);
            message.Data = new SmartClientMessageData() { MessageId = this };
            EnvironmentManager.Instance.SendMessage(message);
        }
    }

    class ShowCameraNameToggleViewItemToolbarPlugin : ViewItemToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("089B3F47-B5AC-4078-8058-C82564BBE39F");

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Shown camera name in message bar sample view item toolbar plugin"; }
        }

        public override ToolbarPluginOverflowMode ToolbarPluginOverflowMode
        {
            get { return ToolbarPluginOverflowMode.AsNeeded; }
        }

        public override ToolbarPluginType ToolbarPluginType
        {
            get { return ToolbarPluginType.Toggle; }
        }

        public override void Init()
        {
            ViewItemToolbarPlaceDefinition.ViewItemIds = new List<Guid>() { ViewAndLayoutItem.CameraBuiltinId };
            ViewItemToolbarPlaceDefinition.WorkSpaceIds = new List<Guid>() { ClientControl.LiveBuildInWorkSpaceId, ClientControl.PlaybackBuildInWorkSpaceId };
            ViewItemToolbarPlaceDefinition.WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal };
        }

        public override void Close()
        {
        }

        public override ViewItemToolbarPluginInstance GenerateViewItemToolbarPluginInstance()
        {
            return new ShowCameraNameToggleViewItemToolbarPluginInstance();
        }
    }
}
