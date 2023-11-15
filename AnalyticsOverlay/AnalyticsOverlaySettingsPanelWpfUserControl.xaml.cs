using System;
using System.Collections.Generic;
using System.Linq;
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
            var form = new ItemPickerWpfWindow()
            {  
                KindsFilter = new List<Guid>() { Kind.Camera },
                SelectionMode = SelectionModeOptions.SingleSelect,
                SelectedItems = new List<Item>() { _selectedCameraItem },
                Items = Configuration.Instance.GetItemsByKind(Kind.Camera),
            };
            form.ShowDialog();
            var val = form.SelectedItems;
            if(val != null)
            {
                if (val.Any())
                {
                    _selectedCameraItem = val.First();
                    buttonCameraSelect.Content = _selectedCameraItem?.Name;
                }
                else
                {
                    _selectedCameraItem = null;
                    buttonCameraSelect.Content = "";
                }
            }
        }
    }
}
