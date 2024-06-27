using System.Windows;
using System.Windows.Media;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    public partial class SetViewItemsBackgroundColorToolbarPluginWpfUserControl : ToolbarPluginWpfUserControl
    {
        public SetViewItemsBackgroundColorToolbarPluginWpfUserControl()
        {
            InitializeComponent();
        }

        public FQID WindowFQID { set; get; }

        public FQID ViewItemInstanceFQID { set; get; }

        private void RedButton_OnClick(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Red);
        }

        private void GreenButton_OnClick(object sender, RoutedEventArgs e)
        {
            SetColor(Brushes.Green);
        }

        private void BlueButton_OnClick(object sender, RoutedEventArgs e)
        {
             SetColor(Brushes.Blue);
        }

        private void SetColor(SolidColorBrush color)
        {
            VideoOS.Platform.Messaging.Message message = new VideoOS.Platform.Messaging.Message(SCToolbarPluginDefinition.SetViewItemBackgroundColor);
            message.Data = new SCToolbarPluginDefinition.ColorMessageData() { Color = color, ViewItemInstanceFQID = ViewItemInstanceFQID, WindowFQID = WindowFQID };
            EnvironmentManager.Instance.SendMessage(message);
        }
    }
}
