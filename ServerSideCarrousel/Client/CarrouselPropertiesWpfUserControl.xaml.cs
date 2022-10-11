using System;
using System.Collections.Generic;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace ServerSideCarrousel.Client
{
    public partial class CarrouselPropertiesWpfUserControl : PropertiesWpfUserControl
    {
        private CarrouselViewItemManager _carrouselViewItemManager;

        public CarrouselPropertiesWpfUserControl(CarrouselViewItemManager carrouselViewItemManager)
        {
            InitializeComponent();
            _carrouselViewItemManager = carrouselViewItemManager;
        }

        private void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            List<Item> items = Configuration.Instance.GetItemConfigurations(ServerSideCarrouselDefinition.CarrouselPluginId, null, ServerSideCarrouselDefinition.CarrouselKind);
            FillContent(items, _carrouselViewItemManager.SelectedCarrouselId);
        }

        internal void FillContent(List<Item> items, Guid selectedId)
        {
            comboBoxSource.Items.Clear();
            ComboBoxNode selectedComboBoxNode = null;
            if (items != null)
            {
                foreach (Item item in items)
                {
                    ComboBoxNode comboBoxNode = new ComboBoxNode(item);
                    comboBoxSource.Items.Add(comboBoxNode);
                    if (comboBoxNode.Item.FQID.ObjectId == selectedId)
                        selectedComboBoxNode = comboBoxNode;
                }

                if (selectedComboBoxNode != null)
                    comboBoxSource.SelectedItem = selectedComboBoxNode;
            }
        }


        #region Event handling

        private void OnSourceSelected(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxSource.SelectedItem != null && comboBoxSource.SelectedItem is ComboBoxNode)
            {
                _carrouselViewItemManager.SelectedCarrouselId = ((ComboBoxNode)comboBoxSource.SelectedItem).Item.FQID.ObjectId;
            }
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
