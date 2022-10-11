using EventTracer.Background;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace EventTracer
{
    /// <summary>
    /// The PluginDefinition is the ‘entry’ point to any plugin.  
    /// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
    /// Several PluginDefinitions are allowed to be available within one DLL.
    /// Here the references to all other plugin known objects and classes are defined.
    /// The class is an abstract class where all implemented methods and properties need to be declared with override.
    /// The class is constructed when the environment is loading the DLL.
    /// </summary>
    public class EventTracerDefinition : PluginDefinition
	{
		private static System.Drawing.Image _treeNodeImage;
		private static System.Drawing.Image _topTreeNodeImage;

		internal static Guid EventTracerPluginId = new Guid("55dd3462-b6b0-4467-835a-d5ee423b4f09");
		internal static Guid EventTracerKind = new Guid("99c70ade-9bb4-4dd2-bdef-d55f17d802d5");
		internal static Guid EventTracerBackgroundPlugin = new Guid("5a583c5b-c1b3-4c20-b977-0ca7f8ed25d8");

		#region Private fields

		//
		// Note that all the plugin are constructed during application start, and the constructors
		// should only contain code that references their own dll, e.g. resource load.

		private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();
		private List<OptionsDialogPlugin> _optionsDialogPlugins = new List<OptionsDialogPlugin>();
		private List<ViewItemPlugin> _viewItemPlugin = new List<ViewItemPlugin>();
		private List<ItemNode> _itemNodes = new List<ItemNode>();
		private List<SidePanelPlugin> _sidePanelPlugins = new List<SidePanelPlugin>();
		private List<String> _messageIdStrings = new List<string>();
		private List<SecurityAction> _securityActions = new List<SecurityAction>();

		#endregion

		#region Initialization

		/// <summary>
		/// Load resources 
		/// </summary>
		static EventTracerDefinition()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string name = assembly.GetName().Name;

			System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.EventTracer.bmp");
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
		/// This method is called when the environment is up and running.
		/// Registration of Messages via RegisterReceiver can be done at this point.
		/// </summary>
		public override void Init()
		{
			_topTreeNodeImage = VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx];
			_backgroundPlugins.Add(new EventTracerBackgroundPlugin());
		}

		/// <summary>
		/// The main application is about to be in an undetermined state, either logging off or exiting.
		/// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
		/// </summary>
		public override void Close()
		{
			_itemNodes.Clear();
			_sidePanelPlugins.Clear();
			_viewItemPlugin.Clear();
			_optionsDialogPlugins.Clear();
			_backgroundPlugins.Clear();
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
				return EventTracerPluginId;
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
				return Guid.Empty;
			}
		}

		/// <summary>
		/// Define name of top level Tree node - e.g. A product name
		/// </summary>
		public override string Name
		{
            get { return "EventTracer"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return "EventTracer"; }
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

		/// <summary>
		/// This property can be set to true, to be able to display your own help UserControl on the entire panel.
		/// When this is false - a standard top and left side is added by the system.
		/// </summary>
		public override bool UserControlFillEntirePanel
		{
			get { return false; }
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

		/// <summary>
		/// An extension plugin running in the Smart Client to add more choices on the Options dialog.
		/// </summary>
		public override List<OptionsDialogPlugin> OptionsDialogPlugins
		{
			get { return _optionsDialogPlugins; }
		}

		/// <summary> 
		/// An extension plugin to add to the side panel of the Smart Client.
		/// </summary>
		public override List<SidePanelPlugin> SidePanelPlugins
		{
			get { return _sidePanelPlugins; }
		}

		/// <summary>
		/// Create and returns the background task.
		/// </summary>
		public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
		{
			get { return _backgroundPlugins; }
		}

		#endregion

	}
}
