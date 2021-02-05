using VideoOS.Platform.Client;

namespace SCMessageAreaMessageTester.Client
{
    public partial class SCMessageAreaMessageTesterSidePanelUserControl : SidePanelUserControl
    {
        public SCMessageAreaMessageTesterSidePanelUserControl()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            base.Init();

            MessageListUserControl messageListUserControl = new MessageListUserControl();
            messageListUserControl.Init();
            _elementHost.Child = messageListUserControl;
        }

        public override void Close()
        {
            base.Close();

            MessageListUserControl messageListUserControl = _elementHost.Child as MessageListUserControl;
            if (messageListUserControl != null)
            {
                messageListUserControl.Close();
                _elementHost.Child = null;
            }
        }
    }
}
