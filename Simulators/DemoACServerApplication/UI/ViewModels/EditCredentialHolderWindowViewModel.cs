using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using Microsoft.Win32;
using System;
using System.IO;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for EditCredentialHolderWindow.
    /// </summary>
    public sealed class EditCredentialHolderWindowViewModel : ViewModelBase
    {
        private CredentialHolder _credentialHolder;

        public EditCredentialHolderWindowViewModel(CredentialHolderViewModel credentialHolderViewModel)
        {
            _credentialHolder = credentialHolderViewModel.CredentialHolder;

            OKCommand = new DelegateCommand(_ => this.OK());
            CancelCommand = new DelegateCommand(_ => this.Close());
            SelectImageCommand = new DelegateCommand(_ => SelectImage());
        }

        public string CredentialHolderName
        {
            get { return _credentialHolder.Name; }
            set
            {
                if (_credentialHolder.Name != value)
                {
                    _credentialHolder.Name = value;
                    OnPropertyChanged("CredentialHolderName");
                }
            }
        }

        public string CredentialHolderPassword
        {
            get { return _credentialHolder.Password; }
            set
            {
                if (_credentialHolder.Password != value)
                {
                    _credentialHolder.Password = value;
                    OnPropertyChanged("CredentialHolderPassword");
                }
            }
        }

        public byte[] CredentialHolderPictureBytes
        {
            get { return _credentialHolder.PictureBytes; }
            private set
            {
                if (_credentialHolder.PictureBytes != value)
                {
                    _credentialHolder.PictureBytes = value;
                    OnPropertyChanged("CredentialHolderPictureBytes");
                }
            }
        }

        public DelegateCommand SelectImageCommand { get; private set; }

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
            ConfigurationManager.Instance.UpdateCredentialHolder(_credentialHolder);
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
