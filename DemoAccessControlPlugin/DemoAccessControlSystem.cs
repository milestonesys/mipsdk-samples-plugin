using DemoAccessControlPlugin.Client;
using DemoAccessControlPlugin.Configuration;
using DemoAccessControlPlugin.Managers;
using System;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin
{
    /// <summary>
    /// The ACSystem represents an instance of the access control system in the VMS,
    /// and exposes the different manager classes needed for handing connection, events, states, etc.
    /// </summary>
    internal class DemoAccessControlSystem : ACSystem
    {
        private SystemProperties _systemProperties;
        private DemoClient _client;
        private CommandManager _commandManager;
        private ConfigurationManager _configurationManager;
        private ConnectionManager _connectionManager;
        private CredentialHolderManager _credentialHolderManager;
        private EventManager _eventManager;
        private StateManager _stateManager;
        private AlarmSynchronizer _alarmSynchronizer;
        private IEnumerable<ACExternalCommand> _externalCommands;

        public override ACCommandManager CommandManager { get { return _commandManager; } }
        public override ACConfigurationManager ConfigurationManager { get { return _configurationManager; } }
        public override ACConnectionManager ConnectionManager { get { return _connectionManager; } }
        public override ACCredentialHolderManager CredentialHolderManager { get { return _credentialHolderManager; } }
        public override ACEventManager EventManager { get { return _eventManager; } }
        public override ACStateManager StateManager { get { return _stateManager; } }
        public override IEnumerable<ACExternalCommand> ExternalCommands { get { return _externalCommands; } }


        /// <summary>
        /// Indicate that the plug-in and Access Control system supports personalized log-in.
        /// The plug-in must implement <see cref="ConfigurationManager.FetchPersonalizedConfigurationAsync(string, string, long)"
        /// and <see cref="CommandManager.ExecuteCommand(string, string, string, string, string)"/>/>
        /// In order to utilize this, the "Operator login required" must be checked in the VMS after having added the system.
        /// </summary>
        public override bool PersonalizedLoginSupported
        {
            get { return true; }
        }

        public override void Init(ACConfiguration configuration)
        {
            ACUtil.Log(false, "DemoACPlugin.DemoAccessControlSystem", "Initializing access control system.");

            _systemProperties = new SystemProperties();
            _client = new DemoClient(_systemProperties);

            _commandManager = new CommandManager(_systemProperties, _client);
            _configurationManager = new ConfigurationManager(_systemProperties, _client, configuration);
            _connectionManager = new ConnectionManager(_client);
            _credentialHolderManager = new CredentialHolderManager(_systemProperties, _client);
            _eventManager = new EventManager(_client);
            _stateManager = new StateManager(_client);

            // Pass the ACAlarmRepository to a separate class for handling two-way alarm synchronization, if needed
            _alarmSynchronizer = new AlarmSynchronizer(_client, ACAlarmRepository);
        }

        public override void Close()
        {
            _commandManager.Close();
            _configurationManager.Close();
            _connectionManager.Close();
            _credentialHolderManager.Close();
            _eventManager.Close();
            _stateManager.Close();
            _alarmSynchronizer.Close();

            _client.Close();
            ACUtil.Log(false, "DemoACPlugin.DemoAccessControlSystem", "Access control system unloaded.");
        }

        /// <summary>
        /// Apply the properties, which are specified when configuring the access control system in the VMS administrator.
        /// SetProperties is called after initialization on both start-up and when configuring the system in the VMS.
        /// </summary>
        public override void SetProperties(IEnumerable<ACProperty> properties)
        {
            _systemProperties.UpdateProperties(properties);

            // Update external commands (uses the system address from the properties)
            var cardholderManagementUri = new UriBuilder("http", _systemProperties.Address, _systemProperties.Port, "DemoACServerApplication/CardholderManagement/").Uri;
            var reportingUri = new UriBuilder("http", _systemProperties.Address, _systemProperties.Port, "DemoACServerApplication/Reporting/").Uri;
            _externalCommands = new[]
            {
                new ACExternalCommand("Cardholder management", cardholderManagementUri.AbsoluteUri, ACExternalCommandTypes.OpenUrlInExternalBrowser, 1),
                new ACExternalCommand("Reporting", reportingUri.AbsoluteUri, ACExternalCommandTypes.OpenUrlInExternalBrowser, 2),
            };
        }
    }
}