using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DemoACServerApplication;

namespace DemoServerApplication.ACSystem
{
    public class AlarmManager
    {
        private List<ClearAlarmCommand> _alarmsToClear = new List<ClearAlarmCommand>();
        private readonly object _lock = new object();

        public class AlarmStateChangedEventArgs : EventArgs
        {
            public Guid DoorId { get; set; }
            public Guid EventTypeId { get; set; }
        }

        public event EventHandler<AlarmStateChangedEventArgs> AlarmClosed = delegate { };

        public static AlarmManager Instance = new AlarmManager();

        public void ClearPowerFailureAlarmOnDoor(Guid doorId)
        {
            AddCommandToQueue(doorId, EventManager.EventTypeDoorControllerPowerFailure.Id);
        }

        public void ClearTamperAlarmOnDoor(Guid doorId)
        {
            AddCommandToQueue(doorId, EventManager.EventTypeDoorControllerTampering.Id);
        }

        public void ClearForcedOpenAlarmOnDoor(Guid doorId)
        {
            AddCommandToQueue(doorId, EventManager.EventTypeDoorForcedOpen.Id);
        }

        public ReadOnlyCollection<ClearAlarmCommand> GetClearAlarmCommands()
        {
            lock (_lock)
            {
                var result = new ReadOnlyCollection<ClearAlarmCommand>(_alarmsToClear);
                _alarmsToClear = new List<ClearAlarmCommand>();
                return result;
            }
        }

        private void AddCommandToQueue(Guid doorId, Guid eventTypeId)
        {
            lock (_lock)
            {
                _alarmsToClear.Add(new ClearAlarmCommand
                {
                    EventTypeId = eventTypeId,
                    DoorId = doorId
                });
            }
        }

        public void CloseAlarm(Guid doorId, Guid eventTypeId)
        {
            AlarmClosed(this, new AlarmStateChangedEventArgs
            {
                DoorId = doorId,
                EventTypeId = eventTypeId
            });
        }
    }
}