using DemoDriverDevice.WebServer;
using System;
using System.Security.Principal;
using System.Windows.Forms;

namespace DemoDriverDevice
{
    internal static class Program
	{
        private struct FormArgs
        {
            public string port;
            public string username;
            public string password;
            public string macAddress;
        }
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main(string[] args)
		{
            if (!IsAdministrator())
            {
                MessageBox.Show("Please run this application as an administrator.", "Error");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = null;
            if (args.Length > 0)
            {
                FormArgs formArgs = new FormArgs();
                try
                {
                    formArgs = GetFormArguments(args);
                }
                catch(IndexOutOfRangeException)
                {
                    MessageBox.Show("Please provide the following arguments:\n<port> <username> <password> [MAC address]", "Too few arguments");
                    return;
                }
                mainForm = new MainForm(formArgs.port, formArgs.username, formArgs.password, formArgs.macAddress, true);
            }

            Application.Run(mainForm ?? new MainForm());
        }

        private static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static FormArgs GetFormArguments(string[] args)
        {
            return new FormArgs()
            {
                port = args[0],
                username = args[1], 
                password = args[2],
                macAddress = args.Length > 3 ? args[3] : null
            };
        }
    }
}
