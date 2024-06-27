using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;

namespace RGBVideoEnhancement.Client
{
    /// <summary>
    /// This UserControl contains the visible part of the Property panel during Setup mode. <br/>
    /// </summary>
    public partial class RGBVideoEnhancementPropertiesWpfUserControl : PropertiesWpfUserControl
    {

        #region private fields

        private RGBVideoEnhancementViewItemManager _viewItemManager;

        #endregion

        #region Initialization & Dispose

        /// <summary>
        /// This class is created by the ViewItemManager.  
        /// </summary>
        /// <param name="viewItemManager"></param>
        public RGBVideoEnhancementPropertiesWpfUserControl(RGBVideoEnhancementViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        /// <summary>
        /// Setup events and message receivers and load stored configuration.
        /// </summary>
        public override void Init()
        {
            if (_viewItemManager.SelectedCamera != null)
            {
                buttonSelect.Content = _viewItemManager.SelectedCamera.Name;
            }
        }

        /// <summary>
        /// Perform any cleanup stuff.
        /// </summary>
        public override void Close()
        {
        }


        #endregion

        #region Event handling

        private void OnSourceSelected(object sender, EventArgs e)
        {
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid>() { Kind.Camera },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
                Items = Configuration.Instance.GetItems()
            };
            form.ShowDialog();
            if(form.SelectedItems != null && form.SelectedItems.Any())
            {
                _viewItemManager.SelectedCamera = form.SelectedItems.First();
                buttonSelect.Content = _viewItemManager.SelectedCamera.Name;
            }
        }

        #endregion

    }
}
