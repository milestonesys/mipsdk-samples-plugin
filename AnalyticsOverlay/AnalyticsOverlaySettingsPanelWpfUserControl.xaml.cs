using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.UI;

namespace AnalyticsOverlay
{
    public partial class AnalyticsOverlaySettingsPanelWpfUserControl : System.Windows.Controls.UserControl
    {
        private Item _selectedCameraItem;

        public AnalyticsOverlaySettingsPanelWpfUserControl()
        {
            InitializeComponent();
        }

        public Item SelectedItem
        {
            get { return _selectedCameraItem; }
            set
            {
                _selectedCameraItem = value;
                if (_selectedCameraItem != null)
                {
                    buttonCameraSelect.Content = _selectedCameraItem.Name;
                }
            }
        }

        private void OnSelectCamera(object sender, System.Windows.RoutedEventArgs e)
        {
            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = Kind.Camera;
            form.SelectedItem = _selectedCameraItem;
            form.AutoAccept = true;
            form.Init();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _selectedCameraItem = form.SelectedItem;
                buttonCameraSelect.Content = "";
                if (_selectedCameraItem != null)
                {
                    buttonCameraSelect.Content = _selectedCameraItem.Name;
                }
            }
        }
    }
}
