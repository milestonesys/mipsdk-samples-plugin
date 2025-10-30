using DemoACServerApplication;
using DemoServerApplication.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DemoServerApplication.ACSystem
{
    public class DoorManager
    {
        public static DoorManager Instance = new DoorManager();
        
        private Dictionary<Guid, DoorStatus> _doorStatus = new Dictionary<Guid, DoorStatus>();

        public class DoorStatusChangedEventArgs : EventArgs
        {
            public DoorStatus DoorStatus;
        }

        public class DoorEnabledStateChangedEventArgs : EventArgs
        {
            public Guid DoorId;
            public bool IsEnabled;
        }

        public event EventHandler<DoorStatusChangedEventArgs> DoorStatusChanged = delegate { };

        public event EventHandler<DoorEnabledStateChangedEventArgs> DoorEnabledStateChanged = delegate { };


        private DoorManager()
        { }

        public bool UnlockDoor(Guid id, string userName = null, string vmsUserName = null)
        {
            Door door = ConfigurationManager.Instance.LookupDoor(id);
            if (door != null)
            {
                if (!door.UnlockCommandSupported)
                {
                    return false;
                }

                lock (_doorStatus)
                {
                    DoorStatus doorStatus = InternalGetDoorStatus(id);
                    if (doorStatus.IsLocked)
                    {
                        doorStatus.IsLocked = false;
                        // Fire status update
                        FireStatusUpdate(doorStatus);
                        FireDoorUnlocked(door, userName, vmsUserName);
                    }
                }

                return true;
            }

            return false;
        }

        public bool LockDoor(Guid id, string userName = null, string vmsUserName = null)
        {
            Door door = ConfigurationManager.Instance.LookupDoor(id);
            if (door != null)
            {
                if (!door.LockCommandSupported)
                {
                    return false;
                }

                lock (_doorStatus)
                {
                    DoorStatus doorStatus = InternalGetDoorStatus(id);
                    if (!doorStatus.IsLocked)
                    {
                        doorStatus.IsLocked = true;
                        FireStatusUpdate(doorStatus);
                        FireDoorLocked(door, userName, vmsUserName);
                    }
                }
                return true;
            }

            return false;            
        }

        public bool LockAllDoors(string userName = null, string vmsUserName = null)
        {
            foreach (Door door in ConfigurationManager.Instance.Doors)
            {
                LockDoor(door.Id, userName, vmsUserName);
            }
            return true;
        }
        
        public bool UnlockAllDoors(string userName = null, string vmsUserName = null)
        {
            foreach (Door door in ConfigurationManager.Instance.Doors)
            {
                UnlockDoor(door.Id, userName, vmsUserName);
            }
            return true;
        }

        public bool LockAllDoorsOnDoorController(Guid id, string userName = null, string vmsUserName = null)
        {
            foreach (Door door in ConfigurationManager.Instance.Doors)
            {
                if (id == door.DoorControllerId)
                {
                    LockDoor(door.Id, userName, vmsUserName);
                }
            }
            return true;
        }

        public bool UnlockAllDoorsOnDoorController(Guid id, string userName = null, string vmsUserName = null)
        {
            foreach (Door door in ConfigurationManager.Instance.Doors)
            {
                if (id == door.DoorControllerId)
                {
                    UnlockDoor(door.Id, userName, vmsUserName);
                }
            }
            return true;
        }

        public bool OpenDoor(Guid id)
        {
            Door door = ConfigurationManager.Instance.LookupDoor(id);
            if (door != null)
            {
                lock (_doorStatus)
                {
                    DoorStatus doorStatus = InternalGetDoorStatus(id);
                    if (!doorStatus.IsOpen && !doorStatus.IsLocked)
                    {
                        doorStatus.IsOpen = true;
                        // Fire status update
                        FireStatusUpdate(doorStatus);
                        FireDoorOpened(door);
                    }
                }
                return true;
            }

            return false;
        }

        public bool CloseAndLockDoor(Guid id)
        {
            Door door = ConfigurationManager.Instance.LookupDoor(id);
            if (door != null)
            {
                lock (_doorStatus)
                {
                    DoorStatus doorStatus = InternalGetDoorStatus(id);
                    if (doorStatus.IsOpen)
                    {
                        doorStatus.IsOpen = false;
                        doorStatus.IsLocked = true;
                        FireStatusUpdate(doorStatus);
                        FireDoorClosedAndLocked(door);
                    }
                }
                return true;
            }

            return false;
        }

        public bool SimulateDoorForcedOpen(Guid id)
        {
            return SimulateEvent(id, EventManager.EventTypeDoorForcedOpen);
        }

        public bool SimulateDoorOpenTooLong(Guid id)
        {
            return SimulateEvent(id, EventManager.EventTypeDoorOpenTooLong);
        }

        public bool SimulateControllerTampering(Guid id)
        {
            return SimulateEvent(id, EventManager.EventTypeDoorControllerTampering);
        }

        public bool SimulateControllerPowerFailure(Guid id)
        {
            return SimulateEvent(id, EventManager.EventTypeDoorControllerPowerFailure);
        }

        private bool SimulateEvent(Guid doorId, EventType eventType)
        {
            Door door = ConfigurationManager.Instance.LookupDoor(doorId);
            if (door != null)
            {
                EventManager.Instance.AddEvent(eventType, new DoorControllerEvent { DoorId = door.Id });
                return true;
            }

            return false;            
        }

        public DoorStatus GetDoorStatus(Guid id)
        {
            Door door = ConfigurationManager.Instance.LookupDoor(id);
            if (door != null)
            {
                lock (_doorStatus)
                {
                    return InternalGetDoorStatus(id);
                }
            }

            return null;
        }

        private DoorStatus InternalGetDoorStatus(Guid id)
        {
            DoorStatus doorStatus;
            if (!_doorStatus.TryGetValue(id, out doorStatus))
            {
                doorStatus = new DoorStatus {DoorId = id, IsLocked = true, IsOpen = false};
                _doorStatus.Add(id, doorStatus);
            }

            return doorStatus;
        }

        private void FireStatusUpdate(DoorStatus doorStatus)
        {
            doorStatus = doorStatus.Clone();
            DoorStatusChanged.Invoke(this, new DoorStatusChangedEventArgs {DoorStatus = doorStatus});
            EventManager.Instance.AddEvent(EventManager.EventTypeDoorStatus, new DoorStatusEvent { Status = doorStatus });
        }

        private void FireDoorUnlocked(Door door, string userName, string vmsUserName)
        {
            EventManager.Instance.AddEvent(EventManager.EventTypeDoorUnlocked, new DoorControllerEvent { DoorId = door.Id, UserName = userName, VmsUserName = vmsUserName });
        }

        private void FireDoorLocked(Door door, string userName, string vmsUserName)
        {
            EventManager.Instance.AddEvent(EventManager.EventTypeDoorLocked, new DoorControllerEvent { DoorId = door.Id, UserName = userName, VmsUserName = vmsUserName });
        }

        private void FireDoorOpened(Door door)
        {
            EventManager.Instance.AddEvent(EventManager.EventTypeDoorOpened, new DoorControllerEvent { DoorId = door.Id });
        }

        private void FireDoorClosedAndLocked(Door door)
        {
            EventManager.Instance.AddEvent(EventManager.EventTypeDoorClosedAndLocked, new DoorControllerEvent { DoorId = door.Id});
        }

        internal void SetEnabledState(string doorId, bool isEnabled)
        {
            Guid guid;
            if (Guid.TryParse(doorId, out guid))
            {
                Door door = ConfigurationManager.Instance.LookupDoor(guid);
                if (door != null)
                {
                    DoorEnabledStateChanged.Invoke(this, new DoorEnabledStateChangedEventArgs { DoorId = guid, IsEnabled = isEnabled });
                    EventManager.Instance.AddEvent(EventManager.EventTypeDoorEnabledStateChanged, new DoorEnabledStatusEvent { DoorId = guid, IsEnabled = isEnabled });

                    door.IsEnabled = isEnabled;
                }
            }
        }
    }
}
