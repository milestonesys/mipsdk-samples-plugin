using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCToolbarPlugin.Client
{
    public partial class BackgroundColorViewItemUserControl : ViewItemUserControl
    {
        private BackgroundColorViewItemManager _viewItemManager;
        private List<object> _messageRegistrationObjects = new List<object>();

        public BackgroundColorViewItemUserControl(BackgroundColorViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();
            BackColor = Color.White;
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
            if(colorMessageData != null)
            {
                if ((colorMessageData.ViewItemInstanceFQID == null || colorMessageData.ViewItemInstanceFQID.Equals(viewItemInstance.FQID)) &&
                    (colorMessageData.WindowFQID == null || colorMessageData.WindowFQID.Equals(WindowInformation.Window.FQID)))
                {
                    BackColor = colorMessageData.Color;

                    Message changeMessage = new Message(SCToolbarPluginDefinition.ViewItemBackgroundColorChanged);
                    changeMessage.Data = new SCToolbarPluginDefinition.ColorMessageData() { Color = BackColor, ViewItemInstanceFQID = viewItemInstance.FQID, WindowFQID = WindowInformation.Window.FQID };
                    EnvironmentManager.Instance.SendMessage(changeMessage);
                }
            }
            return null;
        }

        private void OnClick(object sender, System.EventArgs e)
        {
            FireClickEvent();
        }

        private void OnDoubleClick(object sender, System.EventArgs e)
        {
            FireDoubleClickEvent();
        }
    }
}
