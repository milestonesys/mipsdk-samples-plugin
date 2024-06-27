using DemoAccessControlPlugin.Client;
using DemoAccessControlPlugin.Configuration;
using DemoAccessControlPlugin.Constants;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.AccessControl;
using VideoOS.Platform.AccessControl.Elements;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Managers
{
    /// <summary>
    /// The StateManager is responsible for returning current state on demand (GetStates).
    /// Furthermore, it is responsible for calling FireStatesChanged and FireStatesInvalidated.
    /// </summary>
    internal class StateManager : ACStateManager
    {
        private readonly DemoClient _client;
        private readonly Dictionary<string, ACState> _states = new Dictionary<string, ACState>();
        private readonly object _stateLock = new object();
        private readonly SemaphoreSlim _resolveStateSemaphore = new SemaphoreSlim(1); // Only allow one concurrent request
        private readonly CancellationTokenSource _resolveStateCancellationTokenSource = new CancellationTokenSource();

        public StateManager(DemoClient client)
        {
            _client = client;
            _client.StateChanged += _client_StateChanged;
            _client.Connected += _client_Connected;
            _client.Disconnected += _client_Disconnected;

            SetServerState(StateTypes.ServerDisconnected, false);
        }

        public void Close()
        {
            _client.StateChanged -= _client_StateChanged;
            _client.Connected -= _client_Connected;
            _client.Disconnected -= _client_Disconnected;
            _resolveStateCancellationTokenSource.Cancel();
        }

        public override IEnumerable<ACState> GetStates(IEnumerable<string> operationableInstanceIds)
        {
            foreach (var id in operationableInstanceIds)
            {
                if (id.StartsWith("AP"))
                {
                    // No state on access points
                    yield return new ACState(id, null, ACBuiltInIconKeys.AccessPoint, null);
                    continue;
                }

                var state = GetState(id);

                if (state != null)
                {
                    yield return state;
                }
                else
                {
                    // Return empty state and try to resolve the state asynchronously
                    yield return new ACState(id, null, ACBuiltInIconKeys.DoorUnknown, null);

                    Task.Run(() => RefreshStateAsync(id, _resolveStateCancellationTokenSource.Token));
                }
            }
        }

        private void _client_StateChanged(object sender, StateChangedEventArgs e)
        {
            UpdateState(e.State);
            FireStatesChanged(new[] { e.State });
        }

        private void _client_Connected(object sender, EventArgs e)
        {
            ClearStates();
            var state = SetServerState(StateTypes.ServerConnected, true);

            // Indicate that all states are invalidated
            FireStatesInvalidated(null);
        }

        private void _client_Disconnected(object sender, EventArgs e)
        {
            ClearStates();
            SetServerState(StateTypes.ServerDisconnected, false);

            // Indicate that all states are invalidated
            FireStatesInvalidated(null);
        }

        private async Task RefreshStateAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                // Only allow one concurrent call to the access control system (VMS will likely request status for all doors on startup)
                await _resolveStateSemaphore.WaitAsync(cancellationToken);
                try
                {
                    // Check if status was already updated
                    var state = GetState(id);

                    if (state == null)
                    {
                        try
                        {
                            var doorStatus = await _client.GetDoorStatusAsync(id);

                            cancellationToken.ThrowIfCancellationRequested();
                            if (doorStatus != null)
                            {
                                state = TypeConverter.ToACState(doorStatus);
                                UpdateState(state);
                                FireStatesChanged(new[] { state });
                            }
                        }
                        catch (DemoApplicationClientException ex)
                        {
                            // Should probably retry on failure
                            ACUtil.Log(true, "DemoACPlugin.StateManager", "Error refreshing state for door " + id + ": " + ex.Message);
                        }
                    }
                }
                finally
                {
                    _resolveStateSemaphore.Release();
                }
            }
            catch (OperationCanceledException)
            { }
        }

        private ACState GetState(string id)
        {
            lock (_stateLock)
            {
                ACState state;
                _states.TryGetValue(id, out state);
                return state;
            }
        }

        private void UpdateState(ACState state)
        {
            lock (_stateLock)
            {
                _states[state.OperationableInstanceId] = state;
            }
        }

        private void ClearStates()
        {
            // Clear cache on disconnect
            lock (_stateLock)
            {
                _states.Clear();
            }
        }

        private ACState SetServerState(ACStateType stateType, bool fireStateChange)
        {
            var state = new ACState(_client.ServerId, new[] { stateType.Id }, stateType.IconKey, null);
            lock (_stateLock)
            {
                _states[_client.ServerId] = state;
            }
            return state;
        }
    }
}