using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel base for Door(s) Window.
    /// </summary>
    public abstract class DoorViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        protected Door _door;
        private DoorController _selectedDoorController;
        private ObservableCollection<DoorController> _doorControllers = new ObservableCollection<DoorController>();
        private string _latitude;
        private string _longitude;

        protected DoorViewModelBase(DoorViewModel doorViewModel) :
            this(doorViewModel.Door)
        { }

        protected DoorViewModelBase(Door door)
        {
            _door = door;
            _latitude = _door.Latitude != 0 ? _door.Latitude.ToString(CultureInfo.InvariantCulture) : string.Empty;
            _longitude = _door.Longitude != 0 ? _door.Longitude.ToString(CultureInfo.InvariantCulture) : string.Empty;

            foreach (var doorController in ConfigurationManager.Instance.DoorControllers)
                _doorControllers.Add(doorController);
            SelectedDoorController = _doorControllers.Where(dc => dc.Id == _door.DoorControllerId).FirstOrDefault();

            ValidatePoperty("DoorName", DoorName);
            ValidatePoperty("Latitude", Latitude);
            ValidatePoperty("Longitude", Longitude);

            OKCommand = new DelegateCommand(_ => this.OK());
            CancelCommand = new DelegateCommand(_ => this.Close());
        }

        public string DoorName
        {
            get { return _door.Name; }
            set
            {
                if (_door.Name != value)
                {
                    _door.Name = value;
                    OnPropertyChanged("DoorName");
                    ValidatePoperty("DoorName", value);
                }
            }
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
                    _door.DoorControllerId = _selectedDoorController != null ? _selectedDoorController.Id : Guid.Empty;
                    OnPropertyChanged("SelectedDoorController");
                }
            }
        }

        public bool IsAccessAllowed
        {
            get { return _door.IsAccessAllowed; }
            set
            {
                if (_door.IsAccessAllowed != value)
                {
                    _door.IsAccessAllowed = value;
                    OnPropertyChanged("IsAccessAllowed");
                }
            }
        }

        public bool HasRexButton
        {
            get { return _door.HasRexButton; }
            set
            {
                if (_door.HasRexButton != value)
                {
                    _door.HasRexButton = value;
                    OnPropertyChanged("HasRexButton");
                }
            }
        }

        public bool LockCommandSupported
        {
            get { return _door.LockCommandSupported; }
            set
            {
                if (_door.LockCommandSupported != value)
                {
                    _door.LockCommandSupported = value;
                    OnPropertyChanged("LockCommandSupported");
                }
            }
        }

        public bool UnlockCommandSupported
        {
            get { return _door.UnlockCommandSupported; }
            set
            {
                if (_door.UnlockCommandSupported != value)
                {
                    _door.UnlockCommandSupported = value;
                    OnPropertyChanged("UnlockCommandSupported");
                }
            }
        }

        public string Latitude
        {
            get { return _latitude; }
            set
            {
                if (_latitude != value)
                {
                    _latitude = value;
                    OnPropertyChanged("Latitude");
                    ValidatePoperty("Latitude", value);
                    ValidatePoperty("Longitude", Longitude);
                }
            }
        }

        public string Longitude
        {
            get { return _longitude; }
            set
            {
                if (_longitude != value)
                {
                    _longitude = value;
                    OnPropertyChanged("Longitude");
                    ValidatePoperty("Longitude", value);
                    ValidatePoperty("Latitude", Latitude);
                }
            }
        }

        public bool OKIsEnabled => !HasErrors;

        public bool HasErrors => _errors.Any();

        public string ErrorMessage
        {
            get
            {
                if (_errors.ContainsKey(nameof(DoorName)))
                {
                    return _errors[nameof(DoorName)].LastOrDefault();
                }

                if (_errors.ContainsKey(nameof(Latitude)))
                {
                    return _errors[nameof(Latitude)].LastOrDefault();
                }

                if (_errors.ContainsKey(nameof(Longitude)))
                {
                    return _errors[nameof(Longitude)].LastOrDefault();
                }

                return string.Empty;
            }
        }

        public DelegateCommand OKCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public event EventHandler CancelRequest;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
                return null;

            return _errors[propertyName];
        }

        protected void ValidatePoperty(string propertyName, object value)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
            }

            var validationErrors = new List<string>();

            switch (propertyName)
            {
                case nameof(DoorName):
                    if (string.IsNullOrWhiteSpace(value as string))
                    {
                        validationErrors.Add("Door name is required.");
                    }
                    break;

                case nameof(Latitude):
                    if (string.IsNullOrWhiteSpace(value as string))
                    {
                        if (string.IsNullOrWhiteSpace(Longitude))
                        {
                            _door.Latitude = 0;
                        }
                        else
                        {
                            validationErrors.Add("Latitude is required.");
                        }
                    }
                    else
                    {
                        if (double.TryParse(value as string, NumberStyles.Number, CultureInfo.InvariantCulture, out var latitude))
                        {
                            if (-90 <= latitude && latitude <= 90)
                            {
                                _door.Latitude = latitude;
                            }
                            else
                            {
                                validationErrors.Add("Latitude must be between –90 and 90.");
                            }
                        }
                        else
                        {
                            validationErrors.Add("Latitude must be a number.");
                        }
                    }
                    break;

                case nameof(Longitude):
                    if (string.IsNullOrWhiteSpace(value as string))
                    {
                        if (string.IsNullOrWhiteSpace(Latitude))
                        {
                            _door.Longitude = 0;
                        }
                        else
                        {
                            validationErrors.Add("Longitude is required.");
                        }
                    }
                    else
                    {
                        if (double.TryParse(value as string, NumberStyles.Number, CultureInfo.InvariantCulture, out var longitude))
                        {
                            if (-180 <= longitude && longitude <= 180)
                            {
                                _door.Longitude = longitude;
                            }
                            else
                            {
                                validationErrors.Add("Longitude must be between –180 and 180.");
                            }
                        }
                        else
                        {
                            validationErrors.Add("Longitude must be a number.");
                        }
                    }
                    break;
                default:
                    break;
            }

            if (validationErrors.Any())
            {
                _errors[propertyName] = validationErrors;
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

            OnPropertyChanged("OKIsEnabled");
            OnPropertyChanged("ErrorMessage");
        }

        protected void Close()
        {
            EventHandler handler = CancelRequest;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected abstract void OK();
    }
}
