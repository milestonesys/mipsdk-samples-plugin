using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Background;

namespace Property.Background
{
    /// <summary>
    /// </summary>
    public class PropertyBackgroundPlugin : BackgroundPlugin
    {
        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return PropertyDefinition.PropertyBackgroundPlugin; }
        }

        /// <summary>
        /// The name of this background plugin
        /// </summary>
        public override String Name
        {
            get { return "Property BackgroundPlugin"; }
        }

        /// <summary>
        /// Called by the Environment when the user has logged in.
        /// </summary>
        public override void Init()
        {
            System.Xml.XmlNode result = VideoOS.Platform.Configuration.Instance.GetOptionsConfiguration(PropertyDefinition.MyPropertyId, false);
            string myPropValue = Utility.GetInnerText(result, "not defined");
            EnvironmentManager.Instance.Log(false, "Property read", "Value (global): " + myPropValue, null);
        }

        /// <summary>
        /// Called by the Environment when the user log's out.
        /// You should close all remote sessions and flush cache information, as the
        /// user might logon to another server next time.
        /// </summary>
        public override void Close()
        {
        }

        /// <summary>
        /// Define in what Environments the current background task should be started.
        /// </summary>
        public override List<EnvironmentType> TargetEnvironments
        {
            get { return new List<EnvironmentType>() { EnvironmentType.Service, EnvironmentType.Administration, EnvironmentType.SmartClient, EnvironmentType.Standalone }; }	// Default will run everywhere
        }

    }
}
