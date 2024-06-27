using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VideoOS.Platform;
using VideoOS.Platform.License;
using VideoOS.Platform.Messaging;

namespace SiteLicense.Admin
{
	internal class SiteLicenseHandler
	{
		private static LicenseInformation _myServerLicense;
		private static LicenseInformation _activatedServerLicense;

		private static object _regLicenseReceiver;
		private static bool _initialized = false;

		internal static void Init()
		{
			_regLicenseReceiver = EnvironmentManager.Instance.RegisterReceiver(NewLicense,
																			   new MessageIdFilter(
																				MessageId.System.LicenseChangedIndication));

			// Get the activated license, if available
			_activatedServerLicense = EnvironmentManager.Instance.LicenseManager.GetLicense(
				SiteLicenseDefinition.SiteLicensePluginId,
				SiteLicenseDefinition.SiteLicenseId);

			// Get the stored license, that is either a copy of the activated one, or a TrialMode one as filled below. 
			_myServerLicense = EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.GetLicenseInformation(
				SiteLicenseDefinition.SiteLicensePluginId,
				SiteLicenseDefinition.SiteLicenseId);

			if (_myServerLicense == null)
			{
				// If real license already activated, use that one as a starting point
				if (_activatedServerLicense != null)				
					_myServerLicense = _activatedServerLicense;
				else
				{
					// If no license is available, store today + 30 days as the trial period
					_myServerLicense = new LicenseInformation()
					                   	{
											PluginId = SiteLicenseDefinition.SiteLicensePluginId,
					                   		Counter = 1,
					                   		CustomData = "Here we can add some custom data",
											LicenseType = SiteLicenseDefinition.SiteLicenseId,
					                   		Name = "Site License Sample",
					                   		Expire = DateTime.UtcNow + TimeSpan.FromDays(30),
											TrialMode = true,
					                   		ItemIdentifications = new System.Collections.ObjectModel.Collection<LicenseItem>()
					                   	};
				}
				if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Administration)
				{
					EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.SaveLicenseInformation(_myServerLicense);
				}
			}
			_initialized = true;
		}

		internal static void Close()
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_regLicenseReceiver);
		}


		/// <summary>
		/// The user issued an Activation Reqeust.  We need to supply information for our plugin(s)
		/// </summary>
		/// <returns></returns>
		internal static Collection<LicenseInformation> GetLicenseInformations()
		{
			if (!_initialized)
				Init();
			_myServerLicense.Counter = 1;
			return new Collection<LicenseInformation>() {_myServerLicense};
		}

		/// <summary>
		/// A method to display in what state the License is.
		/// </summary>
		/// <returns></returns>
		internal static string LicenseInfo()
		{
			if (!_initialized)
				Init();

			string mode = _activatedServerLicense==null ? "Trial" : "Licensed";
			string expire = "No Expiration date";
			if (_myServerLicense == null)
			{
				expire = "No License found";
			} else
			if (_myServerLicense.Expire.Year != 9999)
				if (_myServerLicense.Expire < DateTime.UtcNow)
					expire = "Expired " + _myServerLicense.Expire.ToShortDateString();
				else
					expire = "Expires " + _myServerLicense.Expire.ToShortDateString();
			return String.Format("Site License information: {0} - {1}", mode, expire);
		}

		/// <summary>
		/// A new license response has been received via activation. Store it for retrieval by Smart Client plugin.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="s"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		private static object NewLicense(VideoOS.Platform.Messaging.Message message, FQID s, FQID r)
		{
			// We dont use anything from the actual message, as we simply ask for the license 
			LicenseInformation response =
				EnvironmentManager.Instance.LicenseManager.GetLicense(SiteLicenseDefinition.SiteLicensePluginId, SiteLicenseDefinition.SiteLicenseId);
			if (response != null)
			{
				if (response.Counter > 1)
					response.Counter = 1;
				_activatedServerLicense = response.Clone();
				_myServerLicense = response.Clone();
				EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.SaveLicenseInformation(_myServerLicense);
			}
			return null;
		}
	}
}
