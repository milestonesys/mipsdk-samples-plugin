using MessageTester.MessageDataControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MessageTesterViewItemWpfUserControl : ViewItemWpfUserControl, INotifyPropertyChanged
    {
        public class MessageHolder
        {
            public string MessageId { get; }
            public MessageDataSuper Control { get; }
            public MessageHolder(string messageId, MessageDataSuper control)
            {
                MessageId = messageId;
                Control = control;
            }
        }

        #region Component private class variables

        private object _themeChangedReceiver;
        private Dictionary<string, MessageHolder> _dicData = new Dictionary<string, MessageHolder>();
        private object _messageReceiverObject;
        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a MessageTesterViewItemUserControl instance
        /// </summary>
        public MessageTesterViewItemWpfUserControl()
        {
            InitializeComponent();

            SetColorFromTheme();
            InitializeUsageDictionary();
            DataContext = this;
            FillCBControl();
            _messageReceiverObject = EnvironmentManager.Instance.RegisterReceiver(MessageReceiver, null);
        }
        
        private void FillCBControl()
        {
            List<String> messageIds = EnvironmentManager.Instance.MessageIdList;
            messageIds.Sort();
            foreach (string id in messageIds)
            {
                if (!id.EndsWith("Indication") && !id.EndsWith("Message"))
                {
                    if (_dicData.ContainsKey(id))
                        cbMessageIds.Items.Add(_dicData[id]);
                    else
                        cbMessageIds.Items.Add(new MessageHolder(id, new NoSupportUserControl()));
                }
            }
            cbMessageIds.SelectedIndex = 0;
        }

        private void InitializeUsageDictionary()
        {
            _dicData.Add(MessageId.SmartClient.GetCurrentPlaybackTimeRequest, new MessageHolder(MessageId.SmartClient.GetCurrentPlaybackTimeRequest, new InformationOnlyUserControl("Can be used without any parameters")));
            _dicData.Add(MessageId.Control.StartRecordingCommand, new MessageHolder(MessageId.Control.StartRecordingCommand, new DestinationUserControl(Kind.Camera)));
            _dicData.Add(MessageId.Control.StopRecordingCommand, new MessageHolder(MessageId.Control.StopRecordingCommand, new DestinationUserControl(Kind.Camera)));
            _dicData.Add(MessageId.Control.OutputActivate, new MessageHolder(MessageId.Control.OutputActivate, new DestinationUserControl(Kind.Output)));
            _dicData.Add(MessageId.Control.OutputDeactivate, new MessageHolder(MessageId.Control.OutputDeactivate, new DestinationUserControl(Kind.Output)));
            _dicData.Add(MessageId.SmartClient.GetCurrentWorkspaceRequest, new MessageHolder(MessageId.SmartClient.GetCurrentWorkspaceRequest, new InformationOnlyUserControl("Can be used without any parameters")));
            _dicData.Add(MessageId.SmartClient.ApplicationControlCommand, new MessageHolder(MessageId.SmartClient.ApplicationControlCommand, new ApplicationControlCommandUserControl()));
            _dicData.Add(MessageId.SmartClient.GetTimelineSelectedIntervalRequest, new MessageHolder(MessageId.SmartClient.GetTimelineSelectedIntervalRequest, new InformationOnlyUserControl("Can be used without any parameters")));
            _dicData.Add(MessageId.SmartClient.SmartClientMessageCommand, new MessageHolder(MessageId.SmartClient.SmartClientMessageCommand, new SmartClientMessageCommandUserControl()));
            _dicData.Add(MessageId.Control.TriggerCommand, new MessageHolder(MessageId.Control.TriggerCommand, new TriggerCommandUserControl()));
            _dicData.Add(MessageId.SmartClient.SetCameraInViewCommand, new MessageHolder(MessageId.SmartClient.SetCameraInViewCommand, new InformationOnlyUserControl("No support implemented.\nPlease note that the Smart Client Insert Camera plugin sample Will show the use of this command.")));
            _dicData.Add(MessageId.SmartClient.AddToExportCommand, new MessageHolder(MessageId.SmartClient.AddToExportCommand, new AddToExportCommandUserControl()));
            ShownInSmartMapControl();
            ShownInSCViewAndWindow();
            ShownInVideoWallController();
        }

        private void ShownInVideoWallController()
        {
            AddVideoWallMessage(MessageId.Control.VideoWallApplyXmlCommand);
            AddVideoWallMessage(MessageId.Control.VideoWallPresetActivateCommand);
            AddVideoWallMessage(MessageId.Control.VideoWallRemoveCamerasCommand);
            AddVideoWallMessage(MessageId.Control.VideoWallSetCamerasCommand);
            AddVideoWallMessage(MessageId.Control.VideoWallSetLayoutAndCamerasCommand);
            AddVideoWallMessage(MessageId.Control.VideoWallSetLayoutCommand);
            AddVideoWallMessage(MessageId.Control.VideoWallShowTextCommand);
        }

        private void AddVideoWallMessage(string messageId)
        {
            string note = "No support implemented.\nPlease note that the Video Wall Controller sample will show messaging related to Video Wall usage.";
            _dicData.Add(messageId, new MessageHolder(messageId, new NoSupportUserControl(note)));
        }

        private void ShownInSmartMapControl()
        {
            AddSmartMapMessage(MessageId.SmartClient.SmartMapGetPositionRequest);
            AddSmartMapMessage(MessageId.SmartClient.SmartMapGoToAreaCommand);
            AddSmartMapMessage(MessageId.SmartClient.SmartMapGoToLocationCommand);
            AddSmartMapMessage(MessageId.SmartClient.SmartMapGoToPositionCommand);
            AddSmartMapMessage(MessageId.SmartClient.SmartMapNavigateToCamera);
            AddSmartMapMessage(MessageId.SmartClient.SmartMapPositionChangedIndication);
            AddSmartMapMessage(MessageId.SmartClient.SmartMapSelectItemCommand);
        }

        private void AddSmartMapMessage(string messageId)
        {
            string note = "No support implemented.\nPlease note that the Smart Map Control plugin sample will show messaging related to Smart Map.";
            _dicData.Add(messageId, new MessageHolder(messageId, new NoSupportUserControl(note)));
        }

        private void ShownInSCViewAndWindow()
        {
            AddViewAndWindowMessage(MessageId.Control.PTZAUXCommand);
            AddViewAndWindowMessage(MessageId.SmartClient.ClearIndicatorCommand);
            AddViewAndWindowMessage(MessageId.SmartClient.ChangeModeCommand);
            AddViewAndWindowMessage(MessageId.Control.LensCommand);
            AddViewAndWindowMessage(MessageId.Server.GetMapRequest);
            AddViewAndWindowMessage(MessageId.SmartClient.MultiWindowCommand);
            AddViewAndWindowMessage(MessageId.SmartClient.PlaybackCommand);
            AddViewAndWindowMessage(MessageId.Control.PTZMoveAbsoluteCommand);
            AddViewAndWindowMessage(MessageId.Control.PTZGetAbsoluteRequest);
            AddViewAndWindowMessage(MessageId.Control.PTZMoveStartCommand);
            AddViewAndWindowMessage(MessageId.Control.PTZMoveStopCommand);
            AddViewAndWindowMessage(MessageId.Control.PTZMoveCommand);
            AddViewAndWindowMessage(MessageId.SmartClient.ViewItemControlCommand);
            AddViewAndWindowMessage(MessageId.SmartClient.SetSelectedViewItemCommand);
            AddViewAndWindowMessage(MessageId.SmartClient.ShowWorkSpaceCommand);
        }

        private void AddViewAndWindowMessage(string messageId)
        {
           string note = "No support implemented.\nPlease note that the Smart Client View and Window Tools plugin sample will show messaging related to this command.";
            _dicData.Add(messageId, new MessageHolder(messageId, new NoSupportUserControl(note)));
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
            var selectedMessage = cbMessageIds.SelectedItem as MessageHolder;
            if (selectedMessage == null)
            {
                return;
            }

            ResponseTextBox.Text = "";
            object messageData = null;

            if (selectedMessage.Control.Data != null)
            {
                messageData = selectedMessage.Control.Data;
            }
            Message message = new Message(selectedMessage.MessageId, selectedMessage.Control.Related ,messageData);
            try
            {
                var response = EnvironmentManager.Instance.SendMessage(message, selectedMessage.Control.Destination, null);
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
            _messageDataControlsGrid.Children.Clear();
            _messageDataControlsGrid.Children.Add((cbMessageIds.SelectedItem as MessageHolder).Control);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
