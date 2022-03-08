using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCToolbarPlugin.Client
{
    /// <summary>
    /// Interaction logic for BackgroundColorViewItemWpfUserControl.xaml
    /// </summary>
    public partial class BackgroundColorViewItemWpfUserControl : ViewItemWpfUserControl
    {
        private BackgroundColorViewItemManager _viewItemManager;
        private List<object> _messageRegistrationObjects = new List<object>();

        public BackgroundColorViewItemWpfUserControl(BackgroundColorViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();
            Background = System.Windows.Media.Brushes.White;
        }

        public override void Init()
        {
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(SetViewItemBackgroundColorReceiver, new MessageIdFilter(SCToolbarPluginDefinition.SetViewItemBackgroundColor)));
        }

        public override void Close()
        {
            foreach (object messageRegistrationObject in _messageRegistrationObjects)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(messageRegistrationObject);
            }
            _messageRegistrationObjects.Clear();
        }

        private object SetViewItemBackgroundColorReceiver(Message message, FQID sender, FQID related)
        {
            List<Item> mipViewItems = WindowInformation.ViewAndLayoutItem.GetChildren();
            Item viewItemInstance = mipViewItems[int.Parse(_viewItemManager.FQID.ObjectIdString)];

            SCToolbarPluginDefinition.ColorMessageData colorMessageData = message.Data as SCToolbarPluginDefinition.ColorMessageData;
            if (colorMessageData != null)
            {
                if ((colorMessageData.ViewItemInstanceFQID == null || colorMessageData.ViewItemInstanceFQID.Equals(viewItemInstance.FQID)) &&
                    (colorMessageData.WindowFQID == null || colorMessageData.WindowFQID.Equals(WindowInformation.Window.FQID)))
                {
                    Background = colorMessageData.Color;

                    Message changeMessage = new Message(SCToolbarPluginDefinition.ViewItemBackgroundColorChanged);
                    changeMessage.Data = new SCToolbarPluginDefinition.ColorMessageData() { Color = (System.Windows.Media.SolidColorBrush)Background, ViewItemInstanceFQID = viewItemInstance.FQID, WindowFQID = WindowInformation.Window.FQID };
                    EnvironmentManager.Instance.SendMessage(changeMessage);
                }
            }
            return null;
        }

        private void OnMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }
    }
}

