using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{

    public partial class PTZAbsoluteWpfUserControl : UserControl
    {
        public PTZAbsoluteWpfUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adjusts camera to specified pan, tilt, and zoom.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFirePTZ(object sender, RoutedEventArgs e)
        {
            PTZMoveAbsoluteCommandData data = new PTZMoveAbsoluteCommandData();
            Double.TryParse(textBoxPan.Text, out data.Pan);
            Double.TryParse(textBoxTilt.Text, out data.Tilt);
            Double.TryParse(textBoxZoom.Text, out data.Zoom);
            data.AllowRepeats = (bool)checkBoxRepeats.IsChecked;
            data.Speed = 1.0;
            EnvironmentManager.Instance.SendMessage(new Message(MessageId.Control.PTZMoveAbsoluteCommand, data));
        }

        /// <summary>
        /// Gets the current PTZ.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGetCurrent(object sender, RoutedEventArgs e)
        {
            Collection<object> result = EnvironmentManager.Instance.SendMessage(
                new Message(MessageId.Control.PTZGetAbsoluteRequest, null));
            if (result.Count > 0)
            {
                PTZGetAbsoluteRequestData data = (PTZGetAbsoluteRequestData)result[0];
                textBoxPan.Text = data.Pan.ToString();
                textBoxTilt.Text = data.Tilt.ToString();
                textBoxZoom.Text = data.Zoom.ToString();
            }
        }

        private void OnMoveStart(object sender, RoutedEventArgs e)
        {
            PTZMoveStartCommandData data = new PTZMoveStartCommandData();
            Double.TryParse(textBoxPan.Text, out data.Pan);
            Double.TryParse(textBoxTilt.Text, out data.Tilt);
            Double.TryParse(textBoxZoom.Text, out data.Zoom);
            data.Speed = 1.0;
            EnvironmentManager.Instance.SendMessage(
                new Message(MessageId.Control.PTZMoveStartCommand, data));
        }

        private void OnMoveStop(object sender, RoutedEventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new Message(MessageId.Control.PTZMoveStopCommand));
        }
    }
}
