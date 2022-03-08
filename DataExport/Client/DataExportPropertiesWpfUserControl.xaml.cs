using System.Windows.Controls;
using VideoOS.Platform.Client;

namespace DataExport.Client
{
    /// <summary>
    /// Interaction logic for DataExportPropertiesWpfUserControl.xaml
    /// </summary>
    public partial class DataExportPropertiesWpfUserControl : PropertiesWpfUserControl
    {

        #region private fields
        private DataExportViewItemManager _viewItemManager;

        #endregion

        #region Initialization & Dispose

        public DataExportPropertiesWpfUserControl(DataExportViewItemManager viewItemManager)
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

        private void _noteNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewItemManager.NoteName = _noteNameTextBox.Text;
        }

        #endregion
    }
}
