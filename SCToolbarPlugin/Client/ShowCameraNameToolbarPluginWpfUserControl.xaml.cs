using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    public partial class ShowCameraNameToolbarPluginWpfUserControl : ToolbarPluginWpfUserControl
    {
        public ShowCameraNameToolbarPluginWpfUserControl()
        {
            InitializeComponent();
        }

        public string CameraName
        {
            set => _textBlock.Text = value;
        }
    }
}
