using SCImageViewerAddOnSample.Background;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCImageViewerAddOnSample.Client
{

    /// <summary>
    /// This class handles the logic of our SidePanel. It ensures to enable/disable the right buttons and update the UI with the correct information
    /// </summary>
    public partial class SCImageViewerAddOnSampleSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        private const string TimeFormat = "HH:mm:ss:fff";

        private object _viewItemChanged;
        private SCImageViewerAddOnSampleBackgroundPlugin _backgroundPlugin;
        private ImageViewerAddOn _currentImageViewer;
        private double _refWidth = short.MaxValue;
        private double _refHeight = short.MaxValue;
        private Timer _timer;

        /// <summary>
        /// The constructor of our SidePanel
        /// </summary>
        /// <param name="backgroundPlugin">A reference to the background plugin where we can access the ImageViewerAddons</param>
        public SCImageViewerAddOnSampleSidePanelWpfUserControl(SCImageViewerAddOnSampleBackgroundPlugin backgroundPlugin)
        {
            InitializeComponent();
            _backgroundPlugin = backgroundPlugin;
            UpdateImageViewerAddOn();
        }

        public override void Init()
        {
            // We subscribe to messages from the SmartClient which informs us when a new ViewItem has been selected.
            _viewItemChanged = EnvironmentManager.Instance.RegisterReceiver(SelectedViewItemChangedReceived, new MessageIdFilter(MessageId.SmartClient.SelectedViewItemChangedIndication));

            if (_timer == null)
            {
                _timer = new Timer(1000);
                _timer.AutoReset = true;
                _timer.Elapsed += Timer_Elapsed;
                _timer.Enabled = true;
            }
        }

        public override void Close()
        {
            if (_viewItemChanged != null)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(_viewItemChanged);
            }

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }

            if (_currentImageViewer != null && _currentImageViewer.IndependentPlaybackEnabled)
            {
                _currentImageViewer.IndependentPlaybackController.PlaybackTimeChangedEvent -= PlaybackTimeChangedEvent;
            }

            base.Close();
        }

        private void UpdateImageViewerAddOn()
        {
            var selectedImageViewerAddon = _backgroundPlugin.FindSelectedImageViewerAddOn();

            ResetButtons();

            ChangeCurrentImageViewer(selectedImageViewerAddon);

            if (_currentImageViewer?.IndependentPlaybackController != null)
            {
                SetEnableDisableButtonsState(_currentImageViewer.IndependentPlaybackEnabled);
                SetPlaybackButtonState(_currentImageViewer.IndependentPlaybackController.PlaybackMode);

                if (_currentImageViewer.IndependentPlaybackEnabled)
                {
                    labelPlaybackTime.Content = _currentImageViewer.IndependentPlaybackController.PlaybackTime.ToLocalTime().ToString(TimeFormat);
                }
                else
                {
                    labelPlaybackTime.Content = "Not Available";
                }
            }

            SetVideoEffectButtonState();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //We poll here to check whether digital zoom state has been changed via the Smart Client default UI.
            SetDigitalZoomButtonState();
        }

        private object SelectedViewItemChangedReceived(Message message, FQID destination, FQID sender)
        {
            UpdateImageViewerAddOn();
            return null;
        }

        private void PlaybackModeChangedEvent(object sender, PlaybackController.PlaybackModeEventArgs e)
        {
            SetPlaybackButtonState(e.PlaybackMode);
        }

        private void SetPlaybackButtonState(PlaybackController.PlaybackModeType playbackMode)
        {
            SetDigitalZoomButtonState();

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

        private void SetDigitalZoomButtonState()
        {
            bool enable = _currentImageViewer != null && _currentImageViewer.DigitalZoomEnabled;
            Dispatcher.Invoke(() =>
            {
                buttonZoomEnable.IsEnabled = !enable;
                buttonZoomDisable.IsEnabled = enable;
                buttonZoomIn.IsEnabled = enable;
                buttonZoomOut.IsEnabled = enable;
                buttonZoomMoveDown.IsEnabled = enable;
                buttonZoomMoveDownLeft.IsEnabled = enable;
                buttonZoomMoveDownRight.IsEnabled = enable;
                buttonZoomMoveLeft.IsEnabled = enable;
                buttonZoomMoveRight.IsEnabled = enable;
                buttonZoomMoveUpLeft.IsEnabled = enable;
                buttonZoomMoveUpRight.IsEnabled = enable;
                buttonZoomMoveUp.IsEnabled = enable;
                buttonZoomGetRectangle.IsEnabled = enable;
                buttonZoomSetRectangle.IsEnabled = enable;
            });
        }

        private void SetVideoEffectButtonState()
        {
            bool enable = _currentImageViewer != null && _currentImageViewer.VideoEffect != null;
            Dispatcher.Invoke(() =>
            {
                buttonThresholdEffectEnable.IsEnabled = !enable;
                buttonThresholdEffectDisable.IsEnabled = enable;
            });
        }

        private void ChangeCurrentImageViewer(ImageViewerAddOn selectedImageViewerAddon)
        {
            if (_currentImageViewer == null && selectedImageViewerAddon != null)
            {
                _currentImageViewer = selectedImageViewerAddon;
                _currentImageViewer.IndependentPlaybackModeChangedEvent += IndependentPlaybackModeChangedHandler;
            }

            if (_currentImageViewer != null && selectedImageViewerAddon != _currentImageViewer)
            {
                RemoveIndependentPlaybackEvents(_currentImageViewer);
                _currentImageViewer = selectedImageViewerAddon;
            }

            if (_currentImageViewer?.IndependentPlaybackController != null)
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

        private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.ZoomIn);
        }

        private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.ZoomOut);
        }

        private void ButtonZoomMoveUp_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.Up);
        }

        private void ButtonZoomMoveUpLeft_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.UpLeft);
        }

        private void ButtonZoomMoveUpRight_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.UpRight);
        }

        private void ButtonZoomMoveLeft_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.Left);
        }

        private void ButtonZoomMoveRight_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.Right);
        }

        private void ButtonZoomMoveDownLeft_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.DownLeft);
        }

        private void ButtonZoomMoveDownRight_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.DownRight);
        }

        private void ButtonZoomMoveDown_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomMove(PTZMoveCommandData.Down);
        }

        private void ButtonZoomGetRectangle_Click(object sender, RoutedEventArgs e)
        {
            //This code gets the current imageViewerAddOn digital zoom rectangle
            var data = _currentImageViewer.DigitalZoomRectangle;
            _refWidth = data.RefWidth;
            _refHeight = data.RefHeight;
            txtZoomLeft.Text = Convert.ToString(data.Left);
            txtZoomRight.Text = Convert.ToString(data.Right);
            txtZoomDown.Text = Convert.ToString(data.Bottom);
            txtZoomUp.Text = Convert.ToString(data.Top);
        }

        private void ButtonZoomSetRectangle_Click(object sender, RoutedEventArgs e)
        {
            //This code zooms to a specific rectangle in the imageViewerAddOn.
            var data = new PTZRectangleCommandData();
            data.Bottom = Convert.ToDouble(string.IsNullOrEmpty(txtZoomDown.Text) ? "0" : txtZoomDown.Text);
            data.Top = Convert.ToDouble(string.IsNullOrEmpty(txtZoomUp.Text) ? "0" : txtZoomUp.Text);
            data.Left = Convert.ToDouble(string.IsNullOrEmpty(txtZoomLeft.Text) ? "0" : txtZoomLeft.Text);
            data.Right = Convert.ToDouble(string.IsNullOrEmpty(txtZoomRight.Text) ? "0" : txtZoomRight.Text);
            data.RefHeight = _refHeight;
            data.RefWidth = _refWidth;
            _currentImageViewer.DigitalZoomRectangle = data;
        }

        private void ButtonZoomEnable_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomEnabled = true;
            SetDigitalZoomButtonState();
        }

        private void ButtonZoomDisable_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.DigitalZoomEnabled = false;
            SetDigitalZoomButtonState();
        }

        private void ButtonThresholdEffectEnable_Click(object sender, RoutedEventArgs e)
        {
            var effect = new ThresholdEffect();
            _currentImageViewer.VideoEffect = effect;
            SetVideoEffectButtonState();
        }

        private void ButtonThresholdEffectDisable_Click(object sender, RoutedEventArgs e)
        {
            _currentImageViewer.VideoEffect = null;
            SetVideoEffectButtonState();
        }
        
    }
}
