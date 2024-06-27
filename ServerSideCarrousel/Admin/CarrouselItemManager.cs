using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace ServerSideCarrousel.Admin
{
    public class CarrouselItemManager : ItemManager
    {
        private CarrouselUserControl _userControl;
        private Guid _kind;

        public CarrouselItemManager(Guid kind) : base()
        {
            _kind = kind;
        }

        public override UserControl  GenerateDetailUserControl()
        {
            _userControl = new CarrouselUserControl();
            _userControl.ConfigurationChangedByUser += new EventHandler(ConfigurationChangedByUserHandler);
            return _userControl;
        }

        public override ItemNodeUserControl GenerateOverviewUserControl()
        {
            return
                new VideoOS.Platform.UI.HelpUserControl(
                    ServerSideCarrouselDefinition.TreeNodeImage,
                    "Server Side Carrousel",
                    "This sample show how selection of multiple cameras can be done, \r\nand later be retrieved by a Smart Client plug-in.");
        }

        public override void ClearUserControl()
        {
            if (_userControl != null)
                _userControl.ClearUserControl();
        }

        public override void FillUserControl(Item item)
        {
            CurrentItem = item;
            if (_userControl != null)
            {
                _userControl.CarrouselName = item.Name;
                if (item.Properties.ContainsKey("DefaultSeconds"))
                {
                    _userControl.DefaultSeconds = item.Properties["DefaultSeconds"];
                }
                _userControl.FillContent();

                CarrouselConfigUtil.BuildCarrouselList(item, CarrouselListHandler);
            }
        }
        

        private delegate void CarrouselListHandlerDelegate(List<CarrouselTreeNode> list);
        private void CarrouselListHandler(List<CarrouselTreeNode> list)
        {
            //Make sure we are on the UI thread
            if (_userControl.InvokeRequired)
                _userControl.BeginInvoke(new CarrouselListHandlerDelegate(CarrouselListHandler), new object[] {list});
            else				
                _userControl.ItemsSelected = list;
        }

        /// <summary>
        /// Get the name of the current Item.
        /// </summary>
        /// <returns></returns>
        public override string GetItemName()
        {
            if (_userControl != null)
            {
                return _userControl.CarrouselName;
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
                _userControl.CarrouselName = name;
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
                CurrentItem.Name = _userControl.CarrouselName;
                if (_userControl.ItemsSelected != null)
                {
                    StringBuilder sb = new StringBuilder();
                    XmlTextWriter xmlTextWriter = new XmlTextWriter(new System.IO.StringWriter(sb, System.Globalization.CultureInfo.InvariantCulture));
                    xmlTextWriter.WriteStartElement("SelectedDevices");
                    foreach (CarrouselTreeNode item in _userControl.ItemsSelected)
                    {
                        xmlTextWriter.WriteStartElement("Item");
                        item.Item.FQID.Serialize(xmlTextWriter);
                        xmlTextWriter.WriteElementString("Sort", "" + item.Sortix);
                        xmlTextWriter.WriteElementString("Seconds", "" + item.Seconds);
                        xmlTextWriter.WriteEndElement();
                    }
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Close();
                    CurrentItem.Properties["SelectedDevices"] = sb.ToString();
                    CurrentItem.Properties["DefaultSeconds"] = _userControl.DefaultSeconds;
                }
                Configuration.Instance.SaveItemConfiguration(ServerSideCarrouselDefinition.CarrouselPluginId, CurrentItem);
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
            fqid.Kind = ServerSideCarrouselDefinition.CarrouselKind;

            CurrentItem = new Item(fqid, "Enter a name");
            if (_userControl != null)
            {
                _userControl.FillContent();
                _userControl.DefaultSeconds = "10";
            }
            Configuration.Instance.SaveItemConfiguration(ServerSideCarrouselDefinition.CarrouselPluginId, CurrentItem);

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
                Configuration.Instance.DeleteItemConfiguration(ServerSideCarrouselDefinition.CarrouselPluginId, item);
            }
        }


        #region Configuration Access Methods


        /// <summary>
        /// Returns a list of all Items from a specific server.
        /// </summary>
        /// <returns>A list of items.  Allowed to return null if no Items found.</returns>
        public override List<Item> GetItems()
        {
            //All items in this sample are stored with the Video, therefor no ServerId is used.
            List<Item> items = Configuration.Instance.GetItemConfigurations(ServerSideCarrouselDefinition.CarrouselPluginId, null, ServerSideCarrouselDefinition.CarrouselKind);
            return items;
        }

        /// <summary>
        /// Returns a list of all Items from a specific server.
        /// </summary>
        /// <param name="parentItem">The parent Items</param>
        /// <returns>A list of items.  Allowed to return null if no Items found.</returns>
        public override List<Item> GetItems(Item parentItem)
        {
            List<Item> items = Configuration.Instance.GetItemConfigurations(ServerSideCarrouselDefinition.CarrouselPluginId, parentItem, ServerSideCarrouselDefinition.CarrouselKind);
            return items;
        }

        /// <summary>
        /// Returns the Item defined by the FQID. Will return null if not found.
        /// </summary>
        /// <param name="fqid">Fully Qualified ID of an Item</param>
        /// <returns>An Item</returns>
        public override Item GetItem(FQID fqid)
        {
            return Configuration.Instance.GetItemConfiguration(ServerSideCarrouselDefinition.CarrouselPluginId, _kind, fqid.ObjectId);
        }
        #endregion

    }
}

