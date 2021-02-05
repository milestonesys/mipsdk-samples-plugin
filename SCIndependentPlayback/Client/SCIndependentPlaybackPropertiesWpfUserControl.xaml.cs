using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;
using WPF = System.Windows.Controls;

namespace SCIndependentPlayback.Client
{
    /// <summary>
    /// This UserControl contains the visible part of the Property panel during Setup mode. <br/>
    /// It uses ItemPickerForm which currently is still winforms. <br/>
    /// If no properties is required by this ViewItemPlugin, the SCIndependentPlaybackPropertiesWpfUserControl() method on the ViewItemManager can return a value of null.
    /// <br/>
    /// When changing properties the ViewItemManager should continuously be updated with the changes to ensure correct saving of the changes.
    /// <br/>
    /// As the user click on different ViewItem, the displayed property UserControl will be disposed, and a new one created for the newly selected ViewItem.
    /// </summary>
    public partial class SCIndependentPlaybackPropertiesWpfUserControl : PropertiesWpfUserControl
    {
        #region private fields

        private SCIndependentPlaybackViewItemManager _viewItemManager;

        #endregion

        #region Initialization & Dispose
        public SCIndependentPlaybackPropertiesWpfUserControl(SCIndependentPlaybackViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        /// <summary>
        /// Setup events and message receivers and load stored configuration.
        /// </summary>
        public override void Init()
        {
        }

        /// <summary>
        /// Perform any cleanup stuff and event -=
        /// </summary>
        public override void Close()
        {
        }

        #endregion

        #region Event handling
        private void OnSourceSelected(object sender, RoutedEventArgs e)
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
