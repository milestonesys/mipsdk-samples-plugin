using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ServerConnection.Admin;
using ServerConnection.Background;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace ServerConnection
{
	/// <summary>
	/// The PluginDefinition is the ‘entry’ point to any plugin.  
	/// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
	/// Several PluginDefinitions are allowed to be available within one DLL.
	/// Here the references to all other plugin known objects and classes are defined.
	/// The class is an abstract class where all implemented methods and properties need to be declared with override.
	/// The class is constructed when the environment is loading the DLL.
	/// </summary>
	public class ServerConnectionDefinition : PluginDefinition
	{
		private static System.Drawing.Image _treeNodeImage;
		private static System.Drawing.Image _topTreeNodeImage;

		internal static Guid ServerConnectionPluginId = new Guid("f5f7ef89-6d07-449e-b6d6-5464722b678f");
		internal static Guid ServerConnectionKind = new Guid("535a7330-aab0-4d8e-ad8d-f4961a44026d");

		internal static string ServerLicenseId = "MIPSDK-ServerConnectionLicense";


		#region Private fields

		private UserControl _treeNodeInofUserControl;

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
		static ServerConnectionDefinition()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string name = assembly.GetName().Name;

			System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.ServerConnection.bmp");
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

			// We only want this sample to run in the administrators
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Administration)
			{
				// Populate all relevant lists with your plugins etc.
				_itemNodes.Add(new ItemNode(ServerConnectionKind, Guid.Empty,
				                            "ServerConnection", _treeNodeImage,
				                            "ServerConnections", _treeNodeImage,
				                            Category.Text, true,
				                            ItemsAllowed.Many,
				                            new ServerConnectionItemManager(ServerConnectionKind),
				                            null
				               	));
			}
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service)
			{
				_backgroundPlugins.Add(new ServerConnectionDump());
			}
		}

		/// <summary>
		/// The main application is about to be in an undetermined state, either logging off or exiting.
		/// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
		/// </summary>
		public override void Close()
		{
			_itemNodes.Clear();
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
				return ServerConnectionPluginId;
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
            get { return "ServerConnectionLicense"; }
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
			_treeNodeInofUserControl = new HelpPage();
			return _treeNodeInofUserControl;
		}

		/// <summary>
		/// This property can be set to true, to be able to display your own help UserControl on the entire panel.
		/// When this is false - a standard top and left side is added by the system.
		/// </summary>
		public override bool UserControlFillEntirePanel
		{
			get { return false; }
		}

		/// <summary>
		/// Return a list of the LicenseRequest that this plugin needs.
		/// </summary>
		public override System.Collections.ObjectModel.Collection<VideoOS.Platform.License.LicenseInformation> PluginLicenseRequest
		{
			get { return ServerConnectionLicenseHandler.GetLicenseInformations(); }
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
		/// An extention plugin running in the Smart Client to add more choices on the Options dialog.
		/// </summary>
		public override List<OptionsDialogPlugin> OptionsDialogPlugins
		{
			get { return _optionsDialogPlugins; }
		}

		/// <summary> 
		/// An extention plugin to add to the side panel of the Smart Client.
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
