using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace PlatformFileView.WebService
{

	public class WebServiceManager
	{
	    public FileServiceClient ClientProxy { get; private set; }

	    public static WebServiceManager Instance = new WebServiceManager();

        /// <summary>
        /// Create the connection to the service using the given host address
        /// </summary>
        /// <param name="hostAddress">Address of the host</param>
        public void SetRemoteAddress(string hostAddress)
		{
			Close();
		    
            //Create Basic binding that uses Windows authentication
            BasicHttpBinding binding = new BasicHttpBinding();
		    binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

		    //Define URI for SSL Endpoint
		    UriBuilder uriBuilder = new UriBuilder(Uri.UriSchemeHttps, hostAddress, 443, "/FileService");

            //Create the client and specify the credentials (Windows default)
            EndpointAddress webServiceAddress = new EndpointAddress(uriBuilder.Uri);
			ClientProxy = new FileServiceClient(binding, webServiceAddress);
            ClientProxy.ClientCredentials.Windows.ClientCredential = (NetworkCredential) CredentialCache.DefaultCredentials;
            ClientProxy.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication()
            {
                CertificateValidationMode = X509CertificateValidationMode.None
            };

            ClientProxy.Open();          
        }	  

        /// <summary>
        /// Close the connection if it exists
        /// </summary>
        public void Close()
		{
			if (ClientProxy != null && ClientProxy.State == CommunicationState.Opened)
			{
				ClientProxy.Close();
				ClientProxy = null;
			}
		}
	}
}
