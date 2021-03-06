using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Property.Admin;
using Property.Background;
using Property.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace Property
{
    /// <summary>
    /// </summary>
    public class PropertyDefinition : PluginDefinition
    {
        private static System.Drawing.Image _treeNodeImage;
        private static System.Drawing.Image _topTreeNodeImage;

        internal static Guid PropertyPluginId = new Guid("b253b38b-5374-448c-babb-b576c6cce068");
        internal static Guid PropertyKind = new Guid("67d7af26-15ec-447b-9c1d-9ee5e5285206");
        internal static Guid PropertySidePanel = new Guid("db4d93ea-5880-4a6a-97a9-fc6dc1bd0e36");
        internal static Guid PropertyViewItemPlugin = new Guid("a7f45b0d-a306-4a22-a820-e3c0c2a426d0");
        internal static Guid PropertyOptionsDialog = new Guid("f1133d2d-cde9-4ce0-a62f-5eabd960ab4e");
        internal static Guid PropertyBackgroundPlugin = new Guid("deb19303-ff5e-4f9c-a02e-f7d92c86b5be");
        internal static Guid PropertyWorkSpacePluginId = new Guid("d3c29ea4-0105-4228-aaff-7d9f5e7be32c");
        internal static Guid PropertyWorkSpaceViewItemPluginId = new Guid("56d34210-5c1f-4d4b-85a4-03e1f60ad730");
        internal static Guid PropertyToolsOptionsDialog = new Guid("1119323B-1E35-4D22-AD1C-3850F73D34F5");
        internal static Guid MyPropertyId = new Guid("C1D40125-1B1C-448E-8C5B-484C279154CC");


        #region Private fields

        //
        // Note that all the plugin are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.

        private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();
        private List<OptionsDialogPlugin> _optionsDialogPlugins = new List<OptionsDialogPlugin>();
        private List<ViewItemPlugin> _viewItemPlugin = new List<ViewItemPlugin>();
        private List<ItemNode> _itemNodes = new List<ItemNode>();
        private List<SidePanelPlugin> _sidePanelPlugins = new List<SidePanelPlugin>();
        private List<WorkSpacePlugin> _workSpacePlugins = new List<WorkSpacePlugin>();
        private List<ToolsOptionsDialogPlugin> _toolsOptionsDialogPlugins = new List<ToolsOptionsDialogPlugin>();

        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static PropertyDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.Property.bmp");
            if (pluginStream != null)
                _treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
            System.IO.Stream configStream = assembly.GetManifestResourceStream(name + ".Resources.Server.png");
            if (configStream != null)
                _topTreeNodeImage = System.Drawing.Image.FromStream(configStream);
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
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                _workSpacePlugins.Add(new PropertyWorkSpacePlugin());
                _sidePanelPlugins.Add(new PropertySidePanelPlugin());
                _viewItemPlugin.Add(new PropertyViewItemPlugin());
                _viewItemPlugin.Add(new PropertyWorkSpaceViewItemPlugin());
            }

            _optionsDialogPlugins.Add(new PropertyOptionsDialogPlugin());
            _backgroundPlugins.Add(new PropertyBackgroundPlugin());
            _toolsOptionsDialogPlugins.Add(new PropertyToolsOptionDialogPlugin());
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _itemNodes.Clear();
            _sidePanelPlugins.Clear();
            _viewItemPlugin.Clear();
            _optionsDialogPlugins.Clear();
            _backgroundPlugins.Clear();
            _workSpacePlugins.Clear();
            _toolsOptionsDialogPlugins.Clear();
        }

        #region Identification Properties

        /// <summary>
        /// </summary>
        public override Guid Id
        {
            get { return PropertyPluginId; }
        }

        /// <summary>
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
        /// </summary>
        public override string Name
        {
            get { return "Property"; }
        }

        /// <summary>
        /// Your company name
        /// </summary>
        public override string Manufacturer
        {
            get { return PluginSamples.Common.ManufacturerName; }
        }

        /// <summary>
        /// Version of this plugin.
        /// </summary>
        public override string VersionString
        {
            get { return "1.0.0.0"; }
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
            return null;
        }

        /// <summary>
        /// This property can be set to true, to be able to display your own help UserControl on the entire panel.
        /// When this is false - a standard top and left side is added by the system.
        /// </summary>
        public override bool UserControlFillEntirePanel
        {
            get { return false; }
        }

        public override List<ToolsOptionsDialogPlugin> ToolsOptionsDialogPlugins
        {
            get { return _toolsOptionsDialogPlugins; }
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

        /// <summary>
        /// An extension plugin running in the Smart Client to add more choices on the Options dialog.
        /// </summary>
        public override List<OptionsDialogPlugin> OptionsDialogPlugins
        {
            get { return _optionsDialogPlugins; }
        }

        /// <summary> 
        /// An extension plugin to add to the side panel of the Smart Client.
        /// </summary>
        public override List<SidePanelPlugin> SidePanelPlugins
        {
            get { return _sidePanelPlugins; }
        }

        /// <summary>
        /// Return the workspace plugins
        /// </summary>
        public override List<WorkSpacePlugin> WorkSpacePlugins
        {
            get { return _workSpacePlugins; }
        }
        #endregion


        /// <summary>
        /// Create and returns the background task.
        /// </summary>
        public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
        {
            get { return _backgroundPlugins; }
        }

        public static event EventHandler SharedPropertyChanged;

        public static void OnSharedPropertyChange(object sender, EventArgs e)
        {
            if (SharedPropertyChanged != null)
            {
                SharedPropertyChanged.Invoke(sender, e);
            }
        }
    }
}
