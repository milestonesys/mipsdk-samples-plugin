using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using Microsoft.Win32;
using System;
using System.IO;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for AddCredentialHolderWindow.
    /// </summary>
    public sealed class AddCredentialHolderWindowViewModel : AddingCredentialHoldersViewModelBase
    {
        private string _credentialHolderName;
        private string _credentialHolderPassword;
        private byte[] _credentialHolderPictureBytes;

        public AddCredentialHolderWindowViewModel()
            : base()
        {
            SelectImageCommand = new DelegateCommand(_ => SelectImage());
            CredentialHolderName = ConfigurationManager.Instance.GenerateCredentialHolderName("Credential Holder ");
        }

        public string CredentialHolderName 
        {
            get { return _credentialHolderName; }
            set
            {
                if (_credentialHolderName != value)
                {
                    _credentialHolderName = value;
                    OnPropertyChanged("CredentialHolderName");
                }
            }
        }

        public string CredentialHolderPassword 
        {
            get { return _credentialHolderPassword; }
            set
            {
                if (_credentialHolderPassword != value)
                {
                    _credentialHolderPassword = value;
                    OnPropertyChanged("CredentialHolderPassword");
                }
            }
        }

        public byte[] CredentialHolderPictureBytes
        {
            get { return _credentialHolderPictureBytes; }
            private set
            {
                if (_credentialHolderPictureBytes != value)
                {
                    _credentialHolderPictureBytes = value;
                    OnPropertyChanged("CredentialHolderPictureBytes");
                }
            }
        }

        public DelegateCommand SelectImageCommand { get; private set; }

        protected override void OK()
        {
            var credentialHolder = CredentialHolder.CreateTemplate();
            credentialHolder.Name = CredentialHolderName;
            credentialHolder.Password = CredentialHolderPassword;
            credentialHolder.PictureBytes = CredentialHolderPictureBytes;

            ConfigurationManager.Instance.AddCredentialHolder(credentialHolder);

            Close();
        }

        private void SelectImage()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog().GetValueOrDefault())
                CredentialHolderPictureBytes = File.ReadAllBytes(openFileDialog.FileName);
        }
    }
}
