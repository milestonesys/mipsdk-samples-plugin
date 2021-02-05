using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace AnalyticsOverlay
{
	public class AnalyticsOverlayDefinition : PluginDefinition
	{
		internal static Guid AnalyticsOverlayPluginId = new Guid("99eb6158-9254-4e68-938c-08fb95c0153e");
        internal static Guid AnalyticsOverlayOptionDialog = new Guid("36A69E0D-D7B2-423A-9384-5991C754CD23");
        internal static Guid AnalyticsOverlayBackgroundPlugin = new Guid("AA7CDA13-DB96-4CA0-8B21-52FCED8B9DC6");

        #region Private fields

        private BackgroundOverlayPlugin _backgroundOverlayPlugin = new BackgroundOverlayPlugin();
		private AnalyticsOverlayOptionsPlugin _analyticsOverlayOptionsPlugin = new AnalyticsOverlayOptionsPlugin();

		#endregion

		#region Initialization

		static AnalyticsOverlayDefinition()
		{
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
				return AnalyticsOverlayPluginId;
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
            get { return "Analytics Overlay"; }
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
			get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.SDK_VSIx]; }
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
				return new List<ItemNode>();
			}
		}

		/// <summary>
		/// A user control to display when the administrator clicks on the top TreeNode
		/// </summary>
		public override UserControl GenerateUserControl()
		{
			return null;
		}

		#endregion

		#region Client related methods and properties

		public override List<ViewItemPlugin> ViewItemPlugins
		{
			get
			{
				return new List<ViewItemPlugin>();
			}
		}

		/// <summary>
		/// An extention plugin running in the Smart Client to add more choices on the Options dialog.
		/// </summary>
		public override List<OptionsDialogPlugin> OptionsDialogPlugins
		{
			get { return new List<OptionsDialogPlugin>() { _analyticsOverlayOptionsPlugin }; }
		}

		/// <summary>
		/// Create and returns the background task.
		/// </summary>
		public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
		{
			get
			{
				// Should only create the background class first time this is accessed.
				return new List<BackgroundPlugin>() { _backgroundOverlayPlugin };
			}
		}

	
		#endregion

	}
}
