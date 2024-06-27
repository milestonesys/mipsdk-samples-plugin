using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCOverlayGraphOnEvent
{
    internal class CachedOverlayObjects
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<OverlayObject> OverlayObjectList { get; set; }
    }
}
