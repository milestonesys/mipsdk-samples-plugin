using DemoACServerApplication;
using DemoServerApplication.ACSystem;
using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for Door.
    /// </summary>
    public class DoorViewModel : ViewModelBase
    {
        private string _doorStatusString;
        private ImageSource _doorStatusImageSource;

        private readonly DoorManager _doorManager = DoorManager.Instance;
        private readonly AccessManager _accessManager = AccessManager.Instance;
        private readonly AlarmManager _alarmManager = AlarmManager.Instance;

        private bool _isTampered = false;
        private bool _isPowerFailure = false;
        private bool _isForcedOpen = false;
        private bool _isEnabled = true;

        private static readonly ImageSource DoorClosedLockedImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/DoorClosedLocked.png"));
        private static readonly ImageSource DoorClosedUnlockedImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/DoorClosedUnlocked.png"));
        private static readonly ImageSource DoorOpenLockedImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/DoorOpenLocked.png"));
        private static readonly ImageSource DoorOpenUnlockedImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/DoorOpenUnlocked.png"));

        public DoorViewModel(Door door)
        {
            Door = door;
            UpdateDoorStatus(_doorManager.GetDoorStatus(Door.Id));

            REXCommand = new DelegateCommand(_ => REX());
            UnlockCommand = new DelegateCommand(_ => Unlock());
            LockCommand = new DelegateCommand(_ => Lock());
            TamperCommand = new DelegateCommand(_ => Tamper());
            ClearTamperCommand = new DelegateCommand(_ => ClearTamper());
            ForcedOpenCommand = new DelegateCommand(_ => ForcedOpen());
            ClearForcedOpenCommand = new DelegateCommand(_ => ClearForcedOpen());
            PowerFailureCommand = new DelegateCommand(_ => PowerFailure());
            ClearPowerFailureCommand = new DelegateCommand(_ => ClearPowerFailure());

            _doorManager.DoorStatusChanged += Instance_DoorStatusChanged;
            _doorManager.DoorEnabledStateChanged += _doorManager_DoorEnabledStateChanged;
            _alarmManager.AlarmClosed += AlarmManagerOnAlarmClosed;
        }



        #region Properties

        public Door Door { get; private set; }

        public string DoorStatusString
        {
            get { return _doorStatusString; }
            private set { SetProperty(ref _doorStatusString, value); }
        }

        public ImageSource DoorStatusImageSource
        {
            get { return _doorStatusImageSource; }
            private set { SetProperty(ref _doorStatusImageSource, value); }
        }

        public bool IsTampered
        {
            get { return _isTampered; }
            private set { SetProperty(ref _isTampered, value); }
        }

        public bool IsPowerFailure
        {
            get { return _isPowerFailure; }
            private set { SetProperty(ref _isPowerFailure, value); }
        }

        public bool IsForcedOpen
        {
            get { return _isForcedOpen; }
            private set { SetProperty(ref _isForcedOpen, value); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        public DelegateCommand REXCommand { get; private set; }

        public DelegateCommand UnlockCommand { get; private set; }

        public DelegateCommand LockCommand { get; private set; }

        public DelegateCommand TamperCommand { get; private set; }

        public DelegateCommand ClearTamperCommand { get; private set; }

        public DelegateCommand ForcedOpenCommand { get; private set; }

        public DelegateCommand ClearForcedOpenCommand { get; private set; }

        public DelegateCommand PowerFailureCommand { get; private set; }

        public DelegateCommand ClearPowerFailureCommand { get; private set; }

        #endregion

        #region Methods

        private void UpdateDoorStatus(DoorStatus doorStatus)
        {
            DoorStatusString = string.Format("Open: {0} Locked: {1}", doorStatus.IsOpen, doorStatus.IsLocked);

            if (doorStatus.IsOpen)
                DoorStatusImageSource = doorStatus.IsLocked ? DoorOpenLockedImageSource : DoorOpenUnlockedImageSource;
            else
                DoorStatusImageSource = doorStatus.IsLocked ? DoorClosedLockedImageSource : DoorClosedUnlockedImageSource;
        }

        private void REX()
        {
            _accessManager.RequestAccessWithREXButton(Door.Id);
        }

        private void Unlock()
        {
            _doorManager.UnlockDoor(Door.Id);
        }

        private void Lock()
        {
            _doorManager.LockDoor(Door.Id);
        }

        private void PowerFailure()
        {
            if (_doorManager.SimulateControllerPowerFailure(Door.Id))
            {
                IsPowerFailure = true;
            }
        }

        private void ClearPowerFailure()
        {
            if (IsPowerFailure)
            {
                _alarmManager.ClearPowerFailureAlarmOnDoor(Door.Id);
                IsPowerFailure = false;
            }
        }

        private void ForcedOpen()
        {
            if (_doorManager.SimulateDoorForcedOpen(Door.Id))
            {
                IsForcedOpen = true;
            }
        }

        private void ClearForcedOpen()
        {
            if (IsForcedOpen)
            {
                _alarmManager.ClearForcedOpenAlarmOnDoor(Door.Id);
                IsForcedOpen = false;
            }
        }

        private void Tamper()
        {
            if (_doorManager.SimulateControllerTampering(Door.Id))
            {
                IsTampered = true;
            }
        }

        private void ClearTamper()
        {
            if (IsTampered)
            {
                _alarmManager.ClearTamperAlarmOnDoor(Door.Id);
                IsTampered = false;
            }
        }

        private void Instance_DoorStatusChanged(object sender, DoorManager.DoorStatusChangedEventArgs e)
        {
            if (Door.Id == e.DoorStatus.DoorId)
                UpdateDoorStatus(e.DoorStatus);
        }

        private void _doorManager_DoorEnabledStateChanged(object sender, DoorManager.DoorEnabledStateChangedEventArgs e)
        {
            if (Door.Id == e.DoorId)
            {
                IsEnabled = e.IsEnabled;
            }
        }

        private void AlarmManagerOnAlarmClosed(object sender, AlarmManager.AlarmStateChangedEventArgs e)
        {
            if (Door.Id != e.DoorId)
                return;

            if (e.EventTypeId == EventManager.EventTypeDoorControllerTampering.Id)
            {
                IsTampered = false;
            }
            else if (e.EventTypeId == EventManager.EventTypeDoorControllerPowerFailure.Id)
            {
                IsPowerFailure = false;
            }
            else if (e.EventTypeId == EventManager.EventTypeDoorForcedOpen.Id)
            {
                IsForcedOpen = false;
            }
        }

        #endregion
    }
}
