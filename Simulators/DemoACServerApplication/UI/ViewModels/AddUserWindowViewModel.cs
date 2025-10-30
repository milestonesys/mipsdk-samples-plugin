using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using System;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for AddUserWindow.
    /// </summary>
    public sealed class AddUserWindowViewModel : ViewModelBase
    {
        private User _user;

        public AddUserWindowViewModel()
        {
            _user = new User();

            OKCommand = new DelegateCommand(_ => this.OK());
            CancelCommand = new DelegateCommand(_ => this.Close());
        }

        public string UserName 
        {
            get { return _user.Name; }
            set
            {
                if (_user.Name != value)
                {
                    _user.Name = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        public string Password 
        {
            get { return _user.Password; }
            set
            {
                if (_user.Password != value)
                {
                    _user.Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public DelegateCommand OKCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }   

        public event EventHandler CancelRequest;
        private void Close()
        {
            EventHandler handler = CancelRequest;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void OK()
        {
            ConfigurationManager.Instance.AddUser(_user);
            Close();
        }
    }
}
