using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using VideoPreview.Admin;
using VideoPreview.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;

namespace VideoPreview
{
	public class VideoPreviewDefinition : PluginDefinition
	{
		internal protected static System.Drawing.Image _treeNodeImage;

		internal static Guid VideoPreviewPluginId = new Guid("75f30b1a-df9e-4cce-823b-7f81089ae886");
		internal static Guid VideoPreviewKind = new Guid("26f840b6-18df-4102-b73f-8455b16be626");


		#region Private fields

		private UserControl _treeNodeInofUserControl;
		private List<ItemNode> _itemNodes;

		#endregion

		static VideoPreviewDefinition()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string name = assembly.GetName().Name;

			System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.VideoPreview.png");
			if (pluginStream != null)
				_treeNodeImage = System.Drawing.Image.FromStream(pluginStream);

		}
         
		public override Guid Id
		{
			get
			{
				return VideoPreviewPluginId;
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
            get { return "Video Preview"; }
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

		public override System.Drawing.Image Icon
		{
			get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx]; }
		}

		public override UserControl GenerateUserControl()
		{
			if (_treeNodeInofUserControl == null)
				_treeNodeInofUserControl = new HelpPage();
			return _treeNodeInofUserControl;
		}

        public override bool IncludeInExport
        {
            get
            {
                return true;
            }
        }

		public override List<ViewItemPlugin> ViewItemPlugins
		{
			get
			{
				return new List<ViewItemPlugin> { new VideoPreviewViewItemPlugin() };
			}
		}

		public override List<ItemNode> ItemNodes
		{
			get
			{
				if (_itemNodes == null)
				{
					_itemNodes = new List<ItemNode>{
					             		new ItemNode(VideoPreviewKind,
					             		             Guid.Empty,
					             		             "VideoPreview", _treeNodeImage,
					             		             "VideoPreviews", _treeNodeImage,
					             		             Category.Text, true,
					             		             ItemsAllowed.Many,
					             		             new VideoPreviewItemManager(VideoPreviewKind),
					             		             null
					             		            
					             			)
					             	};
				}
				return _itemNodes;
			}
		}

	}
}
