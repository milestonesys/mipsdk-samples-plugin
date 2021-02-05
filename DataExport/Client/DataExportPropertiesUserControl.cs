using System;
using VideoOS.Platform.Client;

namespace DataExport.Client
{
    public partial class DataExportPropertiesUserControl : PropertiesUserControl
    {

        #region private fields
        private DataExportViewItemManager _viewItemManager;

        #endregion

        #region Initialization & Dispose

        public DataExportPropertiesUserControl(DataExportViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        public override void Init()
        {
            if (_viewItemManager.NoteName != null)
            {
                _noteNameTextBox.Text = _viewItemManager.NoteName;
            }
        }

        public override void Close()
        {
        }

        #endregion

        #region Event handling

        private void _noteNameTextBox_TextChanged(object sender, EventArgs e)
        {
            _viewItemManager.NoteName = _noteNameTextBox.Text;
        }
        #endregion
    }
}
