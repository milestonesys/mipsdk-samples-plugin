using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace ActiveElementsOverlay.Background
{
    /// <summary>
    /// Background plugin demonstrating how to add different UIElements to Smart Client camera items. 
    /// For simplicity this sample adds the same UIElements to all ImageViewers. In real life it should in most cases be dependent on the camera. Look at the AnalyticsOverlay sample for an example on how to do this.
    /// There is no active part in this background plugin. It is only reacting to events.
    /// </summary>
    public class ActiveElementsOverlayBackgroundPlugin : BackgroundPlugin
    {
        private Dictionary<ImageViewerAddOn, Button> _imageViewerAddOnButtons = new Dictionary<ImageViewerAddOn, Button>();

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return ActiveElementsOverlayDefinition.ActiveElementsOverlayBackgroundPlugin; }
        }

        /// <summary>
        /// The name of this background plugin
        /// </summary>
        public override String Name
        {
            get { return "ActiveElementsOverlay Background Plugin"; }
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
            get { return new List<EnvironmentType>() { EnvironmentType.SmartClient }; } 
        }

        #region Event registration on/off
        /// <summary>
        /// Register all the events we need on the ImageViewerAddOn
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void RegisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.CloseEvent += new EventHandler(ImageViewerAddOn_CloseEvent);
        }

        /// <summary>
        /// Unhook all event handlers for the ImageViewerAddOn
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void UnregisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.CloseEvent -= new EventHandler(ImageViewerAddOn_CloseEvent);
        }
        #endregion

        #region Event handling

        /// <summary>
        /// A new ImageViewer has been created. 
        /// For simplicity this sample adds the same UIElements to all ImageViewers. In real life it should in most cases be dependent on the camera. Look at the AnalyticsOverlay sample for an example on how to do this.
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void NewImageViewerControlEvent(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.ShowMetadataOverlay = false;

            Guid borderId = AddBorder(imageViewerAddOn);

            var button = new Button()
            {
                Content = "Toggle bounding box",
                Width = 150,
                Height = 30,
                Background = Brushes.Transparent,
                Tag = borderId, // storing the ID for the border active element so that we can use it for toggling
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 10, 10, 0)
            };
            button.Click += Button_Click;

            var textBlock = new TextBlock()
            {
                Text = "This is an element" + System.Environment.NewLine + "that doesn't capture any mouse events",
                IsHitTestVisible = false // this will ensure that the text will not capture any mouse events
            };
            // position is set relative to the top of the video being shown - not the entire view item
            Canvas.SetLeft(textBlock, 250);
            Canvas.SetTop(textBlock, 250);

            imageViewerAddOn.ActiveElementsOverlayAdd(new List<FrameworkElement> { button, textBlock }, new ActiveElementsOverlayRenderParameters() { Placement = OverlayPlacement.FollowImageViewport, ShowAlways = false, ZOrder = 1 });

            RegisterEvents(imageViewerAddOn);
            lock (_imageViewerAddOnButtons)
            {
                _imageViewerAddOnButtons[imageViewerAddOn] = button;
            }
        }

        private Guid AddBorder(ImageViewerAddOn imageViewerAddOn)
        {
            var border = new Border()
            {
                BorderBrush = Brushes.Green,
                BorderThickness = new Thickness(2),
                Width = 100,
                Height = 100
            };
            border.MouseDown += Border_MouseDown;
            border.MouseEnter += Border_MouseEnter;
            border.MouseLeave += Border_MouseLeave;
            Canvas.SetLeft(border, 42);
            Canvas.SetTop(border, 42);
            return imageViewerAddOn.ActiveElementsOverlayAdd(new List<FrameworkElement> { border }, new ActiveElementsOverlayRenderParameters() { Placement = OverlayPlacement.FollowDigitalZoom, ShowAlways = true, ZOrder = 2 });
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
                lock (_imageViewerAddOnButtons)
                {
                    _imageViewerAddOnButtons.Remove(imageViewerAddOn);
                }
            }
        }

        /// <summary>
        /// Operator has moved the mouse over the bounding box
        /// </summary>
        private void Border_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // when the mouse is over the button it will change color and become thicker
            (sender as Border).BorderBrush = Brushes.Red;
            (sender as Border).BorderThickness = new Thickness(4);
        }

        /// <summary>
        /// Operator has moved the mouse away from the bounding box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // mouse is leaving so change back to original
            (sender as Border).BorderBrush = Brushes.Green;
            (sender as Border).BorderThickness = new Thickness(2);
        }

        /// <summary>
        /// Operator has clicked the bounding box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Bounding box clicked");
        }

        /// <summary>
        /// Operator has clicked the button, so let's toggle whether we have the bounding box on the camera or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var imageViewerAddOn = _imageViewerAddOnButtons.First(keyValuePair => keyValuePair.Value == button).Key;
            if ((Guid)button.Tag == Guid.Empty)
            {
                button.Tag = AddBorder(imageViewerAddOn);
            }
            else
            {
                imageViewerAddOn.ActiveElementsOverlayRemove((Guid)button.Tag);
                button.Tag = Guid.Empty;
            }
        }
        #endregion
    }
}
