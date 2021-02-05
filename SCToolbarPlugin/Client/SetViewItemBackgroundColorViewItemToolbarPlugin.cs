using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCToolbarPlugin.Client
{
    class SetViewItemBackgroundColorViewItemToolbarPluginInstance : ViewItemToolbarPluginInstance
    {
        private List<object> _messageRegistrationObjects = new List<object>();

        private Color _color;
        private Image _icon;
        private Item _viewItemInstance;
        private Item _window;

        public SetViewItemBackgroundColorViewItemToolbarPluginInstance(Color color, Image icon)
        {
            _color = color;
            _icon = icon;
        }

        public override void Init(Item viewItemInstance, Item window)
        {
            _viewItemInstance = viewItemInstance;
            _window = window;

            Title = "Set view item background color to " + _color.Name;
            Tooltip = "Click to set view item background color to " + _color.Name;
            Icon = _icon;

            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(ViewItemBackgroundColorChangedReceiver, new MessageIdFilter(SCToolbarPluginDefinition.ViewItemBackgroundColorChanged)));
        }

        public override void Activate()
        {
            VideoOS.Platform.Messaging.Message message = new VideoOS.Platform.Messaging.Message(SCToolbarPluginDefinition.SetViewItemBackgroundColor);
            message.Data = new SCToolbarPluginDefinition.ColorMessageData() {Color = _color, ViewItemInstanceFQID = _viewItemInstance.FQID, WindowFQID = _window.FQID};
            EnvironmentManager.Instance.SendMessage(message);
        }

        public override void Close()
        {
            foreach (object messageRegistrationObject in _messageRegistrationObjects)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(messageRegistrationObject);
            }
            _messageRegistrationObjects.Clear();
        }

        private object ViewItemBackgroundColorChangedReceiver(VideoOS.Platform.Messaging.Message message, FQID sender, FQID related)
        {
            SCToolbarPluginDefinition.ColorMessageData colorMessageData = message.Data as SCToolbarPluginDefinition.ColorMessageData;
            if (colorMessageData != null)
            {
                if (colorMessageData.ViewItemInstanceFQID.Equals(_viewItemInstance.FQID) && colorMessageData.WindowFQID.Equals(_window.FQID))
                {
                    if (colorMessageData.Color == _color)
                    {
                        Enabled = false;
                    }
                    else
                    {
                        Enabled = true;
                    }
                }
            }
            return null;
        }
    }

    class SetViewItemBackgroundColorViewItemToolbarPlugin : ViewItemToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("8A4E43A9-B181-4689-8B4F-BCE331173FE7");

        private Color _color;
        private Image _icon;

        public SetViewItemBackgroundColorViewItemToolbarPlugin(Color color)
        {
            _color = color;

            _icon = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(_icon))
            {
                g.FillRectangle(new SolidBrush(_color), 0, 0, _icon.Width, _icon.Height);
            }
        }

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Set view item background sample view item toolbar plugin"; }
        }

        public override ToolbarPluginOverflowMode ToolbarPluginOverflowMode
        {
            get { return ToolbarPluginOverflowMode.AsNeeded; }
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
            return new SetViewItemBackgroundColorViewItemToolbarPluginInstance(_color, _icon);
        }
    }
}
