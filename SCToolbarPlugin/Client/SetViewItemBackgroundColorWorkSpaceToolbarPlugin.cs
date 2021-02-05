using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    class SetViewItemBackgroundColorWorkSpaceToolbarPluginInstance : WorkSpaceToolbarPluginInstance
    {
        private Color _color;
        private Item _window;

        public SetViewItemBackgroundColorWorkSpaceToolbarPluginInstance(Color color)
        {
            _color = color;
        }

        public override void Init(Item window)
        {
            _window = window;

            Title = _color.Name;
        }

        public override void Activate()
        {
            VideoOS.Platform.Messaging.Message message = new VideoOS.Platform.Messaging.Message(SCToolbarPluginDefinition.SetViewItemBackgroundColor);
            message.Data = new SCToolbarPluginDefinition.ColorMessageData() {Color = _color, ViewItemInstanceFQID = null, WindowFQID = _window.FQID};
            EnvironmentManager.Instance.SendMessage(message);
        }

        public override void Close()
        {
        }

    }

    class SetViewItemBackgroundColorWorkSpaceToolbarPlugin : WorkSpaceToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("EC586601-240C-4433-9F75-65A993113A06");

        private Color _color;

        public SetViewItemBackgroundColorWorkSpaceToolbarPlugin(Color color)
        {
            _color = color;
        }

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Set view item background name sample work space toolbar plugin"; }
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
            return new SetViewItemBackgroundColorWorkSpaceToolbarPluginInstance(_color);
        }
    }
}
