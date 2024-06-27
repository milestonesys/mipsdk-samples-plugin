using ChatWithWebsockets.Admin;
using ChatWithWebsockets.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;

namespace ChatWithWebsockets
{
    /// <summary>
    /// The PluginDefinition is the ‘entry’ point to any plugin.  
    /// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
    /// The class is constructed when the environment is loading the DLL.
    /// </summary>
    public class ChatWithWebsocketsDefinition : PluginDefinition
    {
        private static System.Drawing.Image _treeNodeImage;

        internal static Guid ChatWithWebsocketsPluginId = new Guid("22D3AA2F-CE5A-4E45-BE73-FB3B861AC380");
        internal static Guid ChatWithWebsocketsKind = new Guid("5A9BFBAA-CB35-47CC-BEB3-2C1413338061");
        internal static Guid ChatWithWebsocketsSidePanel = new Guid("C645216F-A2A4-4F15-B1B8-5632FE666712");
        internal static Guid ChatWithWebsocketsViewItemPlugin = new Guid("67566B1B-AF55-4B18-9B8A-407AD826AF18");
        internal static Guid ChatWithWebsocketsBackgroundPlugin = new Guid("6432A4A8-0F1D-4D51-AFE1-3A0A97AC63D2");

        #region Private fields

        private List<ViewItemPlugin> _viewItemPlugin = new List<ViewItemPlugin>();
        private List<ItemNode> _itemNodes = new List<ItemNode>();
        private List<SidePanelPlugin> _sidePanelPlugins = new List<SidePanelPlugin>();

        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static ChatWithWebsocketsDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.ChatWithWebsockets.bmp");
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
        /// </summary>
        public override void Init()
        {
            _itemNodes = new List<ItemNode>
                            {
                                new ItemNode(ChatWithWebsocketsKind,
                                             Guid.Empty,
                                             "ChatWithWebsockets", _treeNodeImage,
                                             "ChatWithWebsockets", _treeNodeImage,
                                             Category.Text, true,
                                             ItemsAllowed.None,
                                             new ChatWithWebsocketsItemManager(),
                                             null
                                    )
                            };

            _sidePanelPlugins.Add(new ChatWithWebsocketsSidePanelPlugin());
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// Release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _itemNodes.Clear();
            _sidePanelPlugins.Clear();
            _viewItemPlugin.Clear();
        }


        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get
            {
                return ChatWithWebsocketsPluginId;
            }
        }

        /// <summary>
        /// This Guid can be defined on several different IPluginDefinitions with the same value,
        /// and will result in a combination of this top level ProductNode for several plugins.
        /// As this is doen with MIP Samples.
        /// </summary>
        public override Guid SharedNodeId
        {
            get
            {
                return PluginSamples.Common.SampleTopNode;
            }
        }

        /// <summary>
        /// Name of top level Tree node
        /// </summary>
        public override string Name
        {
            get { return "ChatWithWebsockets"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        /// <summary>
        /// Company name
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
        /// Icon to be used on top level
        /// </summary>
        public override Image Icon
        {
            get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx]; }
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
        /// False - a standard top and left side is added by the system.
        /// </summary>
        public override bool UserControlFillEntirePanel
        {
            get { return false; }
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
        /// An extension plugin to add to the side panel of the Smart Client.
        /// </summary>
        public override List<SidePanelPlugin> SidePanelPlugins
        {
            get { return _sidePanelPlugins; }
        }

        #endregion

    }
}
