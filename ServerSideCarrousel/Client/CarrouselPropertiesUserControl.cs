using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace ServerSideCarrousel.Client
{
	public partial class CarrouselPropertiesUserControl : PropertiesUserControl
	{
		private ComboBox comboBoxSource;
        private CarrouselViewItemManager _carrouselViewItemManager;

		public CarrouselPropertiesUserControl(CarrouselViewItemManager carrouselViewItemManager)
		{
			InitializeComponent();
			_carrouselViewItemManager = carrouselViewItemManager;
		}

		private void OnLoad(object sender, EventArgs e)
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

		private void OnSourceSelected(object sender, EventArgs e)
		{
			if (comboBoxSource.SelectedItem != null && comboBoxSource.SelectedItem is ComboBoxNode)
			{
				_carrouselViewItemManager.SelectedCarrouselId = ((ComboBoxNode) comboBoxSource.SelectedItem).Item.FQID.ObjectId;				
			}
		}
		 
		#endregion


        /// <summary>
        /// Perform any cleanup stuff and event -=
        /// </summary>
        public override void Close()
        {
		}

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
