using System.Collections.Generic;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace LocationView.Client.Config
{
    internal enum MarkerTypes
    {
        Blue,
        Green,
        LightBlue, 
        Orange,
        Pink,
        Purple,
        Red,
        Yellow,
    }

    internal class MarkerTypesHelper
    {
        public static readonly Dictionary<MarkerTypes, GMarkerGoogleType> Markers =
            new Dictionary<MarkerTypes, GMarkerGoogleType>()
            {
                {MarkerTypes.Blue, GMarkerGoogleType.blue},
                {MarkerTypes.Green, GMarkerGoogleType.green},
                {MarkerTypes.LightBlue, GMarkerGoogleType.lightblue},
                {MarkerTypes.Orange, GMarkerGoogleType.orange},
                {MarkerTypes.Pink, GMarkerGoogleType.pink},
                {MarkerTypes.Purple, GMarkerGoogleType.purple},
                {MarkerTypes.Red, GMarkerGoogleType.red},
                {MarkerTypes.Yellow, GMarkerGoogleType.yellow},
            };
    }

    internal class MarkerFactory
    {
        public static GMapMarker CreateMarker(MarkerTypes type, PointLatLng point = new PointLatLng())
        {
            var googleType = GMarkerGoogleType.blue;
            if (MarkerTypesHelper.Markers.ContainsKey(type))
                googleType = MarkerTypesHelper.Markers[type];

            return new GMarkerGoogle(point, googleType);
        }
    }

    internal class MarkerNames
    {
        private static readonly Dictionary<MarkerTypes, string> Names =
            new Dictionary<MarkerTypes, string>()
            {
                {MarkerTypes.Blue, "Blue"},
                {MarkerTypes.Green, "Green"},
                {MarkerTypes.LightBlue, "Light blue"},
                {MarkerTypes.Orange, "Orange"},
                {MarkerTypes.Pink, "Pink"},
                {MarkerTypes.Purple, "Purple"},
                {MarkerTypes.Red, "Red"},
                {MarkerTypes.Yellow, "Yellow"},
            };

        public static string GetName(MarkerTypes type)
        {
            return TypesHelper.GetName(Names, type);
        }

        public static MarkerTypes GetType(string name)
        {
            return TypesHelper.GetType(Names, name);
        }
    }

}
