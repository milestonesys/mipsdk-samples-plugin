using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace AdminTabHardwarePlugin.Admin
{
    public partial class AdminTabHardwareUserControl : VideoOS.Platform.Admin.TabUserControl
    {
        private Item _associatedItem;
        private bool _igonreChanged = false;
        private AssociatedProperties _associatedProperties;

        public AdminTabHardwareUserControl(Item item)
        {
            InitializeComponent();

            _associatedItem = item;
            labelItemName.Text = item.Name;
            if (item.Properties.ContainsKey(ItemProperties.ProductID) && item.Properties.ContainsKey("DriverNumber"))
            {
                textBoxDriverNumber.Text = item.Properties["DriverNumber"];
                textBoxModel.Text = item.Properties[ItemProperties.ProductID];
            }
            if (item.Properties.ContainsKey("DriverGroup"))
            {
                textBoxManufactor.Text = item.Properties["DriverGroup"];
            }
        }

        public override void Init()
        {
            base.Init();
            _igonreChanged = true;
            textBox1.Text = "";
            textBox2.Text = "";

            _associatedProperties = Configuration.Instance.GetAssociatedProperties(_associatedItem, AdminTabHardwarePlugin._id);

            if (_associatedProperties.Properties.ContainsKey("Property1"))
                textBox1.Text = _associatedProperties.Properties["Property1"];

            if (_associatedProperties.Properties.ContainsKey("Property2"))
                textBox2.Text = _associatedProperties.Properties["Property2"];
            _igonreChanged = false;
        }

        public override void Close()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            base.Close();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!_igonreChanged)
            {
                _associatedProperties.Properties["Property1"] = textBox1.Text;
                _associatedProperties.Properties["Property2"] = textBox2.Text;
                FireConfigurationChanged();
            }
        }

        public override bool ValidateAndSave()
        {
            Configuration.Instance.SaveAssociatedProperties(_associatedProperties);
            return true;
        }

    }
}
