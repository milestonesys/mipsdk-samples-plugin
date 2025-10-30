using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel base class.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected bool SetProperty<TProperty>(ref TProperty storageField, TProperty value, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(storageField, value)) return false;

            storageField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
