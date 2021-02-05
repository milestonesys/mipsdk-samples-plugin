using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ServerSideCarrousel.Admin;
using PlatformFileView.Admin;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;

namespace PlatformFileView
{
    public class PlatformFileViewDefinition : PluginDefinition
    {
        internal protected static System.Drawing.Image _fileImage;
        internal protected static System.Drawing.Image _topTreeNodeImage;
		internal protected static System.Drawing.Image _folderImage;

        internal static Guid PlatformFileViewPluginId = new Guid("64caa3e7-9df0-40fd-a885-2f42ee611815");
        internal static Guid PlatformFolderKind = new Guid("938a712a-64f4-43c3-9965-3053f0bfee6b");
		internal static Guid PlatformFileKind = new Guid("8F7A96B7-75F8-48c5-8095-DF9EECE20338");

        #region Private fields

        private List<ItemNode> _itemNodes;

        #endregion

        static PlatformFileViewDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.PlatformFileView.bmp");
            if (pluginStream != null)
                _fileImage = System.Drawing.Image.FromStream(pluginStream);
            System.IO.Stream configStream = assembly.GetManifestResourceStream(name + ".Resources.Server.png");
            if (configStream != null)
                _topTreeNodeImage = System.Drawing.Image.FromStream(configStream);
			System.IO.Stream folderStream = assembly.GetManifestResourceStream(name + ".Resources.folder_closed.ico");
			if (folderStream != null)
			{
				_folderImage = System.Drawing.Image.FromStream(folderStream);
				if (_folderImage.Width>20)
				{
					_folderImage = new Bitmap(_folderImage, new Size(18, 18));
				}
			}

        }

        public override Guid Id
        {
            get
            {
                return PlatformFileViewPluginId;
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
            get { return "PlatformFileView"; }
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
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        public override System.Drawing.Image Icon
        {
            get { return _topTreeNodeImage; }
        }

        public override UserControl GenerateUserControl()
        {
            return new HelpPage();
        }

        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get
            {
                return null;
            }
        }


        public override List<ItemNode> ItemNodes
        {
            get
            {
                if (_itemNodes == null)
                {
                    _itemNodes = new List<ItemNode>{
					             		new ItemNode(PlatformFolderKind,
					             		             Guid.Empty,
					             		             "PlatformFileView", _folderImage,
					             		             "PlatformFileViews", _folderImage,
					             		             Category.Text, true,
					             		             ItemsAllowed.Many,
					             		             new PlatformFileViewItemManager(),
													 null					             		             
					             			)
					             	};
                }
                return _itemNodes;
            }
        }

    }
}
