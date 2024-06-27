using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace AnalyticsOverlay
{
    public class AnalyticsOverlayDefinition : PluginDefinition
    {
        internal static Guid AnalyticsOverlayPluginId = new Guid("0D607B22-2F72-4884-9655-AED60A182352");
        internal static Guid AnalyticsOverlaySettingsPanel = new Guid("1DE7DEF1-A769-4130-BFA3-AC553297EBB7");
        internal static Guid AnalyticsOverlayBackgroundPlugin = new Guid("91B66939-E6CB-4AC3-BF5F-C23AFF9A6C9E");

        #region Private fields

        private BackgroundOverlayPlugin _backgroundOverlayPlugin = new BackgroundOverlayPlugin();
        private AnalyticsOverlaySettingsPanelPlugin _analyticsOverlaySettingsPanelPlugin = new AnalyticsOverlaySettingsPanelPlugin();

        #endregion

        #region Initialization

        static AnalyticsOverlayDefinition()
        {
        }

        #endregion

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get
            {
                return AnalyticsOverlayPluginId;
            }
        }

        /// <summary>
        /// This Guid can be defined on several different IPluginDefinitions with the same value,
        /// and will result in a combination of this top level ProductNode for several plugins.
        /// Set to Guid.Empty if no sharing is enabled.
        /// </summary>
        public override Guid SharedNodeId
        {
            get
            {
                return PluginSamples.Common.SampleTopNode;
            }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "Analytics Overlay"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        /// <summary>
        /// Your company name
        /// </summary>
        public override string Manufacturer
        {
            get
            {
                return PluginSamples.Common.ManufacturerName;
            }
        }

        /// <summary>
        /// Version of this plugin
        /// </summary>
        public override string VersionString
        {
            get
            {
                return "1.1.0.0";
            }
        }

        /// <summary>
        /// Icon to be used on top level - e.g. a product or company logo
        /// </summary>
        public override System.Drawing.Image Icon
        {
            get { return null; }
        }

        #endregion

        #region Administration properties

        /// <summary>
        /// A list of server side configuration items in the administrator
        /// </summary>
        public override List<ItemNode> ItemNodes
        {
            get
            {
                return new List<ItemNode>();
            }
        }


        #endregion

        #region Client related methods and properties

        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get
            {
                return new List<ViewItemPlugin>();
            }
        }

        /// <summary>
        /// An extension plugin running in the Smart Client to add more choices on the Settings panel.
        /// </summary>
        public override Collection<SettingsPanelPlugin> SettingsPanelPlugins
        {
            get { return new Collection<SettingsPanelPlugin>() { _analyticsOverlaySettingsPanelPlugin }; }
        }

        /// <summary>
        /// Creates and returns the background task.
        /// </summary>
        public override List<BackgroundPlugin> BackgroundPlugins
        {
            get
            {
                // Should only create the background class first time this is accessed.
                return new List<BackgroundPlugin>() { _backgroundOverlayPlugin };
            }
        }

        #endregion

    }
}
