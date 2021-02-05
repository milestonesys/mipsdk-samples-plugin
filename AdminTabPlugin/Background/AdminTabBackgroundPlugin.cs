using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using FormattedText = System.Windows.Media.FormattedText;
using Size = System.Drawing.Size;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;
using Typeface = System.Windows.Media.Typeface;

namespace AdminTabPlugin.Background
{
    public partial class AdminTabBackgroundPlugin : BackgroundPlugin
    {
        #region private fields

        private List<ImageViewerAddOn> _activeImageViewerAddOns = new List<ImageViewerAddOn>();

        private Dictionary<ImageViewerAddOn, Guid> _dicShapes = new Dictionary<ImageViewerAddOn, Guid>();

        private Dictionary<Guid, AssociatedProperties> _cameraAssociatedProperties = new Dictionary<Guid, AssociatedProperties>();
        #endregion

        public AdminTabBackgroundPlugin()
        {

        }

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return AdminTabPluginDefinition.AdminTabPluginBackgroundPlugin; }
        }

        /// <summary>
        /// The name of this background plugin
        /// </summary>
        public override String Name
        {
            get { return "AdminTabPlugin BackgroundPlugin"; }
        }

        /// <summary>
        /// Called by the Environment when the user has logged in.
        /// </summary>
        public override void Init()
        {
            ClientControl.Instance.NewImageViewerControlEvent += new ClientControl.NewImageViewerControlHandler(NewImageViewerControlEvent);
        }

        /// <summary>
        /// Called by the Environment when the user log's out.
        /// You should close all remote sessions and flush cache information, as the
        /// user might logon to another server next time.
        /// </summary>
        public override void Close()
        {
            ClientControl.Instance.NewImageViewerControlEvent -= new ClientControl.NewImageViewerControlHandler(NewImageViewerControlEvent);
        }

        /// <summary>
        /// Define in what Environments the current background task should be started.
        /// </summary>
        public override List<EnvironmentType> TargetEnvironments
        {
            get { return new List<EnvironmentType>() { EnvironmentType.SmartClient }; }	// will run in the Smart Client
        }


        #region Event registration on/off
        /// <summary>
        /// Register all the events we need
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void RegisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.CloseEvent += new EventHandler(ImageViewerAddOn_CloseEvent);
            imageViewerAddOn.StartLiveEvent += new PassRequestEventHandler(ImageViewerAddOn_StartLiveEvent);
            imageViewerAddOn.StopLiveEvent += new PassRequestEventHandler(ImageViewerAddOn_StopLiveEvent);
        }

        /// <summary>
        /// Unhook all my event handlers
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void UnregisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.CloseEvent -= new EventHandler(ImageViewerAddOn_CloseEvent);
            imageViewerAddOn.StartLiveEvent -= new PassRequestEventHandler(ImageViewerAddOn_StartLiveEvent);
            imageViewerAddOn.StopLiveEvent -= new PassRequestEventHandler(ImageViewerAddOn_StopLiveEvent);
        }
        #endregion

        /// <summary>
        /// A new ImageViewer has been created
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void NewImageViewerControlEvent(ImageViewerAddOn imageViewerAddOn)
        {
            lock (_activeImageViewerAddOns)
            {
                RegisterEvents(imageViewerAddOn);
                _activeImageViewerAddOns.Add(imageViewerAddOn);
            }
        }

        /// <summary>
        /// The smart client is now going into setup or playback mode (Or just this one camera is in instant playback mode)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageViewerAddOn_StopLiveEvent(object sender, PassRequestEventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null)
            {
                ClearOverlay(imageViewerAddOn);
            }
        }

        /// <summary>
        /// The Smart Client is now going into live mode.  We would overtake or reject that this item is going into live.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageViewerAddOn_StartLiveEvent(object sender, PassRequestEventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;  // todo does this do anything (is the obects always null)?
            if (imageViewerAddOn != null)
            {
                drawGraphOverlay(imageViewerAddOn);
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
                ClearOverlay(imageViewerAddOn);

                lock (_activeImageViewerAddOns)
                {
                    // Remove from list
                    if (_activeImageViewerAddOns.Contains(imageViewerAddOn))
                        _activeImageViewerAddOns.Remove(imageViewerAddOn);
                }
            }
        }

        #region Drawing the overlay

        private void ClearOverlay(ImageViewerAddOn imageViewerAddOn)
        {
            try
            {
                // Clear the overlay
                Guid shapeID;
                if (_dicShapes.TryGetValue(imageViewerAddOn, out shapeID))
                {
                    ClientControl.Instance.CallOnUiThread(() => imageViewerAddOn.ShapesOverlayRemove(shapeID));
                    _dicShapes.Remove(imageViewerAddOn);
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("ImageViewerAddOn_ClearOverlay", ex);
            }
        }

        /// <summary>
        /// Draw the overlay on one specific imagevieweraddon
        /// </summary>
        /// <param name="addOn"></param>
        private void drawGraphOverlay(ImageViewerAddOn addOn)
        {
            try
            {
                AssociatedProperties associatedProperties;
                if (!_cameraAssociatedProperties.TryGetValue(addOn.CameraFQID.ObjectId, out associatedProperties))
                {
                    var cameraItem = Configuration.Instance.GetItem(addOn.CameraFQID);
                    associatedProperties = Configuration.Instance.GetAssociatedProperties(cameraItem, AdminTabPluginDefinition.AdminTabPluginTabPlugin); 

                    _cameraAssociatedProperties[addOn.CameraFQID.ObjectId] = associatedProperties;
                }

                string property1 = associatedProperties.Properties.ContainsKey("Property1") ? associatedProperties.Properties["Property1"] : null;
                string property2 = associatedProperties.Properties.ContainsKey("Property2") ? associatedProperties.Properties["Property2"] : null;

                List<Shape> shapes = new List<Shape>();
                Size size = new Size(320, 240);
                if (property1 != null)
                    shapes.Add(CreateTextShape(size, "Property1="+property1, 100, 0, 100, System.Windows.Media.Colors.Red));

                if (property2 != null)
                    shapes.Add(CreateTextShape(size, "Property2="+property2, 100, 100, 100, System.Windows.Media.Colors.Red));

                if (!_dicShapes.ContainsKey(addOn))
                {
                    _dicShapes.Add(addOn, addOn.ShapesOverlayAdd(shapes, new ShapesOverlayRenderParameters() { ZOrder = 100 }));
                }
                else
                {
                    addOn.ShapesOverlayUpdate(_dicShapes[addOn], shapes, new ShapesOverlayRenderParameters() { ZOrder = 100 });
                }
            }
            catch (Exception ex) 
            {
                EnvironmentManager.Instance.Log(true, "AdminTabBackgrounPlugin","DrawOverlay(UI):" + ex.Message);
            }
        }

        /// <summary>
        /// scale values of 0 - 1000 will be used to calculate the right placement of true display values
        /// </summary>
        /// <param name="size"></param>
        /// <param name="text"></param>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <param name="scaleFontSize"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private static Shape CreateTextShape(Size size, string text, double scaleX, double scaleY, double scaleFontSize, System.Windows.Media.Color color)
        {
            //Debug.WriteLine(text + " paint size (" + size.Height + "," + size.Width + ")");
            double x = (size.Width * scaleX) / 1000;
            double y = (size.Height * scaleY) / 1000;
            double fontsize = (size.Height * scaleFontSize) / 1000;
            if (fontsize < 7) fontsize = 12;

            return CreateTextShape(text, x, y, fontsize, color);
        }

        private static Shape CreateTextShape(string text, double placeX, double placeY, double fontSize, System.Windows.Media.Color color)
        {
            Shape textShape;
            System.Windows.Media.FontFamily fontFamily = new System.Windows.Media.FontFamily("Times New Roman");
            Typeface typeface = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Bold, new FontStretch());
            Path path = new Path();

            FormattedText fText = new FormattedText(text, System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, typeface, fontSize, System.Windows.Media.Brushes.Black, System.Windows.Media.VisualTreeHelper.GetDpi(path).PixelsPerDip);

            System.Windows.Point textPosition1;
            textPosition1 = new System.Windows.Point(placeX, placeY);
            
            path.Data = fText.BuildGeometry(textPosition1);
            path.Fill = new SolidColorBrush(color);
            textShape = path;
            return textShape;
        }


        #endregion
    }
}

