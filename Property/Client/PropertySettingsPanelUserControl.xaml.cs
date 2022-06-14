using System.Windows.Controls;

namespace Property.Client
{
    public partial class PropertySettingsPanelUserControl : UserControl
    {

        public PropertySettingsPanelUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get and set the property value - controlled from the SettingsPanelPlugin class
        /// </summary>
        public string MyPropValue
        {
            set { textBoxPropValue.Text = value??""; }
            get { return textBoxPropValue.Text; }
        }
        public string MyPropSharedGlobal
        {
            set { textBoxSharedGlobal.Text = value ?? ""; }
            get { return textBoxSharedGlobal.Text; }
        }
        public string MyPropSharedPrivate
        {
            set { textBoxSharedUser.Text = value ?? ""; }
            get { return textBoxSharedUser.Text; }
        }
    }
}

