using DemoServerApplication;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;

namespace DemoACServerApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceHost _serviceHost = null;
        private readonly DemoWebServer _demoWebServer = new DemoWebServer();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            OpenWebService();
            var mainWindow = new MainWindow();

            mainWindow.Show();

            _demoWebServer.Run();
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _demoWebServer.Stop();

            if (_serviceHost != null)
            {
                _serviceHost.Close();
                (_serviceHost as IDisposable).Dispose();
            }
        }
        
        private void OpenWebService()
        {
            var baseAddress = "http://localhost:8732/DemoACServerApplication/WebService/";
            _serviceHost = new ServiceHost(typeof(WebService), new Uri(baseAddress));

            // Check to see if the service host already has a ServiceMetadataBehavior.
            var smb = _serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            // If not, add one.
            if (smb == null)
                smb = new ServiceMetadataBehavior();

            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            _serviceHost.Description.Behaviors.Add(smb);

            _serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            _serviceHost.AddServiceEndpoint(typeof(IWebService), CreateWSHttpBindingPlain(), "");
            _serviceHost.Open();

        }

        private WSHttpBinding CreateWSHttpBindingPlain()
        {
            const int MESSAGE_BUFFER_SIZE = 500000000;
            var binding = new WSHttpBinding(SecurityMode.None);
            binding.MaxReceivedMessageSize =
                binding.MaxBufferPoolSize =
                binding.ReaderQuotas.MaxArrayLength =
                binding.ReaderQuotas.MaxBytesPerRead =
                binding.ReaderQuotas.MaxNameTableCharCount =
                binding.ReaderQuotas.MaxDepth =
                binding.ReaderQuotas.MaxStringContentLength = MESSAGE_BUFFER_SIZE;

            return binding;
        }

    }
}
