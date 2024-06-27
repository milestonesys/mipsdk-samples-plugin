using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace Property.Client
{
    public class PropertyViewItemPlugin : ViewItemPlugin
    {
        public PropertyViewItemPlugin()
        {
        }

        /// <summary>
        /// Called by the Environment when the user has logged in and has received configuration from the server.
        /// </summary>
        public override void Init()
        {
        }

        /// <summary>
        /// Called by the Environment when the user logs out.
        /// You should close all remote sessions and flush cache information, as the
        /// user might logon to another server next time.
        /// </summary>
        public override void Close()
        {
        }

        /// <summary>
        /// Gets the unique id identifying this ViewItem
        /// </summary>
        public override Guid Id
        {
            get { return PropertyDefinition.PropertyViewItemPlugin; }
        }

        public override VideoOSIconSourceBase IconSource { get => PropertyDefinition.TreeNodeImage; protected set => base.IconSource = value; }

        /// <summary>
        /// The text used for a single ViewItem
        /// </summary>
        public override string Name
        {
            get { return "Property"; }
        }

        /// <summary>
        /// Generates a ViewItemManager for managing one ViewItem. A ViewItemManager is generated for each ViewItem defined.
        /// </summary>
        /// <returns></returns>
        public override ViewItemManager GenerateViewItemManager()
        {
            return new PropertyViewItemManager();
        }
    }
}
