using DemoServerApplication.Configuration;
using System;
using System.ComponentModel;

namespace DemoServerApplication.UI.Models
{
    /// <summary>
    /// The MVVM Model for door visible to the user.
    /// </summary>
    public class VisibleDoorModel : INotifyPropertyChanged
    {
        private Door _door;
        private bool _isVisible;

        public VisibleDoorModel(Door door, bool isVisible)
        {
            _door = door;
            _isVisible = isVisible;
        }

        public Guid DoorId { get { return _door.Id; } }

        public string DoorName { get { return _door.Name; } }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged("IsVisible");
                }
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
