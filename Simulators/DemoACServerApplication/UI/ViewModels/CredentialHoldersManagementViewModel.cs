using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using DemoServerApplication.UI.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for credential holder management option.
    /// </summary>
    public class CredentialHoldersManagementViewModel : OptionViewModelBase
    {
        private ObservableCollection<CredentialHolderViewModel> _credentialHolders = new ObservableCollection<CredentialHolderViewModel>();
        private CredentialHolderViewModel _selectedCredentialHolderViewModel;

        private ConfigurationManager _configurationManager = ConfigurationManager.Instance;

        public CredentialHoldersManagementViewModel(string optionName)
            : base(optionName)
        {
            foreach (var credentialHolder in ConfigurationManager.Instance.CredentialHolders)
                _credentialHolders.Add(new CredentialHolderViewModel(credentialHolder));

            AddCredentialHolderCommand = new DelegateCommand(_ => AddCredentialHolder());
            AddMultipleCredentialHoldersCommand = new DelegateCommand(_ => AddMultipleCredentialHolders());
            DeleteCredentialHolderCommand = new DelegateCommand(_ => DeleteCredentialHolder());
            EditCredentialHolderCommand = new DelegateCommand(_ => EditCredentialHolder());

            _configurationManager.CredentialHoldersChanged += Instance_CredentialHoldersChanged;
        }

        public ObservableCollection<CredentialHolderViewModel> CredentialHolders { get { return _credentialHolders; } }

        public CredentialHolderViewModel SelectedCredentialHolder 
        {
            get { return _selectedCredentialHolderViewModel; }
            set
            {
                if (_selectedCredentialHolderViewModel != value)
                {
                    _selectedCredentialHolderViewModel = value;
                    OnPropertyChanged("SelectedCredentialHolder");
                }
            }
        }

        public DelegateCommand AddCredentialHolderCommand { get; private set; }

        public DelegateCommand AddMultipleCredentialHoldersCommand { get; private set; }

        public DelegateCommand DeleteCredentialHolderCommand { get; private set; }

        public DelegateCommand EditCredentialHolderCommand { get; private set; }
        
        private void EditCredentialHolder()
        {
            if (_selectedCredentialHolderViewModel != null)
            {
                var editCredentialHolderWindow = new EditCredentialHolderWindow(_selectedCredentialHolderViewModel);
                editCredentialHolderWindow.ShowDialog();
            }
            else MessageBox.Show("You must first select an credential holder.");
        }

        private void DeleteCredentialHolder()
        {
            if (_selectedCredentialHolderViewModel != null)
                _configurationManager.DeleteCredentialHolder(_selectedCredentialHolderViewModel.CredentialHolder.Id);
            else MessageBox.Show("You must first select an credential holder.");
        }

        private void AddMultipleCredentialHolders()
        {
            var addMultipleCredentialHoldersWindow = new AddMultipleCredentialHoldersWindow();
            addMultipleCredentialHoldersWindow.ShowDialog();
        }

        private void AddCredentialHolder()
        {
            var addCredentialHolderWindow = new AddCredentialHolderWindow();
            addCredentialHolderWindow.ShowDialog();
        }

        void Instance_CredentialHoldersChanged(object sender, EventArgs e)
        {
            _credentialHolders = new ObservableCollection<CredentialHolderViewModel>();
            foreach (var credentialHolder in _configurationManager.CredentialHolders)
                _credentialHolders.Add(new CredentialHolderViewModel(credentialHolder));
            OnPropertyChanged("CredentialHolders");
        }
    }
}
