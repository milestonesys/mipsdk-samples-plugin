using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using DemoServerApplication.UI.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace DemoServerApplication.UI.ViewModels
{
    public class DoorControllersOptionsWindowViewModel : ViewModelBase
    {
        private ObservableCollection<DoorController> _doorControllers = new ObservableCollection<DoorController>();
        private DoorController _selectedDoorController;

        private ConfigurationManager _configurationManager = ConfigurationManager.Instance;

        public DoorControllersOptionsWindowViewModel()
        {
            foreach (var doorControler in ConfigurationManager.Instance.DoorControllers)
                _doorControllers.Add(doorControler);

            SelectedDoorController = _doorControllers.Count > 0 ? _doorControllers[0] : null;

            CreateDoorControllerCommand = new DelegateCommand(_ => this.CreateDoorController());
            DeleteDoorControllerCommand = new DelegateCommand(_ => this.DeleteDoorController());
            EditDoorControllerCommand = new DelegateCommand(_ => this.EditDoorController());
            RemoveUnusedDoorControllersCommand = new DelegateCommand(_ => RemoveUnusedDoorControllers());

            _configurationManager.DoorControllersChanged += Instance_DoorControllersChanged;
        }

        public ObservableCollection<DoorController> DoorControllers { get { return _doorControllers; } }

        public DoorController SelectedDoorController
        {
            get { return _selectedDoorController; }
            set
            {
                if (_selectedDoorController != value)
                {
                    _selectedDoorController = value;
                    OnPropertyChanged("SelectedDoorController");
                }
            }
        }

        public DelegateCommand CreateDoorControllerCommand { get; private set; }

        public DelegateCommand DeleteDoorControllerCommand { get; private set; }

        public DelegateCommand EditDoorControllerCommand { get; private set; }

        public DelegateCommand RemoveUnusedDoorControllersCommand { get; private set; }

        private void EditDoorController()
        {
            if (_selectedDoorController != null)
            {
                var editDoorControllerWindow = new EditDoorControllerWindow(_selectedDoorController);
                editDoorControllerWindow.ShowDialog();
            }
            else MessageBox.Show("You must first select an item.");
        }

        private void DeleteDoorController()
        {
            if (_selectedDoorController != null)
                _configurationManager.DeleteDoorController(_selectedDoorController.Id);
            else MessageBox.Show("You must first select an item.");
        }

        private void CreateDoorController()
        {
            var createDoorControllerWindow = new CreateDoorControllerWindow();
            createDoorControllerWindow.ShowDialog();
        }

        private void RemoveUnusedDoorControllers()
        {
            _configurationManager.RemoveUnusedDoorControllers();
        }       

        private void Instance_DoorControllersChanged(object sender, System.EventArgs e)
        {
            _doorControllers = new ObservableCollection<DoorController>();
            foreach (var doorController in _configurationManager.DoorControllers)
                _doorControllers.Add(doorController);
            OnPropertyChanged("DoorControllers");
        }
    }
}
