using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using SensorMonitor.Admin;
using SensorMonitor.Background;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using System.Globalization;
using System.Threading;
using VideoOS.Platform.RuleAction;

namespace SensorMonitor
{
	public class SensorMonitorDefinition : PluginDefinition
	{
		internal protected static System.Drawing.Image _controllerImage;
		internal protected static System.Drawing.Image _treeNodeSensorImage;
		internal protected static System.Drawing.Image _topTreeNodeImage;

		internal static Guid SensorMonitorPluginId = new Guid("2a13d169-8803-4ab2-b45b-5c1f1c453c93");
		internal static Guid SensorMonitorCtrlKind = new Guid("57d0ed4b-3baf-4fc0-aa1b-d333a82f2f12");
		internal static Guid SensorMonitorSensorKind = new Guid("0375137E-3136-445D-ADD1-F855D4BE1B44");

		internal static Guid SensorMonitorSensorActivated = new Guid("5BEEE454-0BAC-478C-93B7-4C1DB1C4223D");	// Used to change the icon on the MAP

		internal static Collection<MapAlarmContextMenu> _controllerCM;

		#region Private fields

		private UserControl _treeNodeInofUserControl;
		private List<ItemNode> _itemNodes;
		private List<BackgroundPlugin> _backgroundPlugins;
        private SensorMonitorRuleActionActionManager _ruleActionManager;

        #endregion

        #region Initialization

        static SensorMonitorDefinition()
		{
			_controllerImage = new Icon(Properties.Resources.SensorController, 16, 16).ToBitmap();
			_treeNodeSensorImage = new Icon(Properties.Resources.Sensor, 16, 16).ToBitmap();
		}

		public override void Init()
		{
			_topTreeNodeImage = VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.SDK_VSIx];

			List<SecurityAction> securityActionsCtrl = new List<SecurityAction>
			                                       	{
			                                       		new SecurityAction("GENERIC_WRITE", "Manage"),
			                                       		new SecurityAction("GENERIC_READ", "Read"),
			                                       		new SecurityAction("POWEROFF", "Power off controller"),
			                                       		new SecurityAction("POWERON", "Power on controller"),
			                                       	};
			List<SecurityAction> securityActions = new List<SecurityAction>
			                                       	{
			                                       		new SecurityAction("GENERIC_WRITE", "Manage"),
			                                       		new SecurityAction("GENERIC_READ", "Read"),
			                                       	};

            _controllerCM = new Collection<MapAlarmContextMenu>
			                                               	{
			                                               		new MapAlarmContextMenu("Power controller off", "POWEROFF", null),
			                                               		new MapAlarmContextMenu("Power controller on", "POWERON", null),
			                                               		new MapAlarmContextMenu("Start Recording", "STARTREC", null),
			                                               		new MapAlarmContextMenu("Stop Recording", "STOPREC", null),
			                                               		new MapAlarmContextMenu("Trigger event", "TRIGGEREVENT", null),
			                                               	};
			Dictionary<Guid, Icon> sensorMapIcon = new Dictionary<Guid, System.Drawing.Icon>();		// Have 2 Icons ready for use on the MAP
			sensorMapIcon.Add(SensorMonitorSensorKind, Properties.Resources.Sensor);
			sensorMapIcon.Add(SensorMonitorSensorActivated, Properties.Resources.SensorActive);

			_itemNodes = new List<ItemNode>
			             	{
			             		new ItemNode(SensorMonitorCtrlKind,
			             		             Guid.Empty,
			             		             "Controller", _controllerImage,
			             		             "Controllers", _controllerImage,
			             		             Category.Text, true,
			             		             ItemsAllowed.Many,
			             		             new SensorMonitorControllerItemManager(SensorMonitorCtrlKind),
			             		             new List<ItemNode>
			             		             	{
			             		             		new ItemNode(SensorMonitorSensorKind,
			             		             		             SensorMonitorCtrlKind,
			             		             		             "Sensor", _treeNodeSensorImage,
			             		             		             "Sensors", _treeNodeSensorImage,
			             		             		             Category.Text, true,
			             		             		             ItemsAllowed.Many,
			             		             		             new SensorMonitorSensorItemManager(SensorMonitorSensorKind),
			             		             		             null
			             		             			)
			             		             		    {
			             		             		        SecurityActions = securityActions, 
															MapIconDictionary = sensorMapIcon
			             		             		    }
			             		             	},
			             		             null
			             			)
			             		    {
			             		        SecurityActions = securityActionsCtrl, 
                                        MapIcon = Properties.Resources.SensorController,
                                        ContextMenu = _controllerCM,
			             		    }
			             	};

			_backgroundPlugins = new List<BackgroundPlugin> { new SensorMonitorBackgroundplugin() };

            _ruleActionManager = new SensorMonitorRuleActionActionManager();
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
				return SensorMonitorPluginId;
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
			get { return "Sensor Monitor"; }
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
				return "1.2.0.0";
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

		#endregion

		#region Client related methods and properties

		/// <summary>
		/// We have no ViewItems in this sample, Smart Client are using MAP and Alarm ViewItems
		/// </summary>
		public override List<ViewItemPlugin> ViewItemPlugins
		{
			get { return null; }
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
			get { return _backgroundPlugins; }
		}

        /// <summary>
        /// Provide the ActionManager that will handle rule actions
        /// </summary>
        public override ActionManager ActionManager
        {
            get { return _ruleActionManager; }
        }

        #endregion

        #region Internal properties

        /// <summary>
        /// Access to the predefined contextmenu -- used for create triggerItems
        /// </summary>
        internal static Collection<MapAlarmContextMenu> MapAlarmContextMenuList
		{
			get { return _controllerCM; }
		}

        /// <summary>
        /// Common translation dictionary returned by both Item Managers in order to support translation for Smart Map
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        internal static Dictionary<string, string> GetTranslationDictionary(CultureInfo culture)
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var dictionary = new Dictionary<string, string>();
            dictionary[SensorMonitorCtrlKind.ToString().ToUpperInvariant()] = Resources.Strings.Controllers;
            dictionary[SensorMonitorSensorKind.ToString().ToUpperInvariant()] = Resources.Strings.Sensors;
            dictionary["POWEROFF"] = Resources.Strings.POWEROFF;
            dictionary["POWERON"] = Resources.Strings.POWERON;
            dictionary["STARTREC"] = Resources.Strings.STARTREC;
            dictionary["STOPREC"] = Resources.Strings.STOPREC;
            dictionary["TRIGGEREVENT"] = Resources.Strings.TRIGGEREVENT;
            dictionary["ACTIVATESENSOR"] = Resources.Strings.ACTIVATESENSOR;
            dictionary["DEACTIVATESENSOR"] = Resources.Strings.DEACTIVATESENSOR;

            Thread.CurrentThread.CurrentUICulture = currentCulture;

            return dictionary;
        }
        #endregion

    }
}
