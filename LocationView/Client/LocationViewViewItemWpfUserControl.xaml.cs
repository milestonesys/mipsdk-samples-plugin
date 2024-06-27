using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using LocationView.Client.Config;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace LocationView.Client
{
    public partial class LocationViewViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private readonly LocationViewViewItemManager _viewItemManager;

        private readonly NaviItemsList _naviItemsList = new NaviItemsList();

        private MarkerTracker _markerTracker;

        private object _modeChangedReceiver;
        private bool _startUp = true;

        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a LocationViewViewItemUserControl instance
        /// </summary>
        public LocationViewViewItemWpfUserControl(LocationViewViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            SetUpApplicationEventListeners();
            InitializeComponent();

            InitializeComboBox<MapTypes>(comboBoxMapType, MapProviderNames.GetName);
            InitializeComboBox<ToolTipAppearanceTypes>(comboBoxToolTipAppearance, ToolTipAppearanceNames.GetName);
            InitializeComboBox<ToolTipTextTypes>(comboBoxToolTipText, ToolTipTextNames.GetName);

            DataContext = _viewItemManager.Config;

            scrollBarZoom.Minimum = Config.Config.MapZoomLevelMinValue;
            scrollBarZoom.Maximum = Config.Config.MapZoomLevelMaxValue;
        }

        private void SetUpApplicationEventListeners()
        {
            //set up ViewItem event listeners
            _modeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ModeChanged),
                                             new MessageIdFilter(MessageId.System.ModeChangedIndication));
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ViewItem event listeners
            if (_modeChangedReceiver != null)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(_modeChangedReceiver);
                _modeChangedReceiver = null;
            }
        }

        public override void Init()
        {
            _viewItemManager.Config.UpdateItemNames((id) =>
            {
                var item = Configuration.Instance.GetItem(id, Kind.Metadata);
                return (item != null) ? item.Name : null;
            });

            InitializeMapControl(_viewItemManager.Config);
            _markerTracker = new MarkerTracker(_mapControl);

            SetComboSelection(comboBoxMapType, MapProviderNames.GetName(_viewItemManager.Config.MapType));
            SetComboSelection(comboBoxToolTipAppearance, ToolTipAppearanceNames.GetName(_viewItemManager.Config.ToolTip.Appearance));
            SetComboSelection(comboBoxToolTipText, ToolTipTextNames.GetName(_viewItemManager.Config.ToolTip.TextType));

            checkBoxZoomPanel.IsChecked = _viewItemManager.Config.ShowZoomPanel;
            scrollBarZoom.Value = _viewItemManager.Config.MapZoomLevel;

            OnZoomPanelVisibleChanged();

            OnModeChange(EnvironmentManager.Instance.Mode);
            _startUp = false;
        }

        private void InitializeMapControl(Config.Config config)
        {
            _mapControl.MapProvider = MapProviderFactory.CreateMapProvider(config.MapType);

            //get tiles from server only
            _mapControl.Manager.Mode = AccessMode.ServerOnly;

            _mapControl.DragButton = System.Windows.Input.MouseButton.Left;

            //not use proxy
            GMapProvider.WebProxy = null;
            //center map
            _mapControl.Position = new PointLatLng(
                _viewItemManager.Config.MapPosition.Lat,
                _viewItemManager.Config.MapPosition.Lng);

            //zoom min/max; default both = 2
            _mapControl.MinZoom = Config.Config.MapZoomLevelMinValue;
            _mapControl.MaxZoom = Config.Config.MapZoomLevelMaxValue;
            //set zoom
            _mapControl.Zoom = config.MapZoomLevel;

            _mapControl.OnMapZoomChanged += MapControlOnOnMapZoomChanged;
            _mapControl.OnPositionChanged += MapControlOnOnPositionChanged;

            _mapControl.MouseDoubleClick += OnDoubleClick;
            _mapControl.MouseLeftButtonUp += OnClick;
        }

        private delegate string GetName<in T>(T type);

        private void InitializeComboBox<T>(System.Windows.Controls.ComboBox comboBox, GetName<T> nameProvider)
        {
            comboBox.Items.Clear();

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                comboBox.Items.Add(nameProvider(value));
            }
        }

        private void SetComboSelection(ComboBox comboBox, string type)
        {
            comboBox.SelectedIndex = comboBox.Items.IndexOf(type);
        }

        private void UpdateMapPosion(Config.Config config)
        {
            config.MapPosition = new PointLatLng(_mapControl.Position.Lat, _mapControl.Position.Lng);
        }
         
        /// <summary>
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
            UpdateMapPosion(_viewItemManager.Config);
            _viewItemManager.SaveAllProperties();

            RemoveApplicationEventListeners();

            StopNavigationProviders();

            DenitializeMapControl();
        }

        public override bool ShowToolbar
        {
            get { return false; }
        }

        private void DenitializeMapControl()
        {
            mainGrid.Children.Remove(_mapControl);

            _mapControl.OnMapZoomChanged -= MapControlOnOnMapZoomChanged;

            _mapControl.MouseDoubleClick -= OnDoubleClick;
            _mapControl.MouseLeftButtonUp -= OnClick;

            _mapControl = null;
        }

        #endregion

        #region Dealing with Navigation Providers

        private void StopNavigationProviders()
        {
            _naviItemsList.ClearItems();
        }

        #endregion

        private void OnClick(object sender, EventArgs e)
        {
            FireClickEvent();
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            FireDoubleClickEvent();
        }

        #region Component events

        private object ModeChanged(VideoOS.Platform.Messaging.Message message, FQID destination, FQID sender)
        {
            OnModeChange((Mode)message.Data);
            return null;
        }

        private void OnModeChange(Mode mode)
        {
            ShowConfigurationPanel(mode == Mode.ClientSetup);
        }

        private void ShowConfigurationPanel(bool show)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<bool>(ShowConfigurationPanel), new object[] { show });
                return;
            }
            if (!show && (configurationGrid.IsVisible || _startUp))
            {
                // leaving setup
                ChangeNavigationProvider();
            }
            configurationGrid.Visibility = (show ? Visibility.Visible : Visibility.Collapsed);
        }

        private void ChangeNavigationProvider()
        {
            StopNavigationProviders();

            _naviItemsList.Init(_viewItemManager.Config, WindowInformation);
            _naviItemsList.AddMarkers(_mapControl.Markers);
            _naviItemsList.StartItems();
        }

        #endregion

        #region Component properties

        public override bool Maximizable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Tell if Item is selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        /// <summary>
        /// Overrides property (set). First the Base implementation is called.
        /// When maximized the image quality should always be forced to full quality.
        /// </summary>
        public override bool Maximized
        {
            set
            {
                if (base.Maximized != value)
                {
                    base.Maximized = value;
                }
            }
        }

        /// <summary>
        /// Overrides property (set). First the Base implementaion is called. 
        /// </summary>
        public override bool Hidden
        {
            set
            {
                if (Hidden != value)
                {
                    base.Hidden = value;
                }
            }
        }

        #endregion

        #region Component event handlers

        private bool _inTrackBarZoomChange = false;

        private void ZoomScroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            _inTrackBarZoomChange = true;

            var value = scrollBarZoom.Value;
            _mapControl.Zoom = value;
            _viewItemManager.Config.MapZoomLevel = value;

            _inTrackBarZoomChange = false;
        }

        private void MapControlOnOnMapZoomChanged()
        {
            var zoomLevel = (int)_mapControl.Zoom;
            if (false == _inTrackBarZoomChange)
                ChangeTrackBarValue(zoomLevel);
            _viewItemManager.Config.MapZoomLevel = zoomLevel;
        }

        private void ChangeTrackBarValue(int value)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<int>(ChangeTrackBarValue), new object[] { value });
            }
            else
            {
                scrollBarZoom.Value = value;
            }
        }

        private void MapControlOnOnPositionChanged(PointLatLng point)
        {
            var position = _viewItemManager.Config.MapPosition;
            position.Lat = point.Lat;
            position.Lng = point.Lng;
        }

        private void ButtonCenterClick(object sender, RoutedEventArgs e)
        {
            try { _markerTracker.CenterNextMarker(_naviItemsList.GMapMarkers); }
            catch { }
        }

        #endregion

        #region Dealing with markers

        private void ButtonAddMarkerClick(object sender, RoutedEventArgs e)
        {
            AddEditMarker(null);
        }

        private void ButtonEditMarkerClick(object sender, RoutedEventArgs e)
        {
            var selIndex = dataGridViewMarkers.SelectedIndex;

            AddEditMarker(_viewItemManager.Config.Markers[selIndex]);
        }

        private void AddEditMarker(Marker marker)
        {
            var dlg = new MarkerChangeWindow();
            if (null != marker)
                dlg.Marker = marker;
            if (dlg.ShowDialog().Value)
            {
                if (null != marker)
                    marker.InitFrom(dlg.Marker);
                else
                    _viewItemManager.Config.Markers.Add(dlg.Marker);
            }
        }

        private void ButtonRemoveMarkerClick(object sender, RoutedEventArgs e)
        {
            var selIndex = dataGridViewMarkers.SelectedIndex;

            _viewItemManager.Config.Markers.RemoveAt(selIndex);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selIndex = dataGridViewMarkers.SelectedIndex;
            var editRemoveEnabled = (selIndex >= 0);

            buttonEditMarker.IsEnabled = editRemoveEnabled;
            buttonRemoveMarker.IsEnabled = editRemoveEnabled;
        }

        #endregion

        #region Handling UI changes

        private delegate T GetType<out T>(string typeString);

        private delegate void OnType<in T>(T type);

        private void ComboBoxSelectedIndexChanged<T>(ComboBox comboBox, GetType<T> typeGetter, OnType<T> typeSetter)
        {
            var selectedIndex = comboBox.SelectedIndex;

            if ((selectedIndex >= 0) &&
                (selectedIndex < comboBox.Items.Count))
            {
                var typeString = comboBox.Items[selectedIndex].ToString();
                var type = typeGetter(typeString);
                typeSetter(type);
            }
        }

        private void comboBoxMapType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxSelectedIndexChanged(
                comboBoxMapType,
                MapProviderNames.GetType,
                (mapType) =>
                {
                    _mapControl.MapProvider = MapProviderFactory.CreateMapProvider(mapType);
                    _viewItemManager.Config.MapType = mapType;
                });
        }

        private void comboBoxToolTipAppearance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxSelectedIndexChanged(
                comboBoxToolTipAppearance,
                ToolTipAppearanceNames.GetType,
                (toolTipAppearance) => _viewItemManager.Config.ToolTip.Appearance = toolTipAppearance);
        }


        private void comboBoxToolTipText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxSelectedIndexChanged(
                comboBoxToolTipText,
                ToolTipTextNames.GetType,
                (toolTipText) => _viewItemManager.Config.ToolTip.TextType = toolTipText);
        }

        private void checkBoxZoomPanel_CheckedChanged(object sender, RoutedEventArgs e)
        {
            OnZoomPanelVisibleChanged();
        }

        private void OnZoomPanelVisibleChanged()
        {
            _viewItemManager.Config.ShowZoomPanel = (bool)checkBoxZoomPanel.IsChecked;

            scrollBarZoom.Visibility = _viewItemManager.Config.ShowZoomPanel ? Visibility.Visible : Visibility.Hidden;
            buttonCenter.Visibility = _viewItemManager.Config.ShowZoomPanel ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion
    }
}
