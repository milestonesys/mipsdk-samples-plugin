using System;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace DataExport.Client
{
    public partial class DataExportViewItemUserControl : ViewItemUserControl
    {
        #region Component private class variables

        private readonly DataExportViewItemManager _viewItemManager;
        private object _themeChangedReceiver;

        #endregion

        #region Component constructors + dispose

        public DataExportViewItemUserControl(DataExportViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();

            ClientControl.Instance.RegisterUIControlForAutoTheming(_noteTextBox);
        }

        private void SetUpApplicationEventListeners()
        {
            _viewItemManager.PropertyChangedEvent += ViewItemManagerPropertyChangedEvent;

            _themeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(ThemeChangedIndicationHandler,
                                             new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));

        }

        private void RemoveApplicationEventListeners()
        {
            _viewItemManager.PropertyChangedEvent -= ViewItemManagerPropertyChangedEvent;

            EnvironmentManager.Instance.UnRegisterReceiver(_themeChangedReceiver);
            _themeChangedReceiver = null;
        }

        public override void Init()
        {
            SetUpApplicationEventListeners();

            if (DataExportDefinition.SampleDataProvider.ContainsKey(_viewItemManager.NoteName))
            {
                _noteTextBox.Text = DataExportDefinition.SampleDataProvider[_viewItemManager.NoteName];
            }
        }

        public override void Close()
        {
            RemoveApplicationEventListeners();
        }

        #endregion

        #region Print method
        public override void Print()
        {
            Print(_viewItemManager.NoteName, _noteTextBox.Text);
        }

        #endregion


        #region Component events

        private void ViewItemUserControlMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FireClickEvent();
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


        public event EventHandler RightClickEvent;

        protected virtual void FireRightClickEvent(EventArgs e)
        {
            if (RightClickEvent != null)
            {
                RightClickEvent(this, e);
            }
        }

        private void _noteTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FireClickEvent();
            }
            else if (e.Button == MouseButtons.Right)
            {
                FireRightClickEvent(e);
            }
        }

        private void _noteTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FireDoubleClickEvent();
            }
        }

        void ViewItemManagerPropertyChangedEvent(object sender, EventArgs e)
        {
        }

        private object ThemeChangedIndicationHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            return null;
        }

        private void _noteTextBox_TextChanged(object sender, EventArgs e)
        {
            DataExportDefinition.SampleDataProvider[_viewItemManager.NoteName] = _noteTextBox.Text;
        }
        #endregion

        #region Component properties

        public override bool Maximizable
        {
            get { return true; }
        }

        public override bool Selectable
        {
            get { return true; }
        }


        #endregion
    }
}
