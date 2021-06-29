using CameraPlaybackControl.Background;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace CameraPlaybackControl.Client
{

    /// <summary>
    /// This class handles the logic of our SidePanel. It ensures to enable/disable the right buttons and update the UI with the correct information
    /// </summary>
    public partial class CameraPlaybackControlSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        const string TimeFormat = "HH:mm:ss:fff";

        object _itemViewChanged;
        CameraPlaybackControlBackgroundPlugin _backgroundPlugin;
        ImageViewerAddOn _currentImageViewer;
  
        /// <summary>
        /// The constructor of our SidePanel
        /// </summary>
        /// <param name="backgroundPlugin">A reference to the background plugin where we can access the ImageViewerAddons</param>
        public CameraPlaybackControlSidePanelWpfUserControl(CameraPlaybackControlBackgroundPlugin backgroundPlugin)
        {
            InitializeComponent();
            _backgroundPlugin = backgroundPlugin;
        }
       
        public override void Init()
        {
            // We subscribe to messages from the SmartClient which informs us the a new ViewItem has been selected.
            _itemViewChanged = EnvironmentManager.Instance.RegisterReceiver(MessageReceived, new MessageIdFilter(MessageId.SmartClient.SelectedViewItemChangedIndication));
        }

        private object MessageReceived(Message message, FQID destination, FQID sender)
        {
            var selectedImageViewAddon = _backgroundPlugin.FindSelectedImageViewAddOn();

            ResetButtons();

            ChangeCurrentImageViewer(selectedImageViewAddon);
            if (_currentImageViewer == null)
            {
                return null;
            }
            else
            {
                SetEnableDisableButtonsState(_currentImageViewer.IndependentPlaybackEnabled);
                SetPlaybackButtonState(_currentImageViewer.IndependentPlaybackController.PlaybackMode);

                if(_currentImageViewer.IndependentPlaybackEnabled)
                {
                    SetPlaybackButtonState(_currentImageViewer.IndependentPlaybackController.PlaybackMode);
                    labelPlaybackTime.Content = _currentImageViewer.IndependentPlaybackController.PlaybackTime.ToLocalTime().ToString(TimeFormat);
                }
                else
                {
                    labelPlaybackTime.Content = "Not Available";
                }
            }
            return null;
        }

        private void PlaybackModeChangedEvent(object sender, PlaybackController.PlaybackModeEventArgs e)
        {
            SetPlaybackButtonState(e.PlaybackMode);
        }

        private void SetPlaybackButtonState(PlaybackController.PlaybackModeType playbackMode)
        {
            if(!_currentImageViewer.IndependentPlaybackEnabled)
            {
                buttonStartPlayback.IsEnabled = false;
                buttonStopPlayback.IsEnabled = false;
                return;
            }

            if(playbackMode == PlaybackController.PlaybackModeType.Stop)
            {
                buttonStartPlayback.IsEnabled = true;
                buttonStopPlayback.IsEnabled = false;
            }else
            {
                buttonStartPlayback.IsEnabled = false;
                buttonStopPlayback.IsEnabled = true;
            }
        }

        private void ChangeCurrentImageViewer(ImageViewerAddOn selectedImageViewAddon)
        {
            if (_currentImageViewer == null && selectedImageViewAddon != null)
            {
                _currentImageViewer = selectedImageViewAddon;
                _currentImageViewer.IndependentPlaybackModeChangedEvent += IndependentPlaybackModeChangedHandler;
            }

            if (_currentImageViewer != null && selectedImageViewAddon != _currentImageViewer)
            {
                RemoveIndependentPlaybackEvents(_currentImageViewer);
                _currentImageViewer = selectedImageViewAddon;
            }

            if (_currentImageViewer != null)
            {
                SetupNewIndependentPlaybackEvents(_currentImageViewer);
            }
        }

        private void RemoveIndependentPlaybackEvents(ImageViewerAddOn currentImageViewer)
        {
            _currentImageViewer.IndependentPlaybackModeChangedEvent -= IndependentPlaybackModeChangedHandler;
            if (_currentImageViewer.IndependentPlaybackController != null)
            {
                // In order to only receive events from one ImageViewerAddon at a time. We unsubscribe to the event, when a new ViewItem is selected
                _currentImageViewer.IndependentPlaybackController.PlaybackTimeChangedEvent -= PlaybackTimeChangedEvent;
                _currentImageViewer.IndependentPlaybackController.PlaybackModeChangedEvent -= PlaybackModeChangedEvent;
            }
        }

        private void SetupNewIndependentPlaybackEvents(ImageViewerAddOn currentImageViewer)
        {
            _currentImageViewer.IndependentPlaybackModeChangedEvent += IndependentPlaybackModeChangedHandler;

            // When a new ImageViewerAddon is selected and independent playback is enabled, we subscribe to the PlaybackTimeChangedEvent in order to show the current time in the ViewItem
            _currentImageViewer.IndependentPlaybackController.PlaybackTimeChangedEvent += PlaybackTimeChangedEvent;
            _currentImageViewer.IndependentPlaybackController.PlaybackModeChangedEvent += PlaybackModeChangedEvent;
        }

        private void IndependentPlaybackModeChangedHandler(object sender, IndependentPlaybackModeEventArgs e)
        {
            SetEnableDisableButtonsState(e.IndependentPlaybackEnabled);
            SetPlaybackButtonState(PlaybackController.PlaybackModeType.Stop);
        }

        private void ResetButtons()
        {
            buttonDisable.IsEnabled = false;
            buttonEnable.IsEnabled = false;
            buttonStartPlayback.IsEnabled = false;
            buttonStopPlayback.IsEnabled = false;
        }

        private void SetEnableDisableButtonsState(bool playbackEnabled)
        {
            if (playbackEnabled)
            {
                buttonDisable.IsEnabled = true;
                buttonEnable.IsEnabled = false;
            }
            else
            {
                buttonDisable.IsEnabled = false;
                buttonEnable.IsEnabled = true;
            }
        }

        public override void Close()
        {
            if (_itemViewChanged != null)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(_itemViewChanged);
            }
            
            if (_currentImageViewer != null && _currentImageViewer.IndependentPlaybackEnabled)
            {
                _currentImageViewer.IndependentPlaybackController.PlaybackTimeChangedEvent -= PlaybackTimeChangedEvent;
            }

            base.Close();
        }

        private void ButtonDisable_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.IndependentPlaybackController.PlaybackTimeChangedEvent -= PlaybackTimeChangedEvent;
            _currentImageViewer.IndependentPlaybackEnabled = false;
            SetEnableDisableButtonsState(false);
            SetPlaybackButtonState(PlaybackController.PlaybackModeType.Stop);
        }

        private void ButtonEnable_Click(object sender, RoutedEventArgs e)
        {
            // The ImageViewAddon.IndependentPlaybackController will first be available after IndependentPlaybackEnabled is set to enabled. 
            // Therefore we have to set this to true before we access the IndependentPlaybackController
            _currentImageViewer.IndependentPlaybackEnabled = true;
            _currentImageViewer.IndependentPlaybackController.PlaybackTimeChangedEvent += PlaybackTimeChangedEvent;
            SetEnableDisableButtonsState(true);
            SetPlaybackButtonState(_currentImageViewer.IndependentPlaybackController.PlaybackMode);
        }

        private void ButtonStartPlayback_Click(object sender, RoutedEventArgs e)
        {
            // We can control the playback through IndependentPlaybackController.PlaybackMode
            _currentImageViewer.IndependentPlaybackController.PlaybackMode = PlaybackController.PlaybackModeType.Forward;
            SetPlaybackButtonState(PlaybackController.PlaybackModeType.Forward);
        }

        private void ButtonStopPlayback_Click(object sender, RoutedEventArgs e)
        {
            // We can control the playback through IndependentPlaybackController.PlaybackMode
            _currentImageViewer.IndependentPlaybackController.PlaybackMode = PlaybackController.PlaybackModeType.Stop;
            SetPlaybackButtonState(PlaybackController.PlaybackModeType.Stop);
        }

        private void PlaybackTimeChangedEvent(object sender, PlaybackController.TimeEventArgs e)
        {
            string playbackTime = "Not Available";
            if (_currentImageViewer != null && _currentImageViewer.IndependentPlaybackEnabled && _currentImageViewer.IndependentPlaybackController.PlaybackTime != null)
            {
                playbackTime = _currentImageViewer.IndependentPlaybackController.PlaybackTime.ToLocalTime().ToString(TimeFormat);
            }
            labelPlaybackTime.Content = playbackTime;
        }
    }
}
