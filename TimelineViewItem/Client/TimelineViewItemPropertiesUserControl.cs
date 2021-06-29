using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace TimelineViewItem.Client
{
    /// <summary>
    /// This UserControl contains the visible part of the Property panel during Setup mode. <br/>
    /// If no properties is required by this ViewItemPlugin, the GeneratePropertiesUserControl() method on the ViewItemManager can return a value of null.
    /// <br/>
    /// When changing properties the ViewItemManager should continuously be updated with the changes to ensure correct saving of the changes.
    /// <br/>
    /// As the user click on different ViewItem, the displayed property UserControl will be disposed, and a new one created for the newly selected ViewItem.
    /// </summary>
    public partial class TimelineViewItemPropertiesUserControl : PropertiesUserControl
    {

        #region private fields

        private ComboBox comboBoxID;
        private TimelineViewItemViewItemManager _viewItemManager;

        #endregion

        #region Initialization & Dispose

        /// <summary>
        /// This class is created by the ViewItemManager.  
        /// </summary>
        /// <param name="viewItemManager"></param>
        public TimelineViewItemPropertiesUserControl(TimelineViewItemViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        /// <summary>
        /// Setup events and message receivers and load stored configuration.
        /// </summary>
        public override void Init()
        {
            if (_viewItemManager.ConfigItems != null)
            {
                FillContent(_viewItemManager.ConfigItems, _viewItemManager.SomeId);
            }
        }

        /// <summary>
        /// Perform any cleanup stuff and event -=
        /// </summary>
        public override void Close()
        {
        }

        /// <summary>
        /// We have some configuration from the server, that the user can choose from.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="selectedId"></param>
        internal void FillContent(List<Item> config, Guid selectedId)
        {
            comboBoxID.Items.Clear();
            ComboBoxNode selectedComboBoxNode = null;

            foreach (Item item in config)
            {
                ComboBoxNode comboBoxNode = new ComboBoxNode(item);
                comboBoxID.Items.Add(comboBoxNode);
                if (comboBoxNode.Item.FQID.ObjectId == selectedId)
                    selectedComboBoxNode = comboBoxNode;
            }

            if (selectedComboBoxNode != null)
                comboBoxID.SelectedItem = selectedComboBoxNode;
        }


        #endregion

        #region Event handling

        private void OnSourceSelected(object sender, EventArgs e)
        {
            _viewItemManager.SomeId = ((ComboBoxNode)comboBoxID.SelectedItem).Item.FQID.ObjectId;
        }
        #endregion

    }

    internal class ComboBoxNode
    {
        internal Item Item { get; private set; }
        internal ComboBoxNode(Item item)
        {
            Item = item;
        }

        public override string ToString()
        {
            return Item.Name;
        }
    }
}
