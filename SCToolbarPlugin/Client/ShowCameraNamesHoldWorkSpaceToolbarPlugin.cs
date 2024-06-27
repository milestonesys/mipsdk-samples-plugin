using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI.Controls;

namespace SCToolbarPlugin.Client
{
    class ShowCameraNamesHoldWorkSpaceToolbarPluginInstance : WorkSpaceToolbarPluginInstance
    {
        private Item _window;

        public override void Init(Item window)
        {
            _window = window;

            Title = "Show camera names in message bar (Hold)";
            Tooltip = "Hold to show current camera names in message bar.";
            IconSource = new VideoOSIconBuiltInSource() { Icon = VideoOSIconBuiltInSource.Icons.Camera };
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
            message.Data = new SmartClientMessageData() { MessageId = this, Message = CameraNameResolver.ResolveCameraNames(_window) };
            EnvironmentManager.Instance.SendMessage(message);
        }

        private void HideMessage()
        {
            Message message = new Message(MessageId.SmartClient.SmartClientMessageCommand);
            message.Data = new SmartClientMessageData() { MessageId = this };
            EnvironmentManager.Instance.SendMessage(message);
        }
    }

    class ShowCameraNamesHoldWorkSpaceToolbarPlugin : WorkSpaceToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("815BD055-ED14-43FA-9468-2C20E06BC837");

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Shown camera names in message bar sample work space toolbar plugin"; }
        }

        public override Guid? GroupId
        {
            get { return ShowCameraNamesWorkspaceToolbarPluginGroup.PluginGroupId; }
        }

        public override ToolbarPluginType ToolbarPluginType
        {
            get { return ToolbarPluginType.Hold; }
        }

        public override void Init()
        {
            WorkSpaceToolbarPlaceDefinition.WorkSpaceIds = new List<Guid>() { ClientControl.LiveBuildInWorkSpaceId, ClientControl.PlaybackBuildInWorkSpaceId };
            WorkSpaceToolbarPlaceDefinition.WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal };
        }

        public override void Close()
        {
        }

        public override WorkSpaceToolbarPluginInstance GenerateWorkSpaceToolbarPluginInstance()
        {
            return new ShowCameraNamesHoldWorkSpaceToolbarPluginInstance();
        }
    }
}
