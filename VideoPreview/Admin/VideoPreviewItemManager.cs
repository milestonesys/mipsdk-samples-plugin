using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using VideoPreview.Admin;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace VideoPreview.Admin
{
	public class VideoPreviewItemManager : ItemManager
	{
		private VideoPreviewUserControl _userControl;
		private Guid _kind;

		public VideoPreviewItemManager(Guid kind) : base()
		{
			_kind = kind;
		}

		public override void Close()
		{
			ReleaseUserControl();
		}

		public override UserControl GenerateDetailUserControl()
		{
			_userControl = new VideoPreviewUserControl();
			_userControl.ConfigurationChangedByUser += new EventHandler(ConfigurationChangedByUserHandler);
			return _userControl;
		}

		public override ItemNodeUserControl GenerateOverviewUserControl()
		{
			return
				new VideoOS.Platform.UI.HelpUserControl(
					VideoPreviewDefinition._treeNodeImage,
					"Video Preview",
					"This sample show how video can be displayed and used for placing overlay on top of the video, or to simply retrieve basic information about the actual video stream.");
		}

		public override void FillUserControl(Item item)
		{
			CurrentItem = item;
			if (_userControl != null)
			{
				_userControl.FillContent(item);
            }
		}

		public override void ClearUserControl()
		{
			CurrentItem = null;
			if (_userControl!=null)
				_userControl.ClearContent();
		}

		public override void ReleaseUserControl()
		{
			if (_userControl!=null)
			{
				_userControl.Close();
			}
		}

		/// <summary>
		/// Returns a list of all Items from a specific server.
		/// </summary>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems()
		{
			//All items in this sample are stored with the Video, therefor no ServerIs is used.
			List<Item> items = Configuration.Instance.GetItemConfigurations(VideoPreviewDefinition.VideoPreviewPluginId, null, _kind);
			return items;
		}

		/// <summary>
		/// Returns a list of all Items from a specific server.
		/// </summary>
		/// <param name="parentItem">Theparent Items</param>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems(Item parentItem)
		{
			List<Item> items = Configuration.Instance.GetItemConfigurations(VideoPreviewDefinition.VideoPreviewPluginId, parentItem, _kind);
			return items;
		}

		/// <summary>
		/// Returns the Item defined by the FQID. Will return null if not found.
		/// </summary>
		/// <param name="fqid">Fully Qualified ID of an Item</param>
		/// <returns>An Item</returns>
		public override Item GetItem(FQID fqid)
		{
			Item item = Configuration.Instance.GetItemConfiguration(VideoPreviewDefinition.VideoPreviewPluginId, _kind, fqid.ObjectId);
			return item;
		}
		
		public override string GetItemName()
		{
			return _userControl.DisplayName; 
		}

		public override void SetItemName(String name)
		{
			_userControl.DisplayName = name;
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
				_userControl.UpdateItem(CurrentItem);

				//Save configuration together with server
				Configuration.Instance.SaveItemConfiguration(VideoPreviewDefinition.VideoPreviewPluginId, CurrentItem);
			}
			return true;
		}

		/// <summary>
		/// Create a new Item
		/// </summary>
		/// <param name="parentItem">The parent for the new Item</param>
		/// <param name="suggestedFQID">A suggested FQID for the new Item</param>
		public override Item CreateItem(Item parentItem, FQID suggestedFQID)
		{
			CurrentItem = new Item(suggestedFQID, "Enter a name");
			if (_userControl != null)
			{
				_userControl.FillContent(CurrentItem);
			}
			Configuration.Instance.SaveItemConfiguration(VideoPreviewDefinition.VideoPreviewPluginId, CurrentItem);
			return CurrentItem;
		}

		/// <summary>
		/// Delete an old source
		/// </summary>
		/// <param name="fqid"></param>
		public override void DeleteItem(Item item)
		{
			if (item != null)
			{
				Configuration.Instance.DeleteItemConfiguration(VideoPreviewDefinition.VideoPreviewPluginId, item);
			}
		}

	}
}

