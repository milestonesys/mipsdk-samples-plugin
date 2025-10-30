using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DemoACServerApplication;
using DemoServerApplication.Configuration;

namespace DemoServerApplication.ACSystem
{
    public class AccessManager
    {
        private class DoorActionState
        {
            public Guid DoorId;
            public TimeSpan Delay;
        }

        public static AccessManager Instance = new AccessManager();

        public void RequestAccess(Guid credentialHolderId, Guid doorId)
        {
            // Lookup credentialholder and door
            CredentialHolder credentialHolder = ConfigurationManager.Instance.LookupCredentialHolder(credentialHolderId);
            Door door = ConfigurationManager.Instance.LookupDoor(doorId);
            if (credentialHolder != null && door != null)
            {
                // TODO: For now always allow access
                if (door.IsAccessAllowed)
                {
                    if (DoorManager.Instance.UnlockDoor(door.Id))
                    {
                        FireAccessGranted(credentialHolder, door);

                        // Simulate someone entering the door, i.e. open door after 1 sec. close again after 5 secs
                        // Make sure that door is opened after some time
                        ThreadPool.QueueUserWorkItem(this.OpenDoorAction, new DoorActionState { DoorId = door.Id, Delay = new TimeSpan(0, 0, 0, 0, 1000) });
                        // Make sure that door is closed again after some time
                        ThreadPool.QueueUserWorkItem(this.CloseAndLockDoorAction, new DoorActionState { DoorId = door.Id, Delay = new TimeSpan(0, 0, 0, 0, 4000) });
                    }
                    else
                    {
                        FireAccessDenied(credentialHolder, door, 1, "Unknown door");
                    }
                }
                else
                {
                    FireAccessDenied(credentialHolder, door, 1, "Not allowed at this time");                    
                }
            }
        }

        public void RequestAccessWithREXButton(Guid doorId)
        {
            // Lookup credentialholder and door
            Door door = ConfigurationManager.Instance.LookupDoor(doorId);
            if (door != null)
            {
                // Always allow people to get out - no check for access rights
                if (DoorManager.Instance.UnlockDoor(door.Id))
                {
                    FireAccessGrantedAnonymous(door);

                    // Simulate someone entering the door, i.e. open door after 1 sec. close again after 5 secs
                    // Make sure that door is opened after some time
                    ThreadPool.QueueUserWorkItem(this.OpenDoorAction, new DoorActionState { DoorId = door.Id, Delay = new TimeSpan(0, 0, 0, 0, 1000) });
                    // Make sure that door is closed again after some time
                    ThreadPool.QueueUserWorkItem(this.CloseAndLockDoorAction, new DoorActionState { DoorId = door.Id, Delay = new TimeSpan(0, 0, 0, 0, 4000) });
                }
                else
                {
                    FireAccessDenied(null, door, 2, "Unknown door");
                }
            }
        }

        private void FireAccessGranted(CredentialHolder credentialHolder, Door door)
        {
            EventManager.Instance.AddEvent(EventManager.EventTypeAccessGranted, new DoorControllerEvent {DoorId = door.Id, CredentialHolderId = credentialHolder.Id, AccessPoint = 1});
        }

        private void FireAccessGrantedAnonymous(Door door)
        {
            EventManager.Instance.AddEvent(EventManager.EventTypeAccessGrantedAnonymous, new DoorControllerEvent { DoorId = door.Id, AccessPoint = 2 });
        }

        private void FireAccessDenied(CredentialHolder credentialHolder, Door door, int accessPoint, string reason)
        {
            EventManager.Instance.AddEvent(EventManager.EventTypeAccessDenied, new DoorControllerEvent { DoorId = door.Id, AccessPoint = accessPoint, CredentialHolderId = credentialHolder != null ? credentialHolder.Id : Guid.Empty, Reason = reason });
        }

        private void OpenDoorAction(object state)
        {
            DoorActionState doorActionState = state as DoorActionState;
            if (doorActionState != null)
            {
                Thread.Sleep(doorActionState.Delay);
                DoorManager.Instance.OpenDoor(doorActionState.DoorId);
            }
        }

        private void CloseAndLockDoorAction(object state)
        {
            DoorActionState doorActionState = state as DoorActionState;
            if (doorActionState != null)
            {
                Thread.Sleep(doorActionState.Delay);
                DoorManager.Instance.CloseAndLockDoor(doorActionState.DoorId);
            }
        }
    }
}
