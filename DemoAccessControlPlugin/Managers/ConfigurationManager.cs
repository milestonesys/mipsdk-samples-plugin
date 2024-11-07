using DemoAccessControlPlugin.Client;
using DemoAccessControlPlugin.Configuration;
using DemoAccessControlPlugin.Constants;
using DemoAccessControlPlugin.DemoApplicationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.AccessControl.Elements;
using VideoOS.Platform.AccessControl.Plugin;
using VideoOS.Platform.AccessControl.TypeCategories;

namespace DemoAccessControlPlugin.Managers
{
    /// <summary>
    /// This part of the ConfigurationManager handles fetching the configuration from the access control system.
    /// 
    /// Another part of this class handles personalized configuration and is implemented in: PersonalizedConfigurationManger.cs
    /// The class has been separated for clarity.
    /// </summary>
    internal partial class ConfigurationManager : ACConfigurationManager
    {
        private readonly SystemProperties _systemProperties;
        private readonly DemoClient _client;
        private readonly object _ctsLock = new object();
        private CancellationTokenSource _fetchConfigurationCts = null;

        /// <summary>
        /// The GUIDs can be taken from DemoACServerApplication EventManager class. 
        /// Uncomment / Add more items to see the disabled by default functionality 
        /// </summary>
        private List<Guid> _disabledEventTypes = new List<Guid> { /*new Guid("2C19393A-EA35-4BA6-AFCC-5AA6E2A7C970"), new Guid("8120D2CF-EF2A-44D3-AD03-8D82DD6DDCAE")*/};

        /// <summary>
        /// The GUIDs can be taken from DemoACServerApplication
        /// Uncomment / Add more items to see the disabled by default functionality
        /// </summary>
        private List<Guid> _disabledDoors = new List<Guid> { /*new Guid("cdc074cc-14d1-4144-8446-8384ca10f514"), new Guid("ea9a4fe8-0601-4a0e-8873-5ed56309d9dc") */};

        public ConfigurationManager(SystemProperties systemProperties, DemoClient client, ACConfiguration configuration)
        {
            _systemProperties = systemProperties;
            _client = client;
            _client.UserRightsChanged += _client_UserRightsChanged; // Implemented in PersonalizedConfigurationManger.cs

            if (configuration == null)
            {
                // Configuration will be null when initially creating a system. Make up a new server id.
                _client.ServerId = Guid.NewGuid().ToString();
            }
            else
            {
                ApplyConfiguration(configuration);
            }
        }

        public void Close()
        {
            _client.UserRightsChanged -= _client_UserRightsChanged;
        }

        public sealed override void ApplyConfiguration(ACConfiguration configuration)
        {
            ACUtil.Log(false, "DemoACPlugin.ConfigurationManager", "Configuration applied.");

            // Store the server id - used for connected/disconnected events and when rebuilding the configuration
            _client.ServerId = configuration.ACServer.Id;

            UpdateDoorEnabledState(configuration);
            UpdateEventTypeEnabledState(configuration);

            // Other plug-ins might e.g. need to connect to peers listed in the configuration,
            // or need to look up elements to determine the type or read some custom properties.
        }

        private void UpdateEventTypeEnabledState(ACConfiguration configuration)
        {
            if (_systemProperties == null || string.IsNullOrEmpty(_systemProperties.Address))
            {
                return;
            }

            var eventTypes = configuration.ACElements.OfType<ACEventType>().Select(x => new Tuple<string, bool>(x.Id, x.IsEnabled));
            _client.UpdateEventTypeEnabledStates(_systemProperties.AdminUser, _systemProperties.AdminPassword, eventTypes.ToArray()).Wait();
        }

        private void UpdateDoorEnabledState(ACConfiguration configuration)
        {
            if (_systemProperties == null || string.IsNullOrEmpty(_systemProperties.Address))
            {
                return;
            }

            var enabledACUnits = configuration.ACElements.OfType<ACUnit>().Select(acUnit => new Tuple<string, bool>(acUnit.Id, acUnit.IsEnabled));

            _client.UpdateDoorEnabledStates(_systemProperties.AdminUser, _systemProperties.AdminPassword, enabledACUnits.ToArray()).Wait();
        }

        /// <summary>
        /// Fetch configuration using the admin-user.
        /// </summary>
        public override void StartFetchConfiguration()
        {
            CancellationToken cancellationToken;
            lock (_ctsLock)
            {
                if (_fetchConfigurationCts != null)
                {
                    _fetchConfigurationCts.Cancel();
                }
                _fetchConfigurationCts = new CancellationTokenSource();
                cancellationToken = _fetchConfigurationCts.Token;
            }

            // Fetch configuration asynchronously, notifying the framework of changes using FireFetchConfigurationStatusChanged
            Task.Run(() => FetchConfigurationAsync(cancellationToken));
        }

        public override void CancelFetchConfiguration()
        {
            lock (_ctsLock)
            {
                _fetchConfigurationCts.Cancel();
            }
        }

        private async Task FetchConfigurationAsync(CancellationToken cancellationToken)
        {
            try
            {
                FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs(0, "Connecting to demo system..."));

                // Check admin credentials (not actually required)
                var valid = await _client.CheckCredentialsAsync(_systemProperties.AdminUser, _systemProperties.AdminPassword);
                await Task.Delay(500, cancellationToken); // Delay a bit for show
                cancellationToken.ThrowIfCancellationRequested();

                if (!valid)
                {
                    FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs("Invalid credentials."));
                    return;
                }

                FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs(0.25, "Retrieving door controllers..."));

                var doorControllers = await _client.GetDoorControllersAsync();
                await Task.Delay(500, cancellationToken); // Delay a bit for demonstration
                cancellationToken.ThrowIfCancellationRequested();

                FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs(0.5, "Retrieving doors..."));

                var doors = await _client.GetDoorsAsync();
                await Task.Delay(500, cancellationToken); // Delay a bit for demonstration
                cancellationToken.ThrowIfCancellationRequested();

                FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs(0.75, "Retrieving event types..."));

                var eventTypes = await _client.GetEventTypesAsync();
                await Task.Delay(500, cancellationToken); // Delay a bit for demonstration
                cancellationToken.ThrowIfCancellationRequested();

                var configuration = BuildConfiguration(doorControllers, doors, eventTypes);
                if (configuration != null)
                {
                    // Done
                    FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs(configuration));
                }
                else
                {
                    FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs("Invalid configuration."));
                }

            }
            catch (DemoApplicationClientException ex)
            {
                ACUtil.Log(true, "DemoACPlugin.ConfigurationManager", "Error fetching configuration: " + ex.Message);
                FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs("Error communicating with the Demo Application."));
            }
            catch (OperationCanceledException)
            {
                FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs("Fetching configuration canceled."));
            }
        }

        private ACConfiguration BuildConfiguration(DoorControllerDescriptor[] doorControllers, DoorDescriptor[] doors, EventDescriptor[] eventDescriptors)
        {
            var elements = new List<ACElement>();
            
            // Add element types
            elements.Add(ElementTypes.ServerType);
            elements.Add(ElementTypes.DoorControllerType);

            // Add event types
            elements.AddRange(EventTypes.ServerEventTypes);

            foreach (var eventDescriptor in eventDescriptors)
            {
                var acEventType = TypeConverter.ToACEventType(eventDescriptor, !_disabledEventTypes.Contains(eventDescriptor.EventId));
                elements.Add(acEventType);
            }

            // Add state types
            elements.AddRange(StateTypes.ServerStateTypes);
            elements.AddRange(StateTypes.DoorStateTypes);

            // Add command types
            elements.AddRange(CommandTypes.AllCommands);

            // Look up the all events, which can be fired on a door
            // OBS: In the Demo Access Control application, events with source "DoorController" are actually fired on the door.
            var doorEventTypeIds = eventDescriptors.Where(e => e.SourceType == "Door" || e.SourceType == "DoorController").Select(ed => ed.EventId.ToString());
            elements.Add(ElementTypes.CreateDoorType(doorEventTypeIds));

            // Look up the all events, which can be fired on an access point
            var apEventTypeIds = eventDescriptors.Where(ed => ed.SourceType == "AccessPoint").Select(ed => ed.EventId.ToString());
            elements.Add(ElementTypes.CreateAccessPointType(apEventTypeIds));

            // Add server element
            elements.Add(TypeConverter.CreateACServer(_client.ServerId, _systemProperties.Address));

            // Add door controllers
            foreach (var doorController in doorControllers)
            {
                elements.Add(TypeConverter.ToACUnit(doorController));
            }

            // Add doors and access points
            foreach (var door in doors)
            {
                door.Enabled = !_disabledDoors.Contains(door.DoorId);
                elements.AddRange(TypeConverter.ToACUnits(door));
            }

            try
            {
                return ACConfiguration.CreateACConfiguration(DateTime.UtcNow, elements);
            }
            catch (ACConfigurationException ex)
            {
                ACUtil.Log(true, "DemoACPlugin.ConfigurationManager", "Error building configuration: " + ex.Message);
                return null;
            }
        }

        public override void OperationableInstanceEnabledStateChanged(IEnumerable<Tuple<string, bool>> acOperationableInstances)
        {
            _client.UpdateDoorEnabledStates(_systemProperties.AdminUser, _systemProperties.AdminPassword, acOperationableInstances.ToArray()).Wait();
        }

        public override void EventTypeEnabledStateChanged(IEnumerable<Tuple<string, bool>> acEventTypes)
        {
            _client.UpdateEventTypeEnabledStates(_systemProperties.AdminUser, _systemProperties.AdminPassword, acEventTypes.ToArray()).Wait();
        }
    }
}
