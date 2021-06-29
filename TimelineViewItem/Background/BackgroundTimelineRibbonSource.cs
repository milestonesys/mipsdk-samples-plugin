using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using VideoOS.Platform.Client;
using VideoOS.Platform.Util;

namespace TimelineViewItem.Background
{
    public class BackgroundTimelineRibbonSource : TimelineSequenceSource
    {
        private Guid _sourceId;

        public BackgroundTimelineRibbonSource()
        {
            _sourceId = Guid.NewGuid();
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
                return TimelineSequenceSourceType.Ribbon;
            }
        }

        public override string Title
        {
            get
            {
                return "Background sample data";
            }
        }

        public override Color RibbonContentColor
        {
            get
            {
                return Color.Purple;
            }
        }

        private void GetSomeData(object argument)
        {
            var intervals = argument as IEnumerable<TimeInterval>;
            var results = new List<TimelineSourceQueryResult>();
            foreach(var interval in intervals)
            {
                // we will just fake som data here - in real scenario data should of course come from a real source
                results.Add(new TimelineSourceQueryResult(interval) { Sequences = GetOneResult(interval) });
            }
            OnSequencesRetrieved(results);
        }

        private IEnumerable<TimelineDataArea> GetOneResult(TimeInterval interval)
        {
            var result = new List<TimelineDataArea>();
            var nextBeginTime = new DateTime(interval.StartTime.Year, interval.StartTime.Month, interval.StartTime.Day, interval.StartTime.Hour, interval.StartTime.Minute, 30, DateTimeKind.Utc);
            if (nextBeginTime < interval.StartTime)
                nextBeginTime = nextBeginTime.AddMinutes(1);
            var nextEndTime = nextBeginTime.AddSeconds(10);
            while (nextEndTime < interval.EndTime)
            {
                result.Add(new TimelineDataArea(new TimeInterval(nextBeginTime, nextEndTime)));
                nextBeginTime = nextBeginTime.AddMinutes(1);
                nextEndTime = nextBeginTime.AddSeconds(10);
            }
            return result;
        }

        public override void StartGetSequences(IEnumerable<TimeInterval> intervals)
        {
            var t = new Thread(GetSomeData);
            t.Name = "Timeline background sample data thread";
            t.Start(intervals);
        }
    }
}
