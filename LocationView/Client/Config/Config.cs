using System;
using System.Collections.Generic;
using GMap.NET;

namespace LocationView.Client.Config
{
    internal class Marker
    {
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
        public MarkerTypes MarkerType { get; set; }

        public void InitFrom(Marker other)
        {
            DeviceId = other.DeviceId;
            DeviceName = other.DeviceName;
            MarkerType = other.MarkerType;
        }
    }

    internal class ToolTip
    {
        public ToolTipAppearanceTypes Appearance { get; set; }
        public ToolTipTextTypes TextType { get; set; }
    }

    internal class Constants
    {
        internal static string GMapProviderType = "GMapProviderType";
        internal static string ShowZoomPanel = "ShowZoomPanel";
        internal static string TimeoutSeconds = "TimeoutSeconds";
        internal static string MarkersCount = "MarkersCount";
        internal static string MarkerId = "MarkerId";
        internal static string MarkerColor = "MarkerColor";
        internal static string ToolTipAppearance = "ToolTipAppearance";
        internal static string ToolTipText = "ToolTipText";

        internal static string MapPositionLng = "MapPositionLng";
        internal static string MapPositionLat = "MapPositionLat";
        internal static string MapZoomFactor = "MapZoomFactor";
    }

    internal class Config
    {
        public const int MapZoomLevelMinValue = 1;
        public const int MapZoomLevelMaxValue = 20;
        public const int MapZoomLevelInitialValue = 17;

        public Config()
        {
            ToolTip = new ToolTip()
                      {
                          Appearance = ToolTipAppearanceTypes.OnMouse,
                          TextType = ToolTipTextTypes.Location,
                      };
            MapZoomLevel = MapZoomLevelInitialValue;
            MapPosition = new PointLatLng(0, 0);
            TimeoutInSeconds = 10;
        }

        public delegate string PropertyGetter(string name);
        public delegate void PropertySetter(string name, string value);

        public delegate string ItemNameGetter(Guid itemId);

        public readonly List<Marker> Markers = new List<Marker>();

        public MapTypes MapType { get; set; }

        public ToolTip ToolTip { get; private set; }

        public int TimeoutInSeconds { get; set; }

        public bool ShowZoomPanel { get; set; }

        public int MapZoomLevel { get; set; }

        public PointLatLng MapPosition { get; set; }

        public void ReadConfiguration(PropertyGetter propertyGetter)
        {
            // read markers
            var count = 0;
            try { count = Convert.ToInt32(propertyGetter(Constants.MarkersCount)); }
            catch {}

            Markers.Clear();
            for (int index = 0; index < count; index++)
            {
                try
                {
                    var marker = new Marker();
                    marker.DeviceId = new Guid(propertyGetter(GetIndexedName(Constants.MarkerId, index)));
                    marker.MarkerType = MarkerNames.GetType(propertyGetter(GetIndexedName(Constants.MarkerColor, index)));
                    Markers.Add(marker);
                }
                catch
                {
                }
            }

            // read map type
            MapType = MapProviderNames.GetType(propertyGetter(Constants.GMapProviderType));

            // read tooltip
            ToolTip.Appearance = ToolTipAppearanceNames.GetType(propertyGetter(Constants.ToolTipAppearance));
            ToolTip.TextType = ToolTipTextNames.GetType(propertyGetter(Constants.ToolTipText));
            if (ToolTip.TextType < ToolTipTextTypes.Name)
                ToolTip.TextType = ToolTipTextTypes.Name;

            // read timeout in seconds
            try
            {
                var timeOutSecondsString = propertyGetter(Constants.TimeoutSeconds);
                if (!string.IsNullOrWhiteSpace(timeOutSecondsString))
                    TimeoutInSeconds = Convert.ToInt32(timeOutSecondsString);
            }
            catch { }
            

            // read show zoom panel
            ShowZoomPanel = (0 == String.Compare(propertyGetter(Constants.ShowZoomPanel),
                true.ToString(), StringComparison.OrdinalIgnoreCase));

            // read map zoom level
            try { MapZoomLevel = Convert.ToInt32(propertyGetter(Constants.MapZoomFactor)); }
            catch { MapZoomLevel = MapZoomLevelInitialValue; }
            MapZoomLevel = Math.Min(MapZoomLevel, MapZoomLevelMaxValue);
            MapZoomLevel = Math.Max(MapZoomLevel, MapZoomLevelMinValue);

            // read map position
            try
            {
                var position = MapPosition;
                position.Lng = Convert.ToDouble(propertyGetter(Constants.MapPositionLng));
                position.Lat = Convert.ToDouble(propertyGetter(Constants.MapPositionLat));
                MapPosition = position;
            }
            catch { }
        }

        public void UpdateItemNames(ItemNameGetter itemNameGetter)
        {
            foreach (var marker in Markers)
            {
                marker.DeviceName = itemNameGetter(marker.DeviceId);
            }
        }

        public void WriteConfiguration(PropertySetter propertySetter)
        {
            // write markers
            var count = Markers.Count;
            propertySetter(Constants.MarkersCount, count.ToString());

            for (int index = 0; index < count; index++)
            {
                var marker = Markers[index];
                propertySetter(GetIndexedName(Constants.MarkerId, index), marker.DeviceId.ToString());
                propertySetter(GetIndexedName(Constants.MarkerColor, index), MarkerNames.GetName(marker.MarkerType));
            }

            // write map type
            propertySetter(Constants.GMapProviderType, MapProviderNames.GetName(MapType));

            // write tooltip
            propertySetter(Constants.ToolTipAppearance, ToolTipAppearanceNames.GetName(ToolTip.Appearance));
            propertySetter(Constants.ToolTipText, ToolTipTextNames.GetName(ToolTip.TextType));

            // write timout in seconds
            propertySetter(Constants.TimeoutSeconds, TimeoutInSeconds.ToString());

            // write show zoom panel
            propertySetter(Constants.ShowZoomPanel, ShowZoomPanel.ToString());

            // write map zoom level
            propertySetter(Constants.MapZoomFactor, MapZoomLevel.ToString());

            // write map position
            propertySetter(Constants.MapPositionLng, MapPosition.Lng.ToString());
            propertySetter(Constants.MapPositionLat, MapPosition.Lat.ToString());
        }

        private string GetIndexedName(string name, int index)
        {
            return name + index;
        }
    }
}
