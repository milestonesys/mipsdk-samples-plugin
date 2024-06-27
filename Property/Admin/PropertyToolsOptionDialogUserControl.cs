namespace Property.Admin
{
    public partial class PropertyToolsOptionDialogUserControl : VideoOS.Platform.Admin.ToolsOptionsDialogUserControl
    {
        public PropertyToolsOptionDialogUserControl()
        {
            InitializeComponent();
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        public string MyPropValue
        {
            set { textBoxPropValue.Text = value ?? ""; }
            get { return textBoxPropValue.Text; }
        }
    }
}
