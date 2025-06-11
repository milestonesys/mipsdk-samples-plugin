using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.License;
using VideoOS.Platform.Messaging;

namespace ServerConnection.Admin
{
	internal class ServerConnectionLicenseHandler
	{
		private static LicenseInformation _myServerLicense;
		private static LicenseInformation _activatedServerLicense;
		private static int MyGraceCount = 2;				// We will allow 2 extra licenses to be used for trial

		private static object _regLicenseReceiver;


		internal static void Init(ServerConnectionItemManager itemManager)
		{
			_regLicenseReceiver = EnvironmentManager.Instance.RegisterReceiver(NewLicense,
																			   new MessageIdFilter(
																				MessageId.System.LicenseChangedIndication));

			_activatedServerLicense = EnvironmentManager.Instance.LicenseManager.GetLicense(
				ServerConnectionDefinition.ServerConnectionPluginId, 
				ServerConnectionDefinition.ServerLicenseId);
			_myServerLicense = EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.GetLicenseInformation(
				ServerConnectionDefinition.ServerConnectionPluginId, 
				ServerConnectionDefinition.ServerLicenseId);

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
											PluginId = ServerConnectionDefinition.ServerConnectionPluginId,
					                   		Counter = 0,
					                   		CustomData = "Here we can add some custom data",
											LicenseType = ServerConnectionDefinition.ServerLicenseId,
					                   		Name = "Server Connection License Sample",
					                   		Expire = DateTime.UtcNow + TimeSpan.FromDays(30),
											TrialMode = true,
					                   		ItemIdentifications = new System.Collections.ObjectModel.Collection<LicenseItem>(),
                                            CounterMethod = LicenseInformation.CounterMethodEnum.CountByItemIdentifications
					                   	};
				}
				EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.SaveLicenseInformation(_myServerLicense);
			}

			// Now check for consistancy
			List<Item> items = itemManager.GetItems();
			Dictionary<string, Item> dictionaryItems = new Dictionary<string, Item>();
			foreach (Item item in items)
				dictionaryItems.Add(item.FQID.ObjectId.ToString(), item);

			Dictionary<string, LicenseItem> alreadyRegistreredItems = new Dictionary<string, LicenseItem>();
			List<LicenseItem> toRemove = new List<LicenseItem>();
			foreach (LicenseItem li in _myServerLicense.ItemIdentifications)
			{
				if (dictionaryItems.ContainsKey(li.Id) == false)
					toRemove.Add(li);
				alreadyRegistreredItems.Add(li.Id, li);
			}

			// Lets remove any residuals (In case of crash or hack)
			foreach (LicenseItem li in toRemove)
				_myServerLicense.UnRegisterConfiguredLicenseItem(li);

			// Now add any missing items (crash or hack)
			foreach (Item item in items)
			{
				if (alreadyRegistreredItems.ContainsKey(item.FQID.ObjectId.ToString())==false)
				{
					LicenseItem li = new LicenseItem()
					                 	{
					                 		Id = item.FQID.ObjectId.ToString(),
					                 		Name = item.Name,
					                 		ItemTrial = true,
					                 		ItemTrialEnd = DateTime.UtcNow + TimeSpan.FromDays(14)
					                 	};
					_myServerLicense.RegisterConfiguredLicenseItem(li);
				}
			}

			// In this sample the number of reserved Items is also the counter for 'used' licenses
			_myServerLicense.Counter = _myServerLicense.ItemIdentifications.Count;
		    _myServerLicense.CounterMethod = LicenseInformation.CounterMethodEnum.CountByItemIdentifications;
        }

		internal static void Close()
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_regLicenseReceiver);
		}

		internal static void RegisterItem(List<Item> items, Item item)
		{
			LicenseItem li = new LicenseItem() { Id = item.FQID.ObjectId.ToString(), Name = item.Name };       
            li.ItemTrial = true;
            li.ItemTrialEnd = DateTime.Now + TimeSpan.FromDays(14);
            _myServerLicense.RegisterConfiguredLicenseItem(li);
            _myServerLicense.Counter++;
        }

        /// <summary>
        /// The user issued an Activation Request.  We need to supply information for our plugin(s)
        /// </summary>
        /// <returns></returns>
        internal static Collection<LicenseInformation> GetLicenseInformations()
		{
			// We could update other fields as it makes sense. In this sample we simply forward the Registered
			// LicenseInformation Items
			return new System.Collections.ObjectModel.Collection<LicenseInformation>() {_myServerLicense};
		}

		internal static void UnRegisterItem(Item item)
		{

			if (_myServerLicense != null)
			{
				LicenseItem li = new LicenseItem() { Id = item.FQID.ObjectId.ToString(), Name = item.Name };
				_myServerLicense.UnRegisterConfiguredLicenseItem(li);
                _myServerLicense.Counter--;
            }
        }


		internal static bool IsAddPossible(List<Item> items)
		{
			int max = MyGraceCount;
            if (_activatedServerLicense != null)
                max += _activatedServerLicense.Counter;
            return max > items.Count;
		}

		private bool EndOfTrial()
		{
			if (_myServerLicense.TrialMode && _myServerLicense.Expire < DateTime.UtcNow)
				return true;
			return false;
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
			// We dont use anything from the actual message, as we simply ask for the license 
			Collection<LicenseInformation> responses =
				EnvironmentManager.Instance.LicenseManager.GetLicense(ServerConnectionDefinition.ServerConnectionPluginId);
			if (responses != null)
			{
				Debug.WriteLine("------License response-----");
				foreach (LicenseInformation response in responses)
				{
					Debug.WriteLine("--License Name:" + response.Name);
					Debug.WriteLine("--License LicenseType:" + response.LicenseType);
					Debug.WriteLine("--License Counter:" + response.Counter);
					Debug.WriteLine("--License Trial:" + response.TrialMode);
					Debug.WriteLine("--License Expire:" + response.Expire);
					Debug.WriteLine("--License Items:" + response.ItemIdentifications.Count);

					if (response.LicenseType == ServerConnectionDefinition.ServerLicenseId)	// Server License
					{
						LicenseInformation li =
							EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.GetLicenseInformation(
								ServerConnectionDefinition.ServerConnectionPluginId,
								response.LicenseType);
                        if (li != null)
                        {
                            foreach (var licenseItem in li.ItemIdentifications)
                            {
                                if (!response.ItemIdentifications.Any(it => it.Id == licenseItem.Id))
                                {
                                    response.ItemIdentifications.Add(licenseItem);
                                }
                            }
                        }
                        _activatedServerLicense = response.Clone();
						_myServerLicense = response.Clone();
						_myServerLicense.Counter = _myServerLicense.ItemIdentifications.Count;
						EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.SaveLicenseInformation(_myServerLicense);
                    }
				}
			}
			return null;

		}

	}
}
