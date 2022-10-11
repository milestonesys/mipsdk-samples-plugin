using ServiceTest.Admin;
using ServiceTest.Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace ServiceTest
{
    public class ServiceTestDefinition : PluginDefinition
    {
        internal protected static System.Drawing.Image _treeNodeImage;

        internal static Guid ServiceTestPluginId = new Guid("09d8f704-0a78-41fb-92f2-ae8ad53fd8cb");
        internal static Guid ServiceTestKind = new Guid("efbec3c9-b5b8-485f-8308-23ac786c080b");

        #region Private fields

        private UserControl _treeNodeInofUserControl;
        private List<ItemNode> _itemNodes;

        #endregion

        #region Initialization

        static ServiceTestDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.ServiceTest.png");
            if (pluginStream != null)
                _treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
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
                return ServiceTestPluginId;
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
                return "1.1.0.0";
            }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. a product name
        /// </summary>
        public override string Name
        {
            get { return "ServiceTest"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        /// <summary>
        /// Icon to be used on top level - e.g. a product or company logo
        /// </summary>
        public override System.Drawing.Image Icon
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
            get
            {
                if (_itemNodes == null)
                {
                    _itemNodes = new List<ItemNode>{
                                        new ItemNode(ServiceTestKind,
                                                     Guid.Empty,
                                                     "ServiceTest", _treeNodeImage,
                                                     "ServiceTests", _treeNodeImage,
                                                     Category.Text, true,
                                                     ItemsAllowed.One,
                                                     new ServiceTestItemManager(),
                                                     null
                                            )
                                    };
                }
                return _itemNodes;
            }
        }

        /// <summary>
        /// A user control to display when the administrator clicks on the top TreeNode
        /// </summary>
        public override UserControl GenerateUserControl()
        {
            _treeNodeInofUserControl = new HelpPage();
            return _treeNodeInofUserControl;
        }

        #endregion

        #region Client related methods and properties

        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get
            {
                return new List<ViewItemPlugin> { new ServiceTestViewItemPlugin() };
            }
        }

        /// <summary>
        /// An extension plugin running in the Smart Client to add more choices on the Options dialog.
        /// </summary>
        public override List<OptionsDialogPlugin> OptionsDialogPlugins
        {
            get { return null; }
        }

        /// <summary>
        /// Create and returns the background task.
        /// </summary>
        public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
        {
            get
            {
                // Should only create the background class first time this is accessed.
                return new List<BackgroundPlugin>() { };
            }
        }

        #endregion

    }
}
