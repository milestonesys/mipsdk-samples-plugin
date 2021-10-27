using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VideoReplay.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;

namespace VideoReplay
{
	public class VideoReplayDefinition : PluginDefinition
	{
		private static System.Drawing.Image _treeNodeImage;

		internal static Guid VideoReplayPluginId = new Guid("bcde60bf-3537-4787-85dd-250e2be7bec7");
		internal static Guid VideoReplayKind = new Guid("9c16cd76-6226-422c-bd55-4973f3497fa7");
		internal static Guid VideoReplaySidePanel = new Guid("b5d2f61b-0d4a-49e9-8d07-d4d81cef03fb");
		internal static Guid VideoReplayViewItemPlugin = new Guid("7b951f40-6600-46fe-9d5b-3eb494c80923");

		#region Private fields

		//
		// Note that all the plugin are constructed during application start, and the constructors
		// should only contain code that references their own dll, e.g. resource load.

		private List<ViewItemPlugin> _viewItemPlugin = new List<ViewItemPlugin>();
		private List<ItemNode> _itemNodes = new List<ItemNode>();
		private List<String> _messageIdStrings = new List<string>();
		private List<SecurityAction> _securityActions = new List<SecurityAction>();

		#endregion

		#region Initialization

		static VideoReplayDefinition()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string name = assembly.GetName().Name;

			System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.VideoReplay.png");
			if (pluginStream != null)
				_treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
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
		/// Base initialization of the Plugin.
		/// </summary>
		public override void Init()
		{
			// Populate all relevant lists with your plugins etc.
			_viewItemPlugin.Add(new VideoReplayViewItemPlugin());
		}

		public override void Close()
		{
			_viewItemPlugin = new List<ViewItemPlugin>();
		}

		/// <summary>
		/// Return any new messages that this plugin can use in SendMessage or PostMessage,
		/// or has a Receiver set up to listen for.
		/// The suggested format is: "YourCompany.Area.MessageId"
		/// </summary>
		public override List<string> PluginDefinedMessageIds
		{
			get
			{
				return _messageIdStrings;
			}
		}

		/// <summary>
		/// If authorization is to be used, add the SecurityActions the entire plugin 
		/// would like to be available.  E.g. Application level authorization.
		/// </summary>
		public override List<SecurityAction> SecurityActions
		{
			get
			{
				return _securityActions;
			}
			set
			{
			}
		}

		#region Identification Properties

		/// <summary>
		/// Gets the unique id identifying this plugin component
		/// </summary>
		public override Guid Id
		{
			get
			{
				return VideoReplayPluginId;
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
            get { return "Video Replay"; }
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
			get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.SDK_VSIx]; }
		}

		#endregion


		#region Administration properties

		/// <summary>
		/// A list of server side configuration items in the administrator
		/// </summary>
		public override List<ItemNode> ItemNodes
		{
			get { return _itemNodes; }
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
			get { return _viewItemPlugin; }
		}

		#endregion

	}
}
