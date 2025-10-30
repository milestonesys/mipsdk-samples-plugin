using System.ComponentModel;

namespace DemoServerApplication.UI.Models
{
    /// <summary>
    /// The MVVM Model for command visible to the user.
    /// </summary>
    public class VisibleCommandModel : INotifyPropertyChanged
    {
        private bool _isVisible;

        public VisibleCommandModel(string commandName, bool isVisible)
        {
            CommandName = commandName;
            _isVisible = isVisible;
        }

        public string CommandName { get; private set; }

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
