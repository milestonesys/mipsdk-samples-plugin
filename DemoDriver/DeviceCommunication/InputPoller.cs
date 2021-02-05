using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;
using DemoDriverDevice;

namespace DemoDriver
{
    public class InputPoller
    {
        private IEventManager _eventManager;
        private bool _shuttingDown = false;
        private Lazy<Thread> _listenerThread;
        private readonly IDictionary<string, Action<string>> _eventHandlers = new Dictionary<string, Action<string>>();
        private readonly DemoConnectionManager _demoConnectionManager;

        public InputPoller(IEventManager eventManager, DemoConnectionManager demoConnectionManager)
        {
            _demoConnectionManager = demoConnectionManager;
            _eventManager = eventManager;
            InitEventHandlers();
        }

        public void Start()
        {
            Toolbox.Log.Trace("Input poller starting");

            _listenerThread = new Lazy<Thread>(() => new Thread(CommunicationThreadMainLoop)
            {
                Name = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Demo driver listener thread for input events"),
            }, LazyThreadSafetyMode.ExecutionAndPublication);

            _listenerThread.Value.Start();
        }

        public void Stop()
        {
            Toolbox.Log.Trace("Input poller stopping");
            _shuttingDown = true;
        }

        private void CommunicationThreadMainLoop()
        {
            while (!_shuttingDown)
            {
                try
                {
                    string[] events = _demoConnectionManager.GetEvents(0);
                    if (events != null && events.Any())
                    {
                        ProcessEvents(events);

                    }
                    Thread.Sleep(250);      // We check every 250 ms
                }
                catch (Exception e)
                {
                    Toolbox.Log.LogError("Input poller", e.StackTrace);
                    Thread.Sleep(3000);
                }
            }
        }

        private void InitEventHandlers()
        {
            _eventHandlers.Clear();
            _eventHandlers.Add(DemoDeviceConstants.HardwareEventOutOfCpuOrMemory, eventId =>
            {
                Toolbox.Log.Trace("Input poller: Event for CPUorMemory issue");
                _eventManager.NewEvent(Constants.Input2.ToString(), Constants.ResourceIssueReferenceId);
            });
            _eventHandlers.Add(DemoDeviceConstants.HardwareEventReboot, eventId =>
            {
                Toolbox.Log.Trace("Input poller: About to Reboot");
                _eventManager.NewEvent(Constants.Input2.ToString(), Constants.RebootReferenceId);
            });
            _eventHandlers.Add(DemoDeviceConstants.DeviceEventActivateInput1, eventId =>
            {
                _eventManager.NewEvent(Constants.Input1.ToString(), EventId.InputActivated);
            });
            _eventHandlers.Add(DemoDeviceConstants.DeviceEventDeactivateInput1, eventId =>
            {
                _eventManager.NewEvent(Constants.Input1.ToString(), EventId.InputDeactivated);
            });
            _eventHandlers.Add(DemoDeviceConstants.DeviceEventActivateInput2, eventId =>
            {
                _eventManager.NewEvent(Constants.Input2.ToString(), EventId.InputActivated);
            });
            _eventHandlers.Add(DemoDeviceConstants.DeviceEventDeactivateInput2, eventId =>
            {
                _eventManager.NewEvent(Constants.Input2.ToString(), EventId.InputDeactivated);
            });
            // please note that the demo driver and application only triggers events from camera 1
            _eventHandlers.Add(DemoDeviceConstants.DeviceEventMotionDetectionStart, eventId =>
            {
                Toolbox.Log.Trace("Device event. MotionDetectStart(HW)");
                _eventManager.NewEvent(Constants.Camera1.ToString(), EventId.MotionStartedDriver);
            });
            _eventHandlers.Add(DemoDeviceConstants.DeviceEventMotionDetectionStop, eventId =>
            {
                Toolbox.Log.Trace("Device event. MotionDetectStop(HW)");
                _eventManager.NewEvent(Constants.Camera1.ToString(), EventId.MotionStoppedDriver);
            });
            _eventHandlers.Add(DemoDeviceConstants.DeviceEventLPR, eventId =>
            {
                string lpr = eventId.Length > 4 ? eventId.Substring(4) : "";
                Toolbox.Log.Trace(string.Format("Device Event. Analytics event with LPR: {0}", lpr));
                var eventData = new Dictionary<string, string>();
                eventData["LPR"] = lpr;
                _eventManager.NewEvent(Constants.Camera1.ToString(), Constants.AnalyticsEventReferenceId, eventData);
            });
        }

        private void ProcessEvents(string[] events)
        {
            foreach (string receivedEvent in events)
            {
                Action<string> action = GetAction(receivedEvent);
                action?.Invoke(receivedEvent);
            }
        }

        private Action<string> GetAction(string receivedEvent)
        {
            Action<string> action;
            if (_eventHandlers.TryGetValue(receivedEvent, out action) || 
               (receivedEvent.StartsWith(DemoDeviceConstants.DeviceEventLPR)
                && _eventHandlers.TryGetValue(DemoDeviceConstants.DeviceEventLPR, out action)))
            {
                return action;
            }
            return null;
        }
    }
}
