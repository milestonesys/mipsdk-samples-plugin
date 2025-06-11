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

namespace ServerConnection.Background
{
	class ServerConnectionDump : BackgroundPlugin
	{
		private static Guid _id = new Guid("853DC012-BBAE-4E40-9345-4440184449E1");
		private object _regLicenseReceiver;

		public override Guid Id
		{
			get { return _id; }
		}

		public override void Init()
		{
			EnvironmentManager.Instance.EnableConfigurationChangedService = true;
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
			get { return "ServerConnectionLicenseDump"; }
		}

		public override List<VideoOS.Platform.EnvironmentType> TargetEnvironments
		{
			get { return new List<EnvironmentType>() {EnvironmentType.Service}; }
		}

		/// <summary>
		/// A new license response has been received via activation.
		/// We need to adjust the active number of Items according to the new license.
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
			EnvironmentManager.Instance.Log(false, "ServerConnectionLicense","------ServerLicense check-----", null);
			// We don't use anything from the actual message, as we simply ask for the license 
			Collection<LicenseInformation> responses =
				EnvironmentManager.Instance.LicenseManager.GetLicense(ServerConnectionDefinition.ServerConnectionPluginId);
			if (responses != null)
			{
				EnvironmentManager.Instance.Log(false, "ServerConnectionLicense", "------License response-----", null);
				foreach (LicenseInformation response in responses)
				{
					EnvironmentManager.Instance.Log(false, "ServerConnectionLicense", "--License Name:" + response.Name, null);
					EnvironmentManager.Instance.Log(false, "ServerConnectionLicense", "--License LicenseType:" + response.LicenseType, null);
					EnvironmentManager.Instance.Log(false, "ServerConnectionLicense", "--License Counter:" + response.Counter, null);
					EnvironmentManager.Instance.Log(false, "ServerConnectionLicense", "--License Trial:" + response.TrialMode, null);
					EnvironmentManager.Instance.Log(false, "ServerConnectionLicense", "--License Expire:" + response.Expire, null);
					EnvironmentManager.Instance.Log(false, "ServerConnectionLicense", "--License Items:" + response.ItemIdentifications.Count, null);
				}
			}
			return null;

		}

	}
}
