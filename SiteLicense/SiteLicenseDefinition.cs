using SiteLicense.Admin;
using SiteLicense.Background;
using SiteLicense.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace SiteLicense
{
    /// <summary>
    /// The PluginDefinition is the ‘entry’ point to any plugin.  
    /// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
    /// Several PluginDefinitions are allowed to be available within one DLL.
    /// Here the references to all other plugin known objects and classes are defined.
    /// The class is an abstract class where all implemented methods and properties need to be declared with override.
    /// The class is constructed when the environment is loading the DLL.
    /// </summary>
    public class SiteLicenseDefinition : PluginDefinition
    {
        private static Image _treeNodeImage;
        private static Image _topTreeNodeImage;

        internal static Guid SiteLicensePluginId = new Guid("1E7AB63F-EDAE-4ECD-A864-18ACE74BE5B2");
        internal static Guid SiteLicenseKind = new Guid("9A0F4DEE-C608-4FD4-8167-3ED8DE66654D");
        internal static Guid SiteLicenseSettingsPanel = new Guid("1DC49029-17C1-4414-84F1-B81DF86A610E");

        internal static string SiteLicenseId = "MIPSDK-SiteLicense";

        #region Private fields

        //
        // Note that all the plugins are constructed during application start, and the constructors
        // should only contain code that references their own DLL, e.g. resource load.

        private Collection<SettingsPanelPlugin> _settingsPanelPlugins = new Collection<SettingsPanelPlugin>();
        private List<ItemNode> _itemNodes = new List<ItemNode>();
        private List<String> _messageIdStrings = new List<string>();
        private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();

        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static SiteLicenseDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.SiteLicense.bmp");
            if (pluginStream != null)
                _treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
        }


        /// <summary>
        /// Get the icon for the plugin
        /// </summary>
        internal static Image TreeNodeImage
        {
            get { return _treeNodeImage; }
        }

        #endregion

        /// <summary>
        /// This method is called when the environment is up and running.
        /// Registration of Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {
            _topTreeNodeImage = VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx];

            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Administration)
            {
                // Populate all relevant lists with your plugins etc.
                _itemNodes.Add(new ItemNode(SiteLicenseKind, Guid.Empty,
                                            "SiteLicense", _treeNodeImage,
                                            "SiteLicenses", _treeNodeImage,
                                            Category.Text, true,
                                            ItemsAllowed.None,
                                            new SiteLicenseItemManager(),
                                            null
                                ));
            }
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                _settingsPanelPlugins.Add(new SiteLicenseSettingsPanelPlugin());
            }
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service)
            {
                _backgroundPlugins.Add(new SiteLicenseDump());
            }
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _itemNodes.Clear();
            _settingsPanelPlugins.Clear();
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
                return SiteLicensePluginId;
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
            get { return "Site License"; }
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

        /// <summary>
        /// License type name to be shown in the License information tab of the Management Client instead of the one from <see cref="VideoOS.Platform.License.LicenseInformation" />.
        /// </summary>
        public override string CustomLicenseType { get { return "Generic Site License"; } }

        #endregion


        #region Administration properties

        /// <summary>
        /// A list of server side configuration items in the administrator
        /// </summary>
        public override List<ItemNode> ItemNodes
        {
            get { return _itemNodes; }
        }

        /// <summary>
        /// A user control to display when the administrator clicks on the top TreeNode
        /// </summary>
        public override UserControl GenerateUserControl()
        {
            return new UserControl();
        }

        /// <summary>
        /// Return a list of the LicenseRequest that this plugin needs.
        /// </summary>
        public override Collection<VideoOS.Platform.License.LicenseInformation> PluginLicenseRequest
        {
            get { return SiteLicenseHandler.GetLicenseInformations(); }
        }

        #endregion

        /// <summary>
        /// Create and returns the background task.
        /// </summary>
        public override List<BackgroundPlugin> BackgroundPlugins
        {
            get { return _backgroundPlugins; }
        }

        #region Client related methods and properties
        /// <summary>
        /// An extension plugin running in the Smart Client to add more choices on the Settings panel.
        /// </summary>
        public override Collection<SettingsPanelPlugin> SettingsPanelPlugins
        {
            get { return _settingsPanelPlugins; }
        }

        #endregion

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
            return new SiteLicenseExportManager(exportParameters);
        }

        #endregion

    }
}
