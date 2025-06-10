using System;
using System.Drawing;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace AdminMultiTab.Admin
{
    public partial class AdminSecondTab: DetailedUserControl, IPropertyTab
    {
        public event EventHandler ConfigurationChangedByUser = delegate { };
        public AdminSecondTab()
        {
            InitializeComponent();
            TabDisplayName = "Tab 2";
            Image = AdminMultiTabDefinition.TreeNodeImage;
        }
        public override string TabDisplayName { get; set; }
        public override Image Image { get; set; }

        public void FillContent(Item item)
        {
            // Fill the content of the tab
            if (item != null)
            {
                if (item.Properties.TryGetValue("Var2", out var var1Value))
                {
                    Var2Textbox.Text = var1Value;
                }
            }
        }

        public void StorePropertiesInItem(Item item)
        {
            item.Properties["Var2"] = Var2Textbox.Text;
        }

        private void Var2Textbox_TextChanged(object sender, EventArgs e)
        {
            ConfigurationChangedByUser(this, EventArgs.Empty);
        }

        public void ClearContent()
        {
            Var2Textbox.Text = string.Empty;
        }
    }
}
