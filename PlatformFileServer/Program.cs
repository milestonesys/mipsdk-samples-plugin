using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PlatformFileServer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Server server = new Server();
			string result = server.StartService();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormMain(result));

			server.StopService();
		}
	}
}
