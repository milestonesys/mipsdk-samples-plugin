using System;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Client
{
    public class StateChangedEventArgs : EventArgs
    {
        public ACState State { get; private set; }

        public StateChangedEventArgs(ACState state)
        {
            State = state;
        }
    }

    public class EventTriggeredEventArgs : EventArgs
    {
        public ACEvent Event { get; private set; }

        public EventTriggeredEventArgs(ACEvent @event)
        {
            Event = @event;
        }
    }

    public class CredentialHolderChangedEventArgs : EventArgs
    {
        public string CredentialHolderId { get; private set; }

        public CredentialHolderChangedEventArgs(string credentialHolderId)
        {
            CredentialHolderId = credentialHolderId;
        }
    }

    public class UserRightsChangedEventArgs : EventArgs
    {
        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public DateTime LastChange { get; private set; }

        public UserRightsChangedEventArgs(Guid userId, string username, DateTime lastChange)
        {
            UserId = userId;
            Username = username;
            LastChange = lastChange;
        }
    }

    public class AlarmClearedEventArgs : EventArgs
    {
        public string DoorId { get; private set; }
        public string EventTypeId { get; private set; }

        public AlarmClearedEventArgs(string doorId, string eventTypeId)
        {
            DoorId = doorId;
            EventTypeId = eventTypeId;
        }
    }
}
