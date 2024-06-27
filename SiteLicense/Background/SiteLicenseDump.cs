using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Data;
using VideoOS.Platform.License;
using VideoOS.Platform.Messaging;

namespace SiteLicense.Background
{
	class SiteLicenseDump : BackgroundPlugin
	{
		private static Guid _id = new Guid("179C8DD4-52FF-4625-8A7B-C4EDC619F43A");
		private object _regLicenseReceiver;

		public override Guid Id
		{
			get { return _id; }
		}

		public override void Init()
		{
			EnvironmentManager.Instance.EnableConfigurationChangedService = true;
			// Notice, that this call will in most cases NOT receive anything - as the EventServer may is not connected to a VMS system yet.
			_regLicenseReceiver = EnvironmentManager.Instance.RegisterReceiver(NewLicense,
																			   new MessageIdFilter(
																				MessageId.System.LicenseChangedIndication));
			EnvironmentManager.Instance.TraceFunctionCalls = false;
		}
		public override void Close()
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_regLicenseReceiver);
		}


		public override string Name
		{
			get { return "SiteLicenseDump"; }
		}

		public override List<VideoOS.Platform.EnvironmentType> TargetEnvironments
		{
			get { return new List<EnvironmentType>() {EnvironmentType.Service}; }
		}

		/// <summary>
		/// A new license response has been received via activation.
		/// We need to adjust the active number of Items according to the new licnse.
		/// Following cases needs to be done:
		/// - If counter increases, more trial licenses should be set to normal 
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="s"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		private static object NewLicense(VideoOS.Platform.Messaging.Message message, FQID s, FQID r)
		{
			EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "------ServerLicense check-----", null);

			// We dont use anything from the actual message, as we simply ask for the license 
			//Collection<LicenseInformation> responses =
			//	EnvironmentManager.Instance.LicenseManager.GetLicense(SiteLicenseDefinition.SiteLicensePluginId);

			// Get the activated license, if available
			LicenseInformation _activatedServerLicense = EnvironmentManager.Instance.LicenseManager.GetLicense(
				SiteLicenseDefinition.SiteLicensePluginId,
				SiteLicenseDefinition.SiteLicenseId);

			// Get the stored license, that is either a copy of the activated one, or a TrialMode one as filled below. 
			LicenseInformation _myServerLicense = EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.GetLicenseInformation(
				SiteLicenseDefinition.SiteLicensePluginId,
				SiteLicenseDefinition.SiteLicenseId);

			if (_activatedServerLicense != null)
			{
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "------License response:LicenseManager-----", null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "--License Name:" + _activatedServerLicense.Name, null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump",
				                                "--License LicenseType:" + _activatedServerLicense.LicenseType, null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "--License Counter:" + _activatedServerLicense.Counter,
				                                null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "--License Trial:" + _activatedServerLicense.TrialMode,
				                                null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "--License Expire:" + _activatedServerLicense.Expire, null);
			}

			if (_myServerLicense != null)
			{
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "------License response:ReservedLicenseManager----", null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "--License Name:" + _myServerLicense.Name, null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump",
												"--License LicenseType:" + _myServerLicense.LicenseType, null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "--License Counter:" + _myServerLicense.Counter,
												null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "--License Trial:" + _myServerLicense.TrialMode,
												null);
				EnvironmentManager.Instance.Log(false, "SiteLicenseDump", "--License Expire:" + _myServerLicense.Expire, null);
			}

			return null;

		}

	}
}
