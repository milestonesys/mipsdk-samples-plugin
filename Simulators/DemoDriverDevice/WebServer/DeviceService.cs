using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace DemoDriverDevice.WebServer
{
    internal class DeviceService
    {
        private readonly ServiceHost _serviceHost;
        private readonly int _port;

        internal DeviceService(string scheme, int port)
        {
            _port = port;
            string theUrl = $"{scheme}://{Environment.MachineName}:{_port}/DeviceService";
            var baseAddress = new Uri(theUrl);
            _serviceHost = new ServiceHost(typeof(WebServiceHandler), baseAddress);
            if (scheme == Uri.UriSchemeHttps)
            {
                // NOTE: remember to install a certificate and run the "netsh http add sslcert" command if you want to use HTTPS - see the sample documentation for further details
                _serviceHost.AddServiceEndpoint(typeof(IDeviceService), new BasicHttpsBinding(), baseAddress);
            }
            else
            {
                _serviceHost.AddServiceEndpoint(typeof(IDeviceService), new BasicHttpBinding(), baseAddress);
            }

            var sba = _serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            if (sba != null)
            {
                sba.InstanceContextMode = InstanceContextMode.PerCall;
            }
            _serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpsGetEnabled = true
            });
        }

        public void Open()
        {
            _serviceHost.Open();
        }

        public bool IsOpen => _serviceHost.State == CommunicationState.Opened;

        public void Close()
        {
            if (_serviceHost.State == CommunicationState.Faulted)
            {
                _serviceHost.Abort();
            }
            else
            {
                _serviceHost.Close();
            }
        }
    }
}
