using VideoOS.Platform.Client;

namespace Property.Client
{
    public partial class PropertyPropertiesWpfUserControl : PropertiesWpfUserControl
    {
        #region private fields

        private PropertyViewItemManager _viewItemManager;

        #endregion

        #region Initialization & Dispose

        /// <summary>
        /// This class is created by the ViewItemManager.  
        /// </summary>
        /// <param name="viewItemManager"></param>
        public PropertyPropertiesWpfUserControl(PropertyViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        /// <summary>
        /// Setup events and message receivers and load stored configuration.
        /// </summary>
        public override void Init()
        {
            textBoxPropValue.Text = _viewItemManager.MyPropValue;
            textBoxSharedGlobal.Text = _viewItemManager.MyPropShareGlobal;
            textBoxSharedUser.Text = _viewItemManager.MyPropSharePrivate;
        }

        private void PropertyDefinition_SharedPropertyChanged(object sender, System.EventArgs e)
        {
            textBoxSharedGlobal.Text = _viewItemManager.MyPropShareGlobal;
            textBoxSharedUser.Text = _viewItemManager.MyPropSharePrivate;
        }

        /// <summary>
        /// Perform any cleanup stuff and event -=
        /// </summary>
        public override void Close()
        {
            _viewItemManager.MyPropValue = textBoxPropValue.Text;
            _viewItemManager.MyPropShareGlobal=textBoxSharedGlobal.Text;
            _viewItemManager.MyPropSharePrivate=textBoxSharedUser.Text;
        }

        #endregion

        private void TextBoxSharedGlobal_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _viewItemManager.MyPropShareGlobal = textBoxSharedGlobal.Text;
        }

        private void TextBoxSharedUser_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _viewItemManager.MyPropSharePrivate = textBoxSharedUser.Text;
        }
    }
}

