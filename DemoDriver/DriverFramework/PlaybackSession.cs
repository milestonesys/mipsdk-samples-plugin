using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDriver
{
    public class PlaybackSession
    {
        public Guid PlaybackId { get; }
        public DateTime Cursor { get; set; }
        public int SequenceNumber { get; set; }

        public PlaybackSession(Guid playbackId)
        {
            PlaybackId = playbackId;
            Cursor = DateTime.UtcNow;
            SequenceNumber = 0;
        }
    }
}
