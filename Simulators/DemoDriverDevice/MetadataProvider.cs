using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DemoDriverDevice
{
    public class MetadataProvider
    {
        private readonly TimeSpan MetadataDelay = TimeSpan.FromMilliseconds(500);
        private const string MetadataResourceName = "DemoDriverDevice.Resources.Metadata.xml";
        private const string FillColor = "#404040";
        private const string LineColor = "#EE1060F0";
        private const string LineColorBW = "#44808080";

        private readonly string _metadataTemplate;
        private int _boxPhase = 1;
        private Guid _sessionId;
        private DateTime _lastMetadataSentTime;

        public MetadataProvider()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(MetadataResourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                _metadataTemplate = reader.ReadToEnd();
                _metadataTemplate.Replace('\r', ' ').Replace('\n', ' ');
            }
        }

        public Guid StartMetadataSession()
        {
            _lastMetadataSentTime = DateTime.MinValue;
            _sessionId = Guid.NewGuid();
            return _sessionId;
        }

        public byte[] GetMetadata(bool colored)
        {
            if (CheckDelay())
            {
                return new byte[0];
            }

            string color = colored ? LineColor : LineColorBW;
            var gpsRandomizer = new Random();
            string md = string.Format(System.Globalization.CultureInfo.InvariantCulture, _metadataTemplate,
                DateTime.UtcNow.ToString("o"), -0.1 * _boxPhase, 0.1 * _boxPhase, 0.1 * _boxPhase, -0.1 * _boxPhase, FillColor, color,
                10 + gpsRandomizer.NextDouble() * 50, 10 + gpsRandomizer.NextDouble() * 100, 20 + gpsRandomizer.NextDouble() * 20,
                150 + gpsRandomizer.NextDouble() * 10, 30 + gpsRandomizer.NextDouble() * 10);
            _boxPhase++;
            if (_boxPhase > 7)
            {
                _boxPhase = 1;
            }
            return Encoding.ASCII.GetBytes(md);
        }

        private bool CheckDelay()
        {
            var currentSpan = DateTime.UtcNow - _lastMetadataSentTime;
            bool shouldDelay = currentSpan < MetadataDelay;
            if (!shouldDelay)
            {
                _lastMetadataSentTime = DateTime.UtcNow;
            }
            return shouldDelay;
        }
    }
}
