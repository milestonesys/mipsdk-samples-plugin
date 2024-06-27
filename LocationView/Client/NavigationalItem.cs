using GMap.NET;
using GMap.NET.WindowsPresentation;
using LocationView.Client.Config;
using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Metadata;
using ToolTip = System.Windows.Controls.ToolTip;

namespace LocationView.Client
{
    internal class NavigationalItem
    {
        private readonly MetadataSupplier _metadataSupplier;

        private GMapMarker _mapMarker;

        private Marker _marker;

        private Config.Config _config;

        public GMapMarker MapMarker { get { return _mapMarker; }}

        public delegate Item ItemRetriever(Guid itemId);

        public NavigationalItem(Config.Config config, Marker marker, WindowInformation windowInformation)
        {
            _config = config;
            var item = Configuration.Instance.GetItem(marker.DeviceId, Kind.Metadata);
            if (item == null)
            {
                throw new ArgumentException("Could not find metadata item");
            }
            _metadataSupplier = new MetadataSupplier(item, windowInformation);
            InitMarker(marker);
        }

        private void InitMarker(Marker marker)
        {
            _marker = marker;
            _mapMarker = MarkerFactory.CreateMarker(_marker.MarkerType, _config.ToolTip.Appearance);
        }

        public void StartSupplier()
        {
            _metadataSupplier.NewMetadataEvent += _metadataSupplier_NewMetadataEvent;
            _metadataSupplier.MetadataInvalidatedEvent += _metadataSupplier_MetadataInvalidatedEvent;
            _metadataSupplier.Start();
        }

        private void _metadataSupplier_NewMetadataEvent(object sender, MetadataSupplierEventArgs e)
        {           
            if (e.Data != null)
            {
                var stream = e.Data.GetMetadataStream();                
                if (stream != null)
                {
                    OnNavigationalData(stream.NavigationalData);
                }
            }
        }

        private void HideMarker()
        {
            if (!_mapMarker.Shape.CheckAccess())
            {
                _mapMarker.Shape.Dispatcher.Invoke(() => HideMarker());
            }
            else
            {
                _mapMarker.Shape.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void _metadataSupplier_MetadataInvalidatedEvent(object sender, MetadataSupplierMetadataInvalidatedEventArgs e)
        {
            HideMarker();
        }

        public void StopSupplier()
        {
            _metadataSupplier.NewMetadataEvent -= _metadataSupplier_NewMetadataEvent;
            _metadataSupplier.MetadataInvalidatedEvent -= _metadataSupplier_MetadataInvalidatedEvent;
            _metadataSupplier.Stop();
        }

        #region Navigational Data events handling

        private void OnNavigationalData(NavigationalData navigationalData)
        {
            if (!_mapMarker.Shape.CheckAccess())
            {
                _mapMarker.Shape.Dispatcher.Invoke(new Action<NavigationalData>(OnNavigationalData), new object[] { navigationalData });
            }
            else
            {
                if (null != navigationalData &&
                    navigationalData.Latitude.HasValue &&
                    navigationalData.Longitude.HasValue)
                {
                    var position = new PointLatLng(navigationalData.Latitude.Value, navigationalData.Longitude.Value);
                    _mapMarker.Position = position;

                    if (_config.ToolTip.Appearance != ToolTipAppearanceTypes.Never)
                    {
                        ((_mapMarker.Shape as StackPanel).Children[1] as Label).Content = BuildToolTipText(navigationalData);
                    }
                    _mapMarker.Shape.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    _mapMarker.Shape.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        private string BuildToolTipText(NavigationalData navigationalData)
        {
            var bd = new StringBuilder();

            if (ToolTipTextHelper.HasValue(_config.ToolTip.TextType, ToolTipTextTypes.Name))
            {
                bd.Append(_marker.DeviceName);
            }

            if (ToolTipTextHelper.HasValue(_config.ToolTip.TextType, ToolTipTextTypes.NameAndLocation))
            {
                bd.AppendLine();
            }

            if (ToolTipTextHelper.HasValue(_config.ToolTip.TextType, ToolTipTextTypes.Location))
            {
                if (navigationalData.Latitude != null)
                    bd.Append("Latitude: ").AppendLine(navigationalData.Latitude.Value.ToString(CultureInfo.InvariantCulture));
                if (navigationalData.Longitude != null)
                    bd.Append("Longitude: ").Append(navigationalData.Longitude.Value.ToString(CultureInfo.InvariantCulture));
            }

            return bd.ToString();
        }

        #endregion
    }
}
