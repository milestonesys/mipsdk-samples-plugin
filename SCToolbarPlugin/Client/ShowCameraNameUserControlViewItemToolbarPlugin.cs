using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    class ShowCameraNameUserControlViewItemToolbarPluginInstance : ViewItemToolbarPluginInstance
    {
        private Item _viewItemInstance;
        private Item _window;

        public override void Init(Item viewItemInstance, Item window)
        {
            _viewItemInstance = viewItemInstance;
            _window = window;

            Title = "Show camera name in plugin UI";
            Tooltip = "Click to show current camera name in plugin UI.";
        }

        public override ToolbarPluginWpfUserControl GenerateWpfUserControl()
        {
            ShowCameraNameToolbarPluginWpfUserControl showCameraNameUserControl = new ShowCameraNameToolbarPluginWpfUserControl();
            showCameraNameUserControl.CameraName = CameraNameResolver.ResolveCameraName(_viewItemInstance, _window);
            return showCameraNameUserControl;
        }

        public override void Close()
        {
        }
    }

    class ShowCameraNameUserControlViewItemToolbarPlugin : ViewItemToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("837C8D40-8AE1-44A9-8DBC-9F805E3439FF");

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Shown camera name in plugin UI sample view item toolbar plugin"; }
        }

        public override ToolbarPluginOverflowMode ToolbarPluginOverflowMode
        {
            get { return ToolbarPluginOverflowMode.AsNeeded; }
        }

        public override ToolbarPluginType ToolbarPluginType
        {
            get { return ToolbarPluginType.UserControl; }
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
            return new ShowCameraNameUserControlViewItemToolbarPluginInstance();
        }
    }
}
