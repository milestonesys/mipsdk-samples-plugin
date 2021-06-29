using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace TimelineViewItem.Background
{
    public class TimelineBackgroundPlugin : BackgroundPlugin
	{
        Guid _cameraId;
        BackgroundTimelineRibbonSource _timelineSource = new BackgroundTimelineRibbonSource();

        #region abstract class implementation

        public override Guid Id
		{
			get { return new Guid("72FABD7F-6251-4A89-BA8C-D1FB26ADDDFD"); }
		}

		public override void Init()
		{
			ClientControl.Instance.NewImageViewerControlEvent += new ClientControl.NewImageViewerControlHandler(NewImageViewerControlEvent);
		}

		public override void Close()
		{
			ClientControl.Instance.NewImageViewerControlEvent -= new ClientControl.NewImageViewerControlHandler(NewImageViewerControlEvent);
		}

		public override string Name
		{
			get { return "TimelineBackgroundPlugin"; }
		}
		
		/// <summary>
		/// Only run in the Smart Client
		/// </summary>
		public override List<EnvironmentType> TargetEnvironments
		{
			get
			{
				return new List<EnvironmentType>() { EnvironmentType.SmartClient };
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// A new ImageViewer has been created
		/// </summary>
		/// <param name="imageViewerAddOn"></param>
		void NewImageViewerControlEvent(ImageViewerAddOn imageViewerAddOn)
		{
            if (imageViewerAddOn.ImageViewerType == VideoOS.Platform.Client.ImageViewerType.CameraViewItem)
            {
                // we just add sources for image viewer addons that has same camera as the first we see, 
                // but in a real scenario it should of course only be done for ones for which we have relevant data
                if (_cameraId == Guid.Empty || (imageViewerAddOn.CameraFQID != null && imageViewerAddOn.CameraFQID.ObjectId == _cameraId))
                {
                    imageViewerAddOn.RegisterTimelineSequenceSource(_timelineSource);
                    RegisterEvents(imageViewerAddOn);
                    if (imageViewerAddOn.CameraFQID != null)
                    {
                        _cameraId = imageViewerAddOn.CameraFQID.ObjectId;
                    }
                }
            }
		}

        /// <summary>
        /// Register all the events we need
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void RegisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.PropertyChangedEvent += imageViewerAddOn_PropertyChangedEvent;
            imageViewerAddOn.CloseEvent += ImageViewerAddOn_CloseEvent;
        }
        
        /// <summary>
        /// Unhook all my event handlers
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void UnregisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.PropertyChangedEvent -= imageViewerAddOn_PropertyChangedEvent;
            imageViewerAddOn.CloseEvent -= ImageViewerAddOn_CloseEvent;
        }


        /// <summary>
        /// Some property has changed on the cameraview item (or hotspot or ...)
        /// Lets check that the camera id is the one we watch for.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void imageViewerAddOn_PropertyChangedEvent(object sender, EventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null)
            {
                if (imageViewerAddOn.CameraFQID.ObjectId == _cameraId)
                {
                    imageViewerAddOn.RegisterTimelineSequenceSource(_timelineSource);
                }
                else
                {
                    imageViewerAddOn.UnregisterTimelineSequenceSource(_timelineSource);
                }
            }
        }

        /// <summary>
        /// One of the ImageViewer has been closed / Removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageViewerAddOn_CloseEvent(object sender, EventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null)
            {
                UnregisterEvents(imageViewerAddOn);
                // sequence source is automatically unregistered when image viewer is closed
            }
        }
        #endregion

    }
}
