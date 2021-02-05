using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;

namespace EventTracer.Background
{
	/// <summary>
	/// A background plugin will be started during application start and be running until the user logs off or application terminates.<br/>
	/// The Environment will call the methods Init() and Close() when the user login and logout, 
	/// so the background task can flush any cached information.<br/>
	/// The base class implementation of the LoadProperties can get a set of configuration, 
	/// e.g. the configuration saved by the Options Dialog in the Smart Client or a configuration set saved in one of the administrators.  
	/// Identification of which configuration to get is done via the GUID.<br/>
	/// The SaveProperties method can be used if updating of configuration is relevant.
	/// <br/>
	/// The configuration is stored on the server the application is logged into, and should be refreshed when the ApplicationLoggedOn method is called.
	/// Configuration can be user private or shared with all users.<br/>
	/// <br/>
	/// This plugin could be listening to the Message with MessageId == Server.ConfigurationChangedIndication to when when to reload its configuration.  
	/// This event is send by the environment within 60 second after the administrator has changed the configuration.
	/// </summary>
	public class EventTracerBackgroundPlugin : BackgroundPlugin
	{
		private bool _stop = false;
		private Thread _thread;
		private object _msgRef;

		/// <summary>
		/// Gets the unique id identifying this plugin component
		/// </summary>
		public override Guid Id
		{
			get { return EventTracerDefinition.EventTracerBackgroundPlugin; }
		}

		/// <summary>
		/// The name of this background plugin
		/// </summary>
		public override String Name
		{
			get { return "EventTracer BackgroundPlugin"; }
		}

		/// <summary>
		/// Called by the Environment when the user has logged in.
		/// </summary>
		public override void Init()
		{
			_msgRef = EnvironmentManager.Instance.RegisterReceiver(NewEventHandler,
																   new MessageIdFilter(
																	VideoOS.Platform.Messaging.MessageId.Server.NewEventIndication));

			_stop = false;
			_thread = new Thread(new ThreadStart(Run));
			_thread.Start();

		}

		private object NewEventHandler(Message message, FQID f1, FQID f2)
		{
			BaseEvent baseEventData = message.Data as BaseEvent;
			if (baseEventData != null)
			{
				EnvironmentManager.Instance.Log(false, "EventTracer",
				                                "NewEvent:" + baseEventData.EventHeader.Name + ", Message=" +
				                                baseEventData.EventHeader.Message, null);
			}
			return null;
		}


		/// <summary>
		/// Called by the Environment when the user log's out.
		/// You should close all remote sessions and flush cache information, as the
		/// user might logon to another server next time.
		/// </summary>
		public override void Close()
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_msgRef);
			_msgRef = null;
			_stop = true;
		}

		/// <summary>
		/// Define in what Environments the current background task should be started.
		/// </summary>
		public override List<EnvironmentType> TargetEnvironments
		{
			get { return new List<EnvironmentType>() { EnvironmentType.Service }; }	// Default will run in the Event Server
		}


		/// <summary>
		/// the thread doing the work
		/// </summary>
		private void Run()
		{
			EnvironmentManager.Instance.Log(false, "EventTracer background thread", "Now starting...", null);
			while (!_stop)
			{
				// Do some work here.

				Thread.Sleep(2000);
			}
			EnvironmentManager.Instance.Log(false, "EventTracer background thread", "Now stopping...", null);
			_thread = null;
		}
	}
}
