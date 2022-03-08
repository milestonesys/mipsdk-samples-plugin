using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Util;
using Message = VideoOS.Platform.Messaging.Message;

namespace SensorMonitor.Admin
{
	/// <summary>
	/// This class manages the Controllers in the sample.
	/// It has Sensors as Children.
	/// It has a few ContextMenu actions
	/// It simulates ContextMenu actions directly, e.g. no real communication is going on.
	/// ContextMenu actions are created as Children as TriggerEvents, so they can be triggered by anyone
	/// Configuration is stored together with video server configuration.
	/// </summary>
	public class SensorMonitorControllerItemManager : ItemManager
	{
		private SensorMonitorUserControl _userControl;
		private readonly Guid _kind;
		private object msgRef;
		private readonly Collection<Guid> _downItems = new Collection<Guid>();

		// Definitions for Event Server and Alarm handling
		internal static Guid EventGroupId = new Guid("D1B0EF93-F728-4D0C-953A-55966AF28C04");
		internal EventGroup eventGroup = new EventGroup() {ID = EventGroupId, Name = "MIPSDK Sensor Monitor"};

		private Guid IdCtrlUp = new Guid("57F4E0DD-25F6-46E4-A089-E485EE134AC6");
		private Guid IdCtrlDown = new Guid("EE381C22-F1B7-4F1A-91D2-AEC4DEF805B1");
		private Guid IdStartRec = new Guid("AB432B06-69AC-4F3C-9A7E-781F50BFBEA4");
		private Guid IdStopRec = new Guid("50C77C71-87AA-40D1-8A3F-E8A7E43CCC06");
        private Guid IdSensorActive = new Guid("4D55B393-E982-4935-AB32-80ED9B12CBA5");
        private Guid IdSensorPassive = new Guid("C7D25F57-B416-4ADD-B1B9-2EADBD978D25");

        private static string ControllerDownMessage = "Controller Down";
        private static string ControllerUpMessage = "Controller Up";
        private static string StartRecordingMessage = "Start Recording";
        private static string StopRecordingMessage = "Stop Recording";
        internal static string SensorActiveMessage = "Sensor Active";
        internal static string SensorPassiveMessage = "Sensor Passive";

        #region Constructors

        public SensorMonitorControllerItemManager(Guid kind)
		{
			_kind = kind;

		}

		public override void Init()
		{
			msgRef = EnvironmentManager.Instance.RegisterReceiver(TriggerReceiver, new MessageIdFilter(MessageId.Control.TriggerCommand));
			EnvironmentManager.Instance.EnableConfigurationChangedService = true;
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
			_userControl = new SensorMonitorUserControl("Controller");
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
					SensorMonitorDefinition._controllerImage,
					"Sensor Controller",
					"Here you can add a sample sensor controller to be used for display on the Smart Client map, and for event handling in the Event Server.");
		}

		/// <summary>
		/// Clear all user entries on the UserControl.
		/// </summary>
		public override void ClearUserControl()
		{
			CurrentItem = null;
			if (_userControl!=null)
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
		/// The UserControl is not used any more. Release resources used by the UserControl.
		/// </summary>
		public override void ReleaseUserControl()
		{
			if (_userControl!=null)
			{
				_userControl.ConfigurationChangedByUser -= new EventHandler(ConfigurationChangedByUserHandler);
				_userControl = null;
			}
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

				//Send message to get the Sensor Tree display updated with the Sensor's parent node (The controller) updated
				EnvironmentManager.Instance.SendMessage(
					new VideoOS.Platform.Messaging.Message(VideoOS.Platform.Messaging.MessageId.System.ApplicationRefreshTreeViewCommand) { Data = SensorMonitorDefinition.SensorMonitorSensorKind });
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
			Configuration.Instance.SaveItemConfiguration(SensorMonitorDefinition.SensorMonitorPluginId, CurrentItem);
			//Send message to get the Sensor Tree display updated with the Sensors parent node (The controller) updated
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(VideoOS.Platform.Messaging.MessageId.System.ApplicationRefreshTreeViewCommand) { Data = SensorMonitorDefinition.SensorMonitorSensorKind });
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
				//Send message to get the Sensor Tree display updated with the Sensor's parent node (The controller) updated
				EnvironmentManager.Instance.SendMessage(
					new VideoOS.Platform.Messaging.Message(VideoOS.Platform.Messaging.MessageId.System.ApplicationRefreshTreeViewCommand) { Data = SensorMonitorDefinition.SensorMonitorSensorKind });
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
			List<Item> items = Configuration.Instance.GetItemConfigurations(SensorMonitorDefinition.SensorMonitorPluginId, null, _kind);
			List<Item> myItems = new List<Item>();
			foreach (Item item in items)
			{
				myItems.Add(new ControllerItem(item));
			}
			return myItems;
		}

		/// <summary>
		/// Returns a list of all Items from a specific server.
		/// </summary>
		/// <param name="parentItem">The parent Item</param>
		/// <returns>A list of items.  Allowed to return null if no Items found.</returns>
		public override List<Item> GetItems(Item parentItem)
		{
			List<Item> items = Configuration.Instance.GetItemConfigurations(SensorMonitorDefinition.SensorMonitorPluginId, parentItem, _kind);
			List<Item> myItems = new List<Item>();
			foreach (Item item in items)
			{
				myItems.Add(new ControllerItem(item));
			}
			return myItems;
		}

		/// <summary>
		/// Returns the Item defined by the FQID. Will return null if not found.
		/// </summary>
		/// <param name="fqid">Fully Qualified ID of an Item</param>
		/// <returns>An Item</returns>
		public override Item GetItem(FQID fqid)
		{
			Item item = Configuration.Instance.GetItemConfiguration(SensorMonitorDefinition.SensorMonitorPluginId, _kind, fqid.ObjectId);
			if (item == null)
				return null;
			return new ControllerItem(item);
		}

		#endregion

		#region Event Server support

		/// <summary>
		/// Return an Event Group, to assist in configuring the Alarms 
		/// </summary>
		/// <param name="culture"></param>
		/// <returns></returns>
		public override Collection<EventGroup> GetKnownEventGroups(CultureInfo culture)
		{
			return new Collection<EventGroup>() {eventGroup};
		}

	    public override Collection<EventType> GetKnownEventTypes(CultureInfo culture)
		{
			return new Collection<EventType>
			       	{
						new EventType() { 
							GroupID=EventGroupId, 
							ID= IdCtrlUp, 
							DefaultSourceKind=SensorMonitorDefinition.SensorMonitorCtrlKind, 
							Message=ControllerUpMessage, 
							SourceKinds=new List<Guid>(){SensorMonitorDefinition.SensorMonitorCtrlKind}},
						new EventType() { 
							GroupID=EventGroupId, 
							ID= IdCtrlDown, 
							DefaultSourceKind=SensorMonitorDefinition.SensorMonitorCtrlKind, 
							Message=ControllerDownMessage, 
							SourceKinds=new List<Guid>(){SensorMonitorDefinition.SensorMonitorCtrlKind}},
						new EventType() { 
							GroupID=EventGroupId, 
							ID= IdStartRec, 
							DefaultSourceKind=SensorMonitorDefinition.SensorMonitorCtrlKind, 
							Message=StartRecordingMessage, 
							SourceKinds=new List<Guid>(){SensorMonitorDefinition.SensorMonitorCtrlKind}},
						new EventType() { 
							GroupID=EventGroupId, 
							ID= IdStopRec, 
							DefaultSourceKind=SensorMonitorDefinition.SensorMonitorCtrlKind, 
							Message=StopRecordingMessage, 
							SourceKinds=new List<Guid>(){SensorMonitorDefinition.SensorMonitorCtrlKind}},
                  		new EventType() { 
							GroupID=EventGroupId, 
							ID= IdSensorActive, 
							DefaultSourceKind=SensorMonitorDefinition.SensorMonitorSensorKind, 
							Message=SensorActiveMessage, 
							SourceKinds=new List<Guid>(){SensorMonitorDefinition.SensorMonitorSensorKind}},
						new EventType() { 
							GroupID=EventGroupId, 
							ID= IdSensorPassive, 
							DefaultSourceKind=SensorMonitorDefinition.SensorMonitorSensorKind, 
							Message=SensorPassiveMessage, 
							SourceKinds=new List<Guid>(){SensorMonitorDefinition.SensorMonitorSensorKind}}

			       	};
		}

		private object TriggerReceiver(VideoOS.Platform.Messaging.Message message, FQID dest, FQID sender)
		{
			try
			{
				if (dest != null && dest.Kind == _kind)
				{
					string userSID = "";
					if (sender != null && sender.Kind == Kind.User)
						userSID = sender.ObjectIdString;				// Get hold of the user executing the command

					String command = message.Data as String;
					Item item = GetItem(dest);
					if (command != null && item != null)
					{
						if (command == "POWEROFF")
						{
                            EnvironmentManager.Instance.Log(false, "SensorMonitor", "SensorMonitor- Building poweroff Event " + command, null);
							if (userSID == null)
								SecurityAccess.CheckPermission(item, command);
							else
								SecurityAccess.CheckPermission(item, command, userSID);

							if (_downItems.Contains(item.FQID.ObjectId) == false)
								_downItems.Add(item.FQID.ObjectId);
							Item controller = GetItem(dest);
                            if (controller.Properties.ContainsKey("RelatedEventFQID") && controller.Properties.ContainsKey("RelatedFQID"))
                            {
                                FQID cameraFqid = new FQID(controller.Properties["RelatedFQID"]);
                                FQID eventFqid = new FQID(controller.Properties["RelatedEventFQID"]);
                                Message mes = new Message(MessageId.Control.TriggerCommand, cameraFqid);
                                EnvironmentManager.Instance.PostMessage(mes, eventFqid, cameraFqid);
                            }
							FQID cameraFQID = null;
							if (item.Properties.ContainsKey("RelatedFQID"))
							{
								cameraFQID = new FQID(item.Properties["RelatedFQID"]);
							}
							EventHeader eventHeader = new EventHeader()
							{
							    ID = Guid.NewGuid(),
							    Class = "Operational",
							    Type = "Power",
							    Timestamp = DateTime.Now,
							    Message = ControllerDownMessage,
							    Name = item.Name,
							    Source = new EventSource {FQID = item.FQID, Name = item.Name},
							    CustomTag = "<My><MiniXml>With no data-1</MiniXml></My>"
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
							EventServerControl.Instance.ItemStatusChanged(item);
						}
						if (command == "POWERON")
						{
                            EnvironmentManager.Instance.Log(false, "SensorMonitor", "SensorMonitor- Building poweron Event " + command, null);
							if (userSID == null)
								SecurityAccess.CheckPermission(item, command);
							else
								SecurityAccess.CheckPermission(item, command, userSID);

							EnvironmentManager.Instance.Log(false, "SensorMonitor", "SensorMonitor- Building poweron event ", null);
							if (_downItems.Contains(item.FQID.ObjectId))
								_downItems.Remove(item.FQID.ObjectId);
							FQID cameraFQID = null;
							if (item.Properties.ContainsKey("RelatedFQID"))
							{
								cameraFQID = new FQID(item.Properties["RelatedFQID"]);
							}
							EventHeader eventHeader = new EventHeader
							{
								ID = Guid.NewGuid(),
								Class = "Operational",
								Type = "Power",
								Timestamp = DateTime.Now,
								Message = ControllerUpMessage,
								Name = item.Name,
								Source = new EventSource { FQID = item.FQID, Name = item.Name },
							    CustomTag = "<My><MiniXml>With no data-2</MiniXml></My>"
							};
							AnalyticsEvent eventData = new AnalyticsEvent
							{
								EventHeader = eventHeader,				
							};
							if (cameraFQID!=null)
							{
								eventData.ReferenceList = new ReferenceList();
								eventData.ReferenceList.Add(new Reference() { FQID = cameraFQID });		// Ensure that camera will be presented in the preview

							}

							EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Server.NewEventCommand) { Data = eventData, RelatedFQID=cameraFQID });

							EventServerControl.Instance.ItemStatusChanged(item);
						}
						if (command == "STARTREC")
						{
                            EnvironmentManager.Instance.Log(false, "SensorMonitor", "SensorMonitor- Building start recording Event " + command, null);
							Item controller = GetItem(dest);
							if (controller.Properties.ContainsKey("RelatedFQID"))
							{
								FQID fqid = new FQID(controller.Properties["RelatedFQID"]);
								EnvironmentManager.Instance.SendMessage(
									new VideoOS.Platform.Messaging.Message(MessageId.Control.StartRecordingCommand), fqid);

								EventHeader eventHeader = new EventHeader
								{
									ID = Guid.NewGuid(),
									Class = "Operationel",
									Type = "UserCommand",
									Timestamp = DateTime.Now,
									Message = StartRecordingMessage,
									Name = item.Name,
									Source = new EventSource { FQID = item.FQID, Name = item.Name },
									CustomTag = "<My><MiniXml>With no data-3</MiniXml></My>"
								};
								AnalyticsEvent eventData = new AnalyticsEvent
								{
									EventHeader = eventHeader,
								};
								eventData.ReferenceList = new ReferenceList();
								eventData.ReferenceList.Add(new Reference() { FQID = fqid });		// Ensure that camera will be presented in the preview

								// We inform the rest of the Event Server, that this command has been issued (In case there is a AlarmDefinition that matches)
								EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Server.NewEventCommand) { Data = eventData, RelatedFQID = fqid });

							}
						}
						if (command == "STOPREC")
						{
                            EnvironmentManager.Instance.Log(false, "SensorMonitor", "SensorMonitor- Building stop recording Event " + command, null);
							Item controller = GetItem(dest);
							if (controller.Properties.ContainsKey("RelatedFQID"))
							{
								FQID fqid = new FQID(controller.Properties["RelatedFQID"]);
								EnvironmentManager.Instance.SendMessage(
									new VideoOS.Platform.Messaging.Message(MessageId.Control.StopRecordingCommand), fqid);

								EventHeader eventHeader = new EventHeader
								{
									ID = Guid.NewGuid(),
									Class = "Operationel",
									Type = "UserCommand",
									Timestamp = DateTime.Now,
									Message = StopRecordingMessage,
									Name = item.Name,
									Source = new EventSource { FQID = item.FQID, Name = item.Name },
									CustomTag = "<My><MiniXml>With no data-3</MiniXml></My>"
								};
								AnalyticsEvent eventData = new AnalyticsEvent
								{
									EventHeader = eventHeader,
								};
								eventData.ReferenceList = new ReferenceList();
								eventData.ReferenceList.Add(new Reference() { FQID = fqid });		// Ensure that camera will be presented in the preview

								// We inform the rest of the Event Server, that this command has been issued (In case there is a AlarmDefinition that matches)
								EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Server.NewEventCommand) { Data = eventData, RelatedFQID = fqid });

							}
						}
                        if (command == "TRIGGEREVENT")
                        {
                            EnvironmentManager.Instance.Log(false, "SensorMonitor", "SensorMonitor- Building trigger Event " + command, null);
                            Item controller = GetItem(dest);
                            if (controller.Properties.ContainsKey("RelatedEventFQID") && controller.Properties.ContainsKey("RelatedFQID"))
                            {
                                FQID cameraFqid = new FQID(controller.Properties["RelatedFQID"]);
                                FQID eventFqid = new FQID(controller.Properties["RelatedEventFQID"]);
                                Message mes = new Message(MessageId.Control.TriggerCommand, cameraFqid);
                                EnvironmentManager.Instance.PostMessage(mes, eventFqid, cameraFqid);
                            }
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
				EnvironmentManager.Instance.Log(false,"SensorMonitor","SensorMonitor-9 "+ex.Message,new [] {ex});
				//User not authorized to perform the action
			}
			return null;
		}

		public override OperationalState GetOperationalState(Item item)
		{
			if (_downItems.Contains(item.FQID.ObjectId))
				return OperationalState.Error;
			return OperationalState.Ok;
		}

		private int SomeCounter = 1;
		/// <summary>
		/// Build and return some details.  In this sample we ignore the language.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="language"></param>
		/// <returns></returns>
		public override string GetItemStatusDetails(Item item, string language)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.Encoding = Encoding.UTF8;
				xmlWriterSettings.OmitXmlDeclaration = false;
				using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
				{
					xmlWriter.WriteStartElement("details");
					xmlWriter.WriteAttributeString("language", "en-US");

					xmlWriter.WriteStartElement("detail");
					xmlWriter.WriteAttributeString("detailname", "Controller state" );
					xmlWriter.WriteElementString("detail_string", GetOperationalState(item).ToString());
					xmlWriter.WriteEndElement();

					xmlWriter.WriteStartElement("detail");
					xmlWriter.WriteAttributeString("detailname", "Some Counter" );
					xmlWriter.WriteElementString("detail_int64", ""+SomeCounter++);
					xmlWriter.WriteEndElement();

					xmlWriter.WriteEndElement();
					xmlWriter.Flush();
					memoryStream.Seek(0, SeekOrigin.Begin);
					return new StreamReader(memoryStream, Encoding.UTF8).ReadToEnd();
				}
			}
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

