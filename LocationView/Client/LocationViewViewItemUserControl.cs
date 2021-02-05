using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using LocationView.Client.Config;
using System;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using Message = VideoOS.Platform.Messaging.Message;

namespace LocationView.Client
{
    public partial class LocationViewViewItemUserControl 
        : ViewItemUserControl
	{
		#region Component private class variables

		private readonly LocationViewViewItemManager _viewItemManager;
        private object _themeChangedHandler;

        private GMapControl _mapControl;
        private GMapOverlay _overlayOne;

        private readonly NaviItemsList _naviItemsList = new NaviItemsList();

	    private MarkerTracker _markerTracker;

        private object _modeChangedReceiver;
        private bool _startUp = true;

		#endregion

		#region Component constructors + dispose

		/// <summary>
		/// Constructs a LocationViewViewItemUserControl instance
		/// </summary>
		public LocationViewViewItemUserControl(LocationViewViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;

			InitializeComponent();

		    // InitializeMapTypes();
            InitializeComboBox<MapTypes>(comboBoxMapType, MapProviderNames.GetName);
            InitializeComboBox<ToolTipAppearanceTypes>(comboBoxToolTipAppearance, ToolTipAppearanceNames.GetName);
            InitializeComboBox<ToolTipTextTypes>(comboBoxToolTipText, ToolTipTextNames.GetName);

		    bindingSourceMarkers.DataSource = _viewItemManager.Config.Markers;

		    trackBarZoom.Minimum = Config.Config.MapZoomLevelMinValue;
		    trackBarZoom.Maximum = Config.Config.MapZoomLevelMaxValue;
		}

        private void OnLoad(object sender, EventArgs e)
        {
            SetUpApplicationEventListeners();

            ClientControl.Instance.RegisterUIControlForAutoTheming(panelMain);
        }

	    private void SetUpApplicationEventListeners()
		{
			//set up ViewItem event listeners
			_themeChangedHandler = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ThemeChangedIndicationHandler),
														 new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));

            _modeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(ModeChanged, new MessageIdFilter(MessageId.System.ModeChangedIndication));
		}

        private void RemoveApplicationEventListeners()
		{
            //remove ViewItem event listeners
            if (_themeChangedHandler != null)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(_themeChangedHandler);
                _themeChangedHandler = null;
            }
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

            UpdateMarkersList();
            InitializeMapControl(_viewItemManager.Config);
            _markerTracker = new MarkerTracker(_mapControl);
		    
            SetComboSelection(comboBoxMapType, MapProviderNames.GetName(_viewItemManager.Config.MapType));
            SetComboSelection(comboBoxToolTipAppearance, ToolTipAppearanceNames.GetName(_viewItemManager.Config.ToolTip.Appearance));
            SetComboSelection(comboBoxToolTipText, ToolTipTextNames.GetName(_viewItemManager.Config.ToolTip.TextType));
            numericUpDownTimeout.Value = _viewItemManager.Config.TimeoutInSeconds;
            checkBoxZoomPanel.Checked = _viewItemManager.Config.ShowZoomPanel;
		    trackBarZoom.Value = _viewItemManager.Config.MapZoomLevel;

            OnZoomPanelVisibleChanged();

		    OnModeChange(EnvironmentManager.Instance.Mode);
            _startUp = false;
        }

        private void InitializeMapControl(Config.Config config)
        {
            _mapControl = new GMapControl();
            _mapControl.Dock = DockStyle.Fill;
            panelMain.Controls.Add(_mapControl);

            //use google provider
            _mapControl.MapProvider = MapProviderFactory.CreateMapProvider(config.MapType);

            //get tiles from server only
            // _mapControl.Manager.Mode = AccessMode.ServerAndCache;
            _mapControl.Manager.Mode = AccessMode.ServerOnly;

            _mapControl.DragButton = MouseButtons.Left;

            //not use proxy
            GMapProvider.WebProxy = null;
            //center map on moscow
            _mapControl.Position = new PointLatLng(
                _viewItemManager.Config.MapPosition.Lat,
                _viewItemManager.Config.MapPosition.Lng);

            //zoom min/max; default both = 2
            _mapControl.MinZoom = Config.Config.MapZoomLevelMinValue;
            _mapControl.MaxZoom = Config.Config.MapZoomLevelMaxValue;
            //set zoom
            _mapControl.Zoom = config.MapZoomLevel;

            _overlayOne = new GMapOverlay("OverlayOne");
            _mapControl.Overlays.Add(_overlayOne);

            _mapControl.OnMapZoomChanged += MapControlOnOnMapZoomChanged;
            _mapControl.OnPositionChanged += MapControlOnOnPositionChanged;

            _mapControl.DoubleClick += OnDoubleClick;
            _mapControl.Click += OnClick;
        }

	    private delegate string GetName<in T>(T type);

	    private void InitializeComboBox<T>(ComboBox comboBox, GetName<T> nameProvider)
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
            panelMain.Controls.Remove(_mapControl);

            _mapControl.OnMapZoomChanged -= MapControlOnOnMapZoomChanged;

            _mapControl.DoubleClick -= OnDoubleClick;
            _mapControl.Click -= OnClick;

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

		private object ThemeChangedIndicationHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
		{
			Selected = _selected;
			return null;
		}

        private object ModeChanged(Message message, FQID destination, FQID sender)
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
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(ShowConfigurationPanel), new object[] {show});
                return;
            }
            if (!show && (panelConfiguration.Visible || _startUp))
            {
                // leaving setup
                ChangeNavigationProvider();
            }
            panelConfiguration.Visible = show;
        }

        private void ChangeNavigationProvider()
        {
            StopNavigationProviders();

            _naviItemsList.Init(_viewItemManager.Config, WindowInformation);
            _naviItemsList.AddMarkers(_overlayOne.Markers);
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
				if (_hidden != value)
				{
					base.Hidden = value;
				}
			}
		}

		public override bool Selected
		{
			get
			{
				return base.Selected;
			}
			set
			{
				base.Selected = value;
				if (value)
				{
					panelMain.BackColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderColor;
					panelMain.ForeColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderTextColor;
				}
				else
				{
					panelMain.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
					panelMain.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
				}
			}
		}

		#endregion

		#region Component event handlers

        private bool _inTrackBarZoomChange = false;

        private void TrackBarZoomScroll(object sender, EventArgs e)
        {
            _inTrackBarZoomChange = true;

            var value = trackBarZoom.Value;
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
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<int>(ChangeTrackBarValue), new object[] { value });
            }
            else
            {
                trackBarZoom.Value = value;
            }
        }

        private void MapControlOnOnPositionChanged(PointLatLng point)
        {
            var position = _viewItemManager.Config.MapPosition;
            position.Lat = point.Lat;
            position.Lng = point.Lng;
        }

        private void ButtonCenterClick(object sender, EventArgs e)
        {
            try { _markerTracker.CenterNextMarker(_naviItemsList.GMapMarkers); }
            catch {}
        }

        #endregion

        #region Dealing with markers

        private void ButtonAddMarkerClick(object sender, EventArgs e)
        {
            AddEditMarker(null);
        }

        private void ButtonEditMarkerClick(object sender, EventArgs e)
        {
            var selRows = dataGridViewMarkers.SelectedRows;
            var selIndex = selRows[0].Index;

            AddEditMarker(_viewItemManager.Config.Markers[selIndex]);
        }

	    private void AddEditMarker(Marker marker)
	    {
            var dlg = new MarkerChangeForm();
	        if (null != marker)
	            dlg.Marker = marker;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                if (null != marker)
                    marker.InitFrom(dlg.Marker);
                else
                    _viewItemManager.Config.Markers.Add(dlg.Marker);

                UpdateMarkersList();
            }
	    }

        private void ButtonRemoveMarkerClick(object sender, EventArgs e)
        {
            var selRows = dataGridViewMarkers.SelectedRows;
            var selIndex = selRows[0].Index;

            _viewItemManager.Config.Markers.RemoveAt(selIndex);

            UpdateMarkersList();
        }

	    private void UpdateMarkersList()
	    {
	        bindingSourceMarkers.ResetBindings(false);
	    }

        private void DataGridViewMarkersSelectionChanged(object sender, EventArgs e)
        {
            var selRows = dataGridViewMarkers.SelectedRows;
            var editRemoveEnabled = (selRows.Count > 0);

            buttonEditMarker.Enabled = editRemoveEnabled;
            buttonRemoveMarker.Enabled = editRemoveEnabled;
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

        private void ComboBoxMapTypeSelectedIndexChanged(object sender, EventArgs e)
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

        private void ComboBoxToolTipAppearanceSelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxSelectedIndexChanged(
                comboBoxToolTipAppearance,
                ToolTipAppearanceNames.GetType,
                (toolTipAppearance) => _viewItemManager.Config.ToolTip.Appearance = toolTipAppearance);
        }

        private void ComboBoxToolTipTextSelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxSelectedIndexChanged(
                comboBoxToolTipText,
                ToolTipTextNames.GetType,
                (toolTipText) => _viewItemManager.Config.ToolTip.TextType = toolTipText);
        }

        private void NumericUpDownTimeoutValueChanged(object sender, EventArgs e)
        {
            _viewItemManager.Config.TimeoutInSeconds = (int) numericUpDownTimeout.Value;
        }

        private void CheckBoxZoomPanelCheckedChanged(object sender, EventArgs e)
        {
            _viewItemManager.Config.ShowZoomPanel = checkBoxZoomPanel.Checked;

            OnZoomPanelVisibleChanged();
        }

	    private void OnZoomPanelVisibleChanged()
	    {
	        trackBarZoom.Visible = _viewItemManager.Config.ShowZoomPanel;
	        buttonCenter.Visible = _viewItemManager.Config.ShowZoomPanel;
	    }

        #endregion

    }

    internal class MyMessageFilter
        : MessageFilter
    {
        public override bool Match(Message message, FQID destination, FQID sender)
        {
            return base.Match(message, destination, sender);
        }
    }
}
