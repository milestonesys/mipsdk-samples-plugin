using DemoAccessControlPlugin.Constants;
using DemoAccessControlPlugin.DemoApplicationService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using VideoOS.Platform.AccessControl;
using VideoOS.Platform.AccessControl.Elements;
using VideoOS.Platform.AccessControl.Plugin;
using VideoOS.Platform.AccessControl.TypeCategories;

namespace DemoAccessControlPlugin.Configuration
{
    internal static class TypeConverter
    {
        public static ACServer CreateACServer(string id, string address)
        {
            return new ACServer(id, "Demo System on " + address, ACBuiltInIconKeys.ServerConnected, null, TypeId.Server);
        }

        public static ACEvent ToACEvent(DoorControllerEvent dce)
        {
            var eventId = Guid.NewGuid().ToString();
            var eventTypeId = dce.EventId.ToString();

            var relatedCredentialHolderIds = new List<string>();
            if (dce.CredentialHolderId != Guid.Empty)
            {
                relatedCredentialHolderIds.Add(dce.CredentialHolderId.ToString());
            }

            // Check if the event is from the door or the access point
            string sourceId;
            if (dce.AccessPoint != 0)
            {
                sourceId = CreateAccessPointId(dce.DoorId, dce.AccessPoint);
            }
            else

            {
                sourceId = dce.DoorId.ToString();
            }

            return new ACEvent(eventId, eventTypeId, sourceId, dce.Timestamp, string.Empty, dce.Reason, relatedCredentialHolderIds, null, null);
        }

        public static ACState ToACState(DoorStatus doorStatus)
        {
            var states = GetDoorStates(doorStatus);
            var iconKey = GetDoorStateIconKey(doorStatus);

            return new ACState(doorStatus.DoorId.ToString(), states, iconKey, null);
        }

        public static ACUnit ToACUnit(DoorControllerDescriptor doorController)
        {
            return new ACUnit(doorController.DoorControllerId.ToString(), doorController.DoorControllerName, ACBuiltInIconKeys.Controller, null, TypeId.DoorController, null);
        }

        public static IEnumerable<ACUnit> ToACUnits(DoorDescriptor door)
        {
            var unsupportedCommands = new List<string>();

            if (!door.LockCommandSupported)
                unsupportedCommands.Add(DoorCommandId.Lock);

            if (!door.UnlockCommandSupported)
                unsupportedCommands.Add(DoorCommandId.Unlock);

            // Door
            var parentId = door.DoorControllerId != Guid.Empty ? door.DoorControllerId.ToString() : null;
            var doorId = door.DoorId.ToString();
            yield return new ACUnit(doorId, door.DoorName, ACBuiltInIconKeys.Door, null, TypeId.Door, parentId, unsupportedCommands) { IsEnabled = door.Enabled };

            // The access points in the Demo Access Control are not returned explicitly, but are numbered 1 and 2 based on the door.
            // Outside access point
            var accessPoint1Id = CreateAccessPointId(door.DoorId, 1);
            yield return new ACUnit(accessPoint1Id, door.DoorName + " (in)", ACBuiltInIconKeys.AccessPoint, null, TypeId.AccessPoint, doorId);

            // Inside access point (REX button)
            var accessPoint2Id = CreateAccessPointId(door.DoorId, 2);
            if (door.HasRexButton)
                yield return new ACUnit(accessPoint2Id, door.DoorName + " (out)", ACBuiltInIconKeys.AccessPoint, null, TypeId.AccessPoint, doorId);
        }

        public static ACCredentialHolder ToACCredentialHolder(CredentialHolderDescriptor credentialHolderDescriptor, SystemProperties systemProperties)
        {
            Bitmap image = null;

            if (credentialHolderDescriptor.Picture != null && credentialHolderDescriptor.Picture.Length > 0)
            {
                image = new Bitmap(Image.FromStream(new MemoryStream(credentialHolderDescriptor.Picture))); // Make a copy of the image to avoid GDI+ problems due to disposed buffers
            }

            List<ACProperty> properties = new List<ACProperty>();
            if (!string.IsNullOrWhiteSpace(credentialHolderDescriptor.Department))
                properties.Add(new ACProperty("Department", credentialHolderDescriptor.Department));
            if (!string.IsNullOrWhiteSpace(credentialHolderDescriptor.CardId))
                properties.Add(new ACProperty("Card Id", credentialHolderDescriptor.CardId));
            if (!string.IsNullOrWhiteSpace(credentialHolderDescriptor.Department))
                properties.Add(new ACProperty("Expiry date", credentialHolderDescriptor.ExpiryDate.ToLongDateString()));

            // Add "deep link" for cardholder management
            var credentialHolderId = credentialHolderDescriptor.CredentialHolderId.ToString();
            var cardholderManagementUri = new UriBuilder("http", systemProperties.Address, systemProperties.Port, "DemoACServerApplication/CardholderManagement/", "?id=" + credentialHolderId).Uri;
            properties.Add(new ACProperty("Manage cardholder", cardholderManagementUri.AbsoluteUri));

            ACCredentialHolder credentialHolder = new ACCredentialHolder(credentialHolderId, credentialHolderDescriptor.CredentialHolderName, credentialHolderDescriptor.Roles ?? new string[] { }, image, properties);
            return credentialHolder;
        }

        public static ACEventType ToACEventType(EventDescriptor eventDescriptor, bool isEnabled)
        {
            string eventTypeId = eventDescriptor.EventId.ToString();

            return new ACEventType(eventTypeId, eventDescriptor.EventName, null, null,
                ConvertToCategories(eventDescriptor.EventId, eventDescriptor.EventName))
            { IsEnabled = isEnabled };
        }

        private static string CreateAccessPointId(Guid doorId, int accessPoint)
        {
            return string.Format("AP{0}_{1}", accessPoint, doorId);
        }

        private static IEnumerable<string> GetDoorStates(DoorStatus status)
        {
            if (status == null)
            {
                yield return DoorStatusId.Unknown;
            }

            // Return open/closed state
            yield return status.IsOpen ? DoorStatusId.Open : DoorStatusId.Closed;

            // Return add lock state
            yield return status.IsLocked ? DoorStatusId.Locked : DoorStatusId.Unlocked;
        }

        private static string GetDoorStateIconKey(DoorStatus status)
        {
            if (status == null)
            {
                return ACBuiltInIconKeys.DoorUnknown;
            }

            if (status.IsOpen)
            {
                return status.IsLocked ? ACBuiltInIconKeys.DoorOpenLocked : ACBuiltInIconKeys.DoorOpenUnlocked;
            }
            else
            {
                return status.IsLocked ? ACBuiltInIconKeys.DoorClosedLocked : ACBuiltInIconKeys.DoorClosedUnlocked;
            }
        }

        private static IEnumerable<string> ConvertToCategories(Guid eventType, string eventName)
        {
            if (eventName.ToLowerInvariant().StartsWith("access denied"))
            {
                yield return ACBuiltInEventTypeCategories.CredentialHolderAccessDenied;
                yield return ACBuiltInEventTypeCategories.AccessRequest;
            }
            else if (eventName.ToLowerInvariant().StartsWith("access granted"))
                yield return ACBuiltInEventTypeCategories.CredentialHolderAccessGranted;
            else if (eventName.ToLowerInvariant().Contains("error"))
                yield return ACBuiltInEventTypeCategories.Error;
            // For sake of illustration - set certain events to a custom category
            else if (eventName.ToLowerInvariant().Contains("tampering"))
                yield return Categories.DoorErrorEvent.Id;
            else if (eventName.ToLowerInvariant().Contains("forced open"))
                yield return Categories.DoorErrorEvent.Id;
            else if (eventName.ToLowerInvariant().Contains("power failure"))
                yield return Categories.DoorErrorEvent.Id;
        }
    }
}
