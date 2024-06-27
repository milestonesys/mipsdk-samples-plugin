using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Platform.DriverFramework.Data.Settings;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace DemoDriver
{
    public class DemoOutputManager : OutputManager
    {
        private readonly HashSet<TriggerTimerMap> _triggerTimers = new HashSet<TriggerTimerMap>();
        private bool _activated = false;

        private new DemoContainer Container => base.Container as DemoContainer;

        public DemoOutputManager(DemoContainer demoContainer) : base(demoContainer)
        {
        }

        public override bool? IsActivated(string deviceId)
        {
            if (new Guid(deviceId) == Constants.Output1)
            {
                return _activated; // in real driver this should be queried from the device
            }
            throw new MIPDriverException("Device does not support Output commands");
        }

        public override void TriggerOutput(string deviceId, int durationMs)
        {
            if (durationMs == 0)
            {
                var setting = Container.SettingsManager.GetSetting(new DeviceSetting(Setting.OutputTriggerTime, deviceId, "0"));
                if (setting != null && int.TryParse(setting.Value, out var duration))
                {
                    durationMs = duration;
                }
            }
            WriteLine(deviceId, string.Format("{0}, ms= {1}", nameof(TriggerOutput), durationMs));
            if (new Guid(deviceId) == Constants.Output1)
            {
                Container.EventManager.NewEvent(deviceId, EventId.OutputActivated);
                _activated = true;
                TriggerTimerMap map = new TriggerTimerMap()
                {
                    DeviceId = deviceId
                };
                map.TriggerTimer = new Timer(TimerCallback, map, durationMs, Timeout.Infinite);
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
                _activated = true;
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
                _activated = false;
                return;
            }
            throw new MIPDriverException("Device does not support Output commands");
        }

        private void TimerCallback(object state)
        {
            TriggerTimerMap map = state as TriggerTimerMap;
            if (map == null) throw new Exception("Map state unknown");

            Container.EventManager.NewEvent(map.DeviceId, EventId.OutputDeactivated);
            _activated = false;
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
