using DemoAccessControlPlugin.Configuration;
using DemoAccessControlPlugin.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin
{
    internal class DemoAccessControlPlugin : ACPlugin
    {
        public override Guid Id
        {
            get { return new Guid("B392BD2A-C02C-46AB-B19C-1A363E4D2D19"); }
        }

        public override string Name
        {
            // This will appear in the list of access control plug-ins, when adding a new system
            get { return "Demo Access Control System"; }
        }

        public override Image Icon
        {
            // No custom icon for the access control system
            get { return null; }
        }

        public override Version Version
        {
            get { return new Version(2, 0, 0, 0); }
        }

        public override string VersionText
        {
            get { return "2.0"; }
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        public override ACSystem GenerateAccessControlSystem()
        {
            return new DemoAccessControlSystem();
        }

        public override IEnumerable<ACCategoryInfo> GetCategories()
        {
            // For demo purposes, add a custom event category
            return new[] { Categories.DoorErrorEvent };
        }

        public override IEnumerable<ACIconInfo> GetIcons()
        {
            // No custom icons for the access control elements
            return null;
        }

        public override IEnumerable<ACPropertyDefinition> GetPropertyDefinitions()
        {
            return SystemProperties.PropertyDefinitions;
        }

        public override ACPropertyValidationResult ValidateProperties(IEnumerable<ACProperty> properties)
        {
            return SystemProperties.ValidateProperties(properties);
        }
    }
}
