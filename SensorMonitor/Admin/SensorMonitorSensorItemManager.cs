using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Util;

namespace SensorMonitor.Admin
{
    public class SensorMonitorSensorItemManager : ItemManager
    {
        private SensorMonitorUserControl _userControl;
        private readonly Guid _kind;
        private object msgRef;

        #region Constructors

        public SensorMonitorSensorItemManager(Guid kind)
        {
            _kind = kind;
        }

        public override void Init()
        {
            msgRef = EnvironmentManager.Instance.RegisterReceiver(TriggerReceiver, new MessageIdFilter(MessageId.Control.TriggerCommand));
        }

        public override void Close()
        {
            EnvironmentManager.Instance.UnRegisterReceiver(msgRef);
            msgRef = null;
        }

        #endregion

        #region UserControl Methods

        /// <summary>
        /// Generate the UserControl for configuring a type of item that this ItemManager manages.
        /// </summary>
        /// <returns></returns>
        public override UserControl GenerateDetailUserControl()
        {
            _userControl = new SensorMonitorUserControl("Sensor");
            _userControl.ConfigurationChangedByUser += new EventHandler(ConfigurationChangedByUserHandler);
            return _userControl;
        }

        /// <summary>
        /// A user control to display when the administrator clicks on the treeNode.
        /// This can be a help page or a status over of the configuration
        /// </summary>
        public override ItemNodeUserControl GenerateOverviewUserControl()
        {
            return
              new VideoOS.Platform.UI.HelpUserControl(
                SensorMonitorDefinition._treeNodeSensorImage,
                "Sensor Monitor - Sensor",
                "Here you can add a sample sensor assumed to be controlled by its parent controller.\r\nThe sample show how multi level Items can be created.");
        }

        /// <summary>
        /// Clear all user entries on the UserControl.
        /// </summary>
        public override void ClearUserControl()
        {
            CurrentItem = null;
            if (_userControl != null)
                _userControl.ClearContent();
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
        /// Use the Status XML format to describe the parent of the sensor
        /// </summary>
        /// <param name="parent">parent item</param>
        /// <returns></returns>
        private static string SensorDetails(Item parent, CultureInfo culture)
        {
            string parentLabel = SensorMonitorDefinition.GetTranslationString("ITEMSTATUSDETAILS_PARENT", culture);
            string orphanLabel = SensorMonitorDefinition.GetTranslationString("ITEMSTATUSDETAILS_ORPHAN", culture);

            MemoryStream xmlStream = new MemoryStream(256);
            XmlTextWriter xmlWriter = new XmlTextWriter(xmlStream, null) { Formatting = Formatting.None };
            // utf-8 encoding

            // create root element
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("details");
            xmlWriter.WriteAttributeString("language", culture.ToString());
            xmlWriter.WriteStartElement("detail");
            xmlWriter.WriteAttributeString("detailname", parentLabel);
            {
                xmlWriter.WriteStartElement("detail_string");
                if (parent != null)
                {
                    xmlWriter.WriteString(parent.Name);
                }
                else
                {
                    xmlWriter.WriteString(orphanLabel);
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            return Encoding.UTF8.GetString(xmlStream.GetBuffer());
        }

        public override string GetItemStatusDetails(Item item, String language)
        {
            var culture = new CultureInfo(language);

            SensorItem sensor = item as SensorItem;
            if (sensor != null)
            {
                return SensorDetails(sensor.GetParent(), culture);
            }
            return SensorDetails(item.GetParent(), culture);
        }

        private object TriggerReceiver(VideoOS.Platform.Messaging.Message message, FQID dest, FQID sender)
        {
            try
            {
                if (dest != null && dest.Kind == _kind)
                {
                    string userSID = "";
                    if (sender != null && sender.Kind == Kind.User)
                        userSID = sender.ObjectIdString;        // Get hold of the user executing the command

                    String command = message.Data as String;
                    Item item = GetItem(dest);

                    if (command != null && item != null)
                    {
                        // We have selected to use the "Manage" tick-mark for these operations:  ("Manage" is stored as "GENERIC_WRITE")
                        if (userSID == null)
                            SecurityAccess.CheckPermission(item, "GENERIC_WRITE");
                        else
                            SecurityAccess.CheckPermission(item, "GENERIC_WRITE", userSID);

                        if (command == "ACTIVATESENSOR")
                        {
                            EnvironmentManager.Instance.Log(false, "SensorMonitor", "Activate sensor" + command, null);
                            SensorItem.SensorActiveState[item.FQID.ObjectId] = true;
                            EventServerControl.Instance.ItemStatusChanged(item);

                            FQID cameraFQID = null;
                            if (item.Properties.ContainsKey("RelatedFQID"))
                            {
                                cameraFQID = new FQID(item.Properties["RelatedFQID"]);
                            }
                            EventHeader eventHeader = new EventHeader()
                            {
                                ID = Guid.NewGuid(),
                                Class = "Operational",
                                Type = "SensorState",
                                Timestamp = DateTime.Now,
                                Message = SensorMonitorControllerItemManager.SensorActiveMessage,
                                Name = item.Name,
                                Source = new EventSource { FQID = item.FQID, Name = item.Name },
                                CustomTag = "<My><MiniXml>Sensor info</MiniXml></My>"
                            };
                            AnalyticsEvent eventData = new AnalyticsEvent
                            {
                                EventHeader = eventHeader,
                            };
                            if (cameraFQID != null)
                            {
                                eventData.ReferenceList = new ReferenceList();
                                eventData.ReferenceList.Add(new Reference() { FQID = cameraFQID });		// Ensure that camera will be presented in the preview
                            }
                            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Server.NewEventCommand) { Data = eventData, RelatedFQID = cameraFQID });

                        }
                        if (command == "DEACTIVATESENSOR")
                        {
                            EnvironmentManager.Instance.Log(false, "SensorMonitor", "Deactive sensor " + command, null);
                            SensorItem.SensorActiveState[item.FQID.ObjectId] = false;
                            EventServerControl.Instance.ItemStatusChanged(item);

                            FQID cameraFQID = null;
                            if (item.Properties.ContainsKey("RelatedFQID"))
                            {
                                cameraFQID = new FQID(item.Properties["RelatedFQID"]);
                            }
                            EventHeader eventHeader = new EventHeader()
                            {
                                ID = Guid.NewGuid(),
                                Class = "Operational",
                                Type = "SensorState",
                                Timestamp = DateTime.Now,
                                Message = SensorMonitorControllerItemManager.SensorPassiveMessage,
                                Name = item.Name,
                                Source = new EventSource { FQID = item.FQID, Name = item.Name },
                                CustomTag = "<My><MiniXml>Some other info</MiniXml></My>"
                            };
                            AnalyticsEvent eventData = new AnalyticsEvent
                            {
                                EventHeader = eventHeader,
                            };
                            if (cameraFQID != null)
                            {
                                eventData.ReferenceList = new ReferenceList();
                                eventData.ReferenceList.Add(new Reference() { FQID = cameraFQID });		// Ensure that camera will be presented in the preview
                            }
                            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Server.NewEventCommand) { Data = eventData, RelatedFQID = cameraFQID });

                        }
                    }
                }

            }
            catch (NotAuthorizedMIPException)
            {
                throw;
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.Log(false, "SensorMonitor", "SensorMonitor-9 " + ex.Message, new[] { ex });
                //User not authorized to perform the action
            }
            return null;
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
        /// Validate the user entry, and return true for OK
        /// </summary>
        /// <returns></returns>
        public override bool ValidateAndSaveUserControl()
        {
            if (CurrentItem != null)
            {
                //Get user entered fields
                _userControl.UpdateItem(CurrentItem);

                //In this template we save configuration on the VMS system
                Configuration.Instance.SaveItemConfiguration(SensorMonitorDefinition.SensorMonitorPluginId, CurrentItem);
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
            CurrentItem = new SensorItem(suggestedFQID, "Enter a name", parentItem);
            if (_userControl != null)
            {
                _userControl.FillContent(CurrentItem);
            }
            Configuration.Instance.SaveItemConfiguration(SensorMonitorDefinition.SensorMonitorPluginId, CurrentItem);
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
                Configuration.Instance.DeleteItemConfiguration(SensorMonitorDefinition.SensorMonitorPluginId, item);
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
            //All items in this sample are stored with the Video, therefor no ServerIs or parent ids is used.
            List<Item> items = Configuration.Instance.GetItemConfigurations(SensorMonitorDefinition.SensorMonitorPluginId, null, _kind).Select(i => (Item)(new SensorItem(i))).ToList();
            return items;
        }

        /// <summary>
        /// Returns a list of all Items from a specific server.
        /// </summary>
        /// <param name="parentItem">The parent Items</param>
        /// <returns>A list of items.  Allowed to return null if no Items found.</returns>
        public override List<Item> GetItems(Item parentItem)
        {
            List<Item> items = Configuration.Instance.GetItemConfigurations(SensorMonitorDefinition.SensorMonitorPluginId, parentItem, _kind).Select(i => (Item)(new SensorItem(i))).ToList();
            return items;
        }

        /// <summary>
        /// Returns the Item defined by the FQID. Will return null if not found.
        /// </summary>
        /// <param name="fqid">Fully Qualified ID of an Item</param>
        /// <returns>An Item</returns>
        public override Item GetItem(FQID fqid)
        {
            Item item = Configuration.Instance.GetItemConfiguration(SensorMonitorDefinition.SensorMonitorPluginId, _kind, fqid.ObjectId);
            if (item != null)
            {
                return new SensorItem(item);
            }
            return item;
        }

        #endregion

        #region Translation
        /// <summary>
        /// Get dictionary to be used for translation for Smart Map
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override Dictionary<string, string> GetTranslationDictionary(CultureInfo culture)
        {
            return SensorMonitorDefinition.GetTranslationDictionary(culture);
        }
        #endregion
    }


}

