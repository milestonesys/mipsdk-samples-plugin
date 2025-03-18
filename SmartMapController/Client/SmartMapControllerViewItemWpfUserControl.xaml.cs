using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace SmartMapController.Client
{
    /// <summary>
    /// The ViewItemWpfUserControl is the WPF version of the ViewItemUserControl. It is instantiated for every position it is created on the current visible view. When a user select another View or ViewLayout, this class will be disposed.  No permanent settings can be saved in this class.
    /// The Init() method is called when the class is initiated and handle has been created for the UserControl. Please perform resource initialization in this method.
    /// <br>
    /// If Message communication is performed, register the MessageReceivers during the Init() method and UnRegister the receivers during the Close() method.
    /// <br>
    /// The Close() method can be used to Dispose resources in a controlled manor.
    /// <br>
    /// Mouse events not used by this control, should be passed on to the Smart Client by issuing the following methods:<br>
    /// FireClickEvent() for single click<br>
    ///	FireDoubleClickEvent() for double click<br>
    /// The single click will be interpreted by the Smart Client as a selection of the item, and the double click will be interpreted to expand the current viewitem to fill the entire View.
    /// </summary>
    public partial class SmartMapControllerViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private SmartMapControllerViewItemManager _viewItemManager;
        private string _lastTabName;
        private object _mapPosChangedReceiver;
        #endregion

        #region Component constructors + dispose

        /// <summary>
		/// Constructs a SmartMapControllerViewItemUserControl instance
        /// </summary>
		public SmartMapControllerViewItemWpfUserControl(SmartMapControllerViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
            FillWindowCombobox();
        }

        /// <summary>
        /// Method that is called immediately after the view item is displayed.
        /// </summary>
        public override void Init()
        {
        }

        /// <summary>
        /// Method that is called when the view item is closed. The view item should free all resources when the method is called.
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
            if (_mapPosChangedReceiver != null)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(_mapPosChangedReceiver);
                _mapPosChangedReceiver = null;
            }
        }
        #endregion
        
        #region Component events
        private void ViewItemWpfUserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireClickEvent();
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                FireRightClickEvent(e);
            }
        }

        private void ViewItemWpfUserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireDoubleClickEvent();
            }
        }

        /// <summary>
        /// Signals that the form is right clicked
        /// </summary>
        public event EventHandler RightClickEvent;

        /// <summary>
        /// Activates the RightClickEvent
        /// </summary>
        /// <param name="e">Event args</param>
        protected virtual void FireRightClickEvent(EventArgs e)
        {
            if (RightClickEvent != null)
            {
                RightClickEvent(this, e);
            }
        }

        private void WindowSelection_DropDownOpened(object sender, EventArgs e)
        {
            FillWindowCombobox();
        }

        private void GoToCoordinatesClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var latitude = double.Parse(_latitudeBox.Text, CultureInfo.InvariantCulture);
            var longitude = double.Parse(_longitudeBox.Text, CultureInfo.InvariantCulture);
            var zoomLevel = double.Parse(_zoomBox.Text, CultureInfo.InvariantCulture);
            if (0 <= zoomLevel && zoomLevel <= 1)
            {
                if (LatitudeIsValid(latitude) && LongitudeIsValid(longitude))
                {
                    EnvironmentManager.Instance.PostMessage(
                    new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SmartMapGoToPositionCommand,
                    new SmartMapGoToPositionCommandData
                    {
                        Latitude = latitude,
                        Longitude = longitude,
                        ZoomLevel = zoomLevel,
                        Window = _windowComboBox.SelectedItem != null ? ((Item)_windowComboBox.SelectedItem).FQID : null,
                        Index = int.Parse(_indexBox.Text, CultureInfo.InvariantCulture),
                    }));
                }
            }
            else
            {
                System.Windows.MessageBox.Show(string.Format("Zoom level shall be in the range of 0 and 1. You input is {0}", zoomLevel.ToString()), "Input errors");
            }

        }

        private void GetCurPositionClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var response = EnvironmentManager.Instance.SendMessage(
            new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SmartMapGetPositionRequest,
            new SmartMapGetPositionRequestData
            {
                Window = _windowComboBox.SelectedItem != null ? ((Item)_windowComboBox.SelectedItem).FQID : null,
                Index = int.Parse(_indexBox.Text, CultureInfo.InvariantCulture)
            }));
            if (response != null && response.Any() && response[0] is SmartMapPositionData)
            {
                var data = response[0] as SmartMapPositionData;
                _currentLatTextBox.Text = data.CenterLatitude.ToString(CultureInfo.InvariantCulture);
                _currentLongTextBox.Text = data.CenterLongitude.ToString(CultureInfo.InvariantCulture);
                _upperLeftLatTextBoxP.Text = data.UpperLeftLatitude.ToString(CultureInfo.InvariantCulture);
                _upperLeftLongTextBoxP.Text = data.UpperLeftLongitude.ToString(CultureInfo.InvariantCulture);
                _lowerRightLatTextBoxP.Text = data.LowerRightLatitude.ToString(CultureInfo.InvariantCulture);
                _lowerRightLongTextBoxP.Text = data.LowerRightLongitude.ToString(CultureInfo.InvariantCulture);
                _zoomTextBox.Text = data.ZoomLevel.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void GoToAreaClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var upperLeftLat = double.Parse(_upperLeftLatTextBox.Text, CultureInfo.InvariantCulture);
            var upperLeftLong = double.Parse(_upperLeftLongTextBox.Text, CultureInfo.InvariantCulture);
            var lowerRightLat = double.Parse(_lowerRightLatTextBox.Text, CultureInfo.InvariantCulture);
            var lowerRightLong = double.Parse(_lowerRightLongTextBox.Text, CultureInfo.InvariantCulture);
            if (LatitudeIsValid(upperLeftLat) && LongitudeIsValid(upperLeftLong) && LatitudeIsValid(lowerRightLat) && LongitudeIsValid(lowerRightLong))
            {
                if (upperLeftLat >= lowerRightLat && upperLeftLong <= lowerRightLong)
                {
                    EnvironmentManager.Instance.PostMessage(
                    new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SmartMapGoToAreaCommand,
                    new SmartMapGoToAreaCommandData
                    {
                        UpperLeftLatitude = upperLeftLat,
                        UpperLeftLongitude = upperLeftLong,
                        LowerRightLatitude = lowerRightLat,
                        LowerRightLongitude = lowerRightLong,
                        Window = _windowComboBox.SelectedItem != null ? ((Item)_windowComboBox.SelectedItem).FQID : null,
                        Index = int.Parse(_indexBox.Text, CultureInfo.InvariantCulture)
                    }));
                }
                else
                {
                    System.Windows.MessageBox.Show("Upper left latitude shall be greater than lower right latitude and upper left longitude shall be smaller than lower right longitude", "Input errors");
                }

            }
        }
        private void GoToLocationClick(object sender, RoutedEventArgs e)
        {
            var form = new ItemPickerWpfWindow()
            {
                Items = Configuration.Instance.GetItemsByKind(Kind.GisMapLocation),
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect
            };
            form.ShowDialog();
            if(form.SelectedItems != null && form.SelectedItems.Any())
            {
                var item = form.SelectedItems.First();
                EnvironmentManager.Instance.PostMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SmartMapGoToLocationCommand, new SmartMapGoToLocationCommandData
                {
                    Location = item.FQID,
                    Window = ((Item)_windowComboBox.SelectedItem)?.FQID ?? null,
                    Index = int.Parse(_indexBox.Text, CultureInfo.InvariantCulture)
                }));
            }
        }

        private void GoToItemClick(object sender, RoutedEventArgs e)
        {
            var form = new ItemPickerWpfWindow()
            {
                Items = Configuration.Instance.GetItems(),
                SelectionMode = SelectionModeOptions.SingleSelect
            };
            form.ShowDialog();
            if(form.SelectedItems == null || form.SelectedItems.Any())
            {
                var item = form.SelectedItems.First();
                // If the item selected is a ViewItemInstance then we need to extract the camera item and use that instead
                if (item.FQID.Kind == Kind.ViewItemInstance)  
                {                    
                    if (item.Properties.TryGetValue("CurrentCameraId", out string str))
                    {
                        if(Guid.TryParse(str, out Guid guid))
                        {
                            item = Configuration.Instance.GetItem(guid, Kind.Camera) ?? item;
                        }                        
                    }
                }
                EnvironmentManager.Instance.PostMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SmartMapSelectItemCommand, new SmartMapSelectItemCommandData
                {
                    Item = item.FQID,
                    Window = ((Item)_windowComboBox.SelectedItem)?.FQID ?? null,
                    Index = int.Parse(_indexBox.Text, CultureInfo.InvariantCulture)
                }));
            };
        }

        private object MapPositionChangedEventHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID sender)
        {
            var data = message.Data as SmartMapPositionData;
            if (data != null)
            {
                string whichWindow = (data.Window.ObjectId == Kind.Window) ? "Main Window" : "Floating Window";
                _changedDataTextBox.AppendText(string.Format("On {0} position {1} of {2} with Zoom Level {9}, \n\t\t\t\t Upper left [{3}, {4}], \n\t\t\t\t Lower right [{5}, {6}], \n\t\t\t\t Center [{7}, {8}]",
                    Configuration.Instance.GetItem(data.Window.ObjectId, Kind.Window),
                    data.Index,
                    whichWindow,
                    data.UpperLeftLatitude,
                    data.UpperLeftLongitude,
                    data.LowerRightLatitude,
                    data.LowerRightLongitude,
                    data.CenterLatitude,
                    data.CenterLongitude,
                    data.ZoomLevel, CultureInfo.InvariantCulture) + Environment.NewLine);
                _changedDataTextBox.ScrollToEnd();

            }
            return null;
        }

        private void TabSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var tabItem = lowerControl.SelectedItem as TabItem;
            if (tabItem != null)
            {
                if (tabItem.Header.ToString() == _mapIndicationTab.Header.ToString())
                {
                    EnableSmartMapSelectionArea(false);
                    _mapPosChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(MapPositionChangedEventHandler, new MessageIdFilter(MessageId.SmartClient.SmartMapPositionChangedIndication));
                    _lastTabName = _mapIndicationTab.Name;
                }
                else if (_lastTabName != null)
                {
                    _changedDataTextBox.Clear();
                    EnableSmartMapSelectionArea(true);
                    EnvironmentManager.Instance.UnRegisterReceiver(_mapPosChangedReceiver);
                    _mapPosChangedReceiver = null;
                    _lastTabName = null;
                }
            }
        }
        #endregion

        #region Helper method
        private void FillWindowCombobox()
        {
            _windowComboBox.Items.Clear();
            foreach (Item item in Configuration.Instance.GetItemsByKind(Kind.Window))
            {
                _windowComboBox.Items.Add(item);
            }
        }
        private bool LatitudeIsValid(double latitude)
        {
            if (-90 <= latitude && latitude <= 90)
            {
                return true;
            }
            else
            {
                System.Windows.MessageBox.Show(string.Format("Latitude shall be in the range of -90 and 90. You input is {0}", latitude.ToString()), "Input errors");
                return false;
            }
        }

        private bool LongitudeIsValid(double Longitude)
        {
            if (-180 <= Longitude && Longitude <= 180)
            {
                return true;
            }
            else
            {
                System.Windows.MessageBox.Show(string.Format("Longitude shall be in the range of -90 and 90. You input is {0}", Longitude.ToString()), "Input errors");
                return false;
            }
        }

        private void EnableSmartMapSelectionArea(bool isTrue)
        {
            _windowComboBox.IsEnabled = _indexBox.IsEnabled = isTrue;
        }
        #endregion
    }
}
