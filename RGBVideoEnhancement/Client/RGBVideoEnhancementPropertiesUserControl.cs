using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;

namespace RGBVideoEnhancement.Client
{
	/// <summary>
	/// This UserControl contains the visible part of the Property panel during Setup mode. <br/>
	/// If no properties is required by this ViewItemPlugin, the GeneratePropertiesUserControl() method on the ViewItemManager can return a value of null.
	/// <br/>
	/// When changing properties the ViewItemManager should continuously be updated with the changes to ensure correct saving of the changes.
	/// <br/>
	/// As the user click on different ViewItem, the displayed property UserControl will be disposed, and a new one created for the newly selected ViewItem.
	/// </summary>
	public partial class RGBVideoEnhancementPropertiesUserControl : PropertiesUserControl
	{

		#region private fields

		private RGBVideoEnhancementViewItemManager _viewItemManager;

		#endregion

		#region Initialization & Dispose

		/// <summary>
		/// This class is created by the ViewItemManager.  
		/// </summary>
		/// <param name="viewItemManager"></param>
		public RGBVideoEnhancementPropertiesUserControl(RGBVideoEnhancementViewItemManager viewItemManager)
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
				buttonSelect.Text = _viewItemManager.SelectedCamera.Name;
			}
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
			ItemPickerForm form = new ItemPickerForm();
			form.KindFilter = Kind.Camera;
			form.AutoAccept = true;
			form.Init(Configuration.Instance.GetItems());
			if (form.ShowDialog() == DialogResult.OK)
			{
				_viewItemManager.SelectedCamera = form.SelectedItem;
				buttonSelect.Text = _viewItemManager.SelectedCamera.Name;
			}

		}
		#endregion

	}

}
