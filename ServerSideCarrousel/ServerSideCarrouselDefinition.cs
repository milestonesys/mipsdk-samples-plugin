using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using ServerSideCarrousel.Admin;
using ServerSideCarrousel.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;

namespace ServerSideCarrousel
{
    public class ServerSideCarrouselDefinition : PluginDefinition
    {
        internal protected static System.Drawing.Image _treeNodeImage;

        internal static Guid CarrouselPluginId = new Guid("FD2AB85B-B944-448f-BAA9-CC4DCE1172FA");
        internal static Guid CarrouselKind = new Guid("8A9F28E8-042E-480d-BE46-A690537ECEE6");	//Remember to make a new one, if you copy this code



        #region Private fields


        private List<ItemNode> _itemNodes;


        #endregion

        static ServerSideCarrouselDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            _treeNodeImage = System.Drawing.Image.FromStream(assembly.GetManifestResourceStream(name + ".Resources.Carousel.png"));
        }

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
