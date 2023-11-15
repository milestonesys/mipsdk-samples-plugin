using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCToolbarPlugin.Client
{
    class ShowCameraNameHoldViewItemToolbarPluginInstance : ViewItemToolbarPluginInstance
    {
        private Item _viewItemInstance;
        private Item _window;

        public override void Init(Item viewItemInstance, Item window)
        {
            _viewItemInstance = viewItemInstance;
            _window = window;

            Title = "Show camera name in message bar (Hold)";
            Tooltip = "Hold to show current camera name in message bar.";
        }

        public override void OnIsHeldChanged()
        {
            if (IsHeld)
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

    class ShowCameraNameHoldViewItemToolbarPlugin : ViewItemToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("7B6FE846-6AE6-42A7-8744-1ACB673DB148");

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
            get { return ToolbarPluginType.Hold; }
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
            return new ShowCameraNameHoldViewItemToolbarPluginInstance();
        }
    }
}
