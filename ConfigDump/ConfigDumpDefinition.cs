using ConfigDump.Admin;
using ConfigDump.Client;
using ConfigDump2.Background;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace ConfigDump
{
    public class ConfigDumpDefinition : PluginDefinition
    {
        private readonly static VideoOSIconSourceBase _pluginIcon;
        private readonly static System.Drawing.Image _treeNodeImage;

        internal static Guid ConfigDumpPluginId = new Guid("1a1e0e59-f4d6-4193-8570-6ec248825461");
        internal static Guid ConfigDumpKind = new Guid("fe7c0f36-da53-426c-b1b3-cf38d5bb8bd5");

        #region Private fields

        private List<ItemNode> _itemNodes;

        #endregion

        #region Initialization

        static ConfigDumpDefinition()
        {
            var packString = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/ConfigDump.png");
            Uri imageUri = new Uri(packString);
            _pluginIcon = new VideoOSIconUriSource() { Uri = imageUri };
            _treeNodeImage = ResourceToImage(imageUri);
        }

        /// <summary>
        /// WPF requires resources to be stored with Build Action=Resource, which unfortunately cannot easily be read for WinForms controls, so we use this small
        /// utility method
        /// </summary>
        /// <param name="imageUri">Pack URI pointing to the image <seealso cref="https://learn.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf"/></param>
        /// <returns></returns>
        private static System.Drawing.Image ResourceToImage(Uri imageUri)
        {
            var bitmapImage = new BitmapImage(imageUri);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Flush();
                return new System.Drawing.Bitmap(stream);
            }
        }

        internal static VideoOSIconSourceBase PluginIcon => _pluginIcon;

        public override void Init()
        {
            _itemNodes = new List<ItemNode>{
                                        new ItemNode(ConfigDumpKind,
                                                     Guid.Empty,
                                                     "ConfigDump", _treeNodeImage,
                                                     "ConfigDumps", _treeNodeImage,
                                                     Category.Text, false,
                                                     ItemsAllowed.One,
                                                     new ConfigDumpItemManager(),
                                                     null
                                                     
                                            )
                                    };
        }

        public override void Close()
        {
            _itemNodes = null;
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
                return ConfigDumpPluginId;
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
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "ConfigDump"; }
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
                return _itemNodes;
            }
        }

        /// <summary>
        /// A user control to display when the administrator clicks on the top TreeNode
        /// </summary>
        public override UserControl GenerateUserControl()
        {
            return new HelpPage();
        }

        #endregion

        #region Client related methods and properties

        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get
            {
                return new List<ViewItemPlugin> { new ConfigDumpViewItemPlugin() };
            }
        }

        /// <summary>
        /// Create and returns the background task.
        /// </summary>
        public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
        {
            get
            {
                // Should only create the background class first time this is accessed.
                return new List<BackgroundPlugin>() { new BackgroundDump() };
            }
        }

        #endregion

    }
}
