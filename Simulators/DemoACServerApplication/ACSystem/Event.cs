using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoServerApplication.ACSystem
{
    public class EventType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public EventManager.EventSourceTypes SourceType { get; set; }

        public EventType Clone()
        {
            return new EventType() { Id = Id, Name = Name, SourceType = SourceType };
        }

        public override bool Equals(object obj)
        {
            EventType compareObject = obj as EventType;
            if (compareObject != null)
            {
                return compareObject.Id.Equals(Id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
