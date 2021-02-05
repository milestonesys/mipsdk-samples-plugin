using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace AdminTabPlugin.Admin
{
    public class AdminTabCameraPlugin : TabPlugin
    {
        private Item _associatedItem = null;

        /// <summary>
        /// This plugin tab is visiable when a camera is selected
        /// </summary>
        public override Guid AssociatedKind
        {
            get { return Kind.Camera; }
        }

        public override Guid Id
        {
            get { return AdminTabPluginDefinition.AdminTabPluginTabPlugin; }
        }

        public override Image Icon
        {
            get { return Properties.Resources.AdminSetup; }
        }

        public override string Name
        {
            get { return "Sample"; }
        }

        public override void Close()
        {
        }

        public override VideoOS.Platform.Admin.TabUserControl GenerateUserControl(Item item)
        {
            _associatedItem = item;
            return new AdminTabCameraUserControl(item);
        }

        public override void Init()
        {
        }

        /// <summary>
        /// Check to see if this plugin tab should be visible. For this sample we only want to manage Axis cameras.
        /// </summary>
        /// <param name="associatedItem"></param>
        /// <returns></returns>
        public override bool IsVisible(Item associatedItem)
        {
            return 
                associatedItem.Properties.ContainsKey(ItemProperties.ProductID) && 
                associatedItem.Properties[ItemProperties.ProductID].ToUpper().Contains("AXIS");
        }

        /// <summary>
        /// We load the Associated properties configured in the Tab, but we could provide extra information
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override Dictionary<string, string> GetConfigurationReportProperties(Item item)
        {
            AssociatedProperties associatedProperties = new AssociatedProperties(item, Id);
            return associatedProperties.Properties;
        }
    }
}
