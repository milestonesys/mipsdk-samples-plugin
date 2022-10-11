using SCViewAndWindow.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace SCViewAndWindow
{
    public class SCViewAndWindowDefinition : PluginDefinition
	{
		internal protected static System.Drawing.Image _treeNodeImage;

		internal static Guid PlatformToolsId = new Guid("F37BC4E7-0CB5-4782-8A34-6CC48164CCB4");
		internal static Guid SCViewAndWindowPluginId = new Guid("eb548fc4-5970-4a95-91c8-d9123e85659a");
		internal static Guid SCViewAndWindowKind = new Guid("0d03d106-0960-4fbf-8d64-37741b25b7c5");


		#region Initialization

		static SCViewAndWindowDefinition()
		{
			_treeNodeImage = SCViewAndWindow.Properties.Resources.SCVAWTool.ToBitmap();
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
				return SCViewAndWindowPluginId;
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
            get { return "SCViewAndWindow"; }
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
				return "1.1.0.0";
			}
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

		public override UserControl GenerateUserControl()
		{
			return new UserControl();
		}

		/// <summary>
		/// A list of server side configuration items in the administrator
		/// </summary>
		public override List<ItemNode> ItemNodes
		{
			get
			{
				return null;
			}
		}


		#endregion

		#region Client related methods and properties

		public override List<ViewItemPlugin> ViewItemPlugins
		{
			get
			{
				return new List<ViewItemPlugin> { new SCViewAndWindowViewItemPlugin() };
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
				return new List<BackgroundPlugin>() { };
			}
		}

		#endregion

	}
}
