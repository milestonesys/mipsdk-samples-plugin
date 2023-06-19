using System.Collections.Generic;
using System.Linq;
using GMap.NET.WindowsPresentation;
using VideoOS.Platform.Client;

namespace LocationView.Client
{
    internal class NaviItemsList
    {
        private readonly List<NavigationalItem> _navigationalItems = new List<NavigationalItem>();

        public void Init(Config.Config config, WindowInformation windowInformation)
        {
            ClearItems();

            foreach (var marker in config.Markers)
            {
                var navigationalMarker = new NavigationalItem(config, marker, windowInformation);
                _navigationalItems.Add(navigationalMarker);
            }
        }

        public void ClearItems()
        {
            foreach (var navigationalItem in _navigationalItems)
            {
                navigationalItem.StopSupplier();
            }

            _navigationalItems.Clear();
        }

        public void StartItems()
        {
            foreach (var navigationalItem in _navigationalItems)
            {
                navigationalItem.StartSupplier();
            }
        }

        public IEnumerable<GMapMarker> GMapMarkers
        {
            get
            {
                return (from item in _navigationalItems
                        select item.MapMarker).ToList();
            }
        }

        public void AddMarkers(ICollection<GMapMarker> collection)
        {
            collection.Clear();

            foreach (var gMapMarker in GMapMarkers)
            {
                collection.Add(gMapMarker);
            }
        }
    }
}
