using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace MessageTester.Client
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
    public partial class MessageTesterViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private MessageTesterViewItemManager _viewItemManager;
        private object _themeChangedReceiver;
        private string _messageId;
        private Dictionary<string, MessageDataControls.MessageDataSuper> _dicData = new Dictionary<string, MessageDataControls.MessageDataSuper>();
        private MessageDataControls.MessageDataSuper _currentControl;
        private object _messageReceiverObject;
        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a MessageTesterViewItemUserControl instance
        /// </summary>
        public MessageTesterViewItemWpfUserControl(MessageTesterViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();

            SetColorFromTheme();
            InitializeUsageDictionary();
            this.DataContext = _currentControl;
            FillCBControl();
            _messageReceiverObject = EnvironmentManager.Instance.RegisterReceiver(MessageReceiver, null);
        }
        private void FillCBControl()
        {
            List<String> messageIds = EnvironmentManager.Instance.MessageIdList;
            messageIds.Sort();
            foreach (String id in messageIds)
            {
                if (id.EndsWith("Indication") == false && id.EndsWith("Message") == false)
                    cbMessageIds.Items.Add(id);
            }
        }
        private void InitializeUsageDictionary()
        {
            _dicData.Add(MessageId.SmartClient.GetCurrentPlaybackTimeRequest, new MessageDataControls.InformationOnlyUserControl("Can be used without any parameters"));
            _dicData.Add(MessageId.Control.StartRecordingCommand, new MessageDataControls.DestinationUserControl(Kind.Camera));
            _dicData.Add(MessageId.Control.StopRecordingCommand, new MessageDataControls.DestinationUserControl(Kind.Camera));
            _dicData.Add(MessageId.Control.OutputActivate, new MessageDataControls.DestinationUserControl(Kind.Output));
            _dicData.Add(MessageId.Control.OutputDeactivate, new MessageDataControls.DestinationUserControl(Kind.Output));
            _dicData.Add(MessageId.SmartClient.GetCurrentWorkspaceRequest, new MessageDataControls.InformationOnlyUserControl("Can be used without any parameters"));
            _dicData.Add(MessageId.SmartClient.ApplicationControlCommand, new MessageDataControls.ApplicationControlCommandUserControl());
            _dicData.Add(MessageId.SmartClient.GetTimelineSelectedIntervalRequest, new MessageDataControls.InformationOnlyUserControl("Can be used without any parameters"));
            _dicData.Add(MessageId.SmartClient.SmartClientMessageCommand, new MessageDataControls.SmartClientMessageCommandUserControl());
            _dicData.Add(MessageId.Control.TriggerCommand, new MessageDataControls.TriggerCommandUserControl());
            _dicData.Add(MessageId.SmartClient.SetCameraInViewCommand, new MessageDataControls.InformationOnlyUserControl("No support implemented.\nPlease note that the Smart Client Insert Camera plugin sample Will show the use of this command."));
            _dicData.Add(MessageId.SmartClient.AddToExportCommand, new MessageDataControls.AddToExportCommandUserControl());
            ShownInSmartMapControl();
            ShownInSCViewAndWindow();
            ShownInVideoWallController();
        }

        private void ShownInVideoWallController()
        {
            string note = "No support implemented.\nPlease note that the Video Wall Controller sample will show messaging related to Video Wall usage.";
            _dicData.Add(MessageId.Control.VideoWallApplyXmlCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.VideoWallPresetActivateCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.VideoWallRemoveCamerasCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.VideoWallSetCamerasCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.VideoWallSetLayoutAndCamerasCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.VideoWallSetLayoutCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.VideoWallShowTextCommand, new MessageDataControls.InformationOnlyUserControl(note));
        }

        private void ShownInSmartMapControl()
        {
            string note = "No support implemented.\nPlease note that the Smart Map Control plugin sample will show messaging related to Smart Map.";
            _dicData.Add(MessageId.SmartClient.SmartMapGetPositionRequest, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.SmartMapGoToAreaCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.SmartMapGoToLocationCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.SmartMapGoToPositionCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.SmartMapNavigateToCamera, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.SmartMapPositionChangedIndication, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.SmartMapSelectItemCommand, new MessageDataControls.InformationOnlyUserControl(note));
        }

        private void ShownInSCViewAndWindow()
        {
            string note = "No support implemented.\nPlease note that the Smart Client View and Window Tools plugin sample will show messaging related to this command.";
            _dicData.Add(MessageId.Control.PTZAUXCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.ClearIndicatorCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.ChangeModeCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.LensCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Server.GetMapRequest, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.MultiWindowCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.PlaybackCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.PTZMoveAbsoluteCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.PTZGetAbsoluteRequest, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.PTZMoveStartCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.PTZMoveStopCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.Control.PTZMoveCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.ViewItemControlCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.SetSelectedViewItemCommand, new MessageDataControls.InformationOnlyUserControl(note));
            _dicData.Add(MessageId.SmartClient.ShowWorkSpaceCommand, new MessageDataControls.InformationOnlyUserControl(note));
        }

        private MessageDataControls.MessageDataSuper DataControl(string messageId)
        {
            MessageDataControls.MessageDataSuper control;
            
            if (_dicData.TryGetValue(messageId, out control))
            {
                return control;
            }
            else
            {
                return new MessageDataControls.NoSupportUserControl();
            }
        }

        private static Color GetWindowsMediaColor(System.Drawing.Color inColor)
        {
            return Color.FromArgb(inColor.A, inColor.R, inColor.G, inColor.B);
        }

        private void SetColorFromTheme()
        {
            if (Selected)
            {
                _viewItemGrid.Background = new SolidColorBrush(GetWindowsMediaColor(ClientControl.Instance.Theme.BackgroundColor));
            }
            else
            {
                _viewItemGrid.Background = new SolidColorBrush(GetWindowsMediaColor(ClientControl.Instance.Theme.BackgroundColor));
            }
        }

        private void SetUpApplicationEventListeners()
        {
            //set up ViewItem event listeners
            _themeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ThemeChangedIndicationHandler),
                                             new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));

        }

        private void RemoveApplicationEventListeners()
        {
            //remove ViewItem event listeners
            EnvironmentManager.Instance.UnRegisterReceiver(_themeChangedReceiver);
            _themeChangedReceiver = null;
        }

        /// <summary>
        /// Method that is called immediately after the view item is displayed.
        /// </summary>
        public override void Init()
        {
            SetUpApplicationEventListeners();
        }

        /// <summary>
        /// Method that is called when the view item is closed. The view item should free all resources when the method is called.
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
            RemoveApplicationEventListeners();
        }

        #endregion

        #region Print method

        /// <summary>
        /// Method that is called when print is activated while the content holder is selected.
        /// </summary>
        public override void Print()
        {
            Print("Name of this item", "Some extra information");
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
            RightClickEvent?.Invoke(this, e);
        }

        private object ThemeChangedIndicationHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            SetColorFromTheme();
            return null;
        }

        #endregion

        #region Component properties

        /// <summary>
        /// Gets boolean indicating whether the view item can be maximized or not. <br/>
        /// The content holder should implement the click and double click events even if it is not maximizable. 
        /// </summary>
        public override bool Maximizable
        {
            get { return true; }
        }

        /// <summary>
        /// Tell if ViewItem is selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        /// <summary>
        /// Make support for Theme colors to show if this ViewItem is selected or not.
        /// </summary>
        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                SetColorFromTheme();
            }
        }

        #endregion

        private object MessageReceiver(Message message, FQID destination, FQID sender)
        {
            string extra = "";
            if (message.Data != null)
            {
                extra = message.Data.ToString();
            }
            if (sender != null)
            {
                extra += ", sender=" + sender.ObjectId;
            }

            ClientControl.Instance.CallOnUiThread(() =>
            {
                _traceListbox.Items.Insert(0, message.MessageId + "  " + extra);
            });
            return null;
        }

        private void OnSendMessage(object sender, System.Windows.RoutedEventArgs e)
        {
            ResponseTextBox.Text = "";
            object messageData = null;
            if(_currentControl.Destination!= null)
            {

            }
            if (_currentControl.Data != null)
            {
                messageData = _currentControl.Data;
            }
            Message message = new Message(_messageId, _currentControl.Related ,messageData);
            try
            {
                var response = EnvironmentManager.Instance.SendMessage(message, _currentControl.Destination, null);
                if (response != null && response.Count > 0)
                {
                    ResponseTextBox.Text = response[0].ToString();
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("SendMessage", ex);
            }
        }

        private void OnMessageSelect(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _messageId = cbMessageIds.SelectedItem.ToString();
            _messageDataControlsGrid.Children.Clear();
            _currentControl = DataControl(_messageId);
            _messageDataControlsGrid.Children.Add(_currentControl);
            DataContext = _currentControl;
        }
    }
}
