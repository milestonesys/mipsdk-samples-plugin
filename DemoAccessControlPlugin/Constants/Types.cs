using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.AccessControl;
using VideoOS.Platform.AccessControl.Elements;
using VideoOS.Platform.AccessControl.Plugin;
using VideoOS.Platform.AccessControl.TypeCategories;

namespace DemoAccessControlPlugin.Constants
{
    internal static class Categories
    {
        public static ACCategoryInfo DoorErrorEvent = new ACCategoryInfo(CategoryId.DoorErrorEvent, "Door Error", ACCategoryInfo.ACTypes.EventType);
    }

    internal static class EventTypes
    {
        public static ACEventType ServerConnected = new ACEventType(ServerEventId.Connected, "Server connected", null, null, null);
        public static ACEventType ServerDisconnected = new ACEventType(ServerEventId.Disconnected, "Server connection lost", null, null, new[] { ACBuiltInEventTypeCategories.Error });

        public static ACEventType[] ServerEventTypes = new[] { ServerConnected, ServerDisconnected };
    }

    internal static class StateTypes
    {
        public static ACStateType ServerDisconnected = new ACStateType(ServerStatusId.Disconnected, "Disconnected", null, null, new[] { ACBuiltInStateTypeCategories.ServerStatusConnectionNotConnected, ACBuiltInStateTypeCategories.Warning });
        public static ACStateType ServerConnected = new ACStateType(ServerStatusId.Connected, "Connected", null, null, new[] { ACBuiltInStateTypeCategories.ServerStatusConnectionConnected });

        public static ACStateType DoorUnknown = new ACStateType(DoorStatusId.Unknown, "Unknown", null, null, new[] { ACBuiltInStateTypeCategories.Error });
        public static ACStateType DoorOpen = new ACStateType(DoorStatusId.Open, "Open", null, null, new[] { ACBuiltInStateTypeCategories.DoorStateOpen });
        public static ACStateType DoorClosed = new ACStateType(DoorStatusId.Closed, "Closed", null, null, new[] { ACBuiltInStateTypeCategories.DoorStateClosed });
        public static ACStateType DoorLocked = new ACStateType(DoorStatusId.Locked, "Locked", null, null, new[] { ACBuiltInStateTypeCategories.DoorStateLocked });
        public static ACStateType DoorUnlocked = new ACStateType(DoorStatusId.Unlocked, "Unlocked", null, null, new[] { ACBuiltInStateTypeCategories.DoorStateUnlocked });

        public static ACStateType[] ServerStateTypes = new[] { ServerDisconnected, ServerConnected };
        public static ACStateType[] DoorStateTypes = new[] { DoorUnknown, DoorOpen, DoorClosed, DoorLocked, DoorUnlocked };
    }

    internal static class CommandTypes
    {
        public static ACCommandType DoorUnlock = new ACCommandType(DoorCommandId.Unlock, "Unlock", null, null, new[] { ACBuiltInCommandTypeCategories.GrantAccess, ACBuiltInCommandTypeCategories.AccessRequest }, 0);
        public static ACCommandType DoorLock = new ACCommandType(DoorCommandId.Lock, "Lock", null, null, new[] { ACBuiltInCommandTypeCategories.AccessRequest }, 0);

        public static ACCommandType DoorUnlockAccessPoint = new ACCommandType(DoorCommandId.UnlockAccessPoint, "Unlock door", null, null, new[] { ACBuiltInCommandTypeCategories.GrantAccess, ACBuiltInCommandTypeCategories.AccessRequest }, 0);
        public static ACCommandType DoorLockAccessPoint = new ACCommandType(DoorCommandId.LockAccessPoint, "Lock door", null, null, new[] { ACBuiltInCommandTypeCategories.AccessRequest }, 0);

        public static ACCommandType DoorUnlockDoorController = new ACCommandType(DoorCommandId.UnlockDoorController, "Unlock all", null, null, new[] { ACBuiltInCommandTypeCategories.GrantAccess, ACBuiltInCommandTypeCategories.AccessRequest }, 0);
        public static ACCommandType DoorLockDoorController = new ACCommandType(DoorCommandId.LockDoorController, "Lock all", null, null, new[] { ACBuiltInCommandTypeCategories.AccessRequest }, 0);

        public static ACCommandType DoorUnlockAll = new ACCommandType(DoorCommandId.UnlockAll, "Unlock all", null, null, new[] { ACBuiltInCommandTypeCategories.GrantAccess, ACBuiltInCommandTypeCategories.AccessRequest }, 0);
        public static ACCommandType DoorLockAll = new ACCommandType(DoorCommandId.LockAll, "Lock all", null, null, new[] { ACBuiltInCommandTypeCategories.AccessRequest }, 0);

        public static ACCommandType[] DoorCommands = new[] { DoorLock, DoorUnlock };
        public static ACCommandType[] AccessPointCommands = new[] { DoorUnlockAccessPoint, DoorLockAccessPoint };
        public static ACCommandType[] DoorControllerCommandTypes = new[] { DoorUnlockDoorController, DoorLockDoorController };
        public static ACCommandType[] AllDoorsCommands = new[] { DoorLockAll, DoorUnlockAll };

        public static ACCommandType[] AllCommands = DoorCommands.Concat(AccessPointCommands).Concat(DoorControllerCommandTypes).Concat(AllDoorsCommands).ToArray();
    }

    internal static class ElementTypes
    {
        public static ACServerType ServerType = new ACServerType(TypeId.Server, "Demo Server", ACBuiltInIconKeys.ServerConnected, null, null,
            StateTypes.ServerStateTypes.Select(s => s.Id), CommandTypes.AllDoorsCommands.Select(d => d.Id), EventTypes.ServerEventTypes.Select(t => t.Id));

        public static ACUnitType DoorControllerType = new ACUnitType(TypeId.DoorController, "Door Controller", null, null, null, null, CommandTypes.DoorControllerCommandTypes.Select(d => d.Id), null);

        public static ACUnitType CreateDoorType(IEnumerable<string> eventTypeIds)
        {
            return new ACUnitType(TypeId.Door, "Door", null, null, new[] { ACBuiltInUnitTypeCategories.Door },
                StateTypes.DoorStateTypes.Select(s => s.Id), CommandTypes.DoorCommands.Select(d => d.Id), eventTypeIds);
        }

        public static ACUnitType CreateAccessPointType(IEnumerable<string> eventTypeIds)
        {
            return new ACUnitType(TypeId.AccessPoint, "Access Point", null, null, new[] { ACBuiltInUnitTypeCategories.AccessPoint },
                null, CommandTypes.AccessPointCommands.Select(d => d.Id), eventTypeIds);
        }
    }
}
