namespace DemoAccessControlPlugin.Constants
{
    internal static class TypeId
    {
        public const string Server = "T_Server";
        public const string DoorController = "T_DoorController";
        public const string Door = "T_Door";
        public const string AccessPoint = "T_AccessPoint";
    }

    internal static class ServerStatusId
    {
        public const string Connected = "SS_Connected";
        public const string Disconnected = "SS_Disconnected";
    }

    internal static class DoorStatusId
    {
        // Physical states
        public const string Unknown = "DS_Unknown";
        public const string Open = "DS_Open";
        public const string Closed = "DS_Closed";
        public const string Locked = "DS_Locked";
        public const string Unlocked = "DS_Unlocked";
    }

    internal static class DoorCommandId
    {
        public const string Lock = "DC_Lock";
        public const string Unlock = "DC_Unlock";
        public const string LockAccessPoint = "DC_LockAccessPoint";
        public const string UnlockAccessPoint = "DC_UnlockAccessPoint";
        public const string LockDoorController = "DC_LockDoorController";
        public const string UnlockDoorController = "DC_UnlockDoorController";
        public const string LockAll = "DC_LockAll";
        public const string UnlockAll = "DC_UnlockAll";
    }

    internal static class ServerEventId
    {
        public const string Connected = "SE_ServerConnected";
        public const string Disconnected = "SE_ServerDisconnected";
    }

    internal static class CategoryId
    {
        public const string DoorErrorEvent = "EC_DoorError";
    }
}
