using LicenseRegistration.Admin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace LicenseRegistration
{
    /// <summary>
    /// The PluginDefinition is the ‘entry’ point to any plugin.  
    /// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
    /// Several PluginDefinitions are allowed to be available within one DLL.
    /// Here the references to all other plugin known objects and classes are defined.
    /// The class is an abstract class where all implemented methods and properties need to be declared with override.
    /// The class is constructed when the environment is loading the DLL.
    /// </summary>
    public class LicenseRegistrationDefinition : PluginDefinition
	{
		private static System.Drawing.Image _treeNodeImage;
		private static System.Drawing.Image _topTreeNodeImage;

		internal static Guid LicenseRegistrationPluginId = new Guid("bb78769d-79c7-4a12-bd6e-491f61327f3b");
		internal static Guid LicenseRegistrationKind = new Guid("32dc3ec5-fd0f-4c12-899b-ec62e077d101");

		#region Private fields

		private UserControl _treeNodeInofUserControl;

		//
		// Note that all the plugin are constructed during application start, and the constructors
		// should only contain code that references their own dll, e.g. resource load.

		private List<ItemNode> _itemNodes = new List<ItemNode>();

		#endregion

		#region Initialization

		/// <summary>
		/// Load resources 
		/// </summary>
		static LicenseRegistrationDefinition()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string name = assembly.GetName().Name;

			System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.OnlineLicenseRetrieval.png");
			if (pluginStream != null)
				_treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
			System.IO.Stream configStream = assembly.GetManifestResourceStream(name + ".Resources.SDK_tool.ico");
			if (configStream != null)
				_topTreeNodeImage = System.Drawing.Image.FromStream(configStream);
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
			// Populate all relevant lists with your plugins etc.
			_itemNodes.Add(new ItemNode(LicenseRegistrationKind, Guid.Empty,
										"License Registration", _treeNodeImage,
										"License Registrations", _treeNodeImage,
										Category.Text, true,
										ItemsAllowed.One,
										new LicenseRegistrationItemManager(LicenseRegistrationKind),
										null
										));
			 
		}

		/// <summary>
		/// The main application is about to be in an undetermined state, either logging off or exiting.
		/// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
		/// </summary>
		public override void Close()
		{
			_itemNodes.Clear();
		}

		#region Identification Properties

		/// <summary>
		/// Gets the unique id identifying this plugin component
		/// </summary>
		public override Guid Id
		{
			get
			{
				return LicenseRegistrationPluginId;
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
			get { return "LicenseRegistration"; }
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
		#endregion

	}
}
