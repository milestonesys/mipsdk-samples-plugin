using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    class ShowCameraNameViewItemToolbarPluginInstance : ViewItemToolbarPluginInstance
    {
        private Item _viewItemInstance;
        private Item _window;

        public override void Init(Item viewItemInstance, Item window)
        {
            _viewItemInstance = viewItemInstance;
            _window = window;

            Title = "Show camera name";
            Tooltip = "Click to show current camera name.";
        }

        public override void Activate()
        {
            Item currentCameraItem = null;
            Guid currentCameraId = Guid.Empty;

            // Get hold of the most recent ViewLayout 
            ViewAndLayoutItem viewLayout = (ViewAndLayoutItem)_window.GetChildren()[0];
            List<Item> viewItems = viewLayout.GetChildren();
            int index;
            // Puck out the index we work on
            Int32.TryParse(_viewItemInstance.Properties["Index"], NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out index);
            // Look up the current  (updated) version of the ViewItem
            Item viewItem = viewItems[index];
            Guid.TryParse(viewItem.Properties["CurrentCameraId"], out currentCameraId);

            if (currentCameraId != Guid.Empty)
            {
                currentCameraItem = Configuration.Instance.GetItem(currentCameraId, Kind.Camera);
            }

            if (currentCameraItem != null)
            {
                MessageBox.Show("The current camera is: " + currentCameraItem.Name + " (" + currentCameraItem.FQID.ObjectId + ")");
            }
            else
            {
                MessageBox.Show("Failed to look up current camera.");
            }
        }



        public override void Close()
        {
        }
    }

    class ShowCameraNameViewItemToolbarPlugin : ViewItemToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("FC698CAF-0503-4FC7-87BA-CE9D3212D7D8");

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Shown camera name sample view item toolbar plugin"; }
        }

        public override ToolbarPluginOverflowMode ToolbarPluginOverflowMode
        {
            get { return ToolbarPluginOverflowMode.AsNeeded; }
        }

        public override void Init()
        {
            ViewItemToolbarPlaceDefinition.ViewItemIds = new List<Guid>() { ViewAndLayoutItem.CameraBuiltinId };
            ViewItemToolbarPlaceDefinition.WorkSpaceIds = new List<Guid>() { ClientControl.LiveBuildInWorkSpaceId, ClientControl.PlaybackBuildInWorkSpaceId };
            ViewItemToolbarPlaceDefinition.WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal };
        }

        public override void Close()
        {
        }

        public override ViewItemToolbarPluginInstance GenerateViewItemToolbarPluginInstance()
        {
            return new ShowCameraNameViewItemToolbarPluginInstance();
        }
    }
}
