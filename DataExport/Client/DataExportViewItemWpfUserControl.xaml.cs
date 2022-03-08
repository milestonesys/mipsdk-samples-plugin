using System;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace DataExport.Client
{

    public partial class DataExportViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private readonly DataExportViewItemManager _viewItemManager;

        #endregion

        #region Component constructors + dispose

        public DataExportViewItemWpfUserControl(DataExportViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();

        }

        private void SetUpApplicationEventListeners()
        {
            _viewItemManager.PropertyChangedEvent += ViewItemManagerPropertyChangedEvent;
 
        }

        private void RemoveApplicationEventListeners()
        {
            _viewItemManager.PropertyChangedEvent -= ViewItemManagerPropertyChangedEvent;   
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

        private void OnMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }

        void ViewItemManagerPropertyChangedEvent(object sender, EventArgs e)
        {
        }

        private void _noteTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
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
    }

    #endregion

}