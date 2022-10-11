using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    internal static class CameraNameResolver
    {
        internal static string ResolveCameraName(Item viewItemInstance, Item window)
        {
            Item currentCameraItem = null;
            Guid currentCameraId = Guid.Empty;

            // Get hold of the most recent ViewLayout 
            ViewAndLayoutItem viewLayout = (ViewAndLayoutItem)window.GetChildren()[0];
            List<Item> viewItems = viewLayout.GetChildren();
            int index;
            // Pick out the index we work on
            Int32.TryParse(viewItemInstance.Properties["Index"], NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out index);
            // Look up the current  (updated) version of the ViewItem
            Item viewItem = viewItems[index];
            Guid.TryParse(viewItem.Properties["CurrentCameraId"], out currentCameraId);

            if (currentCameraId != Guid.Empty)
            {
                currentCameraItem = Configuration.Instance.GetItem(currentCameraId, Kind.Camera);
            }

            if (currentCameraItem != null)
            {
                return "The current camera is: " + currentCameraItem.Name + " (" + currentCameraItem.FQID.ObjectId + ")";
            }
            return "Failed to look up current camera.";
        }
    }
}
