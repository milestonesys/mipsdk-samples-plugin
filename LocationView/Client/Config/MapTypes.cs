using System.Collections.Generic;
using GMap.NET.MapProviders;

namespace LocationView.Client.Config
{
    public enum MapTypes
    {
        BingMap = 0,
        BingSattelite,
        BingHybrid,

        GoogleMap,
        GoogleSattelite,
        GoogleHybrid,
        GoogleTerrain,

        WikiMapiaMap
    }

    internal class MapProviderFactory
    {
        public static GMapProvider CreateMapProvider(MapTypes mapType)
        {
            GMapProvider result = null;

            switch (mapType)
            {
                case MapTypes.BingMap: 
                    result = BingMapProvider.Instance;
                    break;
                case MapTypes.BingSattelite:
                    result = BingSatelliteMapProvider.Instance;
                    break;
                case MapTypes.BingHybrid:
                    result = BingHybridMapProvider.Instance;
                    break;

                case MapTypes.GoogleMap:
                    result = GoogleMapProvider.Instance;
                    break;
                case MapTypes.GoogleSattelite:
                    result = GoogleSatelliteMapProvider.Instance;
                    break;
                case MapTypes.GoogleHybrid:
                    result = GoogleHybridMapProvider.Instance;
                    break;
                case MapTypes.GoogleTerrain:
                    result = GoogleTerrainMapProvider.Instance;
                    break;

                case MapTypes.WikiMapiaMap:
                    result = WikiMapiaMapProvider.Instance;
                    break;

                default:
                    result = GoogleMapProvider.Instance;
                    break;
            }

            return result;
        }
    }

    internal class MapProviderNames
    {
        private static readonly Dictionary<MapTypes, string> Names =
            new Dictionary<MapTypes, string>()
                {
                    {MapTypes.BingMap, "Bing Map"},
                    {MapTypes.BingSattelite, "Bing Sattelite"},
                    {MapTypes.BingHybrid, "Bing Hybrid"},

                    {MapTypes.GoogleMap, "Google Map"},
                    {MapTypes.GoogleSattelite, "Google Sattelite"},
                    {MapTypes.GoogleHybrid, "Google Hybrid"},
                    {MapTypes.GoogleTerrain, "Google Terrain"},

                    {MapTypes.WikiMapiaMap, "Wikimapia Map"},
                };

        public static string GetName(MapTypes type)
        {
            return TypesHelper.GetName(Names, type);
        }

        public static MapTypes GetType(string name)
        {
            return TypesHelper.GetType(Names, name);
        }
    }
}