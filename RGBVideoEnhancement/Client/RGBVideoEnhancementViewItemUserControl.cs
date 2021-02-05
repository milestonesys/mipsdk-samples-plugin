using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Live;
using VideoOS.Platform.Messaging;
using BitmapData = VideoOS.Platform.Data.BitmapData;
using Message = VideoOS.Platform.Messaging.Message;

namespace RGBVideoEnhancement.Client
{
	/// <summary>
	/// This class has been changed for MIPSDK v4.0 to cope with recorder fall-outs and failover.
	/// 
	/// </summary>
	public partial class RGBVideoEnhancementViewItemUserControl : ViewItemUserControl
	{
		#region Component private class variables

		private readonly RGBVideoEnhancementViewItemManager _viewItemManager;

		private Item _selectedItem = null;
		private BitmapVideoSource _bitmapVideoSource;
		private BitmapVideoSource _bitmapVideoSourceNext;
		private readonly object _bitmapVideoSourceNextLock = new object();

		private BitmapLiveSource _bitmapLiveSource;

		private string _mode = PlaybackPlayModeData.Stop;
		private DateTime _currentShownTime = DateTime.MinValue;
		private bool _redisplay;
		private bool _stopPlayback;
		private bool _stopLive;

		private object _objRef1, _objRef2, _objRef3, _objRef4;

		private ToolkitRGBEnhancement.RGBHandling.Transform _transform;

		private readonly Bitmap _blackImage = new Bitmap(1, 1);

		private MyPlayCommand _nextCommand = MyPlayCommand.None;

		enum MyPlayCommand
		{
			None,
			Start,
			End,
			NextSequence,
			PrevSequence,
			NextFrame,
			PrevFrame
		}

		#endregion

		#region Component constructors + dispose

		/// <summary>
		/// Constructs a RGBVideoEnhancementViewItemUserControl instance
		/// </summary>
		public RGBVideoEnhancementViewItemUserControl(RGBVideoEnhancementViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;

			InitializeComponent();

			Graphics g = Graphics.FromImage(_blackImage);
			g.FillRectangle(Brushes.Black, 0, 0, 1, 1);
			g.Dispose();

			ClientControl.Instance.RegisterUIControlForAutoTheming(panelMain);

			panelTitleBar.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
			panelTitleBar.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
		}

		private void SetUpApplicationEventListeners()
		{
			//set up ViewItem event listeners
			_viewItemManager.PropertyChangedEvent += ViewItemManagerPropertyChangedEvent;

			_objRef1 = EnvironmentManager.Instance.RegisterReceiver(PlaybackTimeChangedHandler,
                                             new PlaybackControllerFilter(MessageId.SmartClient.PlaybackCurrentTimeIndication, WindowInformation));
			_objRef2 = EnvironmentManager.Instance.RegisterReceiver(PlaybackCommandHandler,
											 new PlaybackControllerFilter(MessageId.SmartClient.PlaybackIndication, WindowInformation));
			_objRef3 = EnvironmentManager.Instance.RegisterReceiver(ApplicationModeChangedMessage,
														 new WindowFilter(MessageId.System.ModeChangedIndication, WindowInformation));
			_objRef4 = EnvironmentManager.Instance.RegisterReceiver(ThemeChangedIndicationHandler,
														 new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));
		}

		private void RemoveApplicationEventListeners()
		{
			//remove ViewItem event listeners
			_viewItemManager.PropertyChangedEvent -= ViewItemManagerPropertyChangedEvent;
			EnvironmentManager.Instance.UnRegisterReceiver(_objRef1);
			_objRef1 = null;
			EnvironmentManager.Instance.UnRegisterReceiver(_objRef2);
			_objRef2 = null;
			EnvironmentManager.Instance.UnRegisterReceiver(_objRef3);
			_objRef3 = null;
			EnvironmentManager.Instance.UnRegisterReceiver(_objRef4);
			_objRef4 = null;
		}

		/// <summary>
		/// Method that is called immediately after the view item is displayed.
		/// </summary>
		public override void Init()
		{
			SetUpApplicationEventListeners();

			_transform = new ToolkitRGBEnhancement.RGBHandling.Transform();

			_fetchThread = new Thread(BitmapFetchThread);
			_fetchThread.Start();

			OnScrollChange(null, null);

            ModeHandler(WindowInformation.Mode);
			if (_viewItemManager.SelectedCamera != null)
			{
				ViewItemManagerPropertyChangedEvent(null, null);
			}

		}

		/// <summary>
		/// Method that is called when the view item is closed. The view item should free all resources when the method is called.
		/// Is called when userControl is not displayed anymore. Either because of 
		/// user clicking on another View or Item has been removed from View.
		/// </summary>
		public override void Close()
		{
			_stop = true;
			_stopLive = true;
			_stopPlayback = true;

			CloseLiveSession();
			ClosePlaybackSession();

			RemoveApplicationEventListeners();

            if (pictureBox.Size.Width!=0 && pictureBox.Size.Height !=0)
			    pictureBox.Image = new Bitmap(_blackImage, pictureBox.Size);

		}

		public override bool ShowToolbar
		{
			get { return false; }
		}


		#endregion

		#region Time changed event handler
      

		private object PlaybackTimeChangedHandler(Message message, FQID dest, FQID sender)
		{
            var time = (DateTime) message.Data;
            _nextToFetchTime = time;
		    return null;
		}

		// When the users presses the different buttons on the playback control, this method
		// will follow those instructions
		private object PlaybackCommandHandler(Message message, FQID dest, FQID sender)
        {
            var commandData = message.Data as PlaybackCommandData;
            if (commandData != null)
            {
                switch (commandData.Command)
                {
                    case PlaybackData.Begin:
                        _nextCommand = MyPlayCommand.Start;
                        break;
                    case PlaybackData.End:
                        _nextCommand = MyPlayCommand.End;
                        break;
                    case PlaybackData.Previous:
                        _nextCommand = MyPlayCommand.PrevFrame;
                        break;
                    case PlaybackData.Next:
                        _nextCommand = MyPlayCommand.NextFrame;
                        break;
                    case PlaybackData.PreviousSequence:
                        _nextCommand = MyPlayCommand.PrevSequence;
                        break;
                    case PlaybackData.NextSequence:
                        _nextCommand = MyPlayCommand.NextSequence;
                        break;
                    case PlaybackData.PlayForward:
                    case PlaybackData.Play:
                        _mode = PlaybackPlayModeData.Forward;
                        break;
                    case PlaybackData.PlayReverse:
                        _mode = PlaybackPlayModeData.Reverse;
                        break;
                    case PlaybackData.PlayStop:
                        _mode = PlaybackPlayModeData.Stop;
                        break;
                }
            }
            return null;
		}

        private object ApplicationModeChangedMessage(Message message, FQID destination, FQID sender)
		{
            ModeHandler((Mode) message.Data);
            return null;
		}

		private object ThemeChangedIndicationHandler(Message message, FQID destination, FQID source)
		{
			this.Selected = Selected;   // Reset the header line properties
			return null;
		}

		#endregion

		#region Video Session Open and Close

		private void ModeHandler(Mode newMode)
		{
			if (_bitmapVideoSource != null)
			{
				ClosePlaybackSession();
			}
			if (_bitmapLiveSource != null)
			{
				CloseLiveSession();
			}
			_mode = PlaybackPlayModeData.Stop;

			switch (newMode)
			{
				case Mode.ClientLive:
					OpenLiveSession();
					break;
				case Mode.ClientPlayback:
					_mode = PlaybackPlayModeData.Forward;
				    //Thread thread = new Thread(new ThreadStart(OpenPlaybackSession));
				    //thread.Start();
				    //thread.Join();
                    OpenPlaybackSession();
					break;
				case Mode.ClientSetup:
					break;
			}
		}

		private void OpenPlaybackSession()
		{
			if (_viewItemManager.SelectedCamera != null)
			{
				_selectedItem = Configuration.Instance.GetItem(_viewItemManager.SelectedCamera.FQID);
				if (_selectedItem != null)
				{
					labelName.Text = _selectedItem.Name;
					lock (_bitmapVideoSourceNextLock)
					{
						_bitmapVideoSourceNext = new BitmapVideoSource(_selectedItem);
						_bitmapVideoSourceNext.Width = pictureBox.Width;
						_bitmapVideoSourceNext.Height = pictureBox.Height;
						_bitmapVideoSourceNext.SetKeepAspectRatio(true, true);
					}
					_nextToFetchTime = DateTime.UtcNow;
					_stopPlayback = false;
				}
			}
		}

		private void ClosePlaybackSession()
		{
			_stopPlayback = true;
            if (pictureBox.Height!=0 && pictureBox.Width!=0)
			    pictureBox.Image = new Bitmap(_blackImage, pictureBox.Size);
		}

		private void OpenLiveSession()
		{
			if (_viewItemManager.SelectedCamera != null)
			{
				_selectedItem = Configuration.Instance.GetItem(_viewItemManager.SelectedCamera.FQID);
				if (_selectedItem != null)
				{
					_bitmapLiveSource = new BitmapLiveSource(_selectedItem, BitmapFormat.BGR24);
					_bitmapLiveSource.LiveContentEvent += BitmapLiveSourceLiveContentEvent;
					_bitmapLiveSource.Width = pictureBox.Width;
					_bitmapLiveSource.Height = pictureBox.Height;
					_bitmapLiveSource.SetKeepAspectRatio(true, true);
					_bitmapLiveSource.Init();
					_bitmapLiveSource.LiveModeStart = true;
					_bitmapLiveSource.SingleFrameQueue = true;		// New property from MIPSDK 2014
					_stopLive = false;
				}
			}
		}

		private void CloseLiveSession()
		{
			_stopLive = true;
			if (_bitmapLiveSource != null)
			{
				_bitmapLiveSource.LiveContentEvent -= BitmapLiveSourceLiveContentEvent;
				_bitmapLiveSource.LiveModeStart = false;
				_bitmapLiveSource.Close();
				_bitmapLiveSource = null;
			}
            if (pictureBox.Height != 0 && pictureBox.Width != 0)
                pictureBox.Image = new Bitmap(_blackImage, pictureBox.Size);

		}

		#endregion

		#region Thread to get images in playback mode
		//
		// The primary purpose of this method is to have all calls in Playback mode beging executed on one thread per session,
		// and make sure it is not on the UI thread.

		private bool _stop;
		private Thread _fetchThread;
		private DateTime _nextToFetchTime;
		private PlaybackTimeInformationData _currentTimeInformation = null;
		private bool _requestInProgress = false;

        private void BitmapFetchThread()
        {
            bool errorRecovery = false;

            try
            {
                while (!_stop)
                {
                    try
                    {
                        if (_stopPlayback)
                        {
                            if (_bitmapVideoSource != null)
                            {
                                _bitmapVideoSource.Close();
                            }
                            _bitmapVideoSource = null;
                            _nextCommand = MyPlayCommand.None;
                            _nextToFetchTime = DateTime.MinValue;
                        }

                        if (_bitmapVideoSourceNext != null)
                        {
                            lock (_bitmapVideoSourceNextLock)
                            {
                                if (_bitmapVideoSource != null)
                                {
                                    _bitmapVideoSource.Close();
                                }
                                _bitmapVideoSource = _bitmapVideoSourceNext;
                                _bitmapVideoSourceNext = null;
                            }
                            _bitmapVideoSource.Init();
                            ShowError("");      // Clear messages
                            errorRecovery = false;
                        }

                        if (_bitmapVideoSource != null && _setResolution)
                        {
                            _bitmapVideoSource.Width = _newWidth;
                            _bitmapVideoSource.Height = _newHeight;
                            _bitmapVideoSource.SetWidthHeight();
                            _setResolution = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is CommunicationMIPException)
                        {
                            ShowError("Connection lost to server ...");
                        }
                        else
                        {
                            ShowError(ex.Message);
                        }
                        errorRecovery = true;
                        _bitmapVideoSourceNext = _bitmapVideoSource;
                        _bitmapVideoSource = null;
                        _nextCommand = MyPlayCommand.None;
                        _nextToFetchTime = DateTime.MinValue;
                    }

                    if (errorRecovery)
                    {
                        Thread.Sleep(3000);
                        continue;
                    }

                    if (Selected && _nextCommand != MyPlayCommand.None && _bitmapVideoSource != null && !_requestInProgress)
                    {
                        try
                        {
                            BitmapData bitmapData = null;
                            switch (_nextCommand)
                            {
                                case MyPlayCommand.Start:
                                    bitmapData = _bitmapVideoSource.GetBegin();
                                    break;
                                case MyPlayCommand.End:
                                    bitmapData = _bitmapVideoSource.GetEnd();
                                    break;
                                case MyPlayCommand.PrevSequence:
                                    bitmapData = _bitmapVideoSource.GetPreviousSequence();
                                    break;
                                case MyPlayCommand.NextSequence:
                                    bitmapData = _bitmapVideoSource.GetNextSequence();
                                    break;
                                case MyPlayCommand.PrevFrame:
                                    bitmapData = _bitmapVideoSource.GetPrevious();
                                    break;
                                case MyPlayCommand.NextFrame:
                                    bitmapData = _bitmapVideoSource.GetNext() as BitmapData;
                                    break;
                            }
                            if (bitmapData != null)
                            {
                                ShowBitmap(bitmapData);
                                // Lets get the Smart Client to show this time, (This will issue a new set time command, but is same again (that we ignore))
                                EnvironmentManager.Instance.PostMessage(
                                    new Message(MessageId.SmartClient.PlaybackCommand,
                                        new PlaybackCommandData
                                        {
                                            Command = PlaybackData.Goto,
                                            DateTime = bitmapData.DateTime
                                        }));
                                bitmapData.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex is CommunicationMIPException)
                            {
                                ShowError("Connection lost to server ...");
                            }
                            else
                            {
                                ShowError(ex.Message);
                            }
                            errorRecovery = true;
                            _bitmapVideoSourceNext = _bitmapVideoSource;
                            _bitmapVideoSource = null;
                            continue;
                        }

                        _nextCommand = MyPlayCommand.None;
                    }

                    if (_nextToFetchTime != DateTime.MinValue && 
                        (_nextToFetchTime != _currentShownTime || _redisplay) && 
                        _bitmapVideoSource != null && 
                        !_requestInProgress)
                    {
                        DateTime time = _nextToFetchTime;
                        DateTime utcTime = time.Kind == DateTimeKind.Local ? time.ToUniversalTime() : time;

                        // Next 4 lines new for MIPSDK 2014
                        bool willResultInSameFrame = _currentTimeInformation != null &&
                                                     _currentTimeInformation.NextTime > utcTime &&
                                                     _currentTimeInformation.PreviousTime < utcTime;
                        
                        // Lets validate if we are just asking for the same frame
                        _nextToFetchTime = DateTime.MinValue;

                        if (!willResultInSameFrame)
                        {
                            try
                            {
                                if (_mode == PlaybackPlayModeData.Stop)
                                {
                                    _bitmapVideoSource.GoTo(utcTime, PlaybackPlayModeData.Reverse);
                                }
                                
                                var bitmapData = _bitmapVideoSource.GetAtOrBefore(utcTime) as BitmapData;
                                
                                // For MIPSDK 2014
                                if (bitmapData != null && bitmapData.IsPreviousAvailable == false)
                                {
                                    if (utcTime - TimeSpan.FromMilliseconds(10) < bitmapData.DateTime)
                                        bitmapData.PreviousDateTime = utcTime - TimeSpan.FromMilliseconds(10);
                                    else
                                        bitmapData.PreviousDateTime = bitmapData.DateTime;
                                }

                                if (bitmapData != null)
                                {
                                    ShowBitmap(bitmapData);
                                    bitmapData.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex is CommunicationMIPException)
                                {
                                    ShowError("Connection lost to server ...");
                                }
                                else
                                {
                                    ShowError(ex.Message);
                                }
                                errorRecovery = true;
                                _bitmapVideoSourceNext = _bitmapVideoSource;
                                _bitmapVideoSource = null;
                            }
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionHandler("RGBVideoEnhancement.PlaybackThread", ex);
            }

            if (_bitmapVideoSource != null)
            {
                _bitmapVideoSource.Close();
                _bitmapVideoSource = null;
            }
            _fetchThread = null;
        }

        /// <summary>
        /// New code as from MIPSDK 4.0 - to handle connection issues
        /// </summary>
        /// <param name="errorText"></param>
        private delegate void ShowErrorDelegate(String errorText);
        private void ShowError(String errorText)
        {
            if (InvokeRequired)
            {
				//ClientControl.Instance.CallOnUiThread( () => ShowError(errorText));
                BeginInvoke(new ShowErrorDelegate(ShowError), errorText);
            }
            else
            {
                Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);
                g.DrawString(errorText, new Font(FontFamily.GenericMonospace, 12), Brushes.White, new PointF(20, 100));
                g.Dispose();
                pictureBox.Image = new Bitmap(bitmap, pictureBox.Size);
                bitmap.Dispose();
            }
        }


		#endregion

		#region ShowBitmap in the UI thread

		private delegate void ShowBitmapDelegate(BitmapData bitmapData);

		// The ShowBitmap now devided into 2 methods, for MIPSDK 2014
		private void ShowBitmap(BitmapData bitmapData)
		{
			// Next 15 lines new for MIPSDK 2014
			if (_currentTimeInformation != null &&
				_currentTimeInformation.PreviousTime < bitmapData.DateTime &&
				_currentTimeInformation.NextTime > bitmapData.DateTime)
			{
				Debug.WriteLine("----> Duplicate bitmap at " + bitmapData.DateTime);	// this should only happen a few times during startup
			    if (Selected)
			    {
			        EnvironmentManager.Instance.SendMessage(new Message(
			            MessageId.SmartClient.PlaybackTimeInformation, _currentTimeInformation), null, _viewItemManager.FQID);
			    }
				return;
			}

            // Set here to avoid race-condition. And we should use some locking to ensure thread-safety.
            _nextToFetchTime = bitmapData.DateTime;

			_currentTimeInformation = new PlaybackTimeInformationData
			{
				Item = _selectedItem.FQID,
				CurrentTime = bitmapData.DateTime,
				NextTime = bitmapData.NextDateTime,
				PreviousTime = bitmapData.PreviousDateTime
			};

            if (Selected)
            {
                EnvironmentManager.Instance.SendMessage(new Message(
                    MessageId.SmartClient.PlaybackTimeInformation, _currentTimeInformation), null, _viewItemManager.FQID);
            }

			_requestInProgress = true;
			if (InvokeRequired)
			{
				Invoke(new ShowBitmapDelegate(ShowBitmap2), bitmapData);
			}
			else
			{
				ShowBitmap2(bitmapData);
			}
		}

		private void ShowBitmap2(BitmapData bitmapData)
		{
			{
				if (bitmapData.DateTime != _currentShownTime || _redisplay)
				{
					_redisplay = false;

					// The following code does these functions:
					//    Get a IntPtr to the start of the GBR bitmap
					//    Transform via sample transformation (To be replaced with your C++ code)
					//    Create a Bitmap with the result
					//    Create a new Bitmap scaled to visible area on screen
					//    Assign new Bitmap into PictureBox
					//    Dispose first Bitmap
					//
					// The transformation is therefore done on the original image, but if the transformation is
					// keeping to the same size, then this would be much more effective if the resize was done first,
					// and the transformation afterwards.
					// Scaling can be done by setting the Width and Height on the 

					int width = bitmapData.GetPlaneWidth(0);
					int height = bitmapData.GetPlaneHeight(0);
					int stride = bitmapData.GetPlaneStride(0);

					// When using RGB / BGR bitmaps, they have all bytes continues in memory.  The PlanePointer(0) is used for all planes:
					IntPtr plane0 = bitmapData.GetPlanePointer(0);

					//IntPtr newPlane0 = transform.Perform(plane0, width * height * 3);		// Make the sample transformation / color change
					IntPtr newPlane0 = _transform.Perform(plane0, stride, width, height);		// Make the sample transformation / color change

					Image myImage = new Bitmap(width, height, stride, PixelFormat.Format24bppRgb, newPlane0);

					if (pictureBox.Width != 0 && pictureBox.Height != 0)	// Ignore when window is not visible
					{
						// We need to resize to the displayed area
						pictureBox.Image = new Bitmap(myImage, pictureBox.Width, pictureBox.Height);

						myImage.Dispose();
						// ---- bitmapData.Dispose();  Need to be disposed on the calling thread
						_transform.Release(newPlane0);

						//textBoxTime.Text = bitmapData.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
					}
				}

				_currentShownTime = bitmapData.DateTime;
				Debug.WriteLine("Image time: " + bitmapData.DateTime.ToLocalTime().ToString("HH.mm.ss.fff") + ", Mode=" + _mode);
			}
			_requestInProgress = false;
		}

		void BitmapLiveSourceLiveContentEvent(object sender, EventArgs e)
		{
			try
			{
				if (InvokeRequired)
				{
					// Make sure we execute on the UI thread before updating UI Controls
					BeginInvoke(new EventHandler(BitmapLiveSourceLiveContentEvent), new[] { sender, e });
				}
				else
				{
					var args = e as LiveContentEventArgs;
					if (args != null)
					{
						if (args.LiveContent != null)
						{
							var bitmapContent = args.LiveContent as LiveSourceBitmapContent;
							if (bitmapContent != null)
							{
								if (_stopLive || pictureBox.Width==0 || pictureBox.Height==0)
								{
									bitmapContent.Dispose();
								}
								else
								{
									int width = bitmapContent.GetPlaneWidth(0);
									int height = bitmapContent.GetPlaneHeight(0);
									int stride = bitmapContent.GetPlaneStride(0);
									IntPtr plane0 = bitmapContent.GetPlanePointer(0);

									IntPtr newPlane0 = _transform.Perform(plane0, stride, width, height);		// Make the sample transformation / color change
									Image myImage = new Bitmap(width, height, stride, PixelFormat.Format24bppRgb, newPlane0);
									// We need to resize to the displayed area
									pictureBox.Image = new Bitmap(myImage, pictureBox.Width, pictureBox.Height);

									myImage.Dispose();
									bitmapContent.Dispose();
									_transform.Release(newPlane0);

								    labelDecode.Text = args.LiveContent.HardwareDecodingStatus;
								}
							}
						}
						else if (args.Exception != null)
						{
							// Handle any exceptions occurred inside toolkit or on the communication to the VMS

                            Bitmap bitmap = new Bitmap(320, 240);
                            Graphics g = Graphics.FromImage(bitmap);
                            g.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);
                            if (args.Exception is CommunicationMIPException)
                            {
                                g.DrawString("Connection lost to server ...", new Font(FontFamily.GenericMonospace, 12),
                                             Brushes.White, new PointF(20, 100));
                            }
                            else
                            {
                                g.DrawString(args.Exception.Message, new Font(FontFamily.GenericMonospace, 12),
                                             Brushes.White, new PointF(20, 100));
                            }
                            g.Dispose();
                            pictureBox.Image = new Bitmap(bitmap, pictureBox.Size);
                            bitmap.Dispose();
                        }

					}
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.ExceptionDialog("BitmapLiveSourceLiveContentEvent", ex);
			}
		}


		#endregion

		#region User actions

		private void OnScrollChange(object sender, ScrollEventArgs e)
		{
			_transform.SetVectors(vScrollBarR.Value * hScrollBarExpose.Value, vScrollBarG.Value * hScrollBarExpose.Value, vScrollBarB.Value * hScrollBarExpose.Value, hScrollBarOffset.Value);
			if (_mode == PlaybackPlayModeData.Stop)
			{
				_nextToFetchTime = _currentShownTime;
				_redisplay = true;
			}
		}
		private void OnResize(object sender, EventArgs e)
		{
			if (_mode == PlaybackPlayModeData.Stop)
			{
				_nextToFetchTime = _currentShownTime;
				_redisplay = true;
			}
		}

		#endregion

		#region Print method
		/// <summary>
		/// Method that is called when print is activated while the content holder is selected.
		/// Base implementation calls 'DrawToBitmap" to get a bitmap from the entire UserControl,
		/// and pass on to Print method.
		/// <code>
		/// Bitmap bitmap = new Bitmap(this.Width, this.Height);
		/// this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
		/// ClientControl.Instance.Print(bitmap, this.Name, "");
		/// bitmap.Dispose();
		/// </code>
		/// This implementation does not work for usercontrols with embedded ImageViewerControl.cs
		/// </summary>
		public override void Print()
		{
			Print("Name of this item", "Some extra information");
		}

		#endregion


		#region Component events

		private void ViewItemUserControlMouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				FireClickEvent();

				if (_viewItemManager.SelectedCamera != null)
				{
					EnvironmentManager.Instance.SendMessage(
						new Message(MessageId.SmartClient.SelectedCameraChangedIndication,
						                                       _viewItemManager.SelectedCamera.FQID));
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				FireRightClickEvent(e);
			}
		}

		private void ViewItemUserControlMouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
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

		void ViewItemManagerPropertyChangedEvent(object sender, EventArgs e)
		{
            labelName.Text = _viewItemManager.SelectedCamera.Name;
            if (_selectedItem != null && _selectedItem.FQID.EqualGuids(_viewItemManager.SelectedCamera.FQID))
				return;

			ModeHandler(WindowInformation.Mode);
		}

		private void OnClick(object sender, EventArgs e)
		{
			FireClickEvent();
			if (_viewItemManager.SelectedCamera != null)
			{
				EnvironmentManager.Instance.SendMessage(
					new Message(MessageId.SmartClient.SelectedCameraChangedIndication,
														   _viewItemManager.SelectedCamera.FQID));
			}

		}

		private void OnDoubleClick(object sender, EventArgs e)
		{
			FireDoubleClickEvent();
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

		public override bool Selected
		{
			get
			{
				return base.Selected;
			}
			set
			{
				base.Selected = value;
				if (value)
				{
					panelTitleBar.BackColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderColor;
					panelTitleBar.ForeColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderTextColor;
                    _nextCommand = MyPlayCommand.None;
				} else
				{
					panelTitleBar.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
					panelTitleBar.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
				}
			}
		}
		#endregion

		private int _newWidth = 0;
		private int _newHeight = 0;
		private bool _setResolution = false;
		private void OnResizePictureBox(object sender, EventArgs e)
		{
			if (_bitmapVideoSource!=null)
			{
				_newWidth = pictureBox.Width;
				_newHeight = pictureBox.Height;
				_setResolution = true;
			}
			
			if (_bitmapLiveSource!=null)
			{
				_bitmapLiveSource.Width = pictureBox.Width;
				_bitmapLiveSource.Height = pictureBox.Height;
				_bitmapLiveSource.SetWidthHeight();
			}
			
		}
	}
}
