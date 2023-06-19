using System.Windows;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace SCViewAndWindow.Client
{

    public partial class PTZWpfUserControl : System.Windows.Controls.UserControl
    {
        Item _selectedCamera = null;

        public PTZWpfUserControl()
        {
            InitializeComponent();
        }

        private void OnSelectCamera(object sender, RoutedEventArgs e)
        {
            ItemPickerForm form = new ItemPickerForm();
            form.AutoAccept = true;
            form.KindFilter = Kind.Camera;
            form.Init();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _selectedCamera = form.SelectedItem;
                buttonCamera.Content = _selectedCamera.Name;
            }
            else
            {
                _selectedCamera = null;
                buttonCamera.Content = "Current";
            }
        }

        private void OnUpLeft(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.UpLeft),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnUp(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Up),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnUpRight(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.UpRight),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnLeft(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Left),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnHome(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Home),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnRight(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Right),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnDownLeft(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.DownLeft),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnDown(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Down),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnDownRight(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.DownRight),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnZoomIn(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.ZoomIn),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }

        private void OnZoomOut(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.ZoomOut),
                _selectedCamera == null ? null : _selectedCamera.FQID, null);
        }
    }
}


