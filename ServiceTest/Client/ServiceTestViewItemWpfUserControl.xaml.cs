using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Windows.Input;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace ServiceTest.Client
{

    public partial class ServiceTestViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private bool _inLive = true;
        private ServiceTestViewItemManager _viewItemManager;
        private readonly ResourceManager _stringResourceManager;
        private object _modeChangedReceiver;

        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a ServiceTestViewItemWpfUserControl instance
        /// </summary>
        public ServiceTestViewItemWpfUserControl(ServiceTestViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();

            _stringResourceManager =
                new ResourceManager(Assembly.GetExecutingAssembly().GetName().Name + ".Resources.Strings",
                                                     Assembly.GetExecutingAssembly());

            SetUpApplicationEventListeners();

            InLive = EnvironmentManager.Instance.Mode == Mode.ClientLive;

            FillServiceList();
        }


        private void SetUpApplicationEventListeners()
        {
            //set up ViewItem event listeners
            _viewItemManager.PropertyChangedEvent += new EventHandler(viewItemManager_PropertyChangedEvent);

            //set up ApplicationController events
            _modeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ApplicationModeChangedMessage),
                                                         new MessageIdFilter(MessageId.System.ModeChangedIndication));
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ViewItem event listeners
            _viewItemManager.PropertyChangedEvent -= new EventHandler(viewItemManager_PropertyChangedEvent);

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

        #endregion



        #region Component events


        private void OnMouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }

        void viewItemManager_PropertyChangedEvent(object sender, EventArgs e)
        {
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
                if (base.Hidden != value)
                {
                    base.Hidden = value;
                }
            }
        }

        #endregion



        #region Component event handlers


        private object ApplicationModeChangedMessage(Message message, FQID destination, FQID source)
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

        private void OnRefresh(object sender, EventArgs e)
        {
            FillServiceList();
        }

        private void FillServiceList()
        {
            _listBoxServices.Items.Clear();
            try
            {
                List<Configuration.ServiceURIInfo> serviceUriInfo =
                    Configuration.Instance.GetRegisteredServiceUriInfo(Configuration.Instance.ServerFQID.ServerId);
                foreach (Configuration.ServiceURIInfo si in serviceUriInfo)
                {
                    _listBoxServices.Items.Add(si.Name + ", url=" + si.UriArray[0] + ", ServiceId=" + si.Type.ToString());
                }
            }
            catch (Exception ex)
            {
                _listBoxServices.Items.Add("Server services unavailable:" + ex.Message);
            }
        }
    }
}
