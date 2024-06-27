using SequenceViewer.Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace SequenceViewer
{
    public class SequenceViewerDefinition : PluginDefinition
	{
		private readonly static VideoOSIconSourceBase _treeNodeImage;
		internal protected static System.Drawing.Image _topTreeNodeImage;
		internal static Guid DataSourcePluginId = new Guid("D253F7D6-5FC7-4952-A266-0D1569193FC2");
		internal static Guid DataSourceKind = new Guid("BA24FF40-8410-45E3-BC7E-230D3DF088E1");

		#region Initialization

		static SequenceViewerDefinition()
		{
            var packString = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/SequenceViewer.png");
            _treeNodeImage = new VideoOSIconUriSource() { Uri = new Uri(packString) };
        }

        internal static VideoOSIconSourceBase TreeNodeImage => _treeNodeImage;

        public override void Init()
        {
			_topTreeNodeImage = VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx];
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
				return DataSourcePluginId;
			}
		}

		/// <summary>
		/// This Guid can be defined on several different IPluginDefinitions with the same value,
		/// and will result in a combination of this top level ProductNode for several plugins.
		/// Set to Guid.Empty if no sharing is enabled.
		/// </summary>
		public override Guid SharedNodeId
		{
			get { return PluginSamples.Common.SampleTopNode; }
		}

		/// <summary>
		/// Define name of top level Tree node - e.g. A product name
		/// </summary>
		public override string Name
		{
			get { return "Data Source"; }
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

		#endregion


		#region Client related methods and properties

		public override List<ViewItemPlugin> ViewItemPlugins
		{
			get
			{
				return new List<ViewItemPlugin> { new SequenceViewerViewItemPlugin() };
			}
		}

		#endregion

	}
}
