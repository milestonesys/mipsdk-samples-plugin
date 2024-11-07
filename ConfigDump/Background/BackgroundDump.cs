using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;

namespace ConfigDump2.Background
{
    class BackgroundDump : BackgroundPlugin
	{
		private static Guid _id = new Guid("EB675CE1-58BC-42BB-B78B-BB0B92B95993");
		private Thread _thread;
		private object _msgRef, _msgRef2;
		private readonly AutoResetEvent _shutdownEvent = new AutoResetEvent(false);

		public override Guid Id
		{
			get { return _id; }
		}

		public override void Init()
		{
			_msgRef = EnvironmentManager.Instance.RegisterReceiver(configChangedHandler,
			                                                       new MessageIdFilter(MessageId.System.SystemConfigurationChangedIndication));
			_msgRef2 = EnvironmentManager.Instance.RegisterReceiver(NewEventsHandler,
																    new MessageIdFilter(MessageId.Server.NewEventsIndication));
            _thread = new Thread(new ThreadStart(Run));
			_thread.Start();
		}

        public override void Close()
        {
			EnvironmentManager.Instance.UnRegisterReceiver(_msgRef);
            EnvironmentManager.Instance.UnRegisterReceiver(_msgRef2);
			_shutdownEvent.Set();
            _thread.Join();
        }

        public override string Name
		{
			get { return "ConfigDump"; }
		}

		private object configChangedHandler(Message message, FQID f1, FQID f2)
		{
			Item siteItem = EnvironmentManager.Instance.GetSiteItem(EnvironmentManager.Instance.MasterSite);
			DumpItems(new List<Item>() { siteItem }, 0, true);
			List<Item> items = Configuration.Instance.GetItems();
			DumpItems(items, 0, true);
			return null;	
		}

		private object NewEventsHandler(Message message, FQID f1, FQID f2)
		{
            var events = message.Data as IEnumerable<BaseEvent>;
            if (events != null)
            {
                foreach (BaseEvent baseEventData in events)
                {
                    if (baseEventData != null)
                    {
                        Debug.WriteLine("---> Event:" + baseEventData.EventHeader.Message + " - for:" + baseEventData.EventHeader.Name);
                    }
                }
            }
			return null;
		}

		// This shows that you can have the background service running
		// not only in the Event Server, but the Smart Client and Management Client as well.
		public override List<EnvironmentType> TargetEnvironments
		{
			get { return new List<EnvironmentType>() {EnvironmentType.Service, EnvironmentType.SmartClient}; }
		}

		private void Run()
		{
			while (true)
			{
                try
                {
                    EnvironmentManager.Instance.Log(false,
                        "BackgroundDump:",
                        "This message is being logged from 'Run()' method in the background every 10 minutes");
                }
                catch (Exception)
                {
                    // Ignore, as Smart Client might have been logged out while Sleep was running.
                }				
				if (_shutdownEvent.WaitOne(TimeSpan.FromMinutes(10)))
				{
					break;
				}
            }
        }

		private void DumpItems(List<Item> items, int indent, bool dumpRelated)
		{
		    if (indent > 80) return;

			string filler = "                                                                                               ".Substring(0, indent);
			foreach (Item item in items)
			{
			    Collection<object> result =
			        EnvironmentManager.Instance.SendMessage(new Message(MessageId.Server.GetIPAddressRequest, item.FQID));
			    string ip = "";
                if (result != null && result.Count>0)
                    ip = result[0] as string;

                EnvironmentManager.Instance.Log(false, "ConfigDump", filler + "Item.Name:   " + item.Name, null);
				EnvironmentManager.Instance.Log(false, "ConfigDump", filler + "     FQID:   " + item.FQID.ToString(), null);
				EnvironmentManager.Instance.Log(false, "ConfigDump", filler + "     Kind:   " + Kind.DefaultTypeToNameTable[item.FQID.Kind], null);
                EnvironmentManager.Instance.Log(false, "ConfigDump", filler + "     IP  :   " + ip, null);
			    if (dumpRelated)
			    {
			        List<Item> related = item.GetRelated();
			        if (related != null && related.Count != 0)
			        {
			            EnvironmentManager.Instance.Log(false, "ConfigDump", filler + "   Related ----------------", null);
			            DumpItems(related, indent + 3, false);
			        }
			    }
			    List<Item> children = item.GetChildren();
			    if (children != null && children.Count != 0)
			    {
                    EnvironmentManager.Instance.Log(false, "ConfigDump", filler + "   Children ----------------", null);
                    DumpItems(children, indent + 3, true);
			    }
			}
		}
	}
}
