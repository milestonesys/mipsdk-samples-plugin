using System;
using System.Collections.ObjectModel;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for Administration tab control.
    /// </summary>
    public class AdministrationTabViewModel : TabViewModelBase
    {
        private OptionViewModelBase _selectedOption;

        public AdministrationTabViewModel(string tabName)
            : base(tabName)
        {
            Options = new ObservableCollection<OptionViewModelBase>();
            Options.Add(new GeneralOptionViewModel("General"));
            Options.Add(new DoorManagementViewModel("Doors"));
            Options.Add(new CredentialHoldersManagementViewModel("Credential Holders"));
            Options.Add(new UsersOptionViewModel("Users"));

            SelectedOption = Options[0];
        }

        public ObservableCollection<OptionViewModelBase> Options { get; private set; }

        public OptionViewModelBase SelectedOption
        {
            get { return _selectedOption; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if(_selectedOption != value)
                {
                    _selectedOption = value;
                    OnPropertyChanged("SelectedOption");
                }
            }
        }
    }
}
