using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading;
using VideoOS.Platform.Client;
using VideoOS.Platform.Util;

namespace TimelineViewItem.Client
{
    public class TimelineMarkerSource : TimelineSequenceSource
    {
        private Image _markerImage;
        private Guid _sourceId;

        public TimelineMarkerSource()
        {
            _sourceId = Guid.NewGuid();

            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.TimelineMarker.png");
            if (pluginStream != null)
                _markerImage = Image.FromStream(pluginStream);
        }

        public override Guid Id
        {
            get
            {
                return _sourceId;
            }
        }

        public override TimelineSequenceSourceType SourceType
        {
            get
            {
                return TimelineSequenceSourceType.Marker;
            }
        }

        public override string Title
        {
            get
            {
                return "Sample marker data";
            }
        }

        public override Image MarkerIcon
        {
            get
            {
                return _markerImage;
            }
        }

        public override System.Windows.Controls.UserControl GetPreviewWpfUserControl(object dataId)
        {
            return new MarkerPreviewUserControl() { Id = (Guid)dataId};
        }

        private void GetSomeData(object argument)
        {
            var intervals = argument as IEnumerable<TimeInterval>;
            var results = new List<TimelineSourceQueryResult>();
            foreach(var interval in intervals)
            {
                // adding some fake data - in a real scenario data should come from a real source
                results.Add(new TimelineSourceQueryResult(interval) { Sequences = GetOneResult(interval) });
            }
            OnSequencesRetrieved(results);
        }

        private IEnumerable<TimelineDataArea> GetOneResult(TimeInterval interval)
        {
            var result = new List<TimelineDataArea>();
            var nextBeginTime = new DateTime(interval.StartTime.Year, interval.StartTime.Month, interval.StartTime.Day, interval.StartTime.Hour, interval.StartTime.Minute, 15, DateTimeKind.Utc);
            if (nextBeginTime < interval.StartTime)
                nextBeginTime = nextBeginTime.AddMinutes(1);
            while (nextBeginTime < interval.EndTime)
            {
                result.Add(new TimelineDataArea(Guid.NewGuid(), new TimeInterval(nextBeginTime, nextBeginTime)));
                nextBeginTime = nextBeginTime.AddMinutes(1);
            }
            return result;
        }

        public override void StartGetSequences(IEnumerable<TimeInterval> intervals)
        {
            var t = new Thread(GetSomeData);
            t.Name = "Timeline sample marker data thread";
            t.Start(intervals);
        }
    }
}
