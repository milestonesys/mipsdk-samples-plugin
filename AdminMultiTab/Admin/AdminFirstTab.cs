using System;
using System.Drawing;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace AdminMultiTab.Admin
{
    public partial class AdminFirstTab: DetailedUserControl, IPropertyTab
    {
        public event EventHandler ConfigurationChangedByUser = delegate { };
        public AdminFirstTab()
        {
            InitializeComponent();

            TabDisplayName = "Tab 1";
            TabHelpId = "tabHelpId";
            Image = AdminMultiTabDefinition.TreeNodeImage;
        }
        public override string TabDisplayName { get; set; }
        public override string TabHelpId { get; set; }
        public override Image Image { get; set; }


        public void StorePropertiesInItem(Item item)
        {
            item.Properties["Var1"] = Var1Textbox.Text;            
        }

        public void FillContent(Item item)
        {
            // Fill the content of the tab
            if (item != null)
            {
                if (item.Properties.TryGetValue("Var1", out var var1Value))
                {
                    Var1Textbox.Text = var1Value;
                }
            }
        }

        private void Var1Textbox_TextChanged(object sender, EventArgs e)
        {
            ConfigurationChangedByUser(this, EventArgs.Empty);
        }

        public void ClearContent()
        {
            Var1Textbox.Text = string.Empty;
        }
    }
}
