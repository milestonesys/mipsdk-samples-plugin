using System.Windows.Controls;
using VideoOS.Platform.Client;

namespace Property.Client
{
    public partial class PropertyWorkSpaceViewItemWpfUserControl : ViewItemWpfUserControl
    {
        private PropertyWorkSpaceViewItemManager _wsViewItemManager;

        public PropertyWorkSpaceViewItemWpfUserControl(PropertyWorkSpaceViewItemManager wsViewItemMgr)
        {
            _wsViewItemManager = wsViewItemMgr;
            InitializeComponent();
        }

        public override void Init()
        {
            if (_wsViewItemManager.MyPropValue != null)
            {
                textBoxPropValue.Text = _wsViewItemManager.MyPropValue;
            }
            textBoxSharedGlobal.Text = _wsViewItemManager.MyPropShareGlobal;
            textBoxSharedUser.Text = _wsViewItemManager.MyPropSharePrivate;
            PropertyDefinition.SharedPropertyChanged += PropertyDefinition_SharedPropertyChanged;
        }

        public override void Close()
        {
            PropertyDefinition.SharedPropertyChanged -= PropertyDefinition_SharedPropertyChanged;
            _wsViewItemManager.MyPropValue = textBoxPropValue.Text;
            _wsViewItemManager.MyPropShareGlobal = textBoxSharedGlobal.Text;
            _wsViewItemManager.MyPropSharePrivate = textBoxSharedUser.Text;
        }

        /// <summary>
        /// Do not show the sliding toolbar
        /// </summary>
        public override bool ShowToolbar
        {
            get { return false; }
        }

        private void PropertyDefinition_SharedPropertyChanged(object sender, System.EventArgs e)
        {
            textBoxSharedGlobal.Text = _wsViewItemManager.MyPropShareGlobal;
            textBoxSharedUser.Text = _wsViewItemManager.MyPropSharePrivate;
        }

        private void OnMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }

        public string MyPropShareGlobal
        {
            set { textBoxSharedGlobal.Text = value ?? ""; }
            get { return textBoxSharedGlobal.Text; }
        }

        public string MyPropSharePrivate
        {
            set { textBoxSharedUser.Text = value ?? ""; }
            get { return textBoxSharedUser.Text; }
        }

        private void TextBoxSharedGlobal_TextChanged(object sender, TextChangedEventArgs e)
        {
            _wsViewItemManager.MyPropShareGlobal = textBoxSharedGlobal.Text;
        }

        private void TextBoxSharedUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            _wsViewItemManager.MyPropSharePrivate = textBoxSharedUser.Text;
        }
    }
}
