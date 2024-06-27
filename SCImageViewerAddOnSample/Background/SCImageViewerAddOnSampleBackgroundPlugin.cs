using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace SCImageViewerAddOnSample.Background
{
    /// <summary>
    /// This class is used to keep track of the active ImageViewerAddons. It is useful to use a BackgroundPlugin class for this purpose, 
    /// since it will stay active not matter which workspace or window the user opens.
    /// </summary>
    public class SCImageViewerAddOnSampleBackgroundPlugin : BackgroundPlugin
    {
        private List<ImageViewerAddOn> _imageViewers;

        public override Guid Id => SCImageViewerAddOnSampleDefinition.SCImageViewerAddOnSampleBackgroundPlugin;

        public override string Name => "SCImageViewerAddOnSampleBackgroundPlugin";

        public override List<EnvironmentType> TargetEnvironments => new List<EnvironmentType>() { EnvironmentType.SmartClient };

        public override void Init()
        {
            _imageViewers = new List<ImageViewerAddOn>();

            // Here we subscribe to the event which informs us that a new ImageViewer has been added. 
            ClientControl.Instance.NewImageViewerControlEvent += Instance_NewImageViewerControlEvent;
        }

        /// <summary>
        /// Called by the Environment when the user log's out.
        /// </summary>
        public override void Close()
        {
            // Always remember to unsubscribe from events
            ClientControl.Instance.NewImageViewerControlEvent -= Instance_NewImageViewerControlEvent;
        }

        private void Instance_NewImageViewerControlEvent(ImageViewerAddOn imageViewerAddOn)
        {
            // Whenever we receive a ImageViewerAddon we subscribe to the "CloseEvent", so we can remove it from our list and thereby make certain to have an updated list
            imageViewerAddOn.CloseEvent += ImageViewerAddOn_CloseEvent;
            lock (_imageViewers)
            {
                _imageViewers.Add(imageViewerAddOn);
            }
        }

        private void ImageViewerAddOn_CloseEvent(object sender, System.EventArgs e)
        {
            ImageViewerAddOn closingImageViewer = (ImageViewerAddOn)sender;
            closingImageViewer.CloseEvent -= ImageViewerAddOn_CloseEvent;
            lock (_imageViewers)
            {
                _imageViewers.Remove(closingImageViewer);
            }
        }

        public ImageViewerAddOn FindSelectedImageViewerAddOn()
        {
            lock (_imageViewers)
            {
                // We use the property "IsGlobalSelected" to get the ImageViewerAddon which is currently selected. Note that this won't work with detached windows,
                // because the control is located in the main window, thus clicking on it will always move the global selection back to the main window. Doing this in code that
                // doesn't depend on a button in the main window, however, will work.
                return _imageViewers.FirstOrDefault(x => x.IsGlobalSelected);
            }
        }
    }
}
