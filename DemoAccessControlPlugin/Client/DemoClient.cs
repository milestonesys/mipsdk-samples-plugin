using DemoAccessControlPlugin.Configuration;
using DemoAccessControlPlugin.DemoApplicationService;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Client
{
    /// <summary>
    /// Class for communicating with the Demo Access Control Application.
    /// Other plug-ins may need something similar to communicate with their access control system.
    /// </summary>
    internal class DemoClient
    {
        private readonly SystemProperties _systemProperties;
        private readonly WSHttpBinding _binding;

        private CancellationTokenSource _cts = null;
        private readonly object _ctsLock = new object();

        private WebServiceClient _client = null;
        private readonly object _clientLock = new object();
        private bool _connected = false;

        public event EventHandler Connected = delegate { };
        public event EventHandler Disconnected = delegate { };
        public event EventHandler<StateChangedEventArgs> StateChanged = delegate { };
        public event EventHandler<EventTriggeredEventArgs> EventTriggered = delegate { };
        public event EventHandler<CredentialHolderChangedEventArgs> CredentialHolderChanged = delegate { };
        public event EventHandler<UserRightsChangedEventArgs> UserRightsChanged = delegate { };
        public event EventHandler<AlarmClearedEventArgs> AlarmCleared = delegate { };

        public string ServerId { get; set; }

        public DemoClient(SystemProperties systemProperties)
        {
            _systemProperties = systemProperties;

            _binding = new WSHttpBinding(SecurityMode.None);
            _binding.MaxReceivedMessageSize = 500000000;
            _binding.MaxBufferPoolSize = 500000000;
            _binding.ReaderQuotas.MaxArrayLength = 500000000;
            _binding.ReaderQuotas.MaxBytesPerRead = 500000000;
            _binding.ReaderQuotas.MaxNameTableCharCount = 500000000;
            _binding.ReaderQuotas.MaxDepth = 500000000;
            _binding.ReaderQuotas.MaxStringContentLength = 500000000;
        }

        public void StartEventPolling()
        {
            CancellationToken cancellationToken;
            lock (_ctsLock)
            {
                if (_cts != null)
                {
                    _cts.Cancel();
                }
                _cts = new CancellationTokenSource();
                cancellationToken = _cts.Token;
            }

            Task.Run(() => EventPollingAsync(cancellationToken));
        }

        public void StopEventPolling()
        {
            lock (_ctsLock)
            {
                _cts.Cancel();
            }
        }

        private async Task EventPollingAsync(CancellationToken cancellationToken)
        {
            // Demo Access Control system uses sequence numbers based on ticks, so we initialize it to current time
            var seqNr = DateTime.UtcNow.Ticks;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Get events from the Demo Access Control system
                    var events = await TryCall(client => client.GetEventsAsync(_systemProperties.AdminUser, _systemProperties.AdminPassword, 5000, seqNr, _systemProperties.EventPollingCount));

                    if (!_connected)
                    {
                        _connected = true;
                        ACUtil.Log(false, "DemoACPlugin.DemoClient", "Connected to " + _systemProperties.Address);

                        Connected(this, EventArgs.Empty);
                    }

                    if (events.Any())
                    {
                        seqNr = events.Last().SequenceNumber + 1;
                    }

                    foreach (var baseEvent in events)
                    {
                        var dse = baseEvent as DoorStatusEvent;
                        if (dse != null)
                        {
                            var state = TypeConverter.ToACState(dse.Status);
                            StateChanged(this, new StateChangedEventArgs(state));
                            continue;
                        }

                        var dce = baseEvent as DoorControllerEvent;
                        if (dce != null)
                        {
                            var acEvent = TypeConverter.ToACEvent(dce);
                            EventTriggered(this, new EventTriggeredEventArgs(acEvent));
                            continue;
                        }

                        var cce = baseEvent as CredentialHolderChangedEvent;
                        if (cce != null)
                        {
                            CredentialHolderChanged(this, new CredentialHolderChangedEventArgs(cce.CredentialHolderId.ToString()));
                            continue;
                        }

                        var uce = baseEvent as UserChangedEvent;
                        if (uce != null)
                        {
                            UserRightsChanged(this, new UserRightsChangedEventArgs(uce.UserId, uce.UserName, uce.LastChanged));
                            continue;
                        }
                    }

                    // Get alarm to clear from the Demo Access Control system
                    var alarmsToClear = await TryCall(client => client.GetAlarmsToClearAsync(_systemProperties.AdminUser, _systemProperties.AdminPassword));
                    foreach (var clearAlarmCmd in alarmsToClear)
                    {
                        AlarmCleared(this, new AlarmClearedEventArgs(clearAlarmCmd.DoorId.ToString(), clearAlarmCmd.EventTypeId.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    // Connection broken.
                    if (_connected)
                    {
                        _connected = false;
                        ACUtil.Log(true, "DemoACPlugin.DemoClient", "Connection to " + _systemProperties.Address + " failed: " + ex.Message);

                        Disconnected(this, EventArgs.Empty);
                    }
                }

                try
                {
                    await Task.Delay(_systemProperties.EventPollingPeriodMs, cancellationToken);
                }
                catch (OperationCanceledException) { }
            }

            _connected = false;
            ACUtil.Log(false, "DemoACPlugin.DemoClient", "Connection to " + _systemProperties.Address + " closed.");

            // Don't close the client on Disconnect - wait for Close
        }

        public void Close()
        {
            lock (_clientLock)
            {
                if (_client != null)
                {
                    try
                    {
                        if (_client.State == CommunicationState.Opened)
                        {
                            _client.Close();
                        }
                        _client.Abort();
                    }
                    catch
                    {
                        // Ignore errors on disconnect
                    }
                }
            }
        }

        public bool CheckCredentials(string username, string password)
        {
            var user = TryCall(client => client.Connect(username, password));
            return user != null;
        }

        public async Task<bool> CheckCredentialsAsync(string username, string password)
        {
            var user = await TryCall(client => client.ConnectAsync(username, password));
            return user != null;
        }

        public void LockDoor(string doorId, string username, string password, string vmsUsername)
        {
            Guid doorIdGuid;
            if (Guid.TryParse(doorId, out doorIdGuid))
            {
                var success = TryCall(client => client.LockDoor(username, password, vmsUsername, doorIdGuid));
                if (!success)
                {
                    throw new DemoApplicationClientException("Failed to lock the door.");
                }
            }
            else
            {
                throw new DemoApplicationClientException($"LockDoor: Error parsing Guid. DoorId: {doorId}");
            }
        }

        public void UnlockDoor(string doorId, string username, string password, string vmsUsername)
        {
            Guid doorIdGuid;
            if (Guid.TryParse(doorId, out doorIdGuid))
            {
                var success = TryCall(client => client.UnlockDoor(username, password, vmsUsername, doorIdGuid));
                if (!success)
                {
                    throw new DemoApplicationClientException("Failed to unlock the door.");
                }
            }
            else
            {
                throw new DemoApplicationClientException($"UnlockDoor: Error parsing Guid. DoorId: {doorId}");
            }
        }

        public void LockDoorOnAccessPoint(string accessPointId, string username, string password, string vmsUsername)
        {
            string doorId = TypeConverter.GetDoorIdFromAccessPointId(accessPointId);
            LockDoor(doorId, username, password, vmsUsername);
        }

        public void UnLockDoorOnAccessPoint(string accessPointId, string username, string password, string vmsUsername)
        {
            string doorId = TypeConverter.GetDoorIdFromAccessPointId(accessPointId);
            UnlockDoor(doorId, username, password, vmsUsername);
        }

        public void LockAllDoorsOnController(string controllerId, string username, string password, string vmsUsername)
        {
            Guid doorControllerId;
            if (Guid.TryParse(controllerId, out doorControllerId))
            {
                var success = TryCall(client => client.LockAllDoorsOnDoorController(username, password, vmsUsername, doorControllerId));
                if (!success)
                {
                    throw new DemoApplicationClientException("Failed to lock all the doors on the controller.");
                }
            }
            else
            {
                throw new DemoApplicationClientException($"LockAllDoorsOnController: Error parsing Guid. ControllerId: {doorControllerId}");
            }
        }

        public void UnlockAllDoorsOnController(string controllerId, string username, string password, string vmsUsername)
        {
            Guid controllerIdGuid;
            if (Guid.TryParse(controllerId, out controllerIdGuid))
            {
                var success = TryCall(client => client.UnlockAllDoorsOnDoorController(username, password, vmsUsername, controllerIdGuid));
                if (!success)
                {
                    throw new DemoApplicationClientException("Failed to unlock all the doors on the controller.");
                }
            }
            else
            {
                throw new DemoApplicationClientException($"UnlockAllDoorsOnController: Error parsing Guid. ControllerId: {controllerId}");
            }
        }

        public void LockAllDoors(string username, string password, string vmsUsername)
        {
            var success = TryCall(client => client.LockAllDoors(username, password, vmsUsername));
            if (!success)
            {
                throw new DemoApplicationClientException("Failed to lock all the doors on the system.");
            }
        }

        public void UnlockAllDoors(string username, string password, string vmsUsername)
        {
            var success = TryCall(client => client.UnlockAllDoors(username, password, vmsUsername));
            if (!success)
            {
                throw new DemoApplicationClientException("Failed to unlock all the doors on the system.");
            }
        }

        public async Task CloseAlarmAsync(string doorId, string eventTypeId)
        {
            Guid doorIdGuid;
            Guid eventIdGuid;
            if (Guid.TryParse(doorId, out doorIdGuid) && Guid.TryParse(eventTypeId, out eventIdGuid))
            {
                await TryCall(client => client.CloseAlarmOnDoorAsync(_systemProperties.AdminUser, _systemProperties.AdminPassword, doorIdGuid, eventIdGuid));
            }
            else
            {
                throw new DemoApplicationClientException($"CloseAlarmAsync: Error parsing Guid. DoorId: {doorId}, EventId: {eventTypeId}");
            }
        }

        public async Task<DoorStatus> GetDoorStatusAsync(string doorId)
        {
            Guid doorIdGuid;
            if (Guid.TryParse(doorId, out doorIdGuid))
            {
                return await TryCall(client => client.GetDoorStatusAsync(_systemProperties.AdminUser, _systemProperties.AdminPassword, doorIdGuid));
            }
            else
            {
                throw new DemoApplicationClientException($"GetDoorStatusAsync: Error parsing Guid. DoorId: {doorId}");
            }
        }

        public async Task<DoorDescriptor[]> GetDoorsAsync()
        {
            return await TryCall(client => client.GetDoorsAsync(_systemProperties.AdminUser, _systemProperties.AdminPassword));
        }

        public async Task<DoorDescriptor[]> GetDoorsAsync(string username, string password)
        {
            return await TryCall(client => client.GetDoorsAsync(username, password));
        }

        public async Task<DoorControllerDescriptor[]> GetDoorControllersAsync()
        {
            return await TryCall(client => client.GetDoorControllersAsync(_systemProperties.AdminUser, _systemProperties.AdminPassword));
        }

        public async Task<DoorControllerDescriptor[]> GetDoorControllersAsync(string username, string password)
        {
            return await TryCall(client => client.GetDoorControllersAsync(username, password));
        }

        public async Task UpdateDoorEnabledStates(string username, string password, Tuple<string, bool>[] changedStates)
        {
            await TryCall(client => client.UpdateDoorEnabledStatesAsync(username, password, changedStates));
        }

        public async Task UpdateEventTypeEnabledStates(string username, string password, Tuple<string, bool>[] changedStates)
        {
            await TryCall(client => client.UpdateEventTypeEnabledStatesAsync(username, password, changedStates));
        }

        public async Task<EventDescriptor[]> GetEventTypesAsync()
        {
            return await TryCall(client => client.GetEventTypesAsync(_systemProperties.AdminUser, _systemProperties.AdminPassword));
        }

        public async Task<EventDescriptor[]> GetEventTypesAsync(string username, string password)
        {
            return await TryCall(client => client.GetEventTypesAsync(username, password));
        }

        public CredentialHolderDescriptor GetCredentialHolder(string id)
        {
            Guid idGuid;
            if (Guid.TryParse(id, out idGuid))
            {
                return TryCall(client => client.GetCredentialHolder(idGuid));
            }
            {
                throw new DemoApplicationClientException($"GetCredentialHolder: Error parsing Guid. Id: {id}");
            }
        }

        public async Task<CredentialHolderDescriptor> GetCredentialHolderAsync(string id)
        {
            Guid idGuid;
            if (Guid.TryParse(id, out idGuid))
            {
                return await TryCall(client => client.GetCredentialHolderAsync(idGuid));
            }
            {
                throw new DemoApplicationClientException($"GetCredentialHolderAsync: Error parsing Guid. Id: {id}");
            }
        }

        public CredentialHolderDescriptor[] SearchCredentialHolders(string searchString)
        {
            return TryCall(client => client.SearchCredentialHolders(searchString));
        }

        private WebServiceClient GetClient()
        {
            lock (_clientLock)
            {
                if (_client == null || _client.State == CommunicationState.Faulted)
                {
                    var endpointUri = new UriBuilder("http", _systemProperties.Address, _systemProperties.Port, "/DemoACServerApplication/WebService/").Uri;
                    _client = new WebServiceClient(_binding, new EndpointAddress(endpointUri));
                }
                return _client;
            }
        }

        #region TryCall

        private T TryCall<T>(Func<WebServiceClient, T> call)
        {
            try
            {
                var client = GetClient();
                return call(client);
            }
            catch (TimeoutException ex)
            {
                throw new DemoApplicationClientException("Timeout communicating with the Demo Application.", ex);
            }
            catch (CommunicationException ex)
            {
                throw new DemoApplicationClientException("Error communicating with the Demo Application.", ex);
            }
        }

        private async Task TryCall(Func<WebServiceClient, Task> call)
        {
            try
            {
                var client = GetClient();
                await call(client);
            }
            catch (TimeoutException ex)
            {
                throw new DemoApplicationClientException("Timeout communicating with the Demo Application.", ex);
            }
            catch (CommunicationException ex)
            {
                throw new DemoApplicationClientException("Error communicating with the Demo Application.", ex);
            }
        }

        private async Task<T> TryCall<T>(Func<WebServiceClient, Task<T>> call)
        {
            try
            {
                var client = GetClient();
                return await call(client);
            }
            catch (TimeoutException ex)
            {
                throw new DemoApplicationClientException("Timeout communicating with the Demo Application.", ex);
            }
            catch (CommunicationException ex)
            {
                throw new DemoApplicationClientException("Error communicating with the Demo Application.", ex);
            }
        }

        #endregion
    }
}
