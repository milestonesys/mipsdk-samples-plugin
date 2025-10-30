using DemoServerApplication.Configuration;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for AddMultipleCredentialHoldersWindow.
    /// </summary>
    public sealed class AddMultipleCredentialHoldersWindowViewModel : AddingCredentialHoldersViewModelBase
    {
        private int _credentialHoldersCount;
        private string _credentialHoldersNamePrefix;

        public AddMultipleCredentialHoldersWindowViewModel()
            : base()
        {
            // Setting initial value.
            CredentialHoldersNamePrefix = "Credential Holder ";
            CredentialHoldersCount = 10;
        }

        public int CredentialHoldersCount 
        {
            get { return _credentialHoldersCount; }
            set
            {
                if (_credentialHoldersCount != value)
                {
                    _credentialHoldersCount = value;
                    OnPropertyChanged("CredentialHoldersCount");
                }
            }
        }

        public string CredentialHoldersNamePrefix 
        {
            get { return _credentialHoldersNamePrefix; }
            set
            {
                if (_credentialHoldersNamePrefix != value)
                {
                    _credentialHoldersNamePrefix = value;
                    OnPropertyChanged("CredentialHoldersNamePrefix");
                }
            }
        }

        protected override void OK()
        {
            CredentialHolder _credentialHolder = CredentialHolder.CreateTemplate();

            _credentialHolder.Name = _credentialHoldersNamePrefix;

            ConfigurationManager.Instance.AddCredentialHolders(_credentialHolder, _credentialHoldersCount);

            Close();
        }
    }
}
