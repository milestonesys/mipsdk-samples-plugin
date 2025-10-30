using System;
using System.Collections.Generic;
using System.Linq;
using DemoACServerApplication;

namespace DemoServerApplication.ACSystem
{
    public class EventManager
    {
        public static EventType EventTypeAccessGranted = new EventType { Id = new Guid("2C19393A-EA35-4BA6-AFCC-5AA6E2A7C970"), Name = "Access granted", SourceType = EventSourceTypes.AccessPoint };
        public static EventType EventTypeAccessGrantedAnonymous = new EventType { Id = new Guid("C1FB7D57-0B26-4690-8E33-FD3B508EC4D7"), Name = "Access granted (REX)", SourceType = EventSourceTypes.AccessPoint };
        public static EventType EventTypeAccessDenied = new EventType { Id = new Guid("8120D2CF-EF2A-44D3-AD03-8D82DD6DDCAE"), Name = "Access denied", SourceType = EventSourceTypes.AccessPoint };
        public static EventType EventTypeDoorUnlocked = new EventType { Id = new Guid("50A13514-8BDC-4BC0-BD15-8BFE00BF940A"), Name = "Door unlocked", SourceType = EventSourceTypes.Door };
        public static EventType EventTypeDoorLocked = new EventType { Id = new Guid("2388E3A9-E1B4-4B0A-82E5-6E68A37362EB"), Name = "Door locked", SourceType = EventSourceTypes.Door };
        public static EventType EventTypeDoorOpened = new EventType { Id = new Guid("D540D326-D5E4-41B6-BB0A-BC97D93966AD"), Name = "Door opened", SourceType = EventSourceTypes.Door };
        public static EventType EventTypeDoorClosedAndLocked = new EventType { Id = new Guid("16AFFE3E-18A1-435F-B122-E24B20E390A8"), Name = "Door closed and locked", SourceType = EventSourceTypes.Door };
        public static EventType EventTypeDoorForcedOpen = new EventType { Id = new Guid("3DD01FD4-C77E-4BBD-AFB7-FA4679B6AD40"), Name = "Door forced open", SourceType = EventSourceTypes.Door };
        public static EventType EventTypeDoorOpenTooLong = new EventType { Id = new Guid("C0B3D197-5100-49BC-8D8C-CF5F82FC3053"), Name = "Door open too long", SourceType = EventSourceTypes.Door };
        public static EventType EventTypeDoorControllerTampering = new EventType { Id = new Guid("4FC24FF8-9E47-4C85-940B-77F4573705BC"), Name = "Door controller tampering", SourceType = EventSourceTypes.DoorController };
        public static EventType EventTypeDoorControllerPowerFailure = new EventType { Id = new Guid("18E2767B-5414-45D6-A273-19D117FECA0E"), Name = "Door controller power failure", SourceType = EventSourceTypes.DoorController };

        public static EventType EventTypeEventTypeEnabledStateChanged = new EventType { Id = new Guid("61874AE3-B680-4DBB-A304-005CF3421410"), Name = "Event type enabled state changed", SourceType = EventSourceTypes.System };
        public static EventType EventTypeDoorEnabledStateChanged = new EventType { Id = new Guid("A2F681B6-4976-40D9-9913-9F2B2BA60646"), Name = "Door enabled state changed", SourceType = EventSourceTypes.System };
        public static EventType EventTypeDoorStatus = new EventType { Id = new Guid("8F86FD45-42D5-4F98-B0BE-CE83FA632FEF"), Name = "Door status", SourceType = EventSourceTypes.System };
        public static EventType EventTypeCredentialHolderChanged = new EventType { Id = new Guid("6CEEF3C7-12D8-4437-AFDD-06C7F8CDB7EA"), Name = "Credential holder changed", SourceType = EventSourceTypes.System };
        public static EventType EventTypeUserChanged = new EventType { Id = new Guid("78D4C57E-FFDB-4453-8FE2-4D63F9B488C6"), Name = "User changed", SourceType = EventSourceTypes.System };

        public enum EventSourceTypes
        {
            System,
            DoorController,
            Door,
            AccessPoint
        }

        public static EventManager Instance = new EventManager();

        public class EventAddedEventArgs : EventArgs
        {
            public BaseEvent Event;
        }

        public event EventHandler<EventAddedEventArgs> EventAdded = delegate { };

        // Cache with latest events to allow polling from several clients
        private int _cacheSize = 10000;
        private long _lastSequenceNumber = DateTime.UtcNow.Ticks;
        private List<BaseEvent> _eventsCache = new List<BaseEvent>();

        public void AddEvent(EventType eventType, BaseEvent evt)
        {
            lock(_eventsCache)
            {
                // Adjust sequence number to next number
                long sequenceNumber = DateTime.UtcNow.Ticks; // Make sure that restart of this server does not mess up sequences
                if (sequenceNumber <= _lastSequenceNumber)
                    sequenceNumber = _lastSequenceNumber + 1;

                _lastSequenceNumber = sequenceNumber;

                evt.EventId = eventType.Id;
                evt.SequenceNumber = sequenceNumber;
                evt.Timestamp = DateTime.UtcNow;
                _eventsCache.Add(evt);
                
                if (_eventsCache.Count > _cacheSize)
                    _eventsCache.RemoveAt(0);
            }

            EventAdded.Invoke(this, new EventAddedEventArgs {Event = evt});
        }

        public BaseEvent[] GetEventsFromSequenceNumber(long sequenceNumber)
        {
            lock(_eventsCache)
            {
                return _eventsCache.Where(evt => (evt.SequenceNumber >= sequenceNumber)).ToArray();
            }
        }

        public BaseEvent[] GetEventsAll()
        {
            lock (_eventsCache)
            {
                return _eventsCache.ToArray();
            }
        }
    }
}
