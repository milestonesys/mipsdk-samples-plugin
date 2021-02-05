using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using LocationView.Client.Config;
using System;
using System.Globalization;
using System.Text;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Metadata;

namespace LocationView.Client
{
    internal class NavigationalItem
    {
        private readonly MetadataSupplier _metadataSupplier;

        private GMapMarker _mapMarker = new GMarkerGoogle(new PointLatLng(0, 0), GMarkerGoogleType.arrow);

        private Marker _marker;

        private Config.Config _config;

        public GMapMarker MapMarker { get { return _mapMarker; }}

        public delegate Item ItemRetriever(Guid itemId);

        public NavigationalItem(Config.Config config, Marker marker, int dataTimeoutS, WindowInformation windowInformation)
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
            _mapMarker = MarkerFactory.CreateMarker(_marker.MarkerType);
            _mapMarker.IsVisible = false;
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

        private void _metadataSupplier_MetadataInvalidatedEvent(object sender, MetadataSupplierMetadataInvalidatedEventArgs e)
        {
            _mapMarker.IsVisible = false;
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
//            if (this.InvokeRequired)
//            {
//                this.BeginInvoke(new Action<NavigationalData>(OnNavigationalData), new object[] { navigationalData });
//            }
//            else
            {
                if ((null != navigationalData) &&
                    (true == navigationalData.Latitude.HasValue) &&
                    (true == navigationalData.Longitude.HasValue))
                {
                    var position = new PointLatLng(navigationalData.Latitude.Value, navigationalData.Longitude.Value);
                    _mapMarker.Position = position;

                    _mapMarker.ToolTipMode = ToolTipAppearanceHelper.GetMarkerTooltipMode(_config.ToolTip.Appearance);
                    if (_mapMarker.ToolTipMode != MarkerTooltipMode.Never)
                        _mapMarker.ToolTipText = BuildToolTipText(navigationalData);
                    _mapMarker.IsVisible = true;
                }
                else
                {
                    _mapMarker.IsVisible = false;
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
