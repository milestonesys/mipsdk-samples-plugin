using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace DemoACServerApplication
{
    [DataContract]
    public class EventDescriptor
    {
        [DataMember] public Guid EventId;
        [DataMember] public string EventName;
        [DataMember] public string SourceType; // Door, AccessPoint, Server
    }

    [DataContract]
    public class DoorControllerDescriptor
    {
        [DataMember] public Guid DoorControllerId;
        [DataMember] public string DoorControllerName;
    }

    [DataContract]
    public class DoorDescriptor
    {
        [DataMember] public Guid DoorId;
        [DataMember] public string DoorName;
        [DataMember] public Guid DoorControllerId;
        [DataMember] public bool HasRexButton;
        [DataMember] public bool LockCommandSupported;
        [DataMember] public bool UnlockCommandSupported;
        [DataMember] public bool Enabled;
        [DataMember] public double Latitude;
        [DataMember] public double Longitude;
    }

    [DataContract]
    public class CredentialHolderDescriptor
    {
        [DataMember] public Guid CredentialHolderId;
        [DataMember] public string CredentialHolderName;
        [DataMember] public byte[] Picture;
        [DataMember] public string[] Roles;
        [DataMember] public string Department;
        [DataMember] public string CardId;
        [DataMember] public DateTime ExpiryDate;
    }

    [DataContract]
    public class UserDescriptor
    {
        [DataMember]
        public Guid UserId;
        [DataMember]
        public string UserName;
        [DataMember]
        public DateTime LastChanged;
    }

    [DataContract]
    public abstract class BaseEvent
    {
        [DataMember] public Guid EventId;
        [DataMember] public long SequenceNumber;
        [DataMember] public DateTime Timestamp;
    }

    [DataContract]
    public class DoorStatus
    {
        [DataMember] public Guid DoorId;
        [DataMember] public bool IsOpen;
        [DataMember] public bool IsLocked;
 
        public DoorStatus Clone()
        {
            return new DoorStatus {DoorId = DoorId, IsOpen = IsOpen, IsLocked = IsLocked};
        }
    }

    [DataContract]
    public class DoorStatusEvent : BaseEvent
    {
        [DataMember] public DoorStatus Status;
    }

    [DataContract]
    public class EventTypeEnabledStatusEvent : BaseEvent
    {
        [DataMember] public Guid EventTypeId;
        [DataMember] public bool IsEnabled;
    }

    [DataContract]
    public class DoorEnabledStatusEvent : BaseEvent
    {
        [DataMember] public Guid DoorId;
        [DataMember] public bool IsEnabled;
    }

    [DataContract]
    public class DoorControllerEvent : BaseEvent
    {
        [DataMember] public Guid DoorId;
        [DataMember] public Guid CredentialHolderId;
        [DataMember] public int AccessPoint;
        [DataMember] public string Reason;
        [DataMember] public string UserName;
        [DataMember] public string VmsUserName;
    }

    [DataContract]
    public class CredentialHolderChangedEvent : BaseEvent
    {
        [DataMember]
        public Guid CredentialHolderId;
    }

    [DataContract]
    public class UserChangedEvent : BaseEvent
    {
        [DataMember]
        public Guid UserId;
        [DataMember]
        public string UserName;
        [DataMember]
        public DateTime LastChanged;
    }


    [DataContract]
    public class ClearAlarmCommand
    {
        [DataMember] public Guid DoorId;
        [DataMember] public Guid EventTypeId;
    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWebService" in both code and config file together.
    [ServiceContract]
    [ServiceKnownType(typeof(EventDescriptor))]
    [ServiceKnownType(typeof(DoorDescriptor))]
    [ServiceKnownType(typeof(CredentialHolderDescriptor))]
    [ServiceKnownType(typeof(UserDescriptor))]
    [ServiceKnownType(typeof(BaseEvent))]
    [ServiceKnownType(typeof(DoorStatusEvent))]
    [ServiceKnownType(typeof(EventTypeEnabledStatusEvent))]
    [ServiceKnownType(typeof(DoorControllerEvent))]
    [ServiceKnownType(typeof(DoorEnabledStatusEvent))]
    [ServiceKnownType(typeof(CredentialHolderChangedEvent))]
    [ServiceKnownType(typeof(UserChangedEvent))]
    [ServiceKnownType(typeof(ClearAlarmCommand))]
    public interface IWebService
    {
        [OperationContract] UserDescriptor Connect(string userName, string password);
        [OperationContract] UserDescriptor GetUser(string userName);
        [OperationContract] EventDescriptor[] GetEventTypes(string userName, string password);
        [OperationContract] DoorControllerDescriptor[] GetDoorControllers(string userName, string password);
        [OperationContract] DoorDescriptor[] GetDoors(string userName, string password);
        [OperationContract] CredentialHolderDescriptor GetCredentialHolder(Guid credentialHolderId);
        [OperationContract] CredentialHolderDescriptor[] SearchCredentialHolders(string searchString);
        [OperationContract] BaseEvent[] GetEvents(string userName, string password, int timeoutMilliSecs, long startSequenceNumber, int count);
        [OperationContract] bool UnlockDoor(string userName, string password, string vmsUserName, Guid doorId);
        [OperationContract] bool LockDoor(string userName, string password, string vmsUserName, Guid doorId);
        [OperationContract] bool UnlockAllDoorsOnDoorController(string userName, string password, string vmsUserName, Guid doorControllerId);
        [OperationContract] bool LockAllDoorsOnDoorController(string userName, string password, string vmsUserName, Guid doorControllerId);
        [OperationContract] bool UnlockAllDoors(string userName, string password, string vmsUserName);
        [OperationContract] bool LockAllDoors(string userName, string password, string vmsUserName);
        [OperationContract] DoorStatus GetDoorStatus(string userName, string password, Guid doorId);
        [OperationContract] ClearAlarmCommand[] GetAlarmsToClear(string userName, string password);
        [OperationContract] void CloseAlarmOnDoor(string userName, string password, Guid doorId, Guid eventTypeId);
        [OperationContract] void UpdateDoorEnabledStates(string userName, string password, Tuple<string, bool>[] changedStates);
        [OperationContract] void UpdateEventTypeEnabledStates(string userName, string password, Tuple<string, bool>[] changedStates);
        [OperationContract] void UpdateAccessControlUnitPosition(string userName, string password, Tuple<string, double, double>[] unitPositions);
    }
}
