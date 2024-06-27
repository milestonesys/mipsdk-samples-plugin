using DemoAccessControlPlugin.Client;
using System;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Managers
{
    /// <summary>
    /// The ConnectionManager is responsible for calling FireACSystemConnectionStateChanged,
    /// as well as implementing Connect/Disconnect, which should start/stop event and status subscription.
    /// </summary>
    internal class ConnectionManager : ACConnectionManager
    {
        private readonly DemoClient _client;

        public ConnectionManager(DemoClient client)
        {
            _client = client;
            _client.Connected += _client_Connected;
            _client.Disconnected += _client_Disconnected;
        }

        public override void Connect()
        {
            _client.StartEventPolling();
        }

        public override void Disconnect()
        {
            _client.StopEventPolling();
        }

        public void Close()
        {
            _client.Connected -= _client_Connected;
            _client.Disconnected -= _client_Disconnected;
        }

        /// <summary>
        /// Validate user credentials for personalized log-in.
        /// </summary>
        public override ACUserCredentialsValidationResult ValidateUserCredentials(string username, string password)
        {
            try
            {
                var valid = _client.CheckCredentials(username, password);
                return new ACUserCredentialsValidationResult(valid);
            }
            catch (DemoApplicationClientException ex)
            {
                ACUtil.Log(true, "DemoACPlugin.ConnectionManager", "Error validating user credentials: " + ex.Message);
                return new ACUserCredentialsValidationResult(false);
            }
        }

        private void _client_Connected(object sender, EventArgs e)
        {
            FireACSystemConnectionStateChanged(true);
        }

        private void _client_Disconnected(object sender, EventArgs e)
        {
            FireACSystemConnectionStateChanged(false);
        }
    }
}
