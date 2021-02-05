using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace ServerConnection.Admin
{
	/// <summary>
	/// This class is created once for all Items of a specific Kind and is responsible for getting and saving configuration for 
	/// all Items of that Kind<br/>
	/// <br/>
	/// This class is also responsible for creating, filling and saving 
	/// UserControl and relevant fields changed by the user in the administrators.<br/>
	/// Normally there will be one ItemManager for each ItemNode.<br/>
	/// If the Items being managed are stored on another server, then this class can utilize
	/// the Init() and Close() methods to setup sessions to the remote server in anticipation 
	/// of calls to GetItems().<br/>
	/// <br/>
	/// <b>EventServer and Items on MAP</b><br/>
	/// This class is also involved in defining ContextMenu on the SmartClient MAP, because the plugin 
	/// executing in the Event Server, are providing the Smart Client MAP ViewItem with 
	/// all relevant plugins and their definitions.<br/>
	/// As the Event Server uses authorization for Items, the ItemNode needs to define the SecurityActions
	/// relevant for the items being managed by the ItemManager. A minimum of two SecurityActions must always be defined
	/// for the Items to be available in the Smart Client.<br/>
	/// <br/>
	/// <b>Item Selection in the Administrators</b><br/>
	/// Selection of an Item can happen via mouse click on the TreeView or via ContentMenu Property selection in Enterprise.<br/>
	/// The user action will result in the following sequences of events:<br/>
	/// a)	ItemManager is accessed through the ItemNode<br/>
	/// b)	The GenerateUserControl() is called to allow ItemManager to create the User Control, setup the ConfigurationChangedByUserEvent, and return the UserControl to the caller.<br/>
	/// c)	The caller will add the UserControl to its relevant panel and set userControl.Dock = DockStyle.Fill to fill available area.<br/>
	/// d)	If executing in the enterprise administrator and the floating property window is opened, the size of the window is adjusted to a relevant size based on the UserControl.Size<br/>
	/// e)	Existing configuration is fetched by calling the ItemManager.GetItem(FQID)<br/>
	/// f)	The ItemManager.FillUserControl(item) method is called for let the ItemManager populate the UserControl.<br/>
	/// <br/>
	/// <br/>
	/// When de-selecting an item, the ItemManager.FillUserControl(null) is called to let the ItemManager clear the UserControl. The parent UserControl will also have Enabled=false; set shortly after this call.<br/>
	/// <br/>
	/// <b>Item edit and update</b><br/>
	/// After an Item is selected or created, the system needs to know if any updates have been done by the administrator.  
	/// Ensure the ConfigurationChangedByUserHandler is called whenever the administrator changes something on the UI.  
	/// This will allow the administration application to enable Save and Apply buttons as appropriate.<br/>
	/// When the administrator presses a Save or Apply button, the:<br/>
	///     public bool ValidateAndSaveUserEntry()<br/> 
	/// is called to let the ItemManager and the underlying UserControl validate the entered values. If fields are valid the method returns true.
	/// <br/>
	/// <br/>
	/// <b>TreeView name change</b><br/>
	/// On the TreeView itself, it is possible to press F2 and edit the name directly. In this case the updated name is passed to the ItemManager via the 
	/// SetItemName(name) method.<br/>
	/// The ItemManager should save the new name immediately.<br/>
	/// </summary>
	public class ServerConnectionItemManager : ItemManager
	{
		private ServerConnectionUserControl _userControl;
		private Guid _kind;

		#region Constructors

		public ServerConnectionItemManager(Guid kind)
			: base()
		{
			_kind = kind;
		}

		/// <summary>
		/// Is called when the Environment is initializing, and will soon call GetItem methods
		/// IF you need to establish connection to a remote server, this is a good place to initialize.
		/// </summary>
		public override void Init()
		{
			ServerConnectionLicenseHandler.Init(this);
		}

		/// <summary>
		/// Is called when server is changing or application is closing down.
		/// You should close any remote session you may have, and flush cache.
		/// </summary>
		public override void Close()
		{
			ServerConnectionLicenseHandler.Close();
		}

		#endregion

		#region UserControl Methods

		/// <summary>
		/// Generate the UserControl for configuring a type of item that this ItemManager manages.
		/// </summary>
		/// <returns></returns>
		public override UserControl GenerateDetailUserControl()
		{
			_userControl = new ServerConnectionUserControl();
			_userControl.ConfigurationChangedByUser += new EventHandler(ConfigurationChangedByUserHandler);
			return _userControl;
		}

		/// <summary>
		/// The UserControl is no longer being used, and related resources can be released.
		/// </summary>
		public override void ReleaseUserControl()
		{
			if (_userControl != null)
			{
				_userControl.ConfigurationChangedByUser -= new EventHandler(ConfigurationChangedByUserHandler);
				_userControl = null;
			}
		}

		/// <summary>
		/// Clear all user entries on the UserControl, all visible fields should be blank or default values.
		/// </summary>
		public override void ClearUserControl()
		{
			CurrentItem = null;
			if (_userControl != null)
			{
				_userControl.ClearContent();
			}
		}

		/// <summary>
		/// Fill the UserControl with the content of the Item or the data it represent.
		/// </summary>
		/// <param name="item">The Item to work with</param>
		public override void FillUserControl(Item item)
		{
			CurrentItem = item;
			if (_userControl != null)
			{
				_userControl.FillContent(item);
			}
		}

		/// <summary>
		/// A user control to display when the administrator clicks on the treeNode.
		/// This can be a help page or a status over of the configuration
		/// </summary>
		public override ItemNodeUserControl GenerateOverviewUserControl()
		{
			return
				new VideoOS.Platform.UI.HelpUserControl(
					ServerConnectionDefinition.TreeNodeImage,
					"Server Connection license",
					"Here you can define some virtual connections and see how the license count is handled.");
		}

		/// <summary>
		/// Create a usercontrol to be used for adding a new item.<br/>
		/// Just fill the user control with default content, but do NOT add it to your configuration yet.<br/>
		/// Implementation of this method will determine the flow of calls for creating new Items.<br/>
		/// When implemented, this UserControl is shown to the user, and upon OK pressed the ValidateAddUserControl is called
		/// for the plugin to validate the entry, and a OK response will result in closing the form containing the AddUserControl and 
		/// a call the the Create() method including the AddUserControl.<br/>
		/// If not implementing this method, the Create() method WITHOUT the AddUserControl parameter is called, and a default
		/// Item is then created, stored (where ever this ItemManager stores it configuration), and displayed in the
		/// TreeView.<br/>
		/// Last step is that the new item node is selected and the ItemManager.FillUserControl is called
		/// to fill the large configuration form.
		/// </summary>
		/// <returns></returns>
		public override UserControl GenerateAddUserControl(Item parentItem, FQID suggestedFQID)
		{
			return new ServerConnectionAddUserControl();
		}

		/// <summary>
		/// Validate the content of the AddUserControl generated by this class's GenerateAddUserControl.
		/// </summary>
		/// <param name="addUserControl"></param>
		/// <returns>True if the content is valid.</returns>
		public override bool ValidateAddUserControl(UserControl addUserControl)
		{
            ServerConnectionAddUserControl control = addUserControl as ServerConnectionAddUserControl;
            return control.IsValid;
        }

		#endregion

		#region Working with currentItem

		/// <summary>
		/// Get the name of the current Item.
		/// </summary>
		/// <returns></returns>
		public override string GetItemName()
		{
			if (_userControl != null)
			{
				return _userControl.DisplayName;
			}
			return "";
		}

		/// <summary>
		/// Update the name for current Item.  the user edited the Name via F2 in the TreeView
		/// </summary>
		/// <param name="name"></param>
		public override void SetItemName(string name)
		{
			if (_userControl != null)
			{
				_userControl.DisplayName = name;
			}
		}

		/// <summary>
		/// Validate the user entry, and return true for OK.<br/>
		/// External configuration should be saved during this call.<br/>
		/// Any errors should be displayed to the user, and the field in 
		/// error should get focus.
		/// </summary>
		/// <returns>Indicates error in user entry.  True is a valid entry</returns>
		public override bool ValidateAndSaveUserControl()
		{
			if (CurrentItem != null)
			{
				if (_userControl != null)
				{
					//Get user entered fields
					_userControl.UpdateItem(CurrentItem);
				}

				//In this template we save configuration on the VMS system
				Configuration.Instance.SaveItemConfiguration(ServerConnectionDefinition.ServerConnectionPluginId, CurrentItem);
			}
			return true;
		}

		/// <summary>
		/// Create a new item. Insert default values. 
		/// The parentFQID can be null, when a top level node is created (e.g. no parent)
		/// The new fqid should be filled with ServerID, ParentId, ObjectId or ObjectIdString and Kind.
		/// </summary>
		/// <param name="parentItem">Identifies the configuration parent to the new item.</param>
		/// <param name="suggestedFQID">A pre-generated fqid with above fields filled. 
		/// The ObjectId or ObjectIdString can be overridden.</param>
		/// <returns>A new Item, only the FQID and Name field are required to be filled.  The return valus is used to identify and select the item tree node</returns>
		public override Item CreateItem(Item parentItem, FQID suggestedFQID)
		{
			/*
			 * This CreateItem method is not used when a AddUserControl is returned during a 'new' process.
			 * 
			CurrentItem = new Item(suggestedFQID, "Enter a name");
			if (_userControl != null)
			{
				_userControl.FillContent(CurrentItem);
			}
			Configuration.Instance.SaveItemConfiguration(ServerConnectionDefinition.ServerConnectionPluginId, CurrentItem);

			//Make sure to reserve a license for this item, real or temporary
			ServerConnectionLicenseHandler.RegisterItem(GetItems(), CurrentItem);
			*/
			throw new NotImplementedException();
		}

		/// <summary>
		/// Create a new item. Insert values as user has entered on the AddUserControl.<br/>
		/// The parentFQID can be null, when a top level node is created (e.g. no parent)
		/// The new fqid should be filled with ServerID, ParentId, ObjectId or ObjectIdString and Kind.
		/// </summary>
		/// <param name="parentItem">Identifies the configuration parent to the new item.</param>
		/// <param name="suggestedFQID">A pre-generated fqid with above fields filled. 
		/// The ObjectId or ObjectIdString can be overridden.</param>
		/// <param name="addUserControl">A filled user control returned by the GeneratedAddUserControl method after it has been displayed and edited by the user</param>
		/// <returns>A new Item, only the FQID and Name field are required to be filled.  The return value is used to identify and select the item tree node</returns>
		public override Item CreateItem(Item parentItem, FQID suggestedFQID, UserControl addUserControl)
		{
			CurrentItem = new Item(suggestedFQID, ((ServerConnectionAddUserControl)addUserControl).ItemName);
			Configuration.Instance.SaveItemConfiguration(ServerConnectionDefinition.ServerConnectionPluginId, CurrentItem);

			//Make sure to reserve a license for this item, real or temporary
			ServerConnectionLicenseHandler.RegisterItem(GetItems(), CurrentItem);
			return CurrentItem;
		}

		/// <summary>
		/// When an administrator selects the context menu Delete item, or press the DEL key, 
		/// a confirmation dialog is displayed and upon administrator confirms this method is called.
		/// <code>
		/// For configurations saved on the video server, the following code can be used:
		/// if (item != null)
		/// {
		///     Configuration.Instance.DeleteItemConfiguration(MyPluginId, item);
		///	}
		/// </code>
		/// </summary>
		/// <param name="item">The Item to delete</param>
		public override void DeleteItem(Item item)
		{
			if (item != null)
			{
				Configuration.Instance.DeleteItemConfiguration(ServerConnectionDefinition.ServerConnectionPluginId, item);

				// Unregister the license reserved for this item, and try to reallocate it to another in trial mode
				ServerConnectionLicenseHandler.UnRegisterItem(item);
			}

		}
		#endregion

		#region Configuration Access Methods

		/// <summary>
		/// Returns a list of all Items of this Kind
		/// </summary>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems()
		{
			//All items in this sample are stored with the Video, therefor no ServerIDs or parent ids is used.
			List<Item> items = Configuration.Instance.GetItemConfigurations(ServerConnectionDefinition.ServerConnectionPluginId, null, _kind);

			// At this point, you could loop through to:
			// 1) Construct your own Item class based on the default Item class
			// 2) Mark each item for how each license state is (Licensed, Trial, after end-trial)
			// 3) Return the new set of Items

			return items;
		}

		/// <summary>
		/// Returns a list of all Items from a specific server.
		/// </summary>
		/// <param name="parentItem">The parent Items</param>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems(Item parentItem)
		{
			List<Item> items = Configuration.Instance.GetItemConfigurations(ServerConnectionDefinition.ServerConnectionPluginId, parentItem, _kind);

			// At this point, you could loop through to:
			// 1) Construct your own Item class based on the default Item class
			// 2) Mark each item for how each license state is (Licensed, Trial, after end-trial)
			// 3) Return the new set of Items

			return items;
		}

		/// <summary>
		/// Returns the Item defined by the FQID. Will return null if not found.
		/// </summary>
		/// <param name="fqid">Fully Qualified ID of an Item</param>
		/// <returns>An Item</returns>
		public override Item GetItem(FQID fqid)
		{
			Item item = Configuration.Instance.GetItemConfiguration(ServerConnectionDefinition.ServerConnectionPluginId, _kind, fqid.ObjectId);

			// At this point, you could loop through to:
			// 1) Construct your own Item class based on the default Item class
			// 2) Mark item for how each license state is (Licensed, Trial, after end-trial)
			// 3) Return the new Item

			return item;
		}

		#endregion

		#region Messages and Status

		/// <summary>
		/// Return the operational state of a specific Item.
		/// This is used by the Event Server.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public override OperationalState GetOperationalState(Item item)
		{
			return OperationalState.Ok;		// Everything is OK for the sepcified Item
		}

		/// <summary>
		/// Just before a context menu is displayed, each line on the context menu is checked for it should be enabled or disabled.
		/// This method is called with the following command (If allowed by the ItemNode definition)<br/>
		///   "ADD" - for the "Add new ..." <br/>
		///   "DELETE" - for the "Delete ..."<br/>
		///   "RENAME" - for rename<br/>
		/// If your plugin has the configuration stored on another server, and management is not possible
		/// via the ItemManager, then this method can be used to disable all contextmenu actions.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public override bool IsContextMenuValid(string command)
		{
			// We want to ensure that the administrator cannot add more Items that is allowed by the License count + temporary items in grace period
			if (command == "ADD" && !ServerConnectionLicenseHandler.IsAddPossible(GetItems()))
				return false;
			return true;
		}
		#endregion

	}
}

