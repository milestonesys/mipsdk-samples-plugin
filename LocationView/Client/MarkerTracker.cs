using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

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
                if ((null == markers) ||
                    (false == markers.Any()))
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
            if (marker.IsVisible)
            {
                _mapControl.Position = new PointLatLng(marker.Position.Lat, marker.Position.Lng);

                result = true;
            }

            return result;
        }

        private void TrackMarker()
        {
            throw new NotImplementedException();

            /* TODO later
            // use these members here
            var stepLatAbs = _mapControl.ViewArea.HeightLat;
            var stepLongAbs = _mapControl.ViewArea.WidthLng;
            */
            // choice strategy - move in area or center the marker in the screen
        }
    }
}
