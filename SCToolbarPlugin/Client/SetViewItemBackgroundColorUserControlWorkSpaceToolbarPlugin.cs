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

            // In a real-world scenario, localized strings based on CultureInfo.CurrentUICulture
            // should be provided here.
            Title = "Select color";
            Tooltip = "Select a color for changing background color of the view items to.";
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

        public override Guid? GroupId
        {
            get { return SetViewItemBackgroundColorWorkspaceToolbarPluginGroup.PluginGroupId; }
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
