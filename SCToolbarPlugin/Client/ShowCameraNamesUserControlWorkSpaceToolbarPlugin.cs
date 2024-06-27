using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace SCToolbarPlugin.Client
{
    class ShowCameraNamesUserControlWorkSpaceToolbarPluginInstance : WorkSpaceToolbarPluginInstance
    {
        private Item _window;

        public override void Init(Item window)
        {
            _window = window;

            Title = "Show camera names in plugin UI";
            Tooltip = "Click to show current camera names in plugin UI.";
            IconSource = new VideoOSIconBuiltInSource() { Icon = VideoOSIconBuiltInSource.Icons.Camera };
        }

        public override ToolbarPluginWpfUserControl GenerateWpfUserControl()
        {
            ShowCameraNameToolbarPluginWpfUserControl showCameraNameUserControl = new ShowCameraNameToolbarPluginWpfUserControl();
            showCameraNameUserControl.CameraName = CameraNameResolver.ResolveCameraNames(_window);
            return showCameraNameUserControl;
        }

        public override void Close()
        {
        }
    }

    class ShowCameraNamesUserControlWorkSpaceToolbarPlugin : WorkSpaceToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("3C1123E2-5109-49E5-9375-10526B72D2FF");

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Shown camera names in plugin UI sample work space toolbar plugin"; }
        }

        public override Guid? GroupId
        {
            get { return ShowCameraNamesWorkspaceToolbarPluginGroup.PluginGroupId; }
        }

        public override ToolbarPluginType ToolbarPluginType
        {
            get { return ToolbarPluginType.UserControl; }
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
            return new ShowCameraNamesUserControlWorkSpaceToolbarPluginInstance();
        }
    }
}
