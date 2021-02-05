using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace DemoDriver
{
    public class DemoOutputManager : OutputManager
    {
        private readonly HashSet<TriggerTimerMap> _triggerTimers = new HashSet<TriggerTimerMap>();

        private new DemoContainer Container => base.Container as DemoContainer;

        public DemoOutputManager(DemoContainer demoContainer) : base(demoContainer)
        {
        }

        public override bool? IsActivated(string deviceId)
        {
            return null;
        }

        public override void TriggerOutput(string deviceId, int durationMs)
        {
            WriteLine(deviceId, string.Format("{0}, ms= {1}", nameof(TriggerOutput), durationMs));
            if (new Guid(deviceId) == Constants.Output1)
            {
                Container.EventManager.NewEvent(deviceId, EventId.OutputActivated);

                TriggerTimerMap map = new TriggerTimerMap()
                {
                    DeviceId = deviceId
                };
                map.TriggerTimer = new Timer(TimerCallback, map, 1000, Timeout.Infinite);
                _triggerTimers.Add(map);
                return;
            }
            throw new MIPDriverException("Device does not support Output commands");
        }

        public override void ActivateOutput(string deviceId)
        {
            WriteLine(deviceId, nameof(ActivateOutput)); 
            if (new Guid(deviceId) == Constants.Output1)
            {
                Container.EventManager.NewEvent(deviceId, EventId.OutputActivated);
                return;
            }
            throw new MIPDriverException("Device does not support Output commands");
        }

        public override void DeactivateOutput(string deviceId)
        {
            WriteLine(deviceId, nameof(DeactivateOutput));
            if (new Guid(deviceId) == Constants.Output1)
            {
                Container.EventManager.NewEvent(deviceId, EventId.OutputDeactivated, null);
                return;
            }
            throw new MIPDriverException("Device does not support Output commands");
        }

        private void TimerCallback(object state)
        {
            TriggerTimerMap map = state as TriggerTimerMap;
            if (map == null) throw new Exception("Map state unknown");

            Container.EventManager.NewEvent(map.DeviceId, EventId.OutputDeactivated);
            _triggerTimers.Remove(map);
            map.TriggerTimer.Dispose();
        }

        private void WriteLine(string deviceId, string info)
        {
            string msg = string.Format("Output command for device {0}: {1}", deviceId, info);
            Toolbox.Log.Trace(msg);
            Container.ConnectionManager.SendInfo(deviceId, msg);
        }
    }

    internal class TriggerTimerMap
    {
        internal Timer TriggerTimer;
        internal string DeviceId;
    }
}
