using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace ServiceTest.Admin
{
    public class ServiceTestItemManager : ItemManager
    {
        private ServiceTestUserControl _userControl;

        #region Constructors

        public ServiceTestItemManager()
            : base()
        {
        }

        public override void Init()
        {
            List<Item> checkCurrent = GetItems();
            if (checkCurrent.Count == 0)
            {
                FQID fqid = new FQID(new ServerId("NA", "localhost", 80, Guid.NewGuid()));
                fqid.ObjectId = Guid.NewGuid();
                fqid.Kind = ServiceTestDefinition.ServiceTestKind;

                CurrentItem = new Item(fqid, "Enter a name");
                if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Administration)
                {
                    Configuration.Instance.SaveItemConfiguration(ServiceTestDefinition.ServiceTestPluginId, CurrentItem);
                }
            }
            else
            {
                CurrentItem = checkCurrent[0];
            }
            base.Init();
        }
        #endregion

        #region UserControl Methods

        /// <summary>
        /// Generate the UserControl for configuring a type of item that this ItemManager manages.
        /// </summary>
        /// <returns></returns>
        public override UserControl GenerateDetailUserControl()
        {
            _userControl = new ServiceTestUserControl();
            return _userControl;
        }

        /// <summary>
        /// A user control to display when the administrator clicks on the treeNode.
        /// This can be a help page or a status over of the configuration
        /// </summary>
        public override ItemNodeUserControl GenerateOverviewUserControl()
        {
            return new ItemNodeUserControl();
        }



        /// <summary>
        /// Clear all user entries on the UserControl.
        /// </summary>
        public override void ClearUserControl()
        {
            CurrentItem = null;
            _userControl.ClearContent();
        }

        /// <summary>
        /// Fill the UserControl with the content of the Item or the data it represent.
        /// </summary>
        /// <param name="item">The Item to work with</param>
        public override void FillUserControl(Item item)
        {
            CurrentItem = item;
            _userControl.FillContent(item);
        }

        #endregion

        #region Working with currentItem

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
            if (CurrentItem != null)
            {
                //Get user entered fields

                //Save configuration together with server
                Configuration.Instance.SaveItemConfiguration(ServiceTestDefinition.ServiceTestPluginId, CurrentItem);
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
            FQID fqid = new FQID(new ServerId("NA", "localhost", 80, Guid.NewGuid()));
            fqid.ObjectId = Guid.NewGuid();
            fqid.Kind = ServiceTestDefinition.ServiceTestKind;

            CurrentItem = new Item(fqid, "Enter a name");
            if (_userControl != null)
            {
                _userControl.FillContent(CurrentItem);
            }
            Configuration.Instance.SaveItemConfiguration(ServiceTestDefinition.ServiceTestPluginId, CurrentItem);
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
                Configuration.Instance.DeleteItemConfiguration(ServiceTestDefinition.ServiceTestPluginId, item);
            }

        }
        #endregion

        #region Configuration Access Methods

        public override List<Item> GetItems()
        {
            //All items in this sample are stored with the Video, therefore no ServerId is used.
            List<Item> items = Configuration.Instance.GetItemConfigurations(ServiceTestDefinition.ServiceTestPluginId, null, ServiceTestDefinition.ServiceTestKind);
            return items;
        }

        /// <summary>
        /// Returns a list of all Items from a specific server.
        /// </summary>
        /// <param name="serverId">The server managing the Items</param>
        /// <returns>A list of items.  Allowed to return null if no Items found.</returns>
        public override List<Item> GetItems(Item parentItem)
        {
            //All items in this sample are stored with the Video, therefore no ServerId is used.
            List<Item> items = Configuration.Instance.GetItemConfigurations(ServiceTestDefinition.ServiceTestPluginId, parentItem, ServiceTestDefinition.ServiceTestKind);
            return items;
        }

        /// <summary>
        /// Returns the Item defined by the FQID. Will return null if not found.
        /// </summary>
        /// <param name="fqid">Fully Qualified ID of an Item</param>
        /// <returns>An Item</returns>
        public override Item GetItem(FQID fqid)
        {
            Item item = Configuration.Instance.GetItemConfiguration(ServiceTestDefinition.ServiceTestPluginId, ServiceTestDefinition.ServiceTestKind, fqid.ObjectId);
            return item;
        }

        #endregion

    }
}

