using System;
using System.Windows.Forms;
using VideoOS.Platform.Admin;

namespace Chat.Admin
{
	/// <summary>
	/// </summary>
	public class ChatItemManager : ItemManager
	{
		#region Constructors

		public ChatItemManager()
		{			
		}

		public override void Init()
		{
		}

		public override void Close()
		{
		}

		#endregion

		#region UserControl Methods

		/// <summary>
		/// Generate the UserControl for configuring a type of item that this ItemManager manages.
		/// </summary>
		/// <returns></returns>
		public override UserControl GenerateDetailUserControl()
		{
			return null;
		}

		/// <summary>
		/// A user control to display when the administrator clicks on the treeNode.
		/// This can be a help page or a status over of the configuration
		/// </summary>
		public override ItemNodeUserControl GenerateOverviewUserControl()
		{
			return new ChatItemNodeUserControl();
		}
		/*
		/// <summary>
		/// Clear all user entries on the UserControl.
		/// </summary>
		public override void ClearUserControl()
		{
			CurrentItem = null;
		}

		/// <summary>
		/// Fill the UserControl with the content of the Item or the data it represent.
		/// </summary>
		/// <param name="item">The Item to work with</param>
		public override void FillUserControl(Item item)
		{
		}

		/// <summary>
		/// The UserControl is not used any more. Release resources used by the UserControl.
		/// </summary>
		public override void ReleaseUserControl()
		{
			if (_userControl!=null)
			{
				_userControl = null;
			}
		}
		 */
		#endregion

		#region Working with currentItem

		/*
		/// <summary>
		/// Get the name of the current Item.
		/// </summary>
		/// <returns></returns>
		public override string GetItemName()
		{
			return "";
		}

		/// <summary>
		/// Update the name for current Item.  the user edited the Name via F2 in the TreeView
		/// </summary>
		/// <param name="name"></param>
		public override void SetItemName(string name)
		{
		}

		/// <summary>
		/// Validate the user entry, and return true for OK
		/// </summary>
		/// <returns></returns>
		public override bool ValidateAndSaveUserControl()
		{
			return true;
		}

		/// <summary>
		/// Create a new Item
		/// </summary>
		/// <param name="parentItem">The parent for the new Item</param>
		/// <param name="suggestedFQID">A suggested FQID for the new Item</param>
		public override Item CreateItem(Item parentItem, FQID suggestedFQID)
		{
			return null;
		}

		/// <summary>
		/// Delete an Item
		/// </summary>
		/// <param name="item">The Item to delete</param>
		public override void DeleteItem(Item item)
		{
		}
		 * */
		#endregion

		#region Configuration Access Methods

		/*
		/// <summary>
		/// Returns a list of all Items of this Kind
		/// </summary>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems()
		{
			return new List<Item>();
		}

		/// <summary>
		/// Returns a list of all Items from a specific server.
		/// </summary>
		/// <param name="parentItem">Theparent Items</param>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems(Item parentItem)
		{
			return new List<Item>();
		}

		/// <summary>
		/// Returns the Item defined by the FQID. Will return null if not found.
		/// </summary>
		/// <param name="fqid">Fully Qualified ID of an Item</param>
		/// <returns>An Item</returns>
		public override Item GetItem(FQID fqid)
		{
			return null;
		}
		*/
		#endregion

	}

}

