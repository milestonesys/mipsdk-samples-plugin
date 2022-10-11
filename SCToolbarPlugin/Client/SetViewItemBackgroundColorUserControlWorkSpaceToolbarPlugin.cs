using System;
using System.Collections.Generic;
using System.Windows.Media;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    class SetViewItemBackgroundColorUserControlWorkSpaceToolbarPluginInstance : WorkSpaceToolbarPluginInstance
    {
        private Item _window;

        public SetViewItemBackgroundColorUserControlWorkSpaceToolbarPluginInstance()
        {
        }

        public override void Init(Item window)
        {
            _window = window;
            Title = "Select color";
        }

        public override ToolbarPluginWpfUserControl GenerateWpfUserControl()
        {
            SetViewItemsBackgroundColorToolbarPluginWpfUserControl setViewItemsBackgroundColorUserControl = new SetViewItemsBackgroundColorToolbarPluginWpfUserControl();
            setViewItemsBackgroundColorUserControl.WindowFQID = _window.FQID;
            setViewItemsBackgroundColorUserControl.ViewItemInstanceFQID = null; //All view items in view
            return setViewItemsBackgroundColorUserControl;
        }

        public override void Close()
        {
        }
    }

    class SetViewItemBackgroundColorUserControlWorkSpaceToolbarPlugin : WorkSpaceToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("A805040A-3F91-4F14-B485-C03BF6736FFC");

        public SetViewItemBackgroundColorUserControlWorkSpaceToolbarPlugin()
        {
        }

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Set view item background color user control work space toolbar plugin"; }
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
            return new SetViewItemBackgroundColorUserControlWorkSpaceToolbarPluginInstance();
        }
    }
}
