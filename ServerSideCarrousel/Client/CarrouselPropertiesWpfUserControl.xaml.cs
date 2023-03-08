using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private async void OnLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            //Call will communicate with service, this should be called on another thread than the UI thread
            List<Item> items = await Task<List<Item>>.Run(() =>
            {
                return Configuration.Instance.GetItemConfigurations(ServerSideCarrouselDefinition.CarrouselPluginId, null, ServerSideCarrouselDefinition.CarrouselKind);
            });
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
