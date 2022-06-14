using System;
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
            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = Kind.Camera;
            form.AutoAccept = true;
            form.Init(Configuration.Instance.GetItems());
            if (form.ShowDialog() == DialogResult.OK)
            {
                _viewItemManager.SelectedCamera = form.SelectedItem;
                buttonSelect.Content = _viewItemManager.SelectedCamera.Name;
            }
        }

        #endregion

    }
}
