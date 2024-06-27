using DemoDriverDevice;
using System;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace DemoDriver
{
    public class DemoSpeakerManager : SpeakerManager
    {
        private Guid _streamId;

        private new DemoContainer Container => base.Container as DemoContainer;

        internal DemoSpeakerManager(DemoContainer container) : base(container)
        {
        }

        public override Guid CreateSpeakerStream(string deviceId)
        {
            _streamId = Guid.NewGuid();
            return _streamId;
        }

        public override SpeakerStreamStatus SendFrame(Guid speakerStreamInstance, AudioHeader audioHeader, byte[] data)
        {
            Toolbox.Log.Trace("Speaker header: {0}", audioHeader);

            DemoSpeakerStreamSession s = Container.StreamManager.GetSession(DemoDeviceConstants.DeviceSpeakerChannel) as DemoSpeakerStreamSession;
            if (s != null)
            {
                s.StoreFrameForLoopback(audioHeader, data);
            }
            
            Container.ConnectionManager.SendSpeakerData(data);
            Container.ConnectionManager.SendInfo(Constants.Speaker1.ToString(), "SpeakerFrame, len=" + data.Length);
            return SpeakerStreamStatus.DataSent;
        }

        public override void Destroy(Guid speakerStreamInstance)
        {
            _streamId = Guid.Empty;
        }
    }
}
