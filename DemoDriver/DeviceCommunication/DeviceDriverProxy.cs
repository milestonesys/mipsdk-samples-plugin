using System;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DemoDriver
{
    public class DeviceDriverProxy
    {
        private const int MaxMessageSize = int.MaxValue;
        private const int TickStopDelay = 50000000; // 5000 ms
        public DemoDriverDevice.DeviceServiceClient Client { get; private set; }

        public DeviceDriverProxy(string uri, string username, SecureString password)
        {
            Uri localUri = new Uri(uri);
            EndpointAddress ep = new EndpointAddress(localUri);
            Binding binding;
            if (localUri.Scheme == Uri.UriSchemeHttps)
            {
                binding = new BasicHttpsBinding { MaxReceivedMessageSize = MaxMessageSize };
            }
            else
            {
               binding = new BasicHttpBinding { MaxReceivedMessageSize = MaxMessageSize };
            }
           
            Client = new DemoDriverDevice.DeviceServiceClient(binding, ep);

            Client.Open();
            long tickStop = DateTime.Now.Ticks + TickStopDelay; 
            while (DateTime.Now.Ticks < tickStop && Client.State == CommunicationState.Opening)
            {
                System.Threading.Thread.Sleep(5);
            }
            if (Client.State == CommunicationState.Opening)
            {
                System.Diagnostics.Trace.WriteLine("CommunicationClient", "Did not reach OPEN state with 5 seconds");
            }
        }


        public void Close()
        {
            if (Client != null)
            {
                try
                {
                    Client.Close();
                    Client = null;
                }
                catch { }
            }
        }
    }
}
