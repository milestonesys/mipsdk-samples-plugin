using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCClientAction
{
    internal static class ImageViewerHelper
    {
        private static readonly List<ImageViewerAddOn> _imageViewerAddOns = new List<ImageViewerAddOn>();

        public static void Init()
        {
            ClientControl.Instance.NewImageViewerControlEvent += Instance_NewImageViewerControlEvent;
        }

        public static void Close()
        {
            ClientControl.Instance.NewImageViewerControlEvent -= Instance_NewImageViewerControlEvent;
            foreach (ImageViewerAddOn imageViewerAddOn in _imageViewerAddOns)
            {
                imageViewerAddOn.CloseEvent -= ImageViewerAddOnOnCloseEvent;
            }
            _imageViewerAddOns.Clear();
        }

        private static void Instance_NewImageViewerControlEvent(ImageViewerAddOn imageViewerAddOn)
        {
            if (imageViewerAddOn.ImageViewerType == ImageViewerType.CameraViewItem)
            {
                imageViewerAddOn.CloseEvent += ImageViewerAddOnOnCloseEvent;
                _imageViewerAddOns.Add(imageViewerAddOn);
            }
        }

        private static void ImageViewerAddOnOnCloseEvent(object sender, EventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = (ImageViewerAddOn)sender;
            imageViewerAddOn.CloseEvent -= ImageViewerAddOnOnCloseEvent;
            _imageViewerAddOns.Remove(imageViewerAddOn);
        }

        public static ImageViewerAddOn GetGlobalSelectedImageViewer()
        {
            return _imageViewerAddOns.SingleOrDefault(x => x.IsGlobalSelected);
        }
    }
}
