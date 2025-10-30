using DemoServerApplication.ACSystem;
using DemoServerApplication.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using EventManager = DemoServerApplication.ACSystem.EventManager;

namespace DemoACServerApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WebService" in both code and config file together.
    [ServiceBehavior(UseSynchronizationContext = false)]
    public class WebService : IWebService
    {
        public UserDescriptor Connect(string userName, string password)
        {
            User user = ConfigurationManager.Instance.LookupUser(userName);
            if (user != null && user.Password == password)
            {
                return new UserDescriptor() { UserId = user.Id, UserName = user.Name, LastChanged = user.LastChanged };
            }
            return null;
        }

        public EventDescriptor[] GetEventTypes(string userName, string password)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return null;

            ReadOnlyCollection<EventType> events = ConfigurationManager.Instance.EventTypes;
            List<EventDescriptor> eventDescriptors = new List<EventDescriptor>();
            foreach (EventType evt in events)
            {
                if (user.IsEventTypeVisible(evt.Id))
                    eventDescriptors.Add(new EventDescriptor { EventId = evt.Id, EventName = evt.Name, SourceType = evt.SourceType.ToString() });
            }

            return eventDescriptors.ToArray();
        }

        public DoorControllerDescriptor[] GetDoorControllers(string userName, string password)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
            {
                return null;
            }

            return ConfigurationManager.Instance.DoorControllers
                .Select(dc => new DoorControllerDescriptor { DoorControllerId = dc.Id, DoorControllerName = dc.Name })
                .ToArray();
        }

        public DoorDescriptor[] GetDoors(string userName, string password)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return null;

            ReadOnlyCollection<Door> doors = ConfigurationManager.Instance.Doors;
            List<DoorDescriptor> doorDescriptors = new List<DoorDescriptor>();
            foreach (Door door in doors)
            {
                if (user.IsDoorVisible(door.Id))
                    doorDescriptors.Add(new DoorDescriptor { DoorId = door.Id, DoorName = door.Name, DoorControllerId = door.DoorControllerId, HasRexButton = door.HasRexButton, LockCommandSupported = door.LockCommandSupported && user.IsLockCommandVisible(), UnlockCommandSupported = door.UnlockCommandSupported && user.IsUnlockCommandVisible(), Latitude = door.Latitude, Longitude = door.Longitude });
            }
            return doorDescriptors.ToArray();
        }

        public CredentialHolderDescriptor GetCredentialHolder(Guid credentialHolderId)
        {
            CredentialHolder credentialHolder = ConfigurationManager.Instance.LookupCredentialHolder(credentialHolderId);
            if (credentialHolder != null)
            {
                return new CredentialHolderDescriptor { CredentialHolderId = credentialHolderId, CredentialHolderName = credentialHolder.Name, Roles = credentialHolder.Roles, CardId = credentialHolder.CardId, Department = credentialHolder.Department, ExpiryDate = credentialHolder.ExpiryDate, Picture = credentialHolder.PictureBytes };
            }

            return null;
        }

        public CredentialHolderDescriptor[] SearchCredentialHolders(string searchString)
        {
            IEnumerable<CredentialHolder> credentialHolders = ConfigurationManager.Instance.SearchCredentialHolder(searchString);
            if (credentialHolders != null)
            {
                return new List<CredentialHolder>(credentialHolders).ConvertAll(x => new CredentialHolderDescriptor() { CredentialHolderId = x.Id, CredentialHolderName = x.Name, Roles = x.Roles, CardId = x.CardId, Department = x.Department, ExpiryDate = x.ExpiryDate, Picture = null }).ToArray();
            }
            return null;
        }

        public BaseEvent[] GetEvents(string userName, string password, int timeoutMilliSecs, long startSequenceNumber, int count)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return null;

            System.Diagnostics.Debug.WriteLine("GetEvents, startSequenceNumber=" + startSequenceNumber.ToString());

            BaseEvent[] events = EventManager.Instance.GetEventsFromSequenceNumber(startSequenceNumber);
            if (events.Length > count)
                events = events.Take(count).ToArray();

            if (!user.IsAdministrator)
                events = events.Where(evt => user.IsEventTypeVisible(evt.EventId)).ToArray();

            return events;
        }

        public UserDescriptor GetUser(string userName)
        {
            UserDescriptor result = ((Func<UserDescriptor>)(() =>
           {
               User user = ConfigurationManager.Instance.LookupUser(userName);
               if (user == null)
                   return null;

               return new UserDescriptor() { UserId = user.Id, UserName = user.Name, LastChanged = user.LastChanged };
           }))();
            return result;
        }

        public bool UnlockDoor(string userName, string password, string vmsUserName, Guid doorId)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return false;

            if (!user.IsUnlockCommandVisible())
                return false;

            return DoorManager.Instance.UnlockDoor(doorId, userName, vmsUserName);
        }

        public bool LockDoor(string userName, string password, string vmsUserName, Guid doorId)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return false;

            if (!user.IsLockCommandVisible())
                return false;

            return DoorManager.Instance.LockDoor(doorId, userName, vmsUserName);
        }

        public bool UnlockAllDoorsOnDoorController(string userName, string password, string vmsUserName, Guid doorControllerId)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return false;

            if (!user.IsUnlockCommandVisible())
                return false;

            return DoorManager.Instance.UnlockAllDoorsOnDoorController(doorControllerId, userName, vmsUserName);
        }

        public bool LockAllDoorsOnDoorController(string userName, string password, string vmsUserName, Guid doorControllerId)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return false;

            if (!user.IsLockCommandVisible())
                return false;

            return DoorManager.Instance.LockAllDoorsOnDoorController(doorControllerId, userName, vmsUserName);
        }
        
        public bool UnlockAllDoors(string userName, string password, string vmsUserName)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return false;

            if (!user.IsUnlockCommandVisible())
                return false;

            return DoorManager.Instance.UnlockAllDoors(userName, vmsUserName);
        }

        public bool LockAllDoors(string userName, string password, string vmsUserName)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return false;

            if (!user.IsLockCommandVisible())
                return false;

            return DoorManager.Instance.LockAllDoors(userName, vmsUserName);
        }

        public DoorStatus GetDoorStatus(string userName, string password, Guid doorId)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return null;

            DoorStatus doorStatus = DoorManager.Instance.GetDoorStatus(doorId);
            if (doorStatus != null)
            {
                return new DoorStatus { DoorId = doorId, IsLocked = doorStatus.IsLocked, IsOpen = doorStatus.IsOpen };
            }

            return null;
        }

        public ClearAlarmCommand[] GetAlarmsToClear(string userName, string password)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return null;

            return AlarmManager.Instance.GetClearAlarmCommands().ToArray();
        }

        public void CloseAlarmOnDoor(string userName, string password, Guid doorId, Guid eventTypeId)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return;

            AlarmManager.Instance.CloseAlarm(doorId, eventTypeId);
        }

        public void UpdateDoorEnabledStates(string userName, string password, Tuple<string, bool>[] changedStates)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return;

            foreach (var item in changedStates)
            {
                DoorManager.Instance.SetEnabledState(item.Item1, item.Item2);
            }
        }

        public void UpdateEventTypeEnabledStates(string userName, string password, Tuple<string, bool>[] changedStates)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return;

            foreach (var item in changedStates)
            {
                Guid guid;
                if (Guid.TryParse(item.Item1, out guid))
                {
                    EventManager.Instance.AddEvent(EventManager.EventTypeEventTypeEnabledStateChanged, new EventTypeEnabledStatusEvent { EventTypeId = guid, IsEnabled = item.Item2 });
                }
            }
        }

        public void UpdateAccessControlUnitPosition(string userName, string password, Tuple<string, double, double>[] unitPositions)
        {
            User user = ConfigurationManager.Instance.CheckUser(userName, password);
            if (user == null)
                return;

            foreach (var item in unitPositions)
            {
                Guid guid;
                if (Guid.TryParse(item.Item1, out guid))
                {
                    ConfigurationManager.Instance.SetAccessControlUnitPosition(guid, item.Item2, item.Item3);
                }
            }
        }
    }
}
