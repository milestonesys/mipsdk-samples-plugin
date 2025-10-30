using DemoACServerApplication;
using DemoServerApplication.ACSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DemoServerApplication.Configuration
{
    public class ConfigurationManager
    {
        public static ConfigurationManager Instance = new ConfigurationManager();

        XmlSerializer _xmlSerializer = new XmlSerializer(typeof(SerializeData), new Type[] { typeof(CredentialHolder) });

        private ConfigurationManager()
        {
            _eventTypes.Add(EventManager.EventTypeAccessGranted.Id, EventManager.EventTypeAccessGranted);
            _eventTypes.Add(EventManager.EventTypeAccessGrantedAnonymous.Id, EventManager.EventTypeAccessGrantedAnonymous);
            _eventTypes.Add(EventManager.EventTypeAccessDenied.Id, EventManager.EventTypeAccessDenied);
            _eventTypes.Add(EventManager.EventTypeDoorUnlocked.Id, EventManager.EventTypeDoorUnlocked);
            _eventTypes.Add(EventManager.EventTypeDoorLocked.Id, EventManager.EventTypeDoorLocked);
            _eventTypes.Add(EventManager.EventTypeDoorOpened.Id, EventManager.EventTypeDoorOpened);
            _eventTypes.Add(EventManager.EventTypeDoorClosedAndLocked.Id, EventManager.EventTypeDoorClosedAndLocked);
            _eventTypes.Add(EventManager.EventTypeDoorForcedOpen.Id, EventManager.EventTypeDoorForcedOpen);
            _eventTypes.Add(EventManager.EventTypeDoorOpenTooLong.Id, EventManager.EventTypeDoorOpenTooLong);
            _eventTypes.Add(EventManager.EventTypeDoorControllerTampering.Id, EventManager.EventTypeDoorControllerTampering);
            _eventTypes.Add(EventManager.EventTypeDoorControllerPowerFailure.Id, EventManager.EventTypeDoorControllerPowerFailure);

            LoadConfiguration();
        }

        private object _configurationLock = new object();
        private Dictionary<Guid, CredentialHolder> _credentialHolders = new Dictionary<Guid, CredentialHolder>();
        private Dictionary<Guid, User> _users = new Dictionary<Guid, User>();
        private Dictionary<Guid, DoorController> _doorControllers = new Dictionary<Guid, DoorController>();
        private Dictionary<Guid, Door> _doors = new Dictionary<Guid, Door>();
        private Dictionary<Guid, EventType> _eventTypes = new Dictionary<Guid, EventType>();

        private Random _random = new Random(DateTime.Now.Millisecond);

        public event EventHandler CredentialHoldersChanged = delegate { };
        public event EventHandler DoorsChanged = delegate { };
        public event EventHandler DoorControllersChanged = delegate { };
        public event EventHandler UsersChanged = delegate { };
        public event EventHandler DoorPositionChanged = delegate { };

        public ReadOnlyCollection<CredentialHolder> CredentialHolders
        {
            get 
            {
                lock (_configurationLock)
                {
                    List<CredentialHolder> clonedCredentialHolders = _credentialHolders.Values.Select(credentialHolder => credentialHolder.Clone()).ToList();
                    return new ReadOnlyCollection<CredentialHolder>(clonedCredentialHolders);
                }
            }
        }

        public ReadOnlyCollection<DoorController> DoorControllers
        {
            get
            {
                lock (_configurationLock)
                {
                    return _doorControllers.Values.Select(c => c.Clone()).ToList().AsReadOnly();
                }
            }
        }

        public ReadOnlyCollection<Door> Doors
        {
            get
            {
                lock (_configurationLock)
                {
                    List<Door> clonedDoors = _doors.Values.Select(door => door.Clone()).ToList();
                    return new ReadOnlyCollection<Door>(clonedDoors);
                }
            }
        }

        public ReadOnlyCollection<User> Users
        {
            get
            {
                lock (_configurationLock)
                {
                    List<User> clonedUsers = _users.Values.Select(user => user.Clone()).ToList();
                    return new ReadOnlyCollection<User>(clonedUsers);
                }
            }
        }

        public ReadOnlyCollection<EventType> EventTypes
        {
            get
            {
                lock (_configurationLock)
                {
                    List<EventType> clonedEvents = _eventTypes.Values.Select(evt => evt.Clone()).ToList();
                    return new ReadOnlyCollection<EventType>(clonedEvents);
                }
            }
        }

        public CredentialHolder LookupCredentialHolder(Guid id)
        {
            lock (_configurationLock)
            {
                CredentialHolder credentialHolder;
                if (_credentialHolders.TryGetValue(id, out credentialHolder))
                    return credentialHolder.Clone();

                return null;
            }
        }

        public User LookupUser(Guid id)
        {
            lock (_configurationLock)
            {
                User user;
                if (_users.TryGetValue(id, out user))
                    return user;

                return null;
            }
        }

        public User LookupUser(string userName)
        {
            lock (_configurationLock)
            {
                foreach (User user in _users.Values)
                {
                    if (user.Name.Equals(userName))
                        return user;
                }

                return null;
            }
        }

        public User CheckUser(string userName, string password)
        {
            lock (_configurationLock)
            {
                foreach (User user in _users.Values)
                {
                    if (user.Name.Equals(userName))
                    {
                        if (user.Password.Equals(password))
                            return user;
                        else
                            return null;
                    }
                }
            }

            return null;
        }

        public CredentialHolder LookupCredentialHolder(string userName)
        {
            lock (_configurationLock)
            {
                foreach (CredentialHolder credentialHolder in _credentialHolders.Values)
                {
                    if (string.Equals(credentialHolder.Name, userName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return credentialHolder.Clone();
                    }
                }
                return null;
            }
        }

        public IEnumerable<CredentialHolder> SearchCredentialHolder(string searchString)
        {
            List<CredentialHolder> result = new List<CredentialHolder>();
            lock (_configurationLock)
            {
                foreach (CredentialHolder credentialHolder in _credentialHolders.Values)
                {
                    if (credentialHolder.Name.ToLowerInvariant().Contains(searchString.ToLowerInvariant()))
                    {
                        result.Add(credentialHolder.Clone());
                    }
                }
            }
            return result;
        }

        public Door LookupDoor(Guid id)
        {
            lock (_configurationLock)
            {
                Door door;
                if (_doors.TryGetValue(id, out door))
                    return door.Clone();

                return null;
            }
        }

        public EventType LookupEvent(Guid id)
        {
            lock (_configurationLock)
            {
                EventType evt;
                if (_eventTypes.TryGetValue(id, out evt))
                    return evt.Clone();

                return null;
            }
        }

        public void AddCredentialHolder(CredentialHolder credentialHolder)
        {
            lock (_configurationLock)
            {
                _credentialHolders.Add(credentialHolder.Id, credentialHolder.Clone());
                CredentialHoldersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
                EventManager.Instance.AddEvent(EventManager.EventTypeCredentialHolderChanged, new CredentialHolderChangedEvent { CredentialHolderId = credentialHolder.Id });
            }
        }

        public void AddCredentialHolders(CredentialHolder credentialHolder, int count, bool reset = false, bool setTestPicture = false, bool save=true)
        {
            lock (_configurationLock)
            {
                if (reset) _credentialHolders.Clear();
                var names = FindNextNames(credentialHolder.Name, _credentialHolders.Values.Select(c => c.Name), count);
                foreach (var name in names)
                {
                    CredentialHolder newCredentialHolder = credentialHolder.Clone();
                    newCredentialHolder.Id = Guid.NewGuid();
                    newCredentialHolder.Name = name;
                    if (setTestPicture) newCredentialHolder.SetTestPicture();
                    _credentialHolders.Add(newCredentialHolder.Id, newCredentialHolder);
                    EventManager.Instance.AddEvent(EventManager.EventTypeCredentialHolderChanged, new CredentialHolderChangedEvent { CredentialHolderId = newCredentialHolder.Id });
                }
                CredentialHoldersChanged.Invoke(this, new EventArgs());
                if (save) SaveConfiguration();
            }
        }

        public void UpdateCredentialHolder(CredentialHolder credentialHolder)
        {
            lock (_configurationLock)
            {
                _credentialHolders[credentialHolder.Id] = credentialHolder.Clone();
                CredentialHoldersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
                EventManager.Instance.AddEvent(EventManager.EventTypeCredentialHolderChanged, new CredentialHolderChangedEvent { CredentialHolderId = credentialHolder.Id });
            }
        }

        public void DeleteCredentialHolder(Guid id)
        {
            lock (_configurationLock)
            {
                _credentialHolders.Remove(id);
                CredentialHoldersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
                EventManager.Instance.AddEvent(EventManager.EventTypeCredentialHolderChanged, new CredentialHolderChangedEvent { CredentialHolderId = id });
            }
        }

        public void AddUser(User user)
        {
            lock (_configurationLock)
            {
                user.LastChanged = DateTime.UtcNow;
                _users.Add(user.Id, user.Clone());
                UsersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
                EventManager.Instance.AddEvent(EventManager.EventTypeUserChanged, new UserChangedEvent { UserId = user.Id, UserName = user.Name, LastChanged = user.LastChanged });
            }
        }

        public void UpdateUser(User user)
        {
            lock (_configurationLock)
            {
                user.LastChanged = DateTime.UtcNow;
                _users[user.Id] = user.Clone();
                UsersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
                EventManager.Instance.AddEvent(EventManager.EventTypeUserChanged, new UserChangedEvent { UserId = user.Id, UserName = user.Name, LastChanged = user.LastChanged });
            }
        }

        public void DeleteUser(Guid userId)
        {
            lock (_configurationLock)
            {
                User user = null;
                _users.TryGetValue(userId, out user);
                if (user == null)
                    return;

                _users.Remove(userId);
                UsersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
                EventManager.Instance.AddEvent(EventManager.EventTypeUserChanged, new UserChangedEvent { UserId = user.Id, UserName = user.Name, LastChanged = DateTime.UtcNow });
            }
        }

        public void AddDoor(Door door)
        {
            lock (_configurationLock)
            {
                _doors.Add(door.Id, door.Clone());
                DoorsChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
            }
        }
        public void AddDoorControllers(int count, bool reset = false, bool save = true)
        {
            lock (_configurationLock)
            {
                if (reset) _doorControllers.Clear();
                for (int i = 0; i < count; i++)
                {
                    DoorController _doorController = new DoorController() { Id = Guid.NewGuid(), Name = "DoorController " + (_doorControllers.Count + 1).ToString() };
                    _doorControllers.Add(_doorController.Id, _doorController);
                }
                DoorControllersChanged.Invoke(this, new EventArgs());
                if (save) SaveConfiguration();
            }
        }

        public void AddDoors(Door doorTemplate, int count, bool reset = false, bool save = true, bool withcontroller = false, bool setRandomCoordinates = false)
        {
            lock (_configurationLock)
            {
                if (reset) _doors.Clear();
                var names = FindNextNames(doorTemplate.Name, _doors.Values.Select(d => d.Name), count);
                if (_doorControllers.Count == 0)
                    withcontroller = false;
                Guid[] doorControllerIds = withcontroller ? _doorControllers.Keys.ToArray() : null;
                int i = 0;
                foreach (var name in names)
                {
                    Door newDoor = doorTemplate.Clone();
                    newDoor.Id = Guid.NewGuid();
                    newDoor.Name = name;
                    if (setRandomCoordinates)
                    {
                        newDoor.Latitude = RandomLatitude();
                        newDoor.Longitude = RandomLongitude();
                    }
                    if (withcontroller) newDoor.DoorControllerId = doorControllerIds[i % doorControllerIds.Length];
                    _doors.Add(newDoor.Id, newDoor);
                    i++;
                }
                DoorsChanged.Invoke(this, new EventArgs());
                if (save) SaveConfiguration();
            }
        }

        public void UpdateDoor(Door door)
        {
            lock (_configurationLock)
            {
                _doors[door.Id] = door.Clone();
                DoorsChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
            }
        }

        public void DeleteDoor(Guid id)
        {
            lock (_configurationLock)
            {
                _doors.Remove(id);
            }
        }

        public void NotifyDoorsChanged()
        {
            lock (_configurationLock)
            {
                DoorsChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
            }
        }

        public void AddDoorController(DoorController doorController)
        {
            lock (_configurationLock)
            {
                _doorControllers[doorController.Id] = doorController.Clone();
                DoorControllersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
            }
        }

        public void UpdateDoorController(DoorController doorController)
        {
            lock (_configurationLock)
            {
                _doorControllers[doorController.Id] = doorController.Clone();
                DoorControllersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
            }
        }

        public void DeleteDoorController(Guid id)
        {
            lock (_configurationLock)
            {
                _doorControllers.Remove(id);
                DoorControllersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
            }
        }

        public void RemoveUnusedDoorControllers()
        {
            lock (_configurationLock)
            {
                var usedKeys = new HashSet<Guid>(_doors.Values.Select(d => d.DoorControllerId));
                var unusedKeys = _doorControllers.Keys.Where(k => !usedKeys.Contains(k)).ToList();

                foreach (var key in unusedKeys)
                    _doorControllers.Remove(key);

                DoorControllersChanged.Invoke(this, new EventArgs());
                SaveConfiguration();
            }
        }

        public string GenerateDoorName(string namePrefix)
        {
            lock (_configurationLock)
            {
                return FindNextNames(namePrefix, _doors.Values.Select(d => d.Name), 1).First();
            }
        }

        public void SetAccessControlUnitPosition(Guid doorId, double latitude, double longitude)
        {
            lock (_configurationLock)
            {
                var door = _doors[doorId];
                if (door != null && latitude >= -90 && latitude <= 90 && longitude >= -180 && longitude <= 180)
                {
                    door.Latitude = latitude;
                    door.Longitude = longitude;

                    DoorPositionChanged.Invoke(this, new EventArgs());
                    SaveConfiguration();
                }
            }
        }

        public string GenerateDoorControllerName(string namePrefix)
        {
            lock (_configurationLock)
            {
                return FindNextNames(namePrefix, _doorControllers.Values.Select(dc => dc.Name), 1).First();
            }
        }

        public string GenerateCredentialHolderName(string namePrefix)
        {
            lock(_configurationLock)
            {
                return FindNextNames(namePrefix, _credentialHolders.Values.Select(ch => ch.Name), 1).First();
            }
        }

        private static IEnumerable<string> FindNextNames(string namePrefix, IEnumerable<string> names, int count)
        {
            // Create regex from the namePrefix (make sure to escape regex literals)
            var regex = new Regex(string.Format(CultureInfo.InvariantCulture, @"^{0}(?<index>\d+)$", Regex.Escape(namePrefix)));
            var maxIndex = names
                .Select(name => regex.Match(name))
                .Where(match => match.Success)
                .Select(match => int.Parse(match.Groups["index"].Value))
                .DefaultIfEmpty(-1)
                .Max();

            return Enumerable.Range(maxIndex + 1, count)
                .Select(i => string.Format(CultureInfo.InvariantCulture, "{0}{1}", namePrefix, i));
        }

        private void LoadConfiguration()
        {
            lock (_configurationLock)
            {
                string serializedString = string.Empty;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "configuration.xml");

                if (File.Exists(path))
                {
                    SerializeData serializeData;
                    using (var streamReader = new StreamReader(path, Encoding.UTF8))
                    {
                        serializeData = (SerializeData)_xmlSerializer.Deserialize(streamReader);
                    }

                    //restore credential holders
                    if (serializeData.CredentialHolders != null)
                    {
                        _credentialHolders = serializeData.CredentialHolders.ToDictionary(x => x.Id);
                        serializeData.CredentialHolders = null;
                    }

                    //restore door controllers
                    if (serializeData.DoorControllers != null)
                    {
                        _doorControllers = serializeData.DoorControllers.ToDictionary(x => x.Id);
                        serializeData.DoorControllers = null;
                    }

                    //restore doors
                    if (serializeData.Doors != null)
                    {
                        _doors = serializeData.Doors.ToDictionary(x => x.Id);
                    }

                    // restore users
                    if (serializeData.Users != null)
                    {
                        _users = serializeData.Users.ToDictionary(x => x.Id);
                        serializeData.Users = null;
                    }
                }

                // Create default admin user
                if (_users.Count == 0)
                {
                    User user = new User();
                    user.Name = "admin";
                    user.Password = "pass";
                    user.IsAdministrator = true;
                    user.LastChanged = DateTime.UtcNow;

                    _users.Add(user.Id, user);
                }
            }
        }

        public void SaveConfiguration()
        {
            lock (_configurationLock)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "configuration.xml");
                try
                {
                    SerializeData serializeData = new SerializeData();
                    serializeData.CredentialHolders = _credentialHolders.Values.ToArray();
                    serializeData.DoorControllers = _doorControllers.Values.ToArray();
                    serializeData.Doors = _doors.Values.ToArray();
                    serializeData.Users = _users.Values.ToArray();

                    using (var streamWriter = new StreamWriter(path, false, Encoding.UTF8))
                    {
                        _xmlSerializer.Serialize(streamWriter, serializeData);
                    }


                    FileSecurity fsec = File.GetAccessControl(path);
                    fsec.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.ReadAndExecute, AccessControlType.Allow));
                    File.SetAccessControl(path, fsec);
                }
                catch (Exception e)
                {
                    Debug.Assert(false, "Failed to save configuration to file '" + path + "'. Exception: " + e);
                }
            }
        }

        // It is safe to use a cryptographically weak pseudo-random number generator, because the coordinates are generated only for demo purposes.
        #pragma warning disable CA5394
        private double RandomLongitude() => _random.NextDouble() * 360 - 180;

        private double RandomLatitude() => _random.NextDouble() * 180 - 90;
        #pragma warning restore CA5394 

        public class SerializeData
        {
            public CredentialHolder[] CredentialHolders { get; set; }
            public DoorController[] DoorControllers { get; set; }
            public Door[] Doors { get; set; }
            public User[] Users { get; set; }
        }
    }
}
