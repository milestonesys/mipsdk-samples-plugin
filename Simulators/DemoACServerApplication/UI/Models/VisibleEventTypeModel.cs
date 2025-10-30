using DemoServerApplication.ACSystem;
using System;
using System.ComponentModel;

namespace DemoServerApplication.UI.Models
{
    /// <summary>
    /// The MVVM Model for event type visible to the user.
    /// </summary>
    public class VisibleEventTypeModel : INotifyPropertyChanged
    {
        private EventType _eventType;
        private bool _isVisible;

        public VisibleEventTypeModel(EventType eventType, bool isVisible)
        {
            _eventType = eventType;
            _isVisible = isVisible;
        }

        public Guid EventTypeId { get { return _eventType.Id; } }

        public string EventTypeName { get { return _eventType.Name; } }

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
