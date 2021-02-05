using DemoDriverDevice;
using System;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;

namespace DemoDriver
{
    internal class DemoMicrophoneStreamSession : BaseDemoStreamSession
    {

        private AudioHeader _currentAudioHeader = new AudioHeader
        {
            SequenceNumber = 0,
            CodecType = AudioCodecType.PCM,
            CodecSubtype = 0,
            Timestamp = DateTime.Now,
            ChannelCount = 1,
            BitsPerSample = 16,
            Frequency = 16000,
            SampleCount = 0
        };

       
       public DemoMicrophoneStreamSession(ISettingsManager settingsManager, DemoConnectionManager demoConnectionManager, Guid sessionId, string deviceId, Guid streamId) :
            base(settingsManager, demoConnectionManager, sessionId, deviceId, DemoDeviceConstants.DeviceAudioChannel, streamId)
        {
        }

        protected override bool GetLiveFrameInternal(TimeSpan timeout, out BaseDataHeader header, out byte[] data)
        {
            header = null;
            data = null;
            byte[] frame = _demoConnectionManager.GetLiveFrame(Channel, false);
            if (frame == null)
            {
                return false;
            }
            _currentAudioHeader.Length = (ulong)frame.Length;
            _currentAudioHeader.Timestamp = DateTime.UtcNow;
            _currentAudioHeader.SequenceNumber++;
            _currentAudioHeader.SampleCount = frame.Length / 2; // Assume 16 bits per sample
            header = _currentAudioHeader.Clone();
            data = frame;
            return true;
        }

    }
}
