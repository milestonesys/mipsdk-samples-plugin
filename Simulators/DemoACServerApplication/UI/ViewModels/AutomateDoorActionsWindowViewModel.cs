using DemoACServerApplication;
using DemoServerApplication.ACSystem;
using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for AutomateDoorActionsWindow.
    /// </summary>
    public sealed class AutomateDoorActionsWindowViewModel : ViewModelBase
    {
        #region Private fields

        private enum ActionType { AccessGranted, Unlock, Open, CloseAndLock, Lock }
        private bool _logActions = true;
        private Thread _actionThread;
        private object _padlock = new object();
        private int _eventFrequency = 1;
        private bool _keepRunning = false;
        private StreamWriter _logFileStream;
        private string _logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Milestone\DemoACServerApplicaton");
        private bool _progressBarIndeterminateValue = false;
        private string _startStopButtonText = Properties.Resources.StartButtonText;

        #endregion

        #region Constructor

        public AutomateDoorActionsWindowViewModel(bool logActions = true)
        {
            _eventFrequency = 1;
            LogActions = logActions;

            CancelCommand = new DelegateCommand(_ => this.Close());
            StartStopCommand = new DelegateCommand(_ => this.StartStop());
        }

        #endregion

        public void DoStart()
        {
            if (StartActionThread())
            {
                StartStopButtonText = Properties.Resources.StopButtonText;
                ProgressBarIndeterminateValue = true;
            }
        }

        #region Properties

        public string StartStopButtonText 
        {
            get { return _startStopButtonText; }
            set
            {
                if (_startStopButtonText != value)
                {
                    _startStopButtonText = value;
                    OnPropertyChanged("StartStopButtonText");
                }
            }
        }

        public bool LogActions 
        {
            get { return _logActions; }
            set
            {
                if (_logActions != value)
                {
                    _logActions = value;
                    OnPropertyChanged("LogActions");
                }
            }
        }

        public int EventFrequency 
        {
            get { return _eventFrequency; }
            set
            {
                if (_eventFrequency != value)
                {
                    _eventFrequency = value;
                    OnPropertyChanged("EventFrequency");
                }
            }
        }

        public bool ProgressBarIndeterminateValue
        {
            get { return _progressBarIndeterminateValue; }
            set 
            {
                if (_progressBarIndeterminateValue != value)
                {
                    _progressBarIndeterminateValue = value;
                    OnPropertyChanged("ProgressBarIndeterminateValue");
                }
            }
        }

        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand StartStopCommand { get; private set; }

        #endregion

        #region Private methods

        public event EventHandler CancelRequest;
        private void Close()
        {
            StopActionThread();

            EventHandler handler = CancelRequest;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void StartStop()
        {
            if (ProgressBarIndeterminateValue)
                DoStop();
            else
                DoStart();
        }


        private void DoStop()
        {
            if (StopActionThread())
            {
                StartStopButtonText = Properties.Resources.StartButtonText;
                ProgressBarIndeterminateValue = false;
            }
        }

        private bool StartActionThread()
        {
            lock (_padlock)
            {
                if (_actionThread == null)
                {
                    _keepRunning = true;
                    _actionThread = new Thread(new ThreadStart(ActionThread));
                    _actionThread.Start();
                }
            }

            return true;
        }

        public bool StopActionThread()
        {
            lock (_padlock)
            {
                if (_actionThread != null)
                {
                    _keepRunning = false;
                    _actionThread.Join(10000);
                    _actionThread = null;
                }
            }

            return true;
        }

        private void LogAction(string action, Guid doorId)
        {
            if (_logActions && _logFileStream != null)
            {
                try
                {
                    _logFileStream.Write("{0:yyyy/MM/dd HH:mm:ss:fff},{1},{2}\r\n", DateTime.Now, doorId.ToString(), action);
                }
                catch (Exception) 
                { }
            }
        }

        delegate void ShowErrorInvoker(string errorString);
        private void ShowError(string errorString)
        {
            if (!App.Current.Dispatcher.CheckAccess())
            {
                App.Current.Dispatcher.Invoke(new ShowErrorInvoker(ShowError), errorString);
                return;
            }

            MessageBox.Show(errorString);
        }

        private void ActionThread()
        {
            if (_logActions)
            {
                try
                {
                    Directory.CreateDirectory(_logPath);
                }
                catch (Exception) { }
                try
                {
                    _logFileStream = new StreamWriter(System.IO.Path.Combine(_logPath, "Automation.log"), true);
                }
                catch (Exception ex)
                {
                    ShowError("Failed to open log file (" + _logPath + "\\Automation.log).\r\n\r\nException: " + ex.Message);
                }
            }

            DateTime nextActionTime = DateTime.Now;
            int currentDoorIndex = 0;
            int currentCredentialHolderIndex = 0;

            var doors = ConfigurationManager.Instance.Doors;
            var credentialHolders = ConfigurationManager.Instance.CredentialHolders;

            bool enableAccessGrantedEvent = credentialHolders.Count > 0;
            ActionType nextAction = enableAccessGrantedEvent ? ActionType.AccessGranted : ActionType.Unlock;

            while (_keepRunning)
            {
                long delayBetweenEvents = TimeSpan.TicksPerSecond / _eventFrequency;
                Guid currentDoorId = doors[currentDoorIndex].Id;
                Guid currentCredentialHolderId = Guid.Empty;

                if (enableAccessGrantedEvent)
                    currentCredentialHolderId = credentialHolders[currentCredentialHolderIndex].Id;

                switch (nextAction)
                {
                    case ActionType.AccessGranted:
                        {
                            // not really an action, but will still cause an event towards the server
                            DemoServerApplication.ACSystem.EventManager.Instance.AddEvent(
                                DemoServerApplication.ACSystem.EventManager.EventTypeAccessGranted,
                                new DoorControllerEvent { DoorId = currentDoorId, CredentialHolderId = currentCredentialHolderId, AccessPoint = 1 });
                            nextAction = ActionType.Unlock;
                            break;
                        }
                    case ActionType.Unlock:
                        {
                            LogAction("Unlock", currentDoorId);
                            DoorManager.Instance.UnlockDoor(currentDoorId);
                            nextAction = ActionType.Open;
                            break;
                        }
                    case ActionType.Open:
                        {
                            LogAction("Open", currentDoorId);
                            DoorManager.Instance.OpenDoor(currentDoorId);
                            nextAction = ActionType.CloseAndLock;
                            break;
                        }
                    case ActionType.CloseAndLock:
                        {
                            LogAction("Close and lock", currentDoorId);
                            DoorManager.Instance.CloseAndLockDoor(currentDoorId);
                            nextAction = enableAccessGrantedEvent ? ActionType.AccessGranted : ActionType.Unlock;
                            currentDoorIndex++;
                            break;
                        }
                }
                nextActionTime = nextActionTime.AddTicks(delayBetweenEvents);
                TimeSpan sleepTime = nextActionTime - DateTime.Now;

                if (sleepTime > TimeSpan.Zero)
                    Thread.Sleep(sleepTime);

                if (currentDoorIndex == doors.Count)
                {
                    currentDoorIndex = 0;

                    if (enableAccessGrantedEvent)
                    {
                        currentCredentialHolderIndex++;
                        if (currentCredentialHolderIndex == credentialHolders.Count)
                            currentCredentialHolderIndex = 0;
                    }
                }
            }

            if (_logFileStream != null)
            {
                _logFileStream.Flush();
                _logFileStream.Close();
                _logFileStream = null;
            }

        }

        #endregion       
    }
}
