using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoDriverDevice
{
    public class AudioProvider
    {
        private Guid _instance;
        private Dictionary<string, string> _parameters;
        private DateTime _lastFrame;
        private TimeSpan _minimumInterval;
        private const int FirstPackageSize = 7100;
        private const int Frequency = 16000;

        public Guid StartAudioSession(Dictionary<string, string> parameters)
        {
            _instance = Guid.NewGuid();
            _parameters = parameters;
            _minimumInterval = TimeSpan.FromMilliseconds(200);
            _lastFrame = DateTime.MinValue;

            return _instance;
        }

        public byte[] GetAudio()
        {
            // the below is not ideal as it will give slightly stuttering audio, but at least it ensure constant sound being played
            var now = DateTime.UtcNow;
            TimeSpan ts = (_lastFrame != DateTime.MinValue) ? now - _lastFrame : _minimumInterval;
            if (ts < _minimumInterval)
            {
                return null;
            }
            _lastFrame = now;
            var sampleCount = ts.Milliseconds*Frequency/1000;
            byte[] data = new byte[sampleCount*2];
            for (int ix=0; ix<sampleCount; ix++)
            {
                int sample = (int)( Math.Sin(ix*0.2) * (sampleCount*2));
                data[ix * 2] = (byte)(sample & 0xFF);             // Little endian
                data[ix * 2 + 1] = (byte)(sample >> 8);
            }
            return data;
        }
    }
}
