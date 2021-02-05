using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SensorMonitor.Background
{
	public class SensorMonitorBackgroundplugin : VideoOS.Platform.Background.BackgroundPlugin
	{
		private Guid _id = new Guid("A9E0CE09-5AF5-49B9-85B6-D36704CBF77D");
        private object _controllerConfigChangedObject;

		public SensorMonitorBackgroundplugin()
		{
			
		}

		public override void Init()
		{
            // You can create new Threads here, if that is required

            _controllerConfigChangedObject = EnvironmentManager.Instance.RegisterReceiver(ControllerConfigChangedHandler, new MessageIdAndRelatedKindFilter(MessageId.Server.ConfigurationChangedIndication, SensorMonitorDefinition.SensorMonitorCtrlKind));
        }

        public override void Close()
		{
            EnvironmentManager.Instance.UnRegisterReceiver(_controllerConfigChangedObject);
		}

		public override List<VideoOS.Platform.EnvironmentType> TargetEnvironments
		{
			get
			{
				return new List<EnvironmentType>(new[] { EnvironmentType.Service });
			}
		}

		public override Guid Id
		{
			get { return _id; }
		}

		public override string Name
		{
			get { return "SensorMonitor background sample"; }
		}

        private object ControllerConfigChangedHandler(Message message, FQID dest, FQID sender)
        {
            // Controller configuration changed - potentially connect/reconnect/disconnect to/from external controller
            EnvironmentManager.Instance.Log(false, "SensorMonitorBackgroundplugin", "Controller configuration changed");
            return null;
        }

    }
}
