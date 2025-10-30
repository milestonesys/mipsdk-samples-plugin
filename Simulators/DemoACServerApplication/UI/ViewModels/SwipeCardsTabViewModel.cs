using DemoACServerApplication;
using DemoServerApplication.ACSystem;
using DemoServerApplication.Configuration;
using DemoServerApplication.UI.Models;
using DemoServerApplication.UI.ViewModels.Commands;
using DemoServerApplication.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for SwipeCardsTabView.
    /// </summary>
    public class SwipeCardsTabViewModel : TabViewModelBase
    {
        private List<CredentialHolderViewModel> _credentialHolders = new List<CredentialHolderViewModel>();
        private List<DoorViewModel> _doors = new List<DoorViewModel>();
        private ObservableCollection<EventModel> _events = new ObservableCollection<EventModel>();
        private string _eventsStatusSelectedFilter;

        private DoorManager _doorManager = DoorManager.Instance;
        private ConfigurationManager _configurationManager = ConfigurationManager.Instance;
        private EventManager _eventManager = EventManager.Instance;

        public SwipeCardsTabViewModel(string tabName)
            : base(tabName)
        {
            foreach (var credentialHolder in _configurationManager.CredentialHolders)
                _credentialHolders.Add(new CredentialHolderViewModel(credentialHolder));

            foreach (var door in _configurationManager.Doors)
                _doors.Add(new DoorViewModel(door));

            EventsStatusFilters = new List<string>() { "All", "Events", "Status" };
            EventsStatusSelectedFilter = EventsStatusFilters[0];

            UnlockAllDoorsCommand = new DelegateCommand(_ => UnlockAllDoors());
            LockAllDoorsCommand = new DelegateCommand(_ => LockAllDoors());
            AutomateCommand = new DelegateCommand(_ => Automate());

            Subscribe();
        }

        public List<CredentialHolderViewModel> CredentialHolders { get { return _credentialHolders; } }

        public List<DoorViewModel> Doors { get { return _doors; } }

        public ObservableCollection<EventModel> Events { get { return _events; } }

        public List<string> EventsStatusFilters { get; private set; }

        public string EventsStatusSelectedFilter
        {
            get { return _eventsStatusSelectedFilter; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if (_eventsStatusSelectedFilter != value)
                {
                    _eventsStatusSelectedFilter = value;
                    RefreshEventList();
                    OnPropertyChanged("EventsStatusSelectedFilter");
                }
            }
        }

        public DelegateCommand UnlockAllDoorsCommand { get; private set; }

        public DelegateCommand LockAllDoorsCommand { get; private set; }

        public DelegateCommand AutomateCommand { get; private set; }

        private void LockAllDoors()
        {
            _doorManager.LockAllDoors();
        }

        private void UnlockAllDoors()
        {
            _doorManager.UnlockAllDoors();
        }

        private void Automate()
        {
            var automateDoorActionsWindows = new AutomateDoorActionsWindow();
            automateDoorActionsWindows.ShowDialog();
        }
        
        private void Subscribe()
        {
            _eventManager.EventAdded += Instance_EventAdded;
            _configurationManager.DoorsChanged += Instance_DoorsChanged;
            _configurationManager.CredentialHoldersChanged += Instance_CredentialHoldersChanged;
        }

        private void Instance_EventAdded(object sender, EventManager.EventAddedEventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                InsertEvent(e.Event);
            });
        }

        private void Instance_DoorsChanged(object sender, EventArgs e)
        {
            _doors = new List<DoorViewModel>();
            foreach (var door in _configurationManager.Doors)
                _doors.Add(new DoorViewModel(door));
            OnPropertyChanged("Doors");
        }

        private void Instance_CredentialHoldersChanged(object sender, EventArgs e)
        {
            _credentialHolders = new List<CredentialHolderViewModel>();
            foreach (var credentialHolder in _configurationManager.CredentialHolders)
                _credentialHolders.Add(new CredentialHolderViewModel(credentialHolder));
            OnPropertyChanged("CredentialHolders");
        }

        private void InsertEvent(BaseEvent baseEvent)
        {
            bool acceptEvent = false;

            switch (_eventsStatusSelectedFilter)
            {
                case "All":
                    acceptEvent = true;
                    break;
                case "Events":
                    if (!(baseEvent is DoorStatusEvent))
                        acceptEvent = true;
                    break;
                case "Status":
                    if (baseEvent is DoorStatusEvent)
                        acceptEvent = true;
                    break;
            }

            if (acceptEvent)
            {
                _events.Insert(0, new EventModel(baseEvent));
                if (_events.Count > 500)
                    _events.RemoveAt(_events.Count - 1);
            }
        }

        private void RefreshEventList()
        {
            _events.Clear();
            BaseEvent[] allEvents = _eventManager.GetEventsAll();

            foreach (var baseEvent in allEvents)
                InsertEvent(baseEvent);
        }
    }
}
