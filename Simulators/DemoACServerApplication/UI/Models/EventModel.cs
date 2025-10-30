using DemoACServerApplication;
using DemoServerApplication.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace DemoServerApplication.UI.Models
{
    /// <summary>
    /// The MVVM Model for event/status.
    /// </summary>
    public class EventModel
    {
        public EventModel(BaseEvent evt)
        {
            TimeStampString = evt.Timestamp.ToLocalTime().ToString(CultureInfo.CurrentUICulture);

            switch (evt.GetType().Name)
            {
                case "DoorStatusEvent":
                    {
                        var doorStatusEvent = evt as DoorStatusEvent;
                        var door = ConfigurationManager.Instance.LookupDoor(doorStatusEvent.Status.DoorId);
                        EventString = "Status change, Door '" + door.Name + "': " + (doorStatusEvent.Status.IsLocked ? "Locked" : "Unlocked") + ", " + (doorStatusEvent.Status.IsOpen ? "Open" : "Closed");
                        break;
                    }
                case "DoorControllerEvent":
                    {
                        var doorControllerEvent = evt as DoorControllerEvent;
                        var eventDefinition = ConfigurationManager.Instance.LookupEvent(doorControllerEvent.EventId);
                        var door = ConfigurationManager.Instance.LookupDoor(doorControllerEvent.DoorId);
                        var credentialHolder = ConfigurationManager.Instance.LookupCredentialHolder(doorControllerEvent.CredentialHolderId);

                        EventString = eventDefinition.Name +
                            ((credentialHolder != null) ? " for '" + credentialHolder.Name + "'" : "") +
                            ((door != null) ? " on door '" + door.Name + "'" : "") +
                            ((doorControllerEvent.VmsUserName != null) ? " by '" + doorControllerEvent.VmsUserName + "'" : "") +
                            ((doorControllerEvent.UserName != null) ? " (" + doorControllerEvent.UserName + ")" : "");

                        break;
                    }
                case "CredentialHolderChangedEvent":
                    {
                        var credentialHolderChangedEvent = evt as CredentialHolderChangedEvent;
                        var credentialHolder = ConfigurationManager.Instance.LookupCredentialHolder(credentialHolderChangedEvent.CredentialHolderId);
                        EventString = "Credential holder changed, '" + ((credentialHolder != null) ? credentialHolder.Name + "'" : "<unknown>'");
                        break;
                    }
                case "UserChangedEvent":
                    {
                        var userChangedEvent = evt as UserChangedEvent;
                        var user = ConfigurationManager.Instance.LookupUser(userChangedEvent.UserId);
                        EventString = "User changed, '" + ((user != null) ? user.Name + "'" : "<unknown>'");
                        break;
                    }
                case "EventTypeEnabledStatusEvent":
                    {
                        var eventTypeStatusEvent = evt as EventTypeEnabledStatusEvent;
                        var eventDefinition = ConfigurationManager.Instance.LookupEvent(eventTypeStatusEvent.EventTypeId);
                        if (eventDefinition != null)
                        {
                            EventString = "Event '" + eventDefinition.Name + "' was " + (eventTypeStatusEvent.IsEnabled ? "enabled" : "disabled");
                        }
                        break;
                    }
                case "DoorEnabledStatusEvent":
                    {
                        var doorEnabledStatusEvent = evt as DoorEnabledStatusEvent;
                        var door = ConfigurationManager.Instance.LookupDoor(doorEnabledStatusEvent.DoorId);
                        if (door != null)
                        {
                            EventString = "'"+ door.Name + "'" + " was " + (doorEnabledStatusEvent.IsEnabled ? "enabled" : "disabled");
                        }
                        break;
                    }
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        public string EventString { get; private set; }

        public string TimeStampString { get; private set; }
    }
}
