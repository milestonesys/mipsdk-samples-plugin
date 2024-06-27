using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BatAction.Admin;
using BatAction.Background;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.RuleAction;

namespace BatAction
{
	/// <summary>
	/// </summary>
	public class BatActionDefinition : PluginDefinition
	{

		internal static Guid BatActionPluginId = new Guid("894c9dff-dac7-447e-9acd-372734597eed");
		
		internal static Guid BatActionKind = new Guid("2cbbeba2-8739-46ae-bc07-3a4436135e79");
		internal static Guid BatActionBackgroundPlugin = new Guid("062acf2e-5f23-4c97-bf54-1d319967e986");


		#region Private fields

		private ActionManager _actionManager;
		private static Bitmap _treeNodeImage;
		
		private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();
		private List<ItemNode> _itemNodes = new List<ItemNode>();
		#endregion

		#region Internal fields
		internal static Dictionary<String, Guid> FileGuidIndex = new Dictionary<string, Guid>();

		#endregion

		#region Initialization

		/// <summary>
		/// Load resources 
		/// </summary>
		static BatActionDefinition()
		{
			_treeNodeImage = Properties.Resources.BatAction;
		}


		/// <summary>
		/// Get the icon for the plugin
		/// </summary>
		public override Image Icon
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
			EnvironmentManager.Instance.Log(false, "BatAction","Init() called");
			EnvironmentManager.Instance.TraceFunctionCalls = false;

			_actionManager = new BatActionActionManager();	
			_backgroundPlugins.Add(new BatActionBackgroundPlugin());

		    if (_itemNodes.Count == 0)
		    {
		        _itemNodes.Add(new ItemNode(BatActionKind, Guid.Empty,
		            "Bat File", _treeNodeImage,
		            "Bat Files", _treeNodeImage,
		            Category.Text, true,
		            ItemsAllowed.Many,
		            new BatActionItemManager(),
		            null
		            ));
		    }
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
				return BatActionPluginId;
			}
		}

		/// <summary>
		/// Define name of top level Tree node - e.g. A product name
		/// </summary>
		public override string Name
		{
			get { return "BatAction"; }
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

		#endregion


		#region Administration properties

		
		/// <summary>
		/// A list of server side configuration items in the administrator
		/// </summary>
		public override List<ItemNode> ItemNodes
		{
			get
			{
				if (_itemNodes.Count == 0)
				{
					EnvironmentManager.Instance.Log(true, "BatAction", "ItemNodes being called before Init() ??Check"+Environment.NewLine+Environment.StackTrace);
					// Populate all relevant lists with your plugins etc.
					_itemNodes.Add(new ItemNode(BatActionKind, Guid.Empty,
												"Bat File", _treeNodeImage,
												"Bat Files", _treeNodeImage,
												Category.Text, true,
												ItemsAllowed.Many,
												new BatActionItemManager(),
												null
												));
				}
				return _itemNodes;
			}
		}

		#endregion


		/// <summary>
		/// Create and returns the background task.
		/// </summary>
		public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
		{
			get { return _backgroundPlugins; }
		}
		

		public override ActionManager ActionManager
		{
			get
			{
				EnvironmentManager.Instance.Log(false,"BatAction","Get Action manager ...");
				return _actionManager;
			}
		}
	}
}
