using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    class ShowCameraNameActionViewItemToolbarPluginInstance : ViewItemToolbarPluginInstance
    {
        private Item _viewItemInstance;
        private Item _window;

        public override void Init(Item viewItemInstance, Item window)
        {
            _viewItemInstance = viewItemInstance;
            _window = window;

            Title = "Show camera name in message box";
            Tooltip = "Click to show current camera name in message box.";
        }

        public override void Activate()
        {
            MessageBox.Show(CameraNameResolver.ResolveCameraName(_viewItemInstance, _window));
        }

        public override void Close()
        {
        }
    }

    class ShowCameraNameActionViewItemToolbarPlugin : ViewItemToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("FC698CAF-0503-4FC7-87BA-CE9D3212D7D8");

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Shown camera name in message box sample view item toolbar plugin"; }
        }

        public override ToolbarPluginOverflowMode ToolbarPluginOverflowMode
        {
            get { return ToolbarPluginOverflowMode.AsNeeded; }
        }

        public override ToolbarPluginType ToolbarPluginType
        {
            get { return ToolbarPluginType.Action; }
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
            return new ShowCameraNameActionViewItemToolbarPluginInstance();
        }
    }
}
