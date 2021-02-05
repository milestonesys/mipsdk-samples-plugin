using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCViewAndWindow.Client
{
	public partial class SCViewAndWindowPropertiesUserControl : PropertiesUserControl
	{

		#region private fields

		private ComboBox comboBoxID;
		private SCViewAndWindowViewItemManager _viewItemManager;

		#endregion

		#region Initialization & Dispose

		public SCViewAndWindowPropertiesUserControl(SCViewAndWindowViewItemManager viewItemManager)
		{
			_viewItemManager = viewItemManager;
			InitializeComponent();
		}

		public void OnLoad(object sender, EventArgs e)
		{
			if (!this.DesignMode)
			{
				if (_viewItemManager.Config != null)
				{
					FillContent(_viewItemManager.Config, _viewItemManager.SomeId);
				}
			}
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

		/// <summary>
		/// Perform any cleanup stuff and event -=
		/// </summary>
		public override void Close()
		{
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
