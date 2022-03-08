using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace SequenceViewer.Client
{
    /// <summary>
    /// Interaction logic for SequenceViewerViewItemWpfUserControl.xaml
    /// </summary>
    public partial class SequenceViewerViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private bool _inLive = true;
        private SequenceViewerViewItemManager _viewItemManager;
        private ResourceManager _stringResourceManager;
        private object _modeChangedReceiver;
        private Item _item = null;

        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a DataSourceViewItemUserControl instance
        /// </summary>
        public SequenceViewerViewItemWpfUserControl(SequenceViewerViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();

            _stringResourceManager =
                new ResourceManager(Assembly.GetExecutingAssembly().GetName().Name + ".Resources.Strings",
                                                     Assembly.GetExecutingAssembly());

            SetUpApplicationEventListeners();

            InLive = EnvironmentManager.Instance.Mode == Mode.ClientLive;
        }

        private void SetUpApplicationEventListeners()
        {
            //set up ApplicationController events
            _modeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ApplicationModeChangedMessage),
                                                         new MessageIdFilter(MessageId.System.ModeChangedIndication));
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ApplicationController events
            EnvironmentManager.Instance.UnRegisterReceiver(_modeChangedReceiver);
        }

        /// <summary>
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>

        public override void Close()
        {
            RemoveApplicationEventListeners();
        }

        /// <summary>
        /// Do not show the sliding toolbar!
        /// </summary>
        public override bool ShowToolbar
        {
            get { return false; }
        }

        public override void Print()
        {
            String extra = "Some extra lines of information\r\n" +
                           "Selected camera:" + (_item != null ? _item.Name : "Unselected") + "\r\n+" +
                           "00000\r\n11111\r\n22222\r\n33333\r\n44444\r\n55555\r\n66666\r\n77777\r\n88888\r\n99999" +
                           "00000\r\n11111\r\n22222\r\n33333\r\n44444\r\n55555\r\n66666\r\n77777\r\n88888\r\n99999" +
                           "00000\r\n11111\r\n22222\r\n33333\r\n44444\r\n55555\r\n66666\r\n77777\r\n88888\r\n99999";
            Print("Data Source Sample", extra);
        }
        #endregion


        #region Component events
        private void OnMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }

        #endregion


        #region Component properties

        public bool InLive
        {
            get { return _inLive; }
            set { _inLive = value; }
        }

        public override bool Maximizable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Tell if Item is selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        /// <summary>
        /// Overrides property (set). First the Base implementation is called.
        /// When maximized the image quality should always be forced to full quality.
        /// </summary>
        public override bool Maximized
        {
            set
            {
                if (base.Maximized != value)
                {
                    base.Maximized = value;
                }
            }
        }

        /// <summary>
        /// Overrides property (set). First the Base implementation is called. 
        /// </summary>
        public override bool Hidden
        {
            set
            {
                if (Hidden != value)
                {
                    base.Hidden = value;
                }
            }
        }

        #endregion

        #region Component event handlers


        private object ApplicationModeChangedMessage(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            switch (EnvironmentManager.Instance.Mode)
            {
                case Mode.ClientLive:
                    InLive = true;
                    break;
                case Mode.ClientPlayback:
                    InLive = false;
                    break;
                case Mode.ClientSetup:
                    InLive = false;
                    break;
            }
            return null;
        }

        #endregion

        private void OnSelectCamera(object sender, EventArgs e)
        {

            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = Kind.Camera;
            form.AutoAccept = true;
            form.Init(Configuration.Instance.GetItems());
            if (form.ShowDialog() == DialogResult.OK)
            {
                _item = form.SelectedItem;
                buttonSelectCamera.Content = _item.Name;
                buttonShowSeqAsync.IsEnabled = true;
                buttonShowSeq.IsEnabled = true;
                buttonShowMD.IsEnabled = true;
                buttonShowMDAsync.IsEnabled = true;
                if (_item.FQID.ServerId.ServerType != ServerId.EnterpriseServerType)
                {
                    buttonShowTypes.IsEnabled = true;
                }
            }
        }

        private void OnShowSeq(object sender, EventArgs e)
        {
            if (_item != null)
            {
                SequenceDataSource dataSource = new SequenceDataSource(_item);
                List<object> list = dataSource.GetData(DateTime.Now, new TimeSpan(24, 0, 0), 5, new TimeSpan(0, 0, 0), 0);
                ListBox.Items.Clear();
                if (list != null)
                {
                    foreach (SequenceData sd in list)
                    {
                        ListBox.Items.Add(sd.EventHeader.Class + " " + sd.EventHeader.Name + "  " +
                            sd.EventHeader.Timestamp.ToLocalTime().ToShortTimeString());
                    }
                }
            }
        }

        private void OnRefreshSeqAsync(object sender, EventArgs e)
        {
            if (_item != null)
            {
                SequenceDataSource dataSource = new SequenceDataSource(_item);
                dataSource.GetDataAsync(ListBox, DateTime.Now, new TimeSpan(24, 0, 0), 5, new TimeSpan(0, 0, 0), 0, AsyncSeqHandler);
            }
        }

        private void OnRefreshMD(object sender, EventArgs e)
        {
            SequenceDataSource dataSource = new SequenceDataSource(_item);
            List<object> list = dataSource.GetData(DateTime.Now, new TimeSpan(24, 0, 0), 5, new TimeSpan(0, 0, 0), 0, VideoOS.Platform.Data.DataType.SequenceTypeGuids.MotionSequence);
            ListBox.Items.Clear();
            if (list != null)
            {
                foreach (SequenceData sd in list)
                {
                    ListBox.Items.Add(sd.EventHeader.Class + " " + sd.EventHeader.Name + "  " +
                        sd.EventHeader.Timestamp.ToLocalTime().ToShortTimeString());
                }
            }
        }

        private void OnRefreshMDAsync(object sender, EventArgs e)
        {
            if (_item != null)
            {
                SequenceDataSource dataSource = new SequenceDataSource(_item);
                dataSource.GetDataAsync(ListBox, DateTime.Now, new TimeSpan(24, 0, 0), 5, new TimeSpan(0, 0, 0), 0, VideoOS.Platform.Data.DataType.SequenceTypeGuids.MotionSequence, AsyncSeqHandler);
            }
        }

        private void AsyncSeqHandler(object result, object asyncState)
        {
            _ = new MethodInvoker(() =>
              {
                  ListBox.Items.Clear();
                  if (result != null && result is SequenceData[])
                  {
                      foreach (SequenceData sd in (SequenceData[])result)
                      {
                          ListBox.Items.Add(sd.EventHeader.Class + " " + sd.EventHeader.Name + "  " +
                                             sd.EventHeader.Timestamp.ToLocalTime().ToShortTimeString());
                      }
                  }
              });
        }


        private void OnGetSeqType(object sender, EventArgs e)
        {
            if (_item != null)
            {
                SequenceDataSource dataSource = new SequenceDataSource(_item);
                List<DataType> list = dataSource.GetTypes();
                ListBox.Items.Clear();
                if (list != null)
                {
                    foreach (DataType dt in list)
                    {
                        ListBox.Items.Add(dt.Name + "  " + dt.Id.ToString());
                    }
                }
            }
        }
    }
}

