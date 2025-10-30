using System;

namespace DemoServerApplication.Configuration
{
    public class DoorController
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DoorController Clone()
        {
            return new DoorController { Id = Id, Name = Name, };
        }

        public override bool Equals(object obj)
        {
            var other = obj as DoorController;
            if (other != null)
            {
                return other.Id.Equals(Id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }  
    }
}
