using DemoDriver.Utility;
using System;
using System.Collections.Generic;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;


namespace DemoDriver
{
    /// <summary>
    /// This manager provides data to the VMS for remote retrieval and remote playback.
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

        /// <summary>
        /// Creates a playback session for a device. Note, the recording server can create and destroy sessions for any number of reasons, do not assume
        /// a session will exist for the duration of a playback sequence.
        /// </summary>
        /// <param name="deviceId">The device to start a playback session for.</param>
        /// <returns>A new session id for the created playback session.</returns>
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

        /// <summary>
        /// Destroys a playback session.
        /// </summary>
        /// <param name="playbackId">The playback session id of the session to be destroyed.</param>
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

        /// <summary>
        /// Called to get a list of available sequences in a time window. Called by recording server to provide data to clients when device is set to 
        /// play back recordings directly from the device.
        /// </summary>
        /// <param name="playbackId">Id of the current playback session</param>
        /// <param name="sequenceType">Whether to search for recordings or motion</param>
        /// <param name="dateTime">The central point of the playback window</param>
        /// <param name="maxTimeBefore">Sequences wholly outside this time should not be included.</param>
        /// <param name="maxCountBefore">Maximum number of preceding sequences to include.</param>
        /// <param name="maxTimeAfter">Sequences wholly outside this time should not be included.</param>
        /// <param name="maxCountAfter">Maximum number of following sequences to include.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Moves the playback cursor to a specific time. Should attempt to hit an actual frame time.
        /// </summary>
        /// <param name="playbackId">The id of the playback session</param>
        /// <param name="dateTime">The datetime to move to</param>
        /// <param name="moveCriteria">Search criteria specifying how to find the target time:
        /// Before/After: Will look for the first frame before or after, but not at, the initial datetime
        /// AtOrBefore/AtOrAfter: Will include a frame that is exactly at the specified time.
        /// </param>
        /// <returns></returns>
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

        /// <summary>
        /// Moves the playback cursor in a specified direction. Behavior is undefined if a MoveTo has not been called for this playback session.
        /// </summary>
        /// <param name="playbackId">The id of the playback session</param>
        /// <param name="navigateCriteria">Specifies where to navigate:
        /// First: Moves to the first frame of the first sequence contained on the device.
        /// Last: Moves to the end of the last sequence contained on the device.
        /// Previous: Moves to the previous frame (if JPEG) or GOP (if H.264/H.265). If at the start of a sequence, move to the end of the previous sequence.
        /// Next: Moves to the next frame (if JPEG) or GOP (if H.264/H.265). If at the end of a sequence, move to the start of the next sequence.
        /// PreviousSequence: Moves to the end of the previous sequence.
        /// NextSequence: Moves to the start of the next sequence.
        /// </param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieves the frame (for JPEG) or GOP (for H.264/H.265) at the playback cursor
        /// </summary>
        /// <param name="playbackId">The id of the playback session</param>
        /// <returns>A response containing the frame or GOP data at the current time of the playback cursor.</returns>
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
                // Unlike in live streaming, frames for H.264 and H.265 are returned a full GOP at a time in playback mode and when retrieving data
                // from remote device.
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
                    Frames = new[] { frame }, //Note, this array should contain all frames in the GOP for H.264/H.265
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
