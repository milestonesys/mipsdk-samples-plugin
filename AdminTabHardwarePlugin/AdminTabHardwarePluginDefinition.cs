using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using AdminTabHardwarePlugin.Admin;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace AdminTabHardwarePlugin
{
    /// <summary>
    /// The PluginDefinition is the ‘entry’ point to any plugin.  
    /// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
    /// Several PluginDefinitions are allowed to be available within one DLL.
    /// Here the references to all other plugin known objects and classes are defined.
    /// The class is an abstract class where all implemented methods and properties need to be declared with override.
    /// The class is constructed when the environment is loading the DLL.
    /// </summary>
    public class AdminTabHardwarePluginDefinition : PluginDefinition
    {
        private static System.Drawing.Image _treeNodeImage;
        private static System.Drawing.Image _topTreeNodeImage;

        internal static Guid AdminTabHardwarePluginPluginId = new Guid("abded4df-996e-445f-8eb1-e9450313dc31");
        //internal static Guid AdminTabHardwarePluginKind = new Guid("48504de0-03c5-4885-a213-3b055e16eb2c");
        //internal static Guid AdminTabHardwarePluginSidePanel = new Guid("2073fdca-8d19-42ef-ad09-aea865ad5e89");
        //internal static Guid AdminTabHardwarePluginViewItemPlugin = new Guid("91e5556e-c22f-4cd3-9f32-60700f7cb5fd");
        //internal static Guid AdminTabHardwarePluginOptionsDialog = new Guid("921dff34-c5b3-459c-aac4-f437781b0137");
        //internal static Guid AdminTabHardwarePluginBackgroundPlugin = new Guid("ef49cc34-ece3-4559-a551-b442c16a4dcf");
        //internal static Guid AdminTabHardwarePluginWorkSpacePluginId = new Guid("44fb3b7b-4395-41ce-a85d-873da034fdbf");
        internal static Guid AdminTabHardwarePluginWorkSpaceViewItemPluginId = new Guid("6e760465-9be2-4693-9a98-abe165a4cdde");


        #region Private fields

        //private UserControl _treeNodeInofUserControl;

        //
        // Note that all the plugin are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.

        private ICollection<TabPlugin> _tabPlugins = new List<TabPlugin>();

        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static AdminTabHardwarePluginDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.AdminTabHardwarePlugin.bmp");
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
                _tabPlugins.Add(new Admin.AdminTabHardwarePlugin());
            }
        }

        public override ICollection<TabPlugin> TabPlugins
        {
            get
            {
                return _tabPlugins;
            }
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _tabPlugins.Clear();
        }

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get
            {
                return AdminTabHardwarePluginPluginId;
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
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "AdminTabHardwarePlugin"; }
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


        #region Administration properties
        #endregion

        #region Client related methods and properties
        #endregion


    }
}
