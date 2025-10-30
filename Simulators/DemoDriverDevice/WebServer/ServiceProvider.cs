using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DemoDriverDevice.WebServer
{
    public class ServiceProvider
    {
        private int _port;
        private string _scheme;
        private DeviceService _webService = null;
        private delegate void CallDelegate();
        public event Action OnClosed = delegate {};

        public bool Init(string scheme, int port)
        {
            if (_webService != null)
            {
                Trace.WriteLine("Web service is already running");
                return false;
            }
            _port = port;
            _scheme = scheme;
            Task.Run(() => InitTask());
            return true;
        }

        public void InitTask()
        {
            try
            {
                _webService = new DeviceService(_scheme, _port);
                _webService.Open();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Failed to start web service: {0}", ex));
                _webService = null;
            }
        }

        public void Close()
        {
            Task.Run(() => CloseTask());
        }

        private void CloseTask()
        {
            if (_webService == null)
            {
                Trace.WriteLine("Web service is not running");
                return;
            }
            try
            {
                _webService.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Failed to close web service: {0}", ex));
            }
            finally
            {
                _webService = null;
                OnClosed();
            }
        }

    }
}
