using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using ServerSideCarrousel.Admin;
using ServerSideCarrousel.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace ServerSideCarrousel
{
    public class ServerSideCarrouselDefinition : PluginDefinition
    {
        private static readonly VideoOSIconSourceBase _pluginIcon;
        private static readonly System.Drawing.Image _treeNodeImage;

        internal static Guid CarrouselPluginId = new Guid("FD2AB85B-B944-448f-BAA9-CC4DCE1172FA");
        internal static Guid CarrouselKind = new Guid("8A9F28E8-042E-480d-BE46-A690537ECEE6");	//Remember to make a new one, if you copy this code

        #region Private fields


        private List<ItemNode> _itemNodes;


        #endregion

        static ServerSideCarrouselDefinition()
        {
            var packString = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/Carousel.png");
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
        internal static System.Drawing.Image TreeNodeImage => _treeNodeImage;

        public override Guid Id
        {
            get
            {
                return CarrouselPluginId;
            }
        }

        public override Guid SharedNodeId
        {
            get
            {
                return PluginSamples.Common.SampleTopNode;
            }
        }

        public override string Name
        {
            get { return "Server Side Carrousel"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        public override System.Drawing.Image Icon
        {
            get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx]; }
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

        public override UserControl GenerateUserControl()
        {
            return new CarrouselHelpPage();
        }

        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get
            {
                return new List<ViewItemPlugin> { new CarrouselViewItemPlugin() };
            }
        }

        public override void Init()
        {			
        }
        
        public override List<ItemNode> ItemNodes
        {
            get
            {
                if (_itemNodes == null)
                {
                    _itemNodes = new List<ItemNode>
                                    {
                                        new ItemNode(CarrouselKind,
                                                      Guid.Empty,
                                                      "Server Side Carrousel", _treeNodeImage,
                                                      "Server Side Carrousels", _treeNodeImage,
                                                      Category.Text, true,
                                                      ItemsAllowed.Many,
                                                      new CarrouselItemManager(CarrouselKind),
                                                      null
                                            )
                                    };
                }
                return _itemNodes;
            }
        }
    }
}
