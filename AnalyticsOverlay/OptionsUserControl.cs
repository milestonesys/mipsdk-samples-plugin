using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;

namespace AnalyticsOverlay
{
	/// <summary>
	/// This dialog for the user to select which camera should have the squere overlay
	/// </summary>
	public partial class OptionsUserControl : OptionsDialogUserControl
	{
		private Item _selectedCameraItem;

		public OptionsUserControl()
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
					buttonCameraSelect.Text = _selectedCameraItem.Name;
				}
			}
		}

		private void OnSelectCamera(object sender, EventArgs e)
		{
			ItemPickerForm form = new ItemPickerForm();
			form.KindFilter = Kind.Camera;
			form.SelectedItem = _selectedCameraItem;
			form.AutoAccept = true;
			form.Init();
			if (form.ShowDialog() == DialogResult.OK)
			{
				_selectedCameraItem = form.SelectedItem;
				buttonCameraSelect.Text = "";
				if (_selectedCameraItem != null)
				{
					buttonCameraSelect.Text = _selectedCameraItem.Name;
				}
			}
		}

	}
}
