
using System;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Utilities;
using VideoOS.Platform.DriverFramework.Managers;

namespace DemoDriver
{
    /// <summary>
    /// Simulate data from device
    /// </summary>
    internal abstract class BaseDemoStreamSession : BaseStreamSession
    {
        public Guid Id { get; }

        public int Channel { get; protected set; }
               
        protected readonly string _deviceId;
        protected readonly Guid _streamId;
               
        protected readonly DemoConnectionManager _demoConnectionManager;
        protected readonly ISettingsManager _settingsManager;

        protected int _sequence = 0;

        protected abstract bool GetLiveFrameInternal(TimeSpan timeout, out BaseDataHeader header, out byte[] data);

        public BaseDemoStreamSession(ISettingsManager settingsManager, DemoConnectionManager demoConnectionManager, Guid sessionId, string deviceId, int channelId, Guid streamId)
        {
            Id = sessionId;
            _settingsManager = settingsManager;
            _demoConnectionManager = demoConnectionManager;
            _deviceId = deviceId;
            _streamId = streamId;
            Channel = channelId;
            try
            {
                _demoConnectionManager.StartLiveStream(Channel);
            }
            catch (Exception ex)
            {
                throw new ConnectionLostException(ex.Message + ex.StackTrace);
            }
        }

        public sealed override bool GetLiveFrame(TimeSpan timeout, out BaseDataHeader header, out byte[] data)
        {
            try
            {
                return GetLiveFrameInternal(timeout, out header, out data);
            }
            catch (Exception ex)
            {
                Toolbox.Log.LogError(GetType().Name, 
                    "{0}, Channel {1}: {2}", nameof(GetLiveFrame),  Channel, ex.Message + ex.StackTrace);
                throw new ConnectionLostException(ex.Message + ex.StackTrace);
            }
        }

        public override void Close()
        {
            try
            {
                _sequence = 0;
                _demoConnectionManager.StopLiveStream(Channel);
            }
            catch (Exception ex)
            {
                Toolbox.Log.LogError(this.GetType().Name, "Error calling Destroy: {0}", ex.Message);
            }
        }
    }
}
