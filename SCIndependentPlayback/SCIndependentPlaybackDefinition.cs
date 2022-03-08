using SCIndependentPlayback.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCIndependentPlayback
{
	public class SCIndependentPlaybackDefinition : PluginDefinition
	{
		private static Image _treeNodeImage;
		private static Image _topTreeNodeImage;

		internal static Guid SCIndependentPlaybackPluginId = new Guid("83e83d08-5df0-4ff6-bb69-37bef5c33ccc");
		internal static Guid SCIndependentPlaybackKind = new Guid("c0cc6dc0-a75f-4058-bdea-6a3e7ff002e6");
		internal static Guid SCIndependentPlaybackViewItemPlugin = new Guid("271b944f-0c0e-4872-b664-e44e81bb2d62");

		#region Private fields

		private List<ViewItemPlugin> _viewItemPlugin = new List<ViewItemPlugin>();

		#endregion

		#region Initialization

		/// <summary>
		/// Load resources 
		/// </summary>
		static SCIndependentPlaybackDefinition()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string name = assembly.GetName().Name;
			_treeNodeImage = Properties.Resources.SCIndependentPlayback;
			System.IO.Stream configStream = assembly.GetManifestResourceStream(name + ".Resources.Server.png");
			if (configStream != null)
				_topTreeNodeImage = Image.FromStream(configStream);
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
		/// </summary>
		public override void Init()
		{
			_viewItemPlugin.Add(new SCIndependentPlaybackViewItemPlugin());
		}

		/// <summary>
		/// </summary>
		public override void Close()
		{
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
				return SCIndependentPlaybackPluginId;
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
		/// Define name of top level Tree node - e.g. A product name
		/// </summary>
		public override string Name
		{
			get { return "SCIndependentPlayback"; }
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
		public override Image Icon
		{
			get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.SDK_VSIx]; }
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

		#endregion

	}
}
