using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoServerApplication.Configuration
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime LastChanged { get; set; }
        public Guid[] VisibleDoors { get; set; }
        public Guid[] VisibleEventTypes { get; set; }
        public bool LockCommandVisible { get; set; }
        public bool UnlockCommandVisible { get; set; }
        public bool IsAdministrator { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            VisibleDoors = new Guid[] { };
            VisibleEventTypes = new Guid[] { };
        }

        public User(Guid id, string name, string password, DateTime lastChanged, Guid[] visibleDoors, Guid[] visibleEventTypes, bool lockCommandVisible, bool unlockCommandVisible, bool isAdministrator) : base()
        {
            this.Id = id;
            this.Name = name;
            this.Password = password;
            this.LastChanged = lastChanged;
            this.VisibleDoors = new List<Guid>(visibleDoors).ToArray();
            this.VisibleEventTypes = new List<Guid>(visibleEventTypes).ToArray();
            this.LockCommandVisible = lockCommandVisible;
            this.UnlockCommandVisible = unlockCommandVisible;
            this.IsAdministrator = isAdministrator;
        }

        public User Clone()
        {
            return new User(Id, Name, Password, LastChanged, VisibleDoors, VisibleEventTypes, LockCommandVisible, UnlockCommandVisible, IsAdministrator);
        }

        public override bool Equals(object obj)
        {
            User compareUser = obj as User;
            if (compareUser != null)
            {
                return compareUser.Id.Equals(Id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();               
        }

        public bool IsDoorVisible(Guid doorId)
        {
            if (IsAdministrator)
                return true;

            return VisibleDoors.Contains(doorId);
        }

        public bool IsEventTypeVisible(Guid eventTypeId)
        {
            if (IsAdministrator)
                return true;

            return VisibleEventTypes.Contains(eventTypeId);
        }

        public bool IsUnlockCommandVisible()
        {
            if (IsAdministrator)
                return true;

            return UnlockCommandVisible;
        }

        public bool IsLockCommandVisible()
        {
            if (IsAdministrator)
                return true;

            return LockCommandVisible;
        }
    }
}