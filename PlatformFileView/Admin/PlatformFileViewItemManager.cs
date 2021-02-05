using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace PlatformFileView.Admin
{
    public class PlatformFileViewItemManager : ItemManager
    {
        private PlatformFileViewUserControl _userControl;

        public PlatformFileViewItemManager() : base()
        {
        }

        #region admin user control support 
        public override UserControl GenerateDetailUserControl()
        {
            _userControl = new PlatformFileViewUserControl();
			_userControl.DisplayName = "";
			_userControl.ConfigurationChangedByUser += new EventHandler(ConfigurationChangedByUserHandler);
            return _userControl;
        }

        public override void FillUserControl(Item item)
        {
            CurrentItem = item;
            if (item == null)
            {
                ClearUserControl();
                return;
            }

            _userControl.DisplayName = item.Name;
            _userControl.ServerFQID = item.FQID;
            _userControl.FillContent();
        }

        public override void ClearUserControl()
        {
            if (_userControl != null)
                _userControl.DisplayName = "";
        }
        #endregion

        #region admin treeview name change support 
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

        #endregion

        #region admin management new file server
        /// <summary>
        /// Validate the user entry, and return true for OK
        /// </summary>
        /// <returns></returns>
        public override bool ValidateAndSaveUserControl()
        {
            if (_userControl != null && CurrentItem!=null)
            {
                CurrentItem.Name = _userControl.DisplayName;
                CurrentItem.FQID = _userControl.ServerFQID;
                Configuration.Instance.SaveItemConfiguration(PlatformFileViewDefinition.PlatformFileViewPluginId, CurrentItem);
                
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
			FQID fqid = new FQID(new ServerId("FILE", "localhost", 25243, Guid.NewGuid()));
			fqid.ObjectId = Guid.NewGuid();
			fqid.Kind = PlatformFileViewDefinition.PlatformFolderKind;

			CurrentItem = new Item(fqid, "Enter a name");
			if (_userControl != null)
			{
			    _userControl.DisplayName = CurrentItem.Name;
			    _userControl.ServerFQID = CurrentItem.FQID;
				_userControl.FillContent();
			}
			Configuration.Instance.SaveItemConfiguration(PlatformFileViewDefinition.PlatformFileViewPluginId, CurrentItem);
			return CurrentItem;
		}

		/// <summary>
		/// Delete an Item
		/// </summary>
		/// <param name="item">The Item to delete</param>
		public override void DeleteItem(Item item)
		{
			if (item != null)
			{
				Configuration.Instance.DeleteItemConfiguration(PlatformFileViewDefinition.PlatformFileViewPluginId, item);
			}

		}
        #endregion

        #region provide different lookup methods
        public override List<Item> GetItems()
		{
		    List<Item> items = Configuration.Instance.GetItemConfigurations(PlatformFileViewDefinition.PlatformFileViewPluginId, null,
                PlatformFileViewDefinition.PlatformFolderKind);
            List<Item> result = new List<Item>();

            // Convert all from Item to FolderItem, as it contains some overrides we need (Properties are ignored - we dont use them)
		    foreach (Item item in items)
		    {
                result.Add(new FolderItem(item.FQID, item.Name));
		    }
		    return result;
		}

		public override List<Item> GetItems(Item parentItem)
		{
		    return parentItem.GetChildren();
        }

		public override Item GetItem(FQID fqid)
		{
		    List<Item> rootItems = GetItems();
		    foreach (Item item in rootItems)
		    {
		        if (item.FQID.ObjectId == fqid.ObjectId)
		            return item;
		    }
		    return null;
        }
        #endregion
    }
}

