using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Messaging;
using Message = System.Windows.Forms.Message;

namespace ConfigDump.Admin
{
	public class ConfigDumpItemManager : ItemManager
	{
		private ConfigDumpUserControl _userControl;
		private object appReady;
		#region Constructors

		public ConfigDumpItemManager()
			: base()
		{
		}

		#endregion

		#region UserControl Methods

		public override void Init()
		{
			List<Item> checkCurrent = GetItems();
			if (checkCurrent.Count == 0)
			{
				FQID fqid = new FQID(new ServerId("NA", "localhost", 80, Guid.NewGuid()));
				fqid.ObjectId = Guid.NewGuid();
				fqid.Kind = ConfigDumpDefinition.ConfigDumpKind;

				CurrentItem = new Item(fqid, "Enter a name");
				if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Administration)
				{
					Configuration.Instance.SaveItemConfiguration(ConfigDumpDefinition.ConfigDumpPluginId, CurrentItem);
				}
			} else
			{
				CurrentItem = checkCurrent[0];
			}

			appReady = EnvironmentManager.Instance.RegisterReceiver(ApplicationReady,
			                                                        new MessageIdFilter(
			                                                        	VideoOS.Platform.Messaging.MessageId.System.
			                                                        		SystemConfigurationChangedIndication));
			base.Init();
		}

		private object ApplicationReady(VideoOS.Platform.Messaging.Message message, FQID sender, FQID related)
		{
			if (_userControl!=null)
				_userControl.FillContent(CurrentItem);		// Ignore item here
			return null;
		}


		/// <summary>
		/// Generate the UserControl for configuring a type of item that this ItemManager manages.
		/// </summary>
		/// <returns></returns>
		public override UserControl GenerateDetailUserControl()
		{
			_userControl = new ConfigDumpUserControl();
			_userControl.FillContent(CurrentItem);
			return _userControl;
		}

		/// <summary>
		/// A user control to display when the administrator clicks on the top TreeNode
		/// </summary>
		public override ItemNodeUserControl GenerateOverviewUserControl()
		{
			_userControl = new ConfigDumpUserControl();
			return _userControl;
		}

		/// <summary>
		/// Clear all user entries on the UserControl.
		/// </summary>
		public override void ClearUserControl()
		{
			_userControl.ClearContent();
		}

		/// <summary>
		/// Fill the UserControl with the content of the Item or the data it represent.
		/// </summary>
		/// <param name="item">The Item to work with</param>
		public override void FillUserControl(Item item)
		{		
			_userControl.FillContent(CurrentItem);		// Ignore item here
		}
		
		public override void ReleaseUserControl()
		{
			_userControl = null;
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
				return _userControl.Name;
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
				_userControl.Name = name;
			}
		}

		/// <summary>
		/// Validate the user entry, and return true for OK
		/// </summary>
		/// <returns></returns>
		public override bool ValidateAndSaveUserControl()
		{
			if (CurrentItem != null)
			{
				//Get user entered fields
				CurrentItem.Name = _userControl.Name;

				//Save configuration together with server
				Configuration.Instance.SaveItemConfiguration(ConfigDumpDefinition.ConfigDumpPluginId, CurrentItem);
			}
			return true;
		}

		#endregion

		#region Configuration Access Methods

		/// <summary>
		/// Returns a list of all Items from a specific server.
		/// </summary>
		/// <param name="serverId">The server managing the Items</param>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems(Item parentItem)
		{
			//All items in this sample are stored with the Video, therefor no ServerIs is used.
			List<Item> items = Configuration.Instance.GetItemConfigurations(ConfigDumpDefinition.ConfigDumpPluginId, parentItem, ConfigDumpDefinition.ConfigDumpKind);
			return items;
		}

		/// <summary>
		/// Returns the Item defined by the FQID. Will return null if not found.
		/// </summary>
		/// <param name="fqid">Fully Qualified ID of an Item</param>
		/// <returns>An Item</returns>
		public override Item GetItem(FQID fqid)
		{
			Item item = Configuration.Instance.GetItemConfiguration(ConfigDumpDefinition.ConfigDumpPluginId, fqid.Kind, fqid.ObjectId);
			return item; 
		}

		public override List<Item> GetItems()
		{
			//All items in this sample are stored with the Video, therefor no ServerIs is used.
			List<Item> items = Configuration.Instance.GetItemConfigurations(ConfigDumpDefinition.ConfigDumpPluginId, null, ConfigDumpDefinition.ConfigDumpKind);
			return items;
		}

		#endregion

	}

}

