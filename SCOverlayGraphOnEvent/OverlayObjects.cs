using System;
using VideoOS.Platform;

namespace SCOverlayGraphOnEvent
{
    public class OverlayObject
    {
        public object EventObject { get; set; }
        public DateTime StartTime { get; set; }
        
        public OverlayObject(object eventObject, DateTime startTime)
        {
            EventObject = eventObject;
            StartTime = startTime;
        }
    }
}