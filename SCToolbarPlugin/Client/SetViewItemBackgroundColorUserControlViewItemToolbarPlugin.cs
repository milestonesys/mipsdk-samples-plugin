using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    class SetViewItemBackgroundColorUserControlViewItemToolbarPluginInstance : ViewItemToolbarPluginInstance
    {
        private List<object> _messageRegistrationObjects = new List<object>();

        private Item _viewItemInstance;
        private Item _window;

        public SetViewItemBackgroundColorUserControlViewItemToolbarPluginInstance()
        {
        }

        public override void Init(Item viewItemInstance, Item window)
        {
            _viewItemInstance = viewItemInstance;
            _window = window;

            Title = "Set view item background color";
            Tooltip = "Click to set view item background color.";
        }

        public override ToolbarPluginWpfUserControl GenerateWpfUserControl()
        {
            SetViewItemsBackgroundColorToolbarPluginWpfUserControl setViewItemsBackgroundColorUserControl = new SetViewItemsBackgroundColorToolbarPluginWpfUserControl();
            setViewItemsBackgroundColorUserControl.WindowFQID = _window.FQID;
            setViewItemsBackgroundColorUserControl.ViewItemInstanceFQID = _viewItemInstance.FQID;
            return setViewItemsBackgroundColorUserControl;
        }

        public override void Close()
        {
            foreach (object messageRegistrationObject in _messageRegistrationObjects)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(messageRegistrationObject);
            }
            _messageRegistrationObjects.Clear();
        }
    }

    class SetViewItemBackgroundColorUserControlViewItemToolbarPlugin : ViewItemToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("8A4E43A9-B181-4689-8B4F-BCE331173FE7");

        public SetViewItemBackgroundColorUserControlViewItemToolbarPlugin()
        {
        }

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Set view item background user control view item toolbar plugin"; }
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
            ViewItemToolbarPlaceDefinition.ViewItemIds = new List<Guid>() { BackgroundColorViewItemPlugin.PluginId };
            ViewItemToolbarPlaceDefinition.WorkSpaceIds = new List<Guid>() { ClientControl.LiveBuildInWorkSpaceId, ClientControl.PlaybackBuildInWorkSpaceId };
            ViewItemToolbarPlaceDefinition.WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal };
        }

        public override void Close()
        {
        }

        public override ViewItemToolbarPluginInstance GenerateViewItemToolbarPluginInstance()
        {
            return new SetViewItemBackgroundColorUserControlViewItemToolbarPluginInstance();
        }
    }
}
