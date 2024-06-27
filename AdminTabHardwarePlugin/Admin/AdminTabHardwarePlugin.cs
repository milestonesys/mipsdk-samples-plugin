using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace AdminTabHardwarePlugin.Admin
{
    public class AdminTabHardwarePlugin : TabPlugin
    {
        internal static Guid _id = new Guid("A831B969-6930-4488-B10C-D12026B41234");
        private Item _associatedItem = null;

        /// <summary>
        /// This plugin is visiable when a camera is selected
        /// </summary>
        public override Guid AssociatedKind
        {
            get { return Kind.Hardware; }
        }

        public override Guid Id
        {
            get { return _id; }
        }

        public override string Name
        {
            get { return "Sample-HW"; }
        }

        public override void Close()
        {
        }

        public override VideoOS.Platform.Admin.TabUserControl GenerateUserControl(Item item)
        {
            _associatedItem = item;
            return new AdminTabHardwareUserControl(item);
        }

        public override void Init()
        {
        }

        /// <summary>
        /// Check to see if this tab should be visible
        /// </summary>
        /// <param name="associatedItem"></param>
        /// <returns></returns>
        public override bool IsVisible(Item associatedItem)
        {
            return associatedItem.FQID.Kind == Kind.Hardware;
        }

        public override Dictionary<string, string> GetConfigurationReportProperties(Item item)
        {
            AssociatedProperties associatedProperties = new AssociatedProperties(item, _id);
            return associatedProperties.Properties;
        }
    }
}
