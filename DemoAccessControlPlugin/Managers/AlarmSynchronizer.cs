using System.Linq;
using DemoAccessControlPlugin.Client;
using System.Text.RegularExpressions;
using VideoOS.Platform.AccessControl.Alarms;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Managers
{
    /// <summary>
    /// This class handles two-way alarm synchronization - i.e.
    /// 1. Updating alarms in the VMS after a change in the access control system, and 
    /// 2. Updating alarms in the access control system after a change in the VMS
    /// 
    /// Note: Alarms in the VMS are triggered based on access control events, which match an
    /// alarm definition in the VMS. It is not possible to fire an alarm directly to the VMS.
    /// </summary>
    internal class AlarmSynchronizer
    {
        private readonly DemoClient _client;
        private readonly IACAlarmRepository _alarmRepository;

        public AlarmSynchronizer(DemoClient client, IACAlarmRepository alarmRepository)
        {
            _client = client;
            _alarmRepository = alarmRepository;

            _client.AlarmCleared += _client_AlarmCleared;
            _alarmRepository.AlarmChanged += _alarmRepository_AlarmChanged;
        }

        public void Close()
        {
            _client.AlarmCleared -= _client_AlarmCleared;
            _alarmRepository.AlarmChanged -= _alarmRepository_AlarmChanged;
        }

        private void _client_AlarmCleared(object sender, AlarmClearedEventArgs e)
        {
            // Close all corresponding alarms in the VMS
            var vmsAlarms = _alarmRepository.GetAlarmsForSource(e.DoorId, e.EventTypeId, false);

            foreach (var alarm in vmsAlarms)
            {
                _alarmRepository.UpdateAlarm(alarm.Id, new ACAlarmUpdateRequest { State = BuiltInAlarmStates.Closed });
            }
        }

        private async void _alarmRepository_AlarmChanged(object sender, AlarmChangedEventArgs e)
        {
            // Only handle closed alarms
            if (e.Alarm.StateId != BuiltInAlarmStates.Closed)
            {
                return;
            }

            // If all alarms for this source and event type are closed in VMS, close corresponding alarm in the Demo Access Control system
            var doorId = e.Alarm.ExternalSourceId;
            var eventTypeId = e.Alarm.ExternalEventTypeId;

            var alarms = _alarmRepository.GetAlarmsForSource(doorId, eventTypeId, false);
            if (!alarms.Any())
            {
                try
                {
                    var doorExternalSourceId = GetDoorExternalSourceId(doorId);

                    await _client.CloseAlarmAsync(doorExternalSourceId, eventTypeId);
                }
                catch (DemoApplicationClientException ex)
                {
                    // A retry mechanism would probably be in order...
                    ACUtil.Log(true, "DemoACPlugin.AlarmManager", "Error closing alarm in Demo Access Control system: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 1- If the alarm has been raised by the AC Demo application then the door's externalSourceId is equal to the parameter doorId.
        /// Ie, doorId in the AC System.
        /// 2- If the alarm has been raised from XPCO then we need to extract the door's externalSourceId. Ie, the parameter doorId holds
        /// the internal XPCO doorId. 
        /// </summary>
        /// <param name="doorId">The door identifier.</param>
        private string GetDoorExternalSourceId(string doorId)
        {
            string doorExternalSourceId = doorId;

            var doorIdValueGroup = "DoorId";

            var doorIdRegex = new Regex($@"(?<AcDemoPluginPrefix>.*)_(?<{doorIdValueGroup}>.*)");
            var doorIdMatch = doorIdRegex.Match(doorId);
            if (doorIdMatch.Success)
            {
                doorExternalSourceId = doorIdMatch.Groups[doorIdValueGroup].Value;
            }

            return doorExternalSourceId;
        }

    }
}