using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using TimelineViewItem.Background;
using TimelineViewItem.Client;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace TimelineViewItem
{
    /// <summary>
    /// The PluginDefinition is the ‘entry’ point to any plugin.  
    /// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
    /// Several PluginDefinitions are allowed to be available within one DLL.
    /// Here the references to all other plugin known objects and classes are defined.
    /// The class is an abstract class where all implemented methods and properties need to be declared with override.
    /// The class is constructed when the environment is loading the DLL.
    /// </summary>
    public class TimelineViewItemDefinition : PluginDefinition
    {
        private static readonly VideoOSIconSourceBase _pluginIcon;
        private Image _topTreeNodeImage;

        internal static Guid TimelineViewItemPluginId = new Guid("66875f08-a883-495b-bfd2-5314fde2f597");
        internal static Guid TimelineViewItemKind = new Guid("f266db4a-ff47-483c-87b5-6e97183a256b");
        internal static Guid TimelineViewItemViewItemPlugin = new Guid("e99f7624-6258-4a87-89fe-348c7128259e");


        #region Private fields

        //
        // Note that all the plugin are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.

        private List<ViewItemPlugin> _viewItemPlugin = new List<ViewItemPlugin>();
        private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();

        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static TimelineViewItemDefinition()
        {
            var packString = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/TimelineViewItem.bmp");
            _pluginIcon = new VideoOSIconUriSource() { Uri = new Uri(packString) };
        }

        internal static VideoOSIconSourceBase PluginIcon => _pluginIcon;

        #endregion

        /// <summary>
        /// This method is called when the environment is up and running.
        /// Registration of Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {
            _topTreeNodeImage = VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx];

            // Populate all relevant lists with your plugins etc.
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                _viewItemPlugin.Add(new TimelineViewItemViewItemPlugin());
                _backgroundPlugins.Add(new TimelineBackgroundPlugin());
            }
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _viewItemPlugin.Clear();
            _backgroundPlugins.Clear();
        }
 
        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get
            {
                return TimelineViewItemPluginId;
            }
        }

        /// <summary>
        /// This Guid can be defined on several different IPluginDefinitions with the same value,
        /// and will result in a combination of this top level ProductNode for several plugins.
        /// Set to Guid.Empty if no sharing is enabled.
        /// </summary>
        public override Guid SharedNodeId
        {
            get { return PluginSamples.Common.SampleTopNode; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "TimelineViewItem"; }
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
        /// A list of Client side definitions for Smart Client
        /// </summary>
        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get { return _viewItemPlugin; }
        }

        #endregion

        /// <summary>
        /// Returns the background plugin.
        /// </summary>
        public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
        {
            get { return _backgroundPlugins; }
        }

        #region Smart Client Export

        /// <summary>
        /// We would like to be included in the export
        /// </summary>
        public override bool IncludeInExport
        {
            get { return true; }
        }

        /// <summary>
        /// Have a dummy export manager - we just need the DLLs to be exported
        /// </summary>
        /// <param name="exportParameters"></param>
        /// <returns></returns>
        public override ExportManager GenerateExportManager(ExportParameters exportParameters)
        {
            return new TimelineViewItemExportManager(exportParameters);
        }

        #endregion

    }
}
