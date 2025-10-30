using System;
using System.Collections.Generic;

namespace DemoServerApplication.Configuration
{
    public class Door
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DoorControllerId { get; set; }
        public bool IsAccessAllowed { get; set; }
        public bool HasRexButton { get; set; }
        public bool LockCommandSupported { get; set; }
        public bool UnlockCommandSupported { get; set; }
        public bool IsEnabled { get; set; }
        public int UnlockTimeMilliSecs { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Door Clone()
        {
            Door door = new Door() { Id = Id, Name = Name, DoorControllerId = DoorControllerId, IsAccessAllowed = IsAccessAllowed,
                                     HasRexButton = HasRexButton, LockCommandSupported = LockCommandSupported,
                                     UnlockCommandSupported = UnlockCommandSupported, UnlockTimeMilliSecs = UnlockTimeMilliSecs,
                                     Latitude = Latitude, Longitude = Longitude
            };
            return door;
        }
        public static Door CreateTemplate()
        {
            return new Door()
            {
                Id = Guid.NewGuid(),
                UnlockTimeMilliSecs = 10000,
                IsAccessAllowed = true,
                HasRexButton = true,
                LockCommandSupported = true,
                UnlockCommandSupported = true
            };
        }
        public override bool Equals(object obj)
        {
            Door compareObject = obj as Door;
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
