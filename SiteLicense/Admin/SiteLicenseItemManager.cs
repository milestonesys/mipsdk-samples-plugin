using System.Collections.Generic;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace SiteLicense.Admin
{
	/// <summary>
	/// In this sample the ItemManager is not used for anything else than initializing the SiteLicenseHandler, 
	/// all other methods have dummy code.
	/// </summary>
	public class SiteLicenseItemManager : ItemManager
	{

		#region Constructors

		public SiteLicenseItemManager()
		{
		}

		public override void Init()
		{
			SiteLicenseHandler.Init();
		}

		public override void Close()
		{
		}

		#endregion

		#region UserControl Methods

		public override UserControl GenerateDetailUserControl()
		{
			return new UserControl();
		}

		/// <summary>
		/// A user control to display when the administrator clicks on the treeNode.
		/// This can be a help page or a status over of the configuration
		/// </summary>
		public override ItemNodeUserControl GenerateOverviewUserControl()
		{
			return
				new VideoOS.Platform.UI.HelpUserControl(
					SiteLicenseDefinition.TreeNodeImage,
					"SiteLicense",
					SiteLicenseHandler.LicenseInfo());
		}

		public override void ReleaseUserControl()
		{
		}

		public override void ClearUserControl()
		{
		}

		public override void FillUserControl(Item item)
		{
		}

		#endregion

		#region Working with currentItem

		public override string GetItemName()
		{
			return "SiteLicense";
		}

		public override void SetItemName(string name)
		{
		}

		public override bool ValidateAndSaveUserControl()
		{
			return true;
		}

		public override Item CreateItem(Item parentItem, FQID suggestedFQID)
		{
			throw new MIPException("No create possible in this sample");
		}

		public override void DeleteItem(Item item)
		{
		}
		#endregion

		#region Configuration Access Methods

		public override List<Item> GetItems()
		{
			return new List<Item>();
		}

		public override List<Item> GetItems(Item parentItem)
		{
			return new List<Item>();
		}

		public override Item GetItem(FQID fqid)
		{
			return null;
		}

		#endregion

		#region Messages and Status

		/// <summary>
		/// Just before a context menu is displayed, each line on the context menu is checked for it should be enabled or disabled.
		/// This method is called with the following command (If allowed by the ItemNode definition)<br/>
		///   "ADD" - for the "Add new ..." <br/>
		///   "DELETE" - for the "Delete ..."<br/>
		///   "RENAME" - for rename<br/>
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public override bool IsContextMenuValid(string command)
		{
			return false;		// all commands are not valid in this sample
		}
		#endregion

	}
}

