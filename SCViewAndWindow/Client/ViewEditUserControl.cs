using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Schema;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using Message = System.Windows.Forms.Message;

namespace SCViewAndWindow.Client
{
	public partial class ViewEditUserControl : UserControl
	{
		private Dictionary<Guid, ViewItemPlugin> viewItemPlugins = new Dictionary<Guid, ViewItemPlugin>();
		private ViewAndLayoutItem _viewAndLayoutItem;
		private ConfigItem _groupItem;
		private Item _selectedCameraItem;
		private Item _selectedViewItem = null;
		private bool _inLoad = true;

		public ViewEditUserControl()
		{
			InitializeComponent();
		}

		void OnLoad(object sender, EventArgs e)
		{
			_inLoad = true;
			if (!this.DesignMode)
			{
				comboBoxViewItemType.Items.Clear();
				comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = ViewAndLayoutItem.EmptyBuiltinId, Name = "Empty", Builtin = true });
				comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = ViewAndLayoutItem.CameraBuiltinId, Name = "Camera", Builtin = true });
				comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = ViewAndLayoutItem.HotspotBuiltinId, Name = "Hotspot", Builtin = true });
				comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = ViewAndLayoutItem.HTMLBuiltinId, Name = "HTML", Builtin = true });
				comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = ViewAndLayoutItem.MapBuiltinId, Name = "Map", Builtin = true });
                comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = ViewAndLayoutItem.TextBuiltInId, Name = "Text", Builtin = true });
                comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = ViewAndLayoutItem.ImageBuiltInId, Name = "Image", Builtin = true });
                comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = ViewAndLayoutItem.MatrixBuiltinId, Name = "Matrix", Builtin = true });
                comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = ViewAndLayoutItem.GisMapBuiltinId, Name = "SmartMap", Builtin = true });

                viewItemPlugins = new Dictionary<Guid, ViewItemPlugin>();
				foreach (PluginDefinition pd in EnvironmentManager.Instance.AllPluginDefinitions)
				{
					if (pd.ViewItemPlugins != null)
					{
						foreach (ViewItemPlugin vi in pd.ViewItemPlugins)
						{
							comboBoxViewItemType.Items.Add(new ViewCreateUserControl.GuidTag() { Guid = vi.Id, Name = vi.Name, Builtin = false });
							viewItemPlugins.Add(vi.Id, vi);
						}
					}
				}
			}
			_inLoad = false;
		}

		private void OnSelectGroup(object sender, EventArgs e)
		{
			try
			{
				ItemPickerForm form = new ItemPickerForm();
				form.Init(ClientControl.Instance.GetViewGroupItems());
                //form.Init(ClientControl.Instance.GetSmartWallItems());
                form.AutoAccept = false;
				//form.KindFilter = Kind.SmartWall;
				form.ValidateSelectionEvent += new ItemPickerForm.ValidateSelectionHandler(ValidateSelectionHandler);

				if (form.ShowDialog() == DialogResult.OK)
				{
					_groupItem = (ConfigItem)form.SelectedItem;
					textBoxSelectedGroup.Text = _groupItem.Name;
					groupBoxView.Enabled = true;
				}
				else
				{
					textBoxSelectedGroup.Text = "";
					_groupItem = null;
					groupBoxView.Enabled = false;
				}

				form.ValidateSelectionEvent -= new ItemPickerForm.ValidateSelectionHandler(ValidateSelectionHandler);
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.ExceptionDialog("OnSelectGroup", ex);
			}

		}

		private void ValidateSelectionHandler(ItemPickerForm.ValidateEventArgs e)
		{
			//if (e.Item.FQID.Kind == Kind.View && e.Item.FQID.FolderType != FolderType.No && e.Item is ConfigItem && e.Item.FQID.ParentId!=Guid.Empty)
				e.AcceptSelection = true;
		}

		private void OnDeleteGroup(object sender, EventArgs e)
		{
			if (_groupItem!=null)
			{
				((ConfigItem) _groupItem.GetParent()).RemoveChild(_groupItem);
			}
		}

		private void OnIndexChanged(object sender, EventArgs e)
		{
			//if (!_inLoad)
			{
				buttonSelectCamera.Text = "Select Camera...";
				_selectedCameraItem = null;

				int ix = comboBoxViewItemIx.SelectedIndex;
				if (_viewAndLayoutItem != null)
				{
					if (ix >= 0 && ix < _viewAndLayoutItem.Layout.Length)
					{
						List<Item> viewItems = _viewAndLayoutItem.GetChildren();
						_selectedViewItem = null;
						foreach (Item viewItem in viewItems)
						{
							if (viewItem.FQID.ObjectIdString == ix.ToString())
							{
								_selectedViewItem = viewItem as Item;
								break;
							}
						}
						if (_selectedViewItem == null)
						{
							comboBoxViewItemType.Text = "";
							comboBoxViewItemType.Enabled = true;
						}
						else
						{
							foreach (ViewCreateUserControl.GuidTag tag in comboBoxViewItemType.Items)
							{
								if (tag.Guid.ToString() == _selectedViewItem.Properties["ViewItemId"])
								{
									_inLoad = true;
									if (tag.Guid == ViewAndLayoutItem.CameraBuiltinId && _selectedViewItem.Properties.ContainsKey("CameraId"))
									{
										string cameraGuid = _selectedViewItem.Properties["CameraId"];
										if (cameraGuid != null)
										{
											_selectedCameraItem = Configuration.Instance.GetItem(new Guid(cameraGuid), Kind.Camera);
											if (_selectedCameraItem != null)
											{
												buttonSelectCamera.Text = _selectedCameraItem.Name;
											}
										}
									}
									if (tag.Guid == ViewAndLayoutItem.HTMLBuiltinId)
									{
										if (_selectedViewItem.Properties.ContainsKey("URL"))
											textBoxURL.Text = _selectedViewItem.Properties["URL"];
										if (_selectedViewItem.Properties.ContainsKey("HideNavigationBar"))
											checkBoxNavigationBar.Checked = _selectedViewItem.Properties["HideNavigationBar"]=="false";
										if (_selectedViewItem.Properties.ContainsKey("Addscript"))
											checkBoxAddScript.Checked = _selectedViewItem.Properties["Addscript"] == "true";
									}
								    if (tag.Guid == ViewAndLayoutItem.TextBuiltInId)
								    {
                                        _selectedViewItem.Properties["Text"] = textBoxText.Text;
								    }
								    if (tag.Guid == ViewAndLayoutItem.ImageBuiltInId)
								    {
                                        // Below line not yet implemented in Smart Client
								        //_selectedViewItem.Properties["EmbeddedImage"] = ToBase64(someImage);
								    }
									comboBoxViewItemType.SelectedItem = tag;
									_inLoad = false;
								}
							}
						}
					}
				}
			}
		}
        public static string ToBase64(Image image)
        {
            if (image == null)
                return "";

            using (var oms = new MemoryStream())
            {
                image.Save(oms, image.RawFormat);
                var base64 = Convert.ToBase64String(oms.ToArray());
                return base64;
            }
        }
		private void OnSelectedViewItemChanged(object sender, EventArgs e)
		{
			if (!_inLoad)
			{
				int ix = comboBoxViewItemIx.SelectedIndex;
				if (ix < 0)
				{
					MessageBox.Show("Select the index first");
				}
				else
				{
					ViewCreateUserControl.GuidTag tag = (ViewCreateUserControl.GuidTag) comboBoxViewItemType.SelectedItem;
					if (tag != null)
					{
						if (tag.Builtin)
						{
							if (tag.Guid == ViewAndLayoutItem.HTMLBuiltinId && textBoxURL.Text == "")
								textBoxURL.Text = "http://www.google.com";
							Dictionary<String, String> properties = new Dictionary<string, string>();
							Guid cameraId = _selectedCameraItem == null ? Guid.Empty : _selectedCameraItem.FQID.ObjectId;
							properties.Add("CameraId", cameraId.ToString());

							properties.Add("URL", textBoxURL.Text);
							properties.Add("Scaling", "4"); // fit in 640x480
							properties.Add("Addscript", checkBoxAddScript.Checked.ToString());
							properties.Add("HideNavigationBar", (!checkBoxNavigationBar.Checked).ToString());
						    String mapGuid = buttonSelectMap.Tag as String;
                            if (mapGuid != null)
							    properties.Add("mapguid", mapGuid);

						    if (tag.Guid == ViewAndLayoutItem.TextBuiltInId)
						    {
                                properties.Add("Text", textBoxText.Text);
						    }
							_viewAndLayoutItem.InsertBuiltinViewItem(ix, tag.Guid, properties);
						}
						else
						{
							Dictionary<String, String> properties = new Dictionary<string, string>();
							_viewAndLayoutItem.InsertViewItemPlugin(ix, viewItemPlugins[tag.Guid], properties);
						}
					}
				}
			}
		}

		private bool _ignoreChanged = false;
		private void OnSave(object sender, EventArgs e)
		{
			if (_viewAndLayoutItem != null)
			{
				if (_viewAndLayoutItem.Name == "")
				{
					MessageBox.Show("Please supply a name before Saving");
				}
				else
				{
					_viewAndLayoutItem.Name = textBoxLayout.Text;
					try
					{
						_viewAndLayoutItem.Save();

						_ignoreChanged = true;
						_viewAndLayoutItem = null;
						_selectedViewItem = null;
						_selectedCameraItem = null;
						_groupItem = null;
						textBoxSelectedGroup.Text = "";
						textBoxLayout.Text = "";
						comboBoxViewItemIx.Items.Clear();
						buttonSelectCamera.Text = "Select Camera...";
						textBoxURL.Text = "";
						button7.Enabled = false;
						_ignoreChanged = false;
					}
					catch (Exception ex)
					{
						EnvironmentManager.Instance.ExceptionDialog("Save", ex);
					}
				}
			}
		}

		private void OnSelectView(object sender, EventArgs e)
		{
			try
			{
				ItemPickerForm form = new ItemPickerForm();
				form.Init(ClientControl.Instance.GetViewGroupItems());
                //form.Init(ClientControl.Instance.GetSmartWallItems());
                form.AutoAccept = true;
				form.KindFilter = Kind.View;
				if (form.ShowDialog() == DialogResult.OK)
				{
					_viewAndLayoutItem = (ViewAndLayoutItem)form.SelectedItem;
					textBoxLayout.Text = _viewAndLayoutItem.Name;
					groupBoxView.Enabled = true;
					textBoxCount.Text = ""+_viewAndLayoutItem.Layout.Length;
					_inLoad = true;
					MakeIXList(_viewAndLayoutItem.Layout.Length);
					_inLoad = false;
					OnIndexChanged(null, null);
					//OnSelectedViewItemChanged(null, null);
					button7.Enabled = true;
				}
				else
				{
					textBoxLayout.Text = "";
					_viewAndLayoutItem = null;
					groupBoxView.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.ExceptionDialog("OnSelectGroup", ex);
			}

		}

		private void MakeIXList(int count)
		{
			comboBoxViewItemIx.Items.Clear();
			for (int ix = 0; ix < count; ix++)
				comboBoxViewItemIx.Items.Add("" + ix);
			comboBoxViewItemIx.SelectedIndex = 0;
		}

		private void OnDeleteView(object sender, EventArgs e)
		{
			if (_viewAndLayoutItem!=null)
			{
				((ConfigItem)_viewAndLayoutItem.GetParent()).RemoveChild(_viewAndLayoutItem);
			}
		}

		private void OnSelectCamera(object sender, EventArgs e)
		{
			try
			{
				ItemPickerForm form = new ItemPickerForm();
				form.KindFilter = Kind.Camera;
				form.Init();
				form.AutoAccept = false;				
				if (form.ShowDialog() == DialogResult.OK)
				{
					_selectedCameraItem = form.SelectedItem;
					buttonSelectCamera.Text = _selectedCameraItem.Name;

					OnSelectedViewItemChanged(null, null);		// Store new Camera Selection
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.ExceptionDialog("OnSelectCamera", ex);
			}
		}

		private void OnURLChanged(object sender, EventArgs e)
		{
			if (_ignoreChanged == false)
				OnSelectedViewItemChanged(null, null);		// Store new URL Selection
		}

		private void OnCheckedChanged(object sender, EventArgs e)
		{
			if (_ignoreChanged == false)
				OnSelectedViewItemChanged(null, null);
		}

        private void buttonSelectMap_Click(object sender, EventArgs e)
        {
            MapSelectForm form = new MapSelectForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                buttonSelectMap.Text = form.SelectedMapName;
                buttonSelectMap.Tag = form.SelectedMapId;
            }
        }

		
	}
}
