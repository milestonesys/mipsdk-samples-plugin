using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;

namespace AdminTabPlugin
{
    /// <summary>
    /// </summary>
    public class AdminTabPluginDefinition : PluginDefinition
    {
        internal static Guid AdminTabPluginPluginId = new Guid("12293dcf-5d74-443f-948d-4cf47b62d404");
        internal static Guid AdminTabPluginOptionsDialog = new Guid("56cb12bc-bb43-4a9d-b287-4fe0e52eafde");
        internal static Guid AdminTabPluginBackgroundPlugin = new Guid("6f08aee8-d9c4-4995-9135-238c406d3175");
        internal static Guid AdminTabPluginTabPlugin = new Guid("A831B969-6930-4488-B10C-D12026B44CF8");

        #region Private fields

        //
        // Note that all the plugin are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.

        private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();
        private ICollection<TabPlugin> _tabPlugins = new List<TabPlugin>();
        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static AdminTabPluginDefinition()
        {
        }


        /// <summary>
        /// Get the icon for the plugin
        /// </summary>
        internal static Image TreeNodeImage
        {
            get { return null; }
        }

        public override Image Icon
        {
            get
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// This method is called when the environment is up and running.
        /// Registration of Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                _backgroundPlugins.Add(new Background.AdminTabBackgroundPlugin());
            }
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Administration)
            {
                _tabPlugins.Add(new Admin.AdminTabCameraPlugin());
            }
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _backgroundPlugins.Clear();
            _tabPlugins.Clear();
        }

        public override ICollection<TabPlugin> TabPlugins
        {
            get
            {
                return _tabPlugins;
            }
        }
        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get
            {
                return AdminTabPluginPluginId;
            }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "AdminTabPlugin"; }
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
        /// Version of this plugin.
        /// </summary>
        public override string VersionString
        {
            get
            {
                return "1.0.0.0";
            }
        }

        #endregion


        /// <summary>
        /// An extension plugin running in the Smart Client to add more choices on the Options dialog.
        /// </summary>
        public override List<BackgroundPlugin> BackgroundPlugins
        {
            get { return _backgroundPlugins; }
        }
    }
}
