using DemoAccessControlPlugin.Client;
using DemoAccessControlPlugin.Configuration;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Managers
{
    /// <summary>
    /// The CredentialHolderManager is responsible for fetching credential holder on-demand
    /// and notifying the system, when credential holders have changed
    /// </summary>
    internal class CredentialHolderManager : ACCredentialHolderManager
    {
        private readonly SystemProperties _systemProperties;
        private readonly DemoClient _client;

        public override bool CredentialHolderSearchSupported
        {
            get { return true; }
        }

        public override bool CredentialHolderImageOverrideEnabled
        {
            // For demo purposes, this is configurable. Normally it would depend on the access control system.
            get { return _systemProperties.ImageOverrideEnabled; }
        }

        public CredentialHolderManager(SystemProperties systemProperties, DemoClient client)
        {
            _systemProperties = systemProperties;
            _client = client;
            _client.CredentialHolderChanged += _client_CredentialHolderChanged;
        }

        public void Close()
        {
            _client.CredentialHolderChanged -= _client_CredentialHolderChanged;
        }

        public override ACCredentialHolder GetCredentialHolder(string credentialHolderId)
        {
            // This is called when credential holder isn't already cached - look up synchronously
            try
            {
                var credentialHolderDecriptor = _client.GetCredentialHolder(credentialHolderId);
                if (credentialHolderDecriptor != null)
                {
                    return TypeConverter.ToACCredentialHolder(credentialHolderDecriptor, _systemProperties);
                }
            }
            catch (DemoApplicationClientException ex)
            {
                ACUtil.Log(true, "DemoACPlugin.CredentialHolderManager", "Error looking up credential holder: " + ex.Message);
            }
            return null;
        }

        public override ACCredentialHolderSearchResults SearchCredentialHolders(string searchString, int searchLimit)
        {
            var partialResult = false;
            var searchResult = new List<ACCredentialHolderSearchResult>();
            try
            {
                var credentialHolders = _client.SearchCredentialHolders(searchString);

                foreach (var ch in credentialHolders)
                {
                    searchResult.Add(new ACCredentialHolderSearchResult(ch.CredentialHolderId.ToString(), ch.CredentialHolderName, ch.Roles));
                }

                // Demo Access Control system does not support search limit, so we truncate the result
                if (searchResult.Count > searchLimit)
                {
                    searchResult.RemoveRange(searchLimit, searchResult.Count - searchLimit);
                    partialResult = true;
                }
            }
            catch (DemoApplicationClientException ex)
            {
                ACUtil.Log(true, "DemoACPlugin.CredentialHolderManager", "Error searching credential holders: " + ex.Message);
            }
            return new ACCredentialHolderSearchResults(searchResult, partialResult);
        }

        private async void _client_CredentialHolderChanged(object sender, CredentialHolderChangedEventArgs e)
        {
            // Fetch the updated credential holder asynchronously before calling FireCredentialHoldersChanged
            try
            {
                var credentialHolderDecriptor = await _client.GetCredentialHolderAsync(e.CredentialHolderId);
                if (credentialHolderDecriptor != null)
                {
                    var credentialHolder = TypeConverter.ToACCredentialHolder(credentialHolderDecriptor, _systemProperties);
                    FireCredentialHoldersChanged(new[] { credentialHolder });
                }
            }
            catch (DemoApplicationClientException ex)
            {
                ACUtil.Log(true, "DemoACPlugin.CredentialHolderManager", "Error updating credential holder: " + ex.Message);
            }
        }
    }
}
