using System.Collections.Generic;
using GMap.NET.MapProviders;

namespace LocationView.Client.Config
{
    internal enum MapTypes
    {
        BingMap = 0,
        BingSattelite,
        BingHybrid,

        GoogleMap,
        GoogleSattelite,
        GoogleHybrid,
        GoogleTerrain,

        OviMap,
        OviSattelite,
        OviHybrid,
        OviTerrain,

        WikiMapiaMap,

        YahooMap,
        YahooSattelite,
        YahooHybrid,

        YandexMap,
        YandexSattelite,
        YandexHybrid,
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

                case MapTypes.OviMap:
                    result = OviMapProvider.Instance;
                    break;
                case MapTypes.OviSattelite:
                    result = OviSatelliteMapProvider.Instance;
                    break;
                case MapTypes.OviHybrid:
                    result = OviHybridMapProvider.Instance;
                    break;
                case MapTypes.OviTerrain:
                    result = OviTerrainMapProvider.Instance;
                    break;

                case MapTypes.WikiMapiaMap:
                    result = WikiMapiaMapProvider.Instance;
                    break;

                case MapTypes.YahooMap:
                    result = YahooMapProvider.Instance;
                    break;
                case MapTypes.YahooSattelite:
                    result = YahooSatelliteMapProvider.Instance;
                    break;
                case MapTypes.YahooHybrid:
                    result = YahooHybridMapProvider.Instance;
                    break;

                case MapTypes.YandexMap:
                    result = YandexMapProvider.Instance;
                    break;
                case MapTypes.YandexSattelite:
                    result = YandexSatelliteMapProvider.Instance;
                    break;
                case MapTypes.YandexHybrid:
                    result = YandexHybridMapProvider.Instance;
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

                    {MapTypes.OviMap, "Ovi Map"},
                    {MapTypes.OviSattelite, "Ovi Sattelite"},
                    {MapTypes.OviHybrid, "Ovi Hybrid"},
                    {MapTypes.OviTerrain, "Ovi Terrain"},

                    {MapTypes.WikiMapiaMap, "Wikimapia Map"},

                    {MapTypes.YahooMap, "Yahoo Map"},
                    {MapTypes.YahooSattelite, "Yahoo Sattelite"},
                    {MapTypes.YahooHybrid, "Yahoo Hybrid"},

                    {MapTypes.YandexMap, "Yandex Map"},
                    {MapTypes.YandexSattelite, "Yandex Sattelite"},
                    {MapTypes.YandexHybrid, "Yandex Hybrid"},
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