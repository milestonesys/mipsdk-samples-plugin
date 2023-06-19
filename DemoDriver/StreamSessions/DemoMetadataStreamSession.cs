using DemoDriverDevice;
using System;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Data.Settings;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.Managers;


namespace DemoDriver
{
    internal class DemoMetadataStreamSession : BaseDemoStreamSession
    {

        public DemoMetadataStreamSession(ISettingsManager settingsManager, DemoConnectionManager demoConnectionManager, Guid sessionId, string deviceId, Guid streamId, int channel) : 
            base(settingsManager, demoConnectionManager, sessionId, deviceId, channel, streamId)
        {
        }

        protected override bool GetLiveFrameInternal(TimeSpan timeout, out BaseDataHeader header, out byte[] data)
        {
            bool colored = true;
            data = null;
            header = null;
            MetadataSetting md = _settingsManager.GetSetting(new MetadataSetting(Constants.BoundingBoxColor, _deviceId,
                                 _streamId, MetadataType.BoundingBoxTypeId, ""));
            if (md != null)
            {
                colored = md.Value == "C";
            }
            data = _demoConnectionManager.GetLiveFrame(Channel, colored);
            if (data == null || data.Length == 0)
            {
                return false;
            }
            DateTime dt = DateTime.UtcNow;
            header = new MetadataHeader()
            {
                Length = (ulong)data.Length,
                SequenceNumber = _sequence++,
                Timestamp = dt
            };
            return true;
        }
    }
}
