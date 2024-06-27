using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI.Controls;

namespace SCToolbarPlugin.Client
{
    class SetViewItemBackgroundColorActionViewItemToolbarPluginInstance : ViewItemToolbarPluginInstance
    {
        private List<object> _messageRegistrationObjects = new List<object>();

        private SolidColorBrush _color;
        private VideoOSIconSourceBase _icon;
        private Item _viewItemInstance;
        private Item _window;

        public SetViewItemBackgroundColorActionViewItemToolbarPluginInstance(SolidColorBrush color, VideoOSIconSourceBase icon)
        {
            _color = color;
            _icon = icon;
        }

        public override void Init(Item viewItemInstance, Item window)
        {
            _viewItemInstance = viewItemInstance;
            _window = window;

            string currentColor = ColorConverter.ConvertColorToCommonName(_color.Color);
            Title = "Set view item background color to " + currentColor;
            Tooltip = "Click to set view item background color to " + currentColor;
            IconSource = _icon;
            
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(ViewItemBackgroundColorChangedReceiver, new MessageIdFilter(SCToolbarPluginDefinition.ViewItemBackgroundColorChanged)));
        }

        public override void Activate()
        {
            Message message = new Message(SCToolbarPluginDefinition.SetViewItemBackgroundColor);
            message.Data = new SCToolbarPluginDefinition.ColorMessageData() { Color = _color, ViewItemInstanceFQID = _viewItemInstance.FQID, WindowFQID = _window.FQID };
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

        private object ViewItemBackgroundColorChangedReceiver(Message message, FQID sender, FQID related)
        {
            SCToolbarPluginDefinition.ColorMessageData colorMessageData = message.Data as SCToolbarPluginDefinition.ColorMessageData;
            if (colorMessageData != null)
            {
                if ((colorMessageData.ViewItemInstanceFQID == null || colorMessageData.ViewItemInstanceFQID.Equals(_viewItemInstance.FQID)) && colorMessageData.WindowFQID.Equals(_window.FQID))
                {
                    if (colorMessageData.Color.Color.Equals(_color.Color))
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

    class SetViewItemBackgroundColorActionViewItemToolbarPlugin : ViewItemToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("8A4E43A9-B181-4689-8B4F-BCE331173FE7");
        private SolidColorBrush _color;
        private VideoOSIconSourceBase _icon;

        public SetViewItemBackgroundColorActionViewItemToolbarPlugin(SolidColorBrush color)
        {
            _color = color;
            
            _icon = new VideoOSIconBitmapSource() { BitmapSource = BitmapSourceFromBrush(color) };
        }

        private static System.Windows.Media.Imaging.BitmapSource BitmapSourceFromBrush(SolidColorBrush color, int size = 32, int dpi = 96)
        {
            // RenderTargetBitmap = builds a bitmap rendering of a visual
            var pixelFormat = PixelFormats.Pbgra32;
            RenderTargetBitmap rtb = new RenderTargetBitmap(size, size, dpi, dpi, pixelFormat);

            // Drawing visual allows us to compose graphic drawing parts into a visual to render
            var drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                // Declaring drawing a rectangle using the input brush to fill up the visual
                context.DrawRectangle(color, null, new Rect(0, 0, size, size));
            }

            // Actually rendering the bitmap
            rtb.Render(drawingVisual);
            return rtb;
        }

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Set view item background action view item toolbar plugin"; }
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
            ViewItemToolbarPlaceDefinition.ViewItemIds = new List<Guid>() { BackgroundColorViewItemPlugin.PluginId };
            ViewItemToolbarPlaceDefinition.WorkSpaceIds = new List<Guid>() { ClientControl.LiveBuildInWorkSpaceId, ClientControl.PlaybackBuildInWorkSpaceId };
            ViewItemToolbarPlaceDefinition.WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal };
        }

        public override void Close()
        {
        }

        public override ViewItemToolbarPluginInstance GenerateViewItemToolbarPluginInstance()
        {
            return new SetViewItemBackgroundColorActionViewItemToolbarPluginInstance(_color, _icon);
        }
    }
}
