using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace SCToolbarPlugin.Client
{
    class SetViewItemBackgroundColorActionWorkSpaceToolbarPluginInstance : WorkSpaceToolbarPluginInstance
    {
        private SolidColorBrush _color;
        private VideoOSIconSourceBase _icon;
        private Item _window;

        public SetViewItemBackgroundColorActionWorkSpaceToolbarPluginInstance(SolidColorBrush color, VideoOSIconSourceBase icon)
        {
            _color = color;
            _icon = icon;
        }

        public override void Init(Item window)
        {
            _window = window;

            // In a real-world scenario, localized strings based on CultureInfo.CurrentUICulture
            // should be provided here.
            Title = ColorConverter.ConvertColorToCommonName(_color.Color);
            Tooltip = string.Format($"Change background color of view items to {Title}.");
            IconSource = _icon;
        }

        public override void Activate()
        {
            VideoOS.Platform.Messaging.Message message = new VideoOS.Platform.Messaging.Message(SCToolbarPluginDefinition.SetViewItemBackgroundColor);
            message.Data = new SCToolbarPluginDefinition.ColorMessageData() { Color = _color, ViewItemInstanceFQID = null, WindowFQID = _window.FQID };
            EnvironmentManager.Instance.SendMessage(message);
        }

        public override void Close()
        {
        }
    }

    class SetViewItemBackgroundColorActionWorkSpaceToolbarPlugin : WorkSpaceToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("EC586601-240C-4433-9F75-65A993113A06");

        private SolidColorBrush _color;
        private VideoOSIconSourceBase _icon;

        public SetViewItemBackgroundColorActionWorkSpaceToolbarPlugin(SolidColorBrush color)
        {
            _color = color;
            System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Media.Imaging.BitmapSource.Create(16, 16, 96, 96, PixelFormats.Indexed1, new BitmapPalette(new List<Color>() { color.Color }), new byte[16 * 16], 16 / 8);
            _icon = new VideoOSIconBitmapSource() { BitmapSource = bitmapSource };
        }

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Set view item background color action work space toolbar plugin"; }
        }

        public override Guid? GroupId
        {
            get { return SetViewItemBackgroundColorWorkspaceToolbarPluginGroup.PluginGroupId; }
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
            return new SetViewItemBackgroundColorActionWorkSpaceToolbarPluginInstance(_color, _icon);
        }
    }
}
