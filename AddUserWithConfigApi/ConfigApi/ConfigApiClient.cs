using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Xml;

using VideoOS.ConfigurationAPI;
using VideoOS.Platform.Util;

namespace AddUserSample.ConfigApi
{
    public class ConfigurationApiClient
    {
        /// <summary>
        /// Creates a channel with the server for the configuration API communication
        /// </summary>
        /// <returns></returns>
        public static IConfigurationService CreateClient()
        {
            var environmentManager = VideoOS.Platform.EnvironmentManager.Instance;
            var serverId = environmentManager.CurrentSite.ServerId;
            return CreateClient(environmentManager.LoginNetworkCredential, serverId.ServerHostname, serverId.Serverport);
        }

        public static IConfigurationService CreateClient(NetworkCredential credential, string hostname, int port)
        { 
            // Basic users require HTTPS communication
            var isBasicUser = credential.Domain == "[BASIC]";
            var uriBuilder = GetUriBuilder(isBasicUser, hostname, port);

            System.ServiceModel.Channels.Binding binding = GetBinding(isBasicUser);
            binding.Namespace = "VideoOS.ConfigurationAPI";

            var endpointAddress = new EndpointAddress(uriBuilder.Uri, EndpointIdentity.CreateSpnIdentity(SpnFactory.GetSpn(uriBuilder.Uri)));

            ChannelFactory<IConfigurationService> channel = GetChannelFactoryWithCredentials(binding, endpointAddress, isBasicUser, credential);
            channel.Credentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication()
            {
                CertificateValidationMode = X509CertificateValidationMode.None,
            };

            IConfigurationService client = channel.CreateChannel();

            return client;
        }

        /// <summary>
        /// Creates a channel factory with the proper credentials
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="endpointAddress"></param>
        /// <param name="isSecure"></param>
        /// <returns></returns>
        private static ChannelFactory<IConfigurationService> GetChannelFactoryWithCredentials(System.ServiceModel.Channels.Binding binding, EndpointAddress endpointAddress, bool isSecure, NetworkCredential credential)
        {
            ChannelFactory<IConfigurationService> channel = new ChannelFactory<IConfigurationService>(binding, endpointAddress);

            if (isSecure)
            {
                channel.Credentials.UserName.UserName = credential.UserName;
                channel.Credentials.UserName.Password = credential.Password;
            }
            else
            {
                channel.Credentials.Windows.ClientCredential = credential;
            }

            channel.Credentials.SupportInteractive = false;
            return channel;
        }

        /// <summary>
        /// Creates a URI with the proper hostname, port and protocol
        /// </summary>
        /// <param name="isSecure"></param>
        /// <returns></returns>
        private static UriBuilder GetUriBuilder(bool isSecure, string host, int port)
        {
            var uriScheme = isSecure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
            UriBuilder uriBuilder = new UriBuilder(uriScheme, host, port, "ManagementServer/ConfigurationApiService.svc");

            return uriBuilder;
        }

        /// <summary>
        /// Creates a binding with the necessary settings and proper timeouts
        /// </summary>
        /// <param name="isSecure"></param>
        /// <returns></returns>
        private static System.ServiceModel.Channels.Binding GetBinding(bool isSecure)
        {
            return isSecure ? GetHttpsBinding() : GetHttpBinding();
        }

        private static System.ServiceModel.Channels.Binding GetHttpBinding()
        {
            var binding = new WSHttpBinding();
            var security = binding.Security;
            security.Message.ClientCredentialType = MessageCredentialType.Windows;

            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 2147483647;

            binding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;

            return binding;
        }

        private static System.ServiceModel.Channels.Binding GetHttpsBinding()
        {
            var binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 2147483647;

            binding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;

            return binding;
        }
    }
}
