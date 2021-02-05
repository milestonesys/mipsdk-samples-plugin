using DemoDriverDevice;
using System;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;

namespace DemoDriver
{
    internal class DemoSpeakerStreamSession : BaseDemoStreamSession
    {
        private byte[] _currentSpeakerData = null;
        private AudioHeader _currentSpeakerHeader = null;

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

       
       public DemoSpeakerStreamSession(ISettingsManager settingsManager, DemoConnectionManager demoConnectionManager, Guid sessionId, string deviceId, Guid streamId) :
            base(settingsManager, demoConnectionManager, sessionId, deviceId, DemoDeviceConstants.DeviceSpeakerChannel, streamId)
        {
        }

        protected override bool GetLiveFrameInternal(TimeSpan timeout, out BaseDataHeader header, out byte[] data)
        {
            header = null;
            data = null;
            if (_currentSpeakerData != null && _currentSpeakerHeader != null)
            {
                header = _currentSpeakerHeader.Clone();
                data = _currentSpeakerData;
                _currentSpeakerData = null;
                return true;
            }
            return false;
        }

        public void StoreFrameForLoopback(AudioHeader ah, byte[] data)
        {
            _currentSpeakerHeader = ah;
            _currentSpeakerData = data;
        }
    }
}
