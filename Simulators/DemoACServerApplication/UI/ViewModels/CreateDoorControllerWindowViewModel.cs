using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using System;

namespace DemoServerApplication.UI.ViewModels
{
    public class CreateDoorControllerWindowViewModel : ViewModelBase
    {
        private string _doorControllerName;

        public CreateDoorControllerWindowViewModel()
        {
            DoorControllerName = ConfigurationManager.Instance.GenerateDoorControllerName("Controller ");

            OKCommand = new DelegateCommand(_ => this.OK());
            CancelCommand = new DelegateCommand(_ => this.Close());
        }

        public string DoorControllerName
        {
            get { return _doorControllerName; }
            set
            {
                if (_doorControllerName != value)
                {
                    _doorControllerName = value;
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
            var doorController = new DoorController { Id = Guid.NewGuid(), Name = _doorControllerName };
            ConfigurationManager.Instance.AddDoorController(doorController);
            Close();
        }
    }
}
