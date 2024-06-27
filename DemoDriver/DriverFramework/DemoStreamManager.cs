using DemoDriverDevice;
using System;
using System.Linq;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace DemoDriver
{
    public class DemoStreamManager : SessionEnabledStreamManager
    {
        private new DemoContainer Container => base.Container as DemoContainer;

        public DemoStreamManager(DemoContainer demoContainer) : base(demoContainer)
        {
        }

        public override GetLiveFrameResult GetLiveFrame(Guid sessionId, TimeSpan timeout)
        {
            try
            {
                return base.GetLiveFrame(sessionId, timeout);
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                Toolbox.Log.Trace("DemoDriver.StreamManager.GetLiveFrame: Exception={0}", ex.Message);
                return GetLiveFrameResult.ErrorResult(StreamLiveStatus.NoConnection);
            }
        }

        internal BaseStreamSession GetSession(int channel)
        {
            return GetAllSessions().OfType<BaseDemoStreamSession>()
                .FirstOrDefault(s => s.Channel == channel);
        }

        protected override BaseStreamSession CreateSession(string deviceId, Guid streamId, Guid sessionId)
        {
            Guid dev = new Guid(deviceId);
            if (dev == Constants.Camera1)
            {
                return new DemoVideoStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, DemoDeviceConstants.DeviceVideoChannel1, streamId);
            }
            if (dev == Constants.Camera2)
            {
                return new DemoVideoStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, DemoDeviceConstants.DeviceVideoChannel2, streamId);
            }
            if (dev == Constants.Speaker1)
            {
                return new DemoSpeakerStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId);
            }
            if (dev == Constants.Audio1)
            {
                return new DemoMicrophoneStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId);
            }
            if (dev == Constants.Metadata1)
            {
                return new DemoMetadataStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId, DemoDeviceConstants.DeviceMetadataChannel1);
            }
            if (dev == Constants.Metadata2)
            {
                return new DemoMetadataStreamSession(Container.SettingsManager, Container.ConnectionManager, sessionId, deviceId, streamId, DemoDeviceConstants.DeviceMetadataChannel2);
            }

            Toolbox.Log.LogError("This device ID: {0} is not supported", deviceId);
            throw new MIPDriverException();
        }
    }
}
