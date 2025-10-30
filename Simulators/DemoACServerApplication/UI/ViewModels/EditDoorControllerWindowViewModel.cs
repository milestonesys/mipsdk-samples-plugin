using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using System;

namespace DemoServerApplication.UI.ViewModels
{
    public class EditDoorControllerWindowViewModel : ViewModelBase
    {
        private DoorController _selectedDoorController;

        public EditDoorControllerWindowViewModel(DoorController selectedDoorController)
        {
            _selectedDoorController = selectedDoorController;

            OKCommand = new DelegateCommand(_ => this.OK());
            CancelCommand = new DelegateCommand(_ => this.Close());
        }

        public string DoorControllerName
        {
            get { return _selectedDoorController.Name; }
            set
            {
                if (_selectedDoorController.Name != value)
                {
                    _selectedDoorController.Name = value;
                    OnPropertyChanged("DoorControllerName");
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
            ConfigurationManager.Instance.UpdateDoorController(_selectedDoorController);
            Close();
        }
    }
}
