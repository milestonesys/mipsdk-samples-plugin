using System;
using System.Collections.Generic;
using SCAudioExport.Client;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCAudioExport
{
    /// <summary>
    /// This sample shows how to do WAV file export from selection different audio devices.
    /// The plugin is in the Playback tab mode, side panel plugin in the Smart Client
    /// </summary>
    public class SCAudioExportDefinition : PluginDefinition
    {
        private static System.Drawing.Image _topTreeNodeImage;

        internal static Guid SCAudioExportPluginId = new Guid("f1ce8b1a-4612-4795-994c-70c1fb1fb12f");
        internal static Guid SCAudioExportSidePanel = new Guid("592e3347-6d1a-4698-aaf4-e8ed9bdd49b1");

        #region Private fields


        //
        // Note that all the plugin are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.

        private List<SidePanelPlugin> _sidePanelPlugins = new List<SidePanelPlugin>();

        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static SCAudioExportDefinition()
        {
        }


        #endregion

        /// <summary>
        /// This method is called when the environment is up and running.
        /// Registration of Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {
            _topTreeNodeImage = VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx];
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                _sidePanelPlugins.Add(new SCAudioExportSidePanelPlugin());
            }

        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _sidePanelPlugins.Clear();
        }

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get
            {
                return SCAudioExportPluginId;
            }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "SCAudioExport"; }
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

        /// <summary>
        /// Icon to be used on top level - e.g. a product or company logo
        /// </summary>
        public override System.Drawing.Image Icon
        {
            get { return _topTreeNodeImage; }
        }

        #endregion

        #region Client related methods and properties
        /// <summary> 
        /// An extension plugin to add to the side panel of the Smart Client.
        /// </summary>
        public override List<SidePanelPlugin> SidePanelPlugins
        {
            get { return _sidePanelPlugins; }
        }
        #endregion

    }
}
