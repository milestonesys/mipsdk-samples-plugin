using VideoOS.Platform.Client;

namespace Property.Client
{
    public partial class PropertySidePanelWpfUserControl : SidePanelWpfUserControl
    {
        private PropertySidePanelPlugin _sidePanel;

        public PropertySidePanelWpfUserControl(PropertySidePanelPlugin sidePanel)
        {
            _sidePanel = sidePanel;
            InitializeComponent();
        }
        public override void Init()
        {
            textBoxPropValue.Text = _sidePanel.MyPropValue;
            textBoxSharedGlobal.Text = _sidePanel.MyPropShareGlobal;
            textBoxSharedUser.Text = _sidePanel.MyPropSharePrivate;
            PropertyDefinition.SharedPropertyChanged += PropertyDefinition_SharedPropertyChanged;
        }

        public override void Close()
        {
            PropertyDefinition.SharedPropertyChanged -= PropertyDefinition_SharedPropertyChanged;
            _sidePanel.MyPropValue = textBoxPropValue.Text;
            _sidePanel.MyPropShareGlobal = textBoxSharedGlobal.Text;
            _sidePanel.MyPropSharePrivate = textBoxSharedUser.Text;
        }

        private void PropertyDefinition_SharedPropertyChanged(object sender, System.EventArgs e)
        {
            textBoxSharedGlobal.Text = _sidePanel.MyPropShareGlobal;
            textBoxSharedUser.Text = _sidePanel.MyPropSharePrivate;
        }

        private void TextBoxSharedGlobal_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _sidePanel.MyPropShareGlobal = textBoxSharedGlobal.Text;
        }

        private void TextBoxSharedUser_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _sidePanel.MyPropSharePrivate = textBoxSharedUser.Text;
        }
    }
}
