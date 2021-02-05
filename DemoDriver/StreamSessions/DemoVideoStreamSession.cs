using DemoDriverDevice;
using System;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;

namespace DemoDriver
{
    internal class DemoVideoStreamSession : BaseDemoStreamSession
    {

        public DemoVideoStreamSession(ISettingsManager settingsManager, DemoConnectionManager demoConnectionManager, Guid sessionId, string deviceId, int channelId, Guid streamId) : 
            base(settingsManager, demoConnectionManager, sessionId, deviceId, channelId, streamId)
        {
        }

        protected override bool GetLiveFrameInternal(TimeSpan timeout, out BaseDataHeader header, out byte[] data)
        {
            header = null;
            data = _demoConnectionManager.GetLiveFrame(Channel, false);
            if (data.Length == 0)
            {
                return false;
            }
            DateTime dt = DateTime.UtcNow;
            var codec = VideoCodecType.JPEG;

            // If you wanted to support multiple codecs, the commented code below demonstrates how this could be done (and also how to get stream settings for any other purpose)
            // var setting = _settingsManager.GetSetting(new StreamSetting(Constants.Codec, _deviceId, _streamId, ""));
            // if (setting.Value == VideoCodecType.H264.ToString())
            // {
            //     codec = VideoCodecType.H264;
            // }
            // For video codec types other than JPEG, an important thing to note here is that we always transfer the data out of the GetLiveFrame
            // call in single frame chunks. That is to say, regardless of what kind of frame it might be (P-frame, I-frame, etc.) it is sent as
            // one frame per message.
            header = new VideoHeader()
            {
                CodecType = codec,
                Length = (ulong)data.Length,
                SequenceNumber = _sequence++,
                SyncFrame = true, // For codecs other than MJPEG, this should only be true for key frames.
                TimestampSync = dt, // If the video codec is e.g. H.264, this should be the time stamp of the most recent keyframe. On a keyframe, this will be the same as TimestampFrame.
                TimestampFrame = dt
            };
            return true;
        }
    }
}
