using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocationView.Client
{
    internal class MarkerTracker
    {
        private readonly GMapControl _mapControl;

        private int _markerIndex = -1;

        public MarkerTracker(GMapControl mapControl)
        {
            _mapControl = mapControl;
        }

        public void CenterNextMarker(IEnumerable<GMapMarker> markers)
        {
            try
            {
                if (null == markers || !markers.Any())
                    return;

                var markerCentered = false;
                for (int index = 0; (index < markers.Count()) && (false == markerCentered); index++)
                {
                    markerCentered = TryCenterNextMarker(markers);
                }
            }
            catch (Exception)
            {
            }
        }

        private bool TryCenterNextMarker(IEnumerable<GMapMarker> markers)
        {
            var result = false;

            _markerIndex++;
            _markerIndex %= markers.Count();

            var marker = markers.ElementAt(_markerIndex);
            if (marker.Shape.IsVisible)
            {
                _mapControl.Position = new PointLatLng(marker.Position.Lat, marker.Position.Lng);

                result = true;
            }

            return result;
        }
    }
}
