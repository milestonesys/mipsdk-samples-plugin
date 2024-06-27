using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCViewAndWindow.Client
{
    public partial class SCViewAndWindowPropertiesWpfUserControl : PropertiesWpfUserControl
    {
        #region private fields

        private SCViewAndWindowViewItemManager _viewItemManager;

        #endregion


        #region Initialization & Dispose

        public SCViewAndWindowPropertiesWpfUserControl(SCViewAndWindowViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        private void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewItemManager.Config != null)
            {
                FillContent(_viewItemManager.Config, _viewItemManager.SomeId);
            }
        }

        /// <summary>
        /// We have some configuration from the server, that the user can choose from. The combobox is only enabled when there are IDs to choose from.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="selectedId"></param>
        internal void FillContent(List<Item> config, Guid selectedId)
        {
            comboBoxId.IsEnabled = true;
            comboBoxId.Items.Clear();
            ComboBoxNode selectedComboBoxNode = null;

            if(config.Count == 0)
            {
                comboBoxId.IsEnabled = false;
            }
            else 
            { 
            foreach (Item item in config)
            {
                ComboBoxNode comboBoxNode = new ComboBoxNode(item);
                comboBoxId.Items.Add(comboBoxNode);
                if (comboBoxNode.Item.FQID.ObjectId == selectedId)
                    selectedComboBoxNode = comboBoxNode;
            }

            if (selectedComboBoxNode != null)
                comboBoxId.SelectedItem = selectedComboBoxNode;
            }
        }

        #endregion

        #region Event handling

        private void OnSourceSelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _viewItemManager.SomeId = ((ComboBoxNode)comboBoxId.SelectedItem).Item.FQID.ObjectId;
        }


        #endregion


    }

    internal class ComboBoxNode
    {
        internal Item Item { get; set; }
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
