using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace PlatformFileServer
{
	public class WebService
	{
		private readonly ServiceHost _serviceHost;

        /// <summary>
        /// Constructor: Starts the Web service
        /// </summary>
		public WebService()
		{
            //Define SSL endpoint
		    String address = "localhost";
            UriBuilder uriBuilder = new UriBuilder(Uri.UriSchemeHttps, address, 443, "/FileService");
			_serviceHost = new ServiceHost(typeof(FileService), uriBuilder.Uri);

            //Create a Basic binding with Windows authentication
		    BasicHttpBinding binding = new BasicHttpBinding();
		    binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

		    _serviceHost.AddServiceEndpoint(typeof(IFileService), binding, uriBuilder.Uri);

			ServiceBehaviorAttribute sba = _serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();		    
            if (sba == null)
				sba = new ServiceBehaviorAttribute();
			sba.InstanceContextMode = InstanceContextMode.Single;
			ServiceMetadataBehavior serviceMetadataBehavior = new ServiceMetadataBehavior();
			serviceMetadataBehavior.HttpsGetEnabled = true;
			_serviceHost.Description.Behaviors.Add(serviceMetadataBehavior);	    
		}

        /// <summary>
        /// Can throw AddressAccessDeniedException 
        /// </summary>
		public void Open()
		{
			_serviceHost.Open();
		}

        /// <summary>
        /// Gracefully close the connection
        /// </summary>
		public void Close()
		{
			if (_serviceHost != null && _serviceHost.State == CommunicationState.Opened)
			{
				_serviceHost.Close();
			}
		}
	}

}
