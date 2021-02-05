using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

namespace PlatformFileServer
{
	public class Server : System.ServiceProcess.ServiceBase
	{
		static WebService _webService;
	    private String result = "";

		public string StartService()
		{
			try
			{
				OnStart(null);
			}
			catch (Exception ex)
			{
				result = "Server could not start: " + ex.Message;
			}
		    return result;
		}

		public void StopService()
		{
			try
			{
				OnStop();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Server could not stop: " + ex.Message + "\r\n" + ex.StackTrace);
				throw;
			}
		}

		protected override void OnStart(string[] args)
		{
			_webService = new WebService();
            try
            {
                _webService.Open();
                result = "Opened ...";
            }
            catch (AddressAccessDeniedException)
            {
                result = "Access denied - consider to run 'as administrator'";
            }
		}

        protected override void OnStop()
        {
			_webService.Close();		        	
		}
	}
}
