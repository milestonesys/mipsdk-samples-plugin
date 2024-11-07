using DemoAccessControlPlugin.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Managers
{
    /// <summary>
    /// This part of the ConfigurationManager class handles the personalized configuration.
    /// The class has been separated for clarity.
    /// </summary>
    internal partial class ConfigurationManager : ACConfigurationManager
    {
        private readonly Dictionary<string, long> _userVersion = new Dictionary<string, long>();
        private readonly object _userVersionLock = new object();

        /// <summary>
        /// Fetch configuration using the personalized log-in user.
        /// </summary>
        public override bool StartFetchPersonalizedConfiguration(string username, string password, string configVersion)
        {
            // Optially return false, if user already has the lastest change
            long latestVersion;
            lock (_userVersionLock)
            {
                long currentVersion;
                if (_userVersion.TryGetValue(username, out latestVersion) && long.TryParse(configVersion, out currentVersion))
                {
                    if (currentVersion >= latestVersion)
                    {
                        return false;
                    }
                }
            }

            // Fetch configuration asynchronously, notifying the framework of changes using FireFetchPersonalizedConfigurationStatusChanged
            Task.Run(() => FetchPersonalizedConfigurationAsync(username, password, latestVersion));
            return true;
        }

        public override void CancelFetchPersonalizedConfiguration(string username)
        {
            // Cancellation not implemented for this sample
        }

        private void _client_UserRightsChanged(object sender, UserRightsChangedEventArgs e)
        {
            lock (_userVersionLock)
            {
                // Use ticks as version number for demonstration
                _userVersion[e.Username] = e.LastChange.Ticks;
            }
            FirePersonalizedConfigurationChanged(e.Username);
        }

        private async Task FetchPersonalizedConfigurationAsync(string username, string password, long latestVersion)
        {
            try
            {
                FireFetchPersonalizedConfigurationStatusChanged(new ACFetchPersonalizedConfigurationStatusChangedEventArgs(0, "Waiting for connection to be established", username));

                var authorized = await _client.CheckCredentialsAsync(username, password);
                if (!authorized)
                {
                    FireFetchPersonalizedConfigurationStatusChanged(new ACFetchPersonalizedConfigurationStatusChangedEventArgs("Invalid username or password.", username));
                    return;
                }

                FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs(0.25, "Retrieving door controllers..."));

                var doorControllers = await _client.GetDoorControllersAsync(username, password);
                await Task.Delay(500); // Delay a bit for demonstration

                FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs(0.5, "Retrieving doors..."));

                var doors = await _client.GetDoorsAsync(username, password);
                await Task.Delay(500); // Delay a bit for demonstration

                FireFetchConfigurationStatusChanged(new ACFetchConfigurationStatusChangedEventArgs(0.75, "Retrieving event types..."));

                var eventTypes = await _client.GetEventTypesAsync(username, password);
                await Task.Delay(500); // Delay a bit for demonstration

                var configuration = BuildConfiguration(doorControllers, doors, eventTypes);
                if (configuration != null)
                {
                    // Done
                    FireFetchPersonalizedConfigurationStatusChanged(new ACFetchPersonalizedConfigurationStatusChangedEventArgs(configuration, username, latestVersion.ToString()));
                }
                else
                {
                    FireFetchPersonalizedConfigurationStatusChanged(new ACFetchPersonalizedConfigurationStatusChangedEventArgs("Invalid configuration.", username));
                }
            }
            catch (DemoApplicationClientException ex)
            {
                ACUtil.Log(true, "DemoACPlugin.ConfigurationManager", "Error fetching configuration for user " + username + ": " + ex.Message);
                FireFetchPersonalizedConfigurationStatusChanged(new ACFetchPersonalizedConfigurationStatusChangedEventArgs("Error communicating with the Demo Application.", username));
            }
        }
    }
}
