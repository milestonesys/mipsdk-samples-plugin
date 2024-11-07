using LocationView.Client.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.UI;

namespace LocationView.Client
{
    /// <summary>
    /// Interaction logic for MarkerChangeWindow.xaml
    /// </summary>
    public partial class MarkerChangeWindow : Window, INotifyPropertyChanged
    {
        public enum Mode
        {
            Add = 0,
            Edit
        }

        private void InitializeMarkerTypes()
        {
            comboBoxMarker.Items.Clear();
            foreach (MarkerTypes type in Enum.GetValues(typeof(MarkerTypes)))
            {
                comboBoxMarker.Items.Add(MarkerNames.GetName(type));
            }

            SetSelectedMarkerType(MarkerNames.GetName(_marker.MarkerType));
        }

        private void SetSelectedMarkerType(string markerType)
        {
            var index = comboBoxMarker.Items.IndexOf(markerType);
            if ((index < 0) || (index >= comboBoxMarker.Items.Count))
                index = 0;
            comboBoxMarker.SelectedIndex = index;
        }

        private Mode _mode = Mode.Add;

        internal Mode WorkMode
        {
            get { return _mode; }
            private set
            {
                _mode = value;
            }
        }

        private readonly Marker _marker = new Marker();

        internal Marker Marker
        {
            get
            {
                return new Marker()
                {
                    DeviceId = _marker.DeviceId,
                    DeviceName = _marker.DeviceName,
                    MarkerType = _marker.MarkerType
                };
            }
            set
            {
                _marker.InitFrom(value);

                SelectedMetadataItem = Configuration.Instance.GetItem(_marker.DeviceId, Kind.Metadata);
                buttonDevice.Content = SelectedMetadataItem.Name;

                SetSelectedMarkerType(MarkerNames.GetName(_marker.MarkerType));
            }
        }

        public bool OkEnabled
        {
            get => SelectedMetadataItem != null;
        }

        private Item _selectedMetadataItem = null;
        private Item SelectedMetadataItem
        {
            get => _selectedMetadataItem;
            set
            {
                _selectedMetadataItem = value;
                OnPropertyChanged(nameof(OkEnabled));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MarkerChangeWindow(Mode mode = Mode.Add)
        {
            InitializeComponent();

            InitializeMarkerTypes();

            WorkMode = mode;

            DataContext = this;
        }

        private void deviceButton_Click(object sender, RoutedEventArgs e)
        {
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid>() { Kind.Metadata },
                SelectionMode = SelectionModeOptions.SingleSelect,
                SelectedItems = new List<Item>() { SelectedMetadataItem },
                Items = Configuration.Instance.GetItemsByKind(Kind.Metadata),
            };
            form.ShowDialog();
            if (form.SelectedItems != null && form.SelectedItems.Any())
            {
                SelectedMetadataItem = form.SelectedItems.First();

                _marker.DeviceId = SelectedMetadataItem.FQID.ObjectId;
                _marker.DeviceName = SelectedMetadataItem.Name;

                buttonDevice.Content = SelectedMetadataItem.Name;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selIndex = comboBox.SelectedIndex;
            if ((selIndex >= 0) &
                (selIndex < comboBox.Items.Count))
            {
                _marker.MarkerType = MarkerNames.GetType((string)comboBox.Items[selIndex]);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
