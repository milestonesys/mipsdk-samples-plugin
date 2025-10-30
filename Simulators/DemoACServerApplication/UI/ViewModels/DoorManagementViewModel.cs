using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using DemoServerApplication.UI.Views;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for door management option.
    /// </summary>
    public class DoorManagementViewModel : OptionViewModelBase
    {
        private ObservableCollection<DoorViewModel> _doors = new ObservableCollection<DoorViewModel>();
        private IList _selectedDoorsViewModels;

        private ConfigurationManager _configurationManager = ConfigurationManager.Instance;

        public DoorManagementViewModel(string optionName)
            : base(optionName)
        {
            foreach (var door in _configurationManager.Doors)
                _doors.Add(new DoorViewModel(door));

            AddDoorCommand = new DelegateCommand(_ => AddDoor());
            AddMultipleDoorsCommand = new DelegateCommand(_ => AddMultipleDoors());
            DeleteDoorCommand = new DelegateCommand(_ => DeleteDoors());
            EditDoorCommand = new DelegateCommand(_ => EditDoor());
            OpenDoorControllersOptionsCommand = new DelegateCommand(_ => OpenDoorControllersOptions());

            _configurationManager.DoorsChanged += Instance_RefreshGrid;
            _configurationManager.DoorControllersChanged += Instance_RefreshGrid;
            _configurationManager.DoorPositionChanged += Instance_RefreshGrid;
        }

        public ObservableCollection<DoorViewModel> Doors { get { return _doors; } }

        public IList SelectedDoors 
        {
            get { return _selectedDoorsViewModels; }
            set
            { 
                _selectedDoorsViewModels = value;
                OnPropertyChanged("SelectedDoors");
            }
        }

        public DelegateCommand AddDoorCommand { get; private set; }

        public DelegateCommand AddMultipleDoorsCommand { get; private set; }

        public DelegateCommand DeleteDoorCommand { get; private set; }

        public DelegateCommand EditDoorCommand { get; private set; }
        
        public DelegateCommand OpenDoorControllersOptionsCommand { get; private set; }

        private void AddDoor()
        {
            var addDoorWindow = new AddDoorWindow();
            addDoorWindow.ShowDialog();
        }

        private void AddMultipleDoors()
        {
            var addMultipleDoorsWindow = new AddMultipleDoorsWindow();
            addMultipleDoorsWindow.ShowDialog();
        }

        private void DeleteDoors()
        {
            if (_selectedDoorsViewModels == null || _selectedDoorsViewModels.Count == 0)
            {
                MessageBox.Show("You must first select an item.");
            }
            else
            {
                foreach (var doorViewModel in _selectedDoorsViewModels.OfType<DoorViewModel>().ToList())
                {
                    _configurationManager.DeleteDoor(doorViewModel.Door.Id);
                }

                _configurationManager.NotifyDoorsChanged();
            }
        }

        private void EditDoor()
        {
            if (_selectedDoorsViewModels == null || _selectedDoorsViewModels.Count == 0)
            {
                MessageBox.Show("You must first select an item.");
            }
            else if (_selectedDoorsViewModels.Count > 1)
            {
                MessageBox.Show("Select a single item to edit.");
            }
            else
            {
                var editDoorWindow = new EditDoorWindow(_selectedDoorsViewModels.OfType<DoorViewModel>().FirstOrDefault());
                editDoorWindow.ShowDialog();
            }
        }

        private void OpenDoorControllersOptions()
        {
            var doorControllersOptionsWindow = new DoorControllersOptionsWindow();
            doorControllersOptionsWindow.ShowDialog();
        }

        private void RefreshGrid()
        {
            _doors = new ObservableCollection<DoorViewModel>();
            foreach (var door in _configurationManager.Doors)
                _doors.Add(new DoorViewModel(door));
            OnPropertyChanged("Doors");
        }

        private void Instance_RefreshGrid(object sender, EventArgs e)
        {
            RefreshGrid();
        }
    }
}
