using DemoDriver.Utility;
using System;
using System.Collections.Generic;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;


namespace DemoDriver
{
    /// <summary>
    /// In this sample we assume we have video in all odd-minutes and no video in even minutes,
    /// Also sequences follow this as well.
    /// </summary>
    public class DemoPlaybackManager : PlaybackManager
    {
        private readonly object _playbackLockObj = new object();
        private readonly Dictionary<Guid, PlaybackSession> _playbackSessions = new Dictionary<Guid, PlaybackSession>();
        private TimeSpan _frameDistance = TimeSpan.FromMilliseconds(100);

        private new DemoContainer Container => base.Container as DemoContainer;

        public DemoPlaybackManager(DemoContainer container) : base(container)
        {
            base.MaxParallelDevices = 2;
        }

        public override Guid Create(string deviceId)
        {
            if (deviceId != Constants.Camera1.ToString() && deviceId != Constants.Camera2.ToString())
            {
                throw new NotSupportedException();
            }
            lock (_playbackLockObj)
            {
                Guid id = Guid.NewGuid();
                _playbackSessions[id] = new PlaybackSession(id);
                return id;
            }
        }

        public override void Destroy(Guid playbackId)
        {
            lock (_playbackLockObj)
            {
                if (_playbackSessions.ContainsKey(playbackId))
                {
                    _playbackSessions.Remove(playbackId);
                }
            }
        }

        public override ICollection<SequenceEntry> GetSequences(Guid playbackId, SequenceType sequenceType, DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter)
        {
            // Note that current time is NOT updated in this method
            var result = new List<SequenceEntry>();

            dateTime = dateTime.Truncate(_frameDistance.Ticks);

            //Avoids the scenario where a sequence would be missed because we are exactly at it.
            if (IsAtVideo(dateTime) && dateTime.Second == 0)
            {
                result.Add(new SequenceEntry(dateTime, dateTime.AddMinutes(1)));
            }

            DateTime cur = PreviousSequence(dateTime);
            int cnt = 0;
            while (cur >= dateTime - maxTimeBefore && cnt++ < maxCountBefore)
            {
                result.Add(new SequenceEntry(cur, cur + TimeSpan.FromMinutes(1)));
                cur = PreviousSequence(cur);
            }

            cur = NextSequence(dateTime);
            cnt = 0;
            while (cur <= dateTime + maxTimeAfter && cnt++ < maxCountAfter)
            {
                result.Add(new SequenceEntry(cur, cur + TimeSpan.FromMinutes(1)));
                cur = NextSequence(cur);
            }
            return result;
        }

        public override bool MoveTo(Guid playbackId, DateTime dateTime, MoveCriteria moveCriteria)
        {
            lock (_playbackLockObj)
            {
                if (!_playbackSessions.ContainsKey(playbackId))
                {
                    throw new KeyNotFoundException(nameof(playbackId));
                }
            }
            DateTime cur = dateTime;
            bool atVideo = IsAtVideo(cur);
            Toolbox.Log.Trace(nameof(MoveTo) + " - " + moveCriteria + ",  Asked time: " + dateTime.ToString("o") + ", At Video: " + atVideo);

            switch (moveCriteria)
            {
                case MoveCriteria.After:
                    if (atVideo)
                        do
                        {
                            cur = cur + _frameDistance;
                        } while (!IsAtVideo(cur)); //this avoids the edge case where the current time is at video, but +framedistance is not.
                    else
                        cur = NextSequence(cur);
                    break;
                case MoveCriteria.AtOrAfter:
                    if (!atVideo)
                    {
                        cur = NextSequence(cur);
                    }
                    break;
                case MoveCriteria.AtOrBefore:
                    if (!atVideo)
                    {
                        cur = PreviousSequence(cur) + TimeSpan.FromSeconds(60) - _frameDistance;
                    }
                    break;
                case MoveCriteria.Before:
                    if (atVideo)
                    {
                        cur = cur - _frameDistance;
                    }
                    else
                    {
                        cur = PreviousSequence(cur) + TimeSpan.FromSeconds(60) - _frameDistance;
                    }
                    break;
            }
            Toolbox.Log.Trace("{0} - now at:{1}", nameof(MoveTo), cur.ToString("o") );
            lock (_playbackLockObj)
            {
                _playbackSessions[playbackId].Cursor = cur;
            }
            return true;
        }

        public override bool Navigate(Guid playbackId, NavigateCriteria navigateCriteria)
        {
            PlaybackSession session;
            DateTime cur;
            lock (_playbackLockObj)
            {
                if (!_playbackSessions.TryGetValue(playbackId, out session))
                {
                    throw new KeyNotFoundException(nameof(playbackId));
                }
            }
            cur = session.Cursor;
            int sequenceNumber = session.SequenceNumber;
            bool atVideo = IsAtVideo(cur);
            Toolbox.Log.Trace("Navigate - " + navigateCriteria + ",  From:" + cur.ToString("o"));


            switch (navigateCriteria)
            {
                case NavigateCriteria.First:
                    cur = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                    break;
                case NavigateCriteria.Last:
                    cur = DateTime.UtcNow;
                    break;
                case NavigateCriteria.Previous:
                    cur = cur - _frameDistance;
                    if (!IsAtVideo(cur))
                    {
                        cur = PreviousSequence(cur);
                        sequenceNumber -= 1;
                    }
                    break;
                case NavigateCriteria.Next:
                    if (atVideo)
                    {
                        cur = cur + _frameDistance;
                        if(!IsAtVideo(cur))
                        {
                            cur = NextSequence(cur);
                            sequenceNumber += 1;
                        }
                    }
                    else
                    {
                        cur = NextSequence(cur);
                        sequenceNumber += 1;
                    }
                    break;
                case NavigateCriteria.PreviousSequence:
                    cur = PreviousSequence(cur);
                    sequenceNumber -= 1;
                    break;
                case NavigateCriteria.NextSequence:
                    cur = NextSequence(cur);
                    sequenceNumber += 1;
                    break;
            }
            Toolbox.Log.Trace("   --- now at:{0}", cur.ToString("o"));
            lock (_playbackLockObj)
            {
                session.Cursor = cur;
                session.SequenceNumber = sequenceNumber;
            }
            return true;
        }

        public override PlaybackReadResponse ReadData(Guid playbackId)
        {
            PlaybackSession session;
            DateTime cur;
            lock (_playbackLockObj)
            {
                if (!_playbackSessions.TryGetValue(playbackId, out session))
                {
                    throw new KeyNotFoundException(nameof(playbackId));
                }
            }
            cur = session.Cursor;
            bool atVideo = IsAtVideo(cur);
            Toolbox.Log.Trace("{0} at {1}, AtVideo: {2}", nameof(ReadData), cur.ToString("o"), atVideo);

            int channel = 0;
            try
            {
                if (!atVideo)
                {
                    return null;
                }

                byte[] data = Container.ConnectionManager.GetPlaybackFrame(channel, cur);
                if (data == null)
                {
                    Toolbox.Log.Trace("--- No data returned ");
                    return null;
                }
                // Just as is the case for the live stream, information in a remote playback or remote retrieval scenario
                // is transferred to the recorder frame by frame (whether that be a video frame or an audio frame, and in the case of e.g.
                // H.264 or H.265 video, whether it be a P-frame, I-frame, etc.)
                VideoHeader jpegHeader = new VideoHeader();
                jpegHeader.CodecType = VideoCodecType.JPEG;
                jpegHeader.SyncFrame = true; // Only set this to true for key frames.

                PlaybackFrame frame = new PlaybackFrame() { Data = data, Header = jpegHeader, AnyMotion = true };
                jpegHeader.SequenceNumber = 0;
                jpegHeader.Length = (ulong)data.Length;
                jpegHeader.TimestampFrame = cur; // The time stamp of the current frame.
                jpegHeader.TimestampSync = cur; // The time stamp of the most recent keyframe. If the codec does not differentiate between frame types, or if this is a keyframe, set it to the same as TimestampFrame.
                DateTime prev = cur - _frameDistance; // The time stamp of the previous frame.
                DateTime next = cur + _frameDistance; // The time stamp of the next frame.
                if (!IsAtVideo(next))
                {
                    next = NextSequence(next);
                }
                return new PlaybackReadResponse()
                {
                    SequenceNumber = session.SequenceNumber,
                    Next = next,
                    Previous = prev,
                    Frames = new[] { frame },
                };
            }
            catch (Exception e)
            {
                Toolbox.Log.Trace("{0}: Exception={1}", nameof(ReadData), e.Message + e.StackTrace);
                throw;
            }
        }

        private DateTime PreviousSequence(DateTime dt)
        {
            DateTime r = dt - TimeSpan.FromSeconds(1);
            return new DateTime(r.Year, r.Month, r.Day, r.Hour, (r.Minute / 2) * 2, 0, DateTimeKind.Utc).Truncate(_frameDistance.Ticks);
        }

        private DateTime NextSequence(DateTime dt)
        {
            DateTime r = dt + TimeSpan.FromSeconds(120);
            return new DateTime(r.Year, r.Month, r.Day, r.Hour, (r.Minute / 2) * 2, 0, DateTimeKind.Utc).Truncate(_frameDistance.Ticks);
        }

        private static bool IsAtVideo(DateTime cur)
        {
            return cur.Minute % 2 == 0;
        }
    }
}
