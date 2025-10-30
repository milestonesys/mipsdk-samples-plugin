using DemoServerApplication.Configuration;
using DemoServerApplication.UI.Models;
using DemoServerApplication.UI.ViewModels.Commands;
using DemoServerApplication.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for users option.
    /// </summary>
    public class UsersOptionViewModel : OptionViewModelBase
    {
        private User _selectedUser;
        private ObservableCollection<User> _users;
        private ObservableCollection<VisibleDoorModel> _selectedUserVisibleDoors;
        private ObservableCollection<VisibleEventTypeModel> _selectedUserVisibleEventTypes;
        private ObservableCollection<VisibleCommandModel> _selectedUserVisibleCommandTypes;

        private ConfigurationManager _configurationManager = ConfigurationManager.Instance;

        public UsersOptionViewModel(string optionName)
            : base(optionName)
        {
            _users = new ObservableCollection<User>(_configurationManager.Users);

            AddUserCommand = new DelegateCommand(_ => AddUser());
            DeleteUserCommand = new DelegateCommand(_ => DeleteUser());
            SaveCommand = new DelegateCommand(_ => Save());

            _configurationManager.UsersChanged += Instance_UsersChanged;
        }

        public ObservableCollection<User> Users { get { return _users; } }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value == null ? null : value.Clone();
                    LoadSelectedUserData();
                    OnPropertyChanged("SelectedUser");
                }
            }
        }

        public bool IsAdministrator
        {
            get 
            {
                if (_selectedUser != null)
                    return _selectedUser.IsAdministrator;
                else return false;            
            }
            set
            {
                if (_selectedUser != null)
                {
                    _selectedUser.IsAdministrator = value;
                    LoadSelectedUserVisibleData();
                }

                OnPropertyChanged("IsAdministrator");
            }
        }

        public string UserName
        {
            get 
            {
                if (_selectedUser != null)
                    return _selectedUser.Name;
                else return String.Empty;
            }
            set
            {
                if (_selectedUser != null)
                    _selectedUser.Name = value;

                OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get
            {
                if (_selectedUser != null)
                    return _selectedUser.Password;
                else return String.Empty;
            }
            set
            {
                if (_selectedUser != null)
                    _selectedUser.Password = value;

                OnPropertyChanged("Password");
            }
        }

        public ObservableCollection<VisibleDoorModel> SelectedUserVisibleDoors { get { return _selectedUserVisibleDoors; } }

        public ObservableCollection<VisibleCommandModel> SelectedUserVisibleCommandTypes { get { return _selectedUserVisibleCommandTypes; } }

        public ObservableCollection<VisibleEventTypeModel> SelectedUserVisibleEventTypes { get { return _selectedUserVisibleEventTypes; } }

        public DelegateCommand AddUserCommand { get; private set; }

        public DelegateCommand DeleteUserCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        private void LoadSelectedUserData()
        {
            if (_selectedUser != null)
            {
                IsAdministrator = _selectedUser.IsAdministrator;
                UserName = _selectedUser.Name;
                Password = _selectedUser.Password;
            }
        }

        private void LoadSelectedUserVisibleData()
        {
            if (_selectedUser != null)
            {
                _selectedUserVisibleDoors = new ObservableCollection<VisibleDoorModel>();
                foreach (var door in _configurationManager.Doors)
                    _selectedUserVisibleDoors.Add(new VisibleDoorModel(door, _selectedUser.IsDoorVisible(door.Id)));
                OnPropertyChanged("SelectedUserVisibleDoors");

                _selectedUserVisibleEventTypes = new ObservableCollection<VisibleEventTypeModel>();
                foreach (var eventType in _configurationManager.EventTypes)
                    _selectedUserVisibleEventTypes.Add(new VisibleEventTypeModel(eventType, _selectedUser.IsEventTypeVisible(eventType.Id)));
                OnPropertyChanged("SelectedUserVisibleEventTypes");

                _selectedUserVisibleCommandTypes = new ObservableCollection<VisibleCommandModel>();
                _selectedUserVisibleCommandTypes.Add(new VisibleCommandModel("Lock", _selectedUser.IsLockCommandVisible()));
                _selectedUserVisibleCommandTypes.Add(new VisibleCommandModel("Unlock", _selectedUser.IsUnlockCommandVisible()));
                OnPropertyChanged("SelectedUserVisibleCommandTypes");
            }
        }

        private void RefreshUserData()
        {
            SelectedUser = null;
            IsAdministrator = false;
            UserName = null;
            Password = null;

            _selectedUserVisibleDoors = new ObservableCollection<VisibleDoorModel>();
            OnPropertyChanged("SelectedUserVisibleDoors");
            _selectedUserVisibleEventTypes = new ObservableCollection<VisibleEventTypeModel>();
            OnPropertyChanged("SelectedUserVisibleEventTypes");
            _selectedUserVisibleCommandTypes = new ObservableCollection<VisibleCommandModel>();
            OnPropertyChanged("SelectedUserVisibleCommandTypes");
        }

        private void AddUser()
        {
            var addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog();
        }

        private void DeleteUser()
        {
            if (_selectedUser != null)
            {
                _configurationManager.DeleteUser(_selectedUser.Id);
                RefreshUserData();
            }
            else MessageBox.Show("You must first select user.");
        }

        private void Save()
        {
            if (_selectedUser != null)
            {
                var visibleDoors = new List<Guid>();
                if (!IsAdministrator)
                {
                    foreach (var item in _selectedUserVisibleDoors.ToList())
                    {
                        if (item.IsVisible)
                            visibleDoors.Add(item.DoorId);
                    }
                }
                _selectedUser.VisibleDoors = visibleDoors.ToArray();

                var visibleEventTypes = new List<Guid>();
                if (!IsAdministrator)
                {
                    foreach (var item in _selectedUserVisibleEventTypes.ToList())
                    {
                        if (item.IsVisible)
                            visibleEventTypes.Add(item.EventTypeId);
                    }
                }
                _selectedUser.VisibleEventTypes = visibleEventTypes.ToArray();

                foreach (var item in _selectedUserVisibleCommandTypes.ToList())
                {
                    switch (item.CommandName)
                    {
                        case "Lock":
                            _selectedUser.LockCommandVisible = item.IsVisible;
                            break;
                        case "Unlock":
                            _selectedUser.UnlockCommandVisible = item.IsVisible;
                            break;
                        default:
                            break;
                    }
                }

                _configurationManager.UpdateUser(_selectedUser);
            }
        }

        private void Instance_UsersChanged(object sender, EventArgs e)
        {
            _users = new ObservableCollection<User>(_configurationManager.Users);
            LoadSelectedUserData();
            OnPropertyChanged("Users");
        }
    }
}
