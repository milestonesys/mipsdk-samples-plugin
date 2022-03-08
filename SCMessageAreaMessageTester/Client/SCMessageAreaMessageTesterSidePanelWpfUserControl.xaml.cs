using VideoOS.Platform.Client;

namespace SCMessageAreaMessageTester.Client
{
    /// <summary>
    /// Interaction logic for SCMessageAreaMessageTesterSidePanelWpfUserControl.xaml
    /// </summary>
    public partial class SCMessageAreaMessageTesterSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        public SCMessageAreaMessageTesterSidePanelWpfUserControl()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            base.Init();

            MessageListUserControl messageListUserControl = new MessageListUserControl();
            messageListUserControl.Init();
            grid.Children.Add(messageListUserControl);
        }
    }
}
