using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace SCToolbarPlugin.Client
{
    class ShowCameraNamesActionWorkSpaceToolbarPluginInstance : WorkSpaceToolbarPluginInstance
    {
        private Item _window;

        public override void Init(Item window)
        {
            _window = window;

            Title = "Show camera names in message box";
            Tooltip = "Click to show current camera names in message box.";
            IconSource = new VideoOSIconBuiltInSource() { Icon = VideoOSIconBuiltInSource.Icons.Camera };
        }

        public override void Activate()
        {
            MessageBox.Show(CameraNameResolver.ResolveCameraNames(_window));
        }

        public override void Close()
        {
        }
    }

    class ShowCameraNamesActionWorkSpaceToolbarPlugin : WorkSpaceToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("2329EEBE-7FCB-4E81-A5A8-C186B2EBF449");

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Shown camera names in message box sample work space toolbar plugin"; }
        }

        public override Guid? GroupId
        {
            get { return ShowCameraNamesWorkspaceToolbarPluginGroup.PluginGroupId; }
        }

        public override ToolbarPluginType ToolbarPluginType
        {
            get { return ToolbarPluginType.Action; }
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
            return new ShowCameraNamesActionWorkSpaceToolbarPluginInstance();
        }
    }
}
