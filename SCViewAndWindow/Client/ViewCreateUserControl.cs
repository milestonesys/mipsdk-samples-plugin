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

namespace SCViewAndWindow.Client
{
	public partial class ViewCreateUserControl : UserControl
	{
		private ConfigItem _VGitem;						// The View Group (Top Level)
		private ConfigItem _groupItem;					// The Group
		private ViewAndLayoutItem _viewAndLayoutItem;	// The ViewLayout
		private Dictionary<Guid, ViewItemPlugin> viewItemPlugins = new Dictionary<Guid, ViewItemPlugin>();

		public ViewCreateUserControl()
		{
			InitializeComponent();
		}

		void OnLoad(object sender, EventArgs e)
		{
			if (!this.DesignMode)
			{
				comboBoxViewItemType.Items.Clear();
				comboBoxViewItemType.Items.Add(new GuidTag() { Guid = ViewAndLayoutItem.EmptyBuiltinId, Name = "Empty", Builtin = true });
				comboBoxViewItemType.Items.Add(new GuidTag() { Guid = ViewAndLayoutItem.CameraBuiltinId, Name = "Camera", Builtin = true });
				comboBoxViewItemType.Items.Add(new GuidTag() { Guid = ViewAndLayoutItem.HotspotBuiltinId, Name = "Hotspot", Builtin = true });
				comboBoxViewItemType.Items.Add(new GuidTag() { Guid = ViewAndLayoutItem.HTMLBuiltinId, Name = "HTML", Builtin = true });
				comboBoxViewItemType.Items.Add(new GuidTag() { Guid = ViewAndLayoutItem.MapBuiltinId, Name = "Map", Builtin = true });

				viewItemPlugins = new Dictionary<Guid, ViewItemPlugin>();
				foreach (PluginDefinition pd in EnvironmentManager.Instance.AllPluginDefinitions)
				{
					if (pd.ViewItemPlugins != null)
					{
						foreach (ViewItemPlugin vi in pd.ViewItemPlugins)
						{
							comboBoxViewItemType.Items.Add(new GuidTag() { Guid = vi.Id, Name = vi.Name, Builtin = false });
							viewItemPlugins.Add(vi.Id, vi);
						}
					}
				}
			}
		}


		private void OnCreateTempVG(object sender, EventArgs e)
		{
			if (textBoxVGNewName.Text!="")
			{
				_VGitem = (ConfigItem)ClientControl.Instance.CreateTemporaryGroupItem(textBoxVGNewName.Text);
				buttonCreateGroup.Enabled = true;
				textBoxGroupName.Enabled = true;

				textBoxSelectedGroup.Text = "";
				_groupItem = null;
				textBoxLayout.Text = "";
				groupBoxView.Enabled = false;

				buttonSelectGroup.Enabled = false;
			}
		}


		private void OnSelectGroup(object sender, EventArgs e)
		{
			try
			{
				ItemPickerForm form = new ItemPickerForm();
				form.Init(ClientControl.Instance.GetViewGroupItems());
				form.AutoAccept = false;
				form.KindFilter = Kind.View;
				form.ValidateSelectionEvent += new ItemPickerForm.ValidateSelectionHandler(ValidateSelectionHandler);

				if (form.ShowDialog() == DialogResult.OK)
				{
					_groupItem = (ConfigItem)form.SelectedItem;
					textBoxSelectedGroup.Text = _groupItem.Name;
					groupBoxView.Enabled = true;

					buttonCreateTemp.Enabled = false;
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
			if (e.Item.FQID.Kind == Kind.View && e.Item.FQID.FolderType != FolderType.No && e.Item is ConfigItem && e.Item.FQID.ParentId != Guid.Empty)
				e.AcceptSelection = true;
		}

		private void OnCreateGroup(object sender, EventArgs e)
		{
			if (_VGitem != null)
			{
				_groupItem = _VGitem.AddChild(textBoxGroupName.Text, Kind.View, FolderType.UserDefined);
				textBoxSelectedGroup.Text = _groupItem.Name;
				groupBoxView.Enabled = true;
			}
			else
				MessageBox.Show("Select where to Create new Group: Either a ViewGroup or Group");
		}

		private void OnCreate2x1(object sender, EventArgs e)
		{
			try
			{
				if (String.IsNullOrEmpty(textBoxLayout.Text))
				{
					MessageBox.Show("Please specifiy name of layout before creating");
				}
				else
				{
					// Check that the name entered is not already used:
					bool dup = false;
					foreach (Item currentView in _groupItem.GetChildren())
					{
						if (textBoxLayout.Text == currentView.Name)
						{
							MessageBox.Show("Duplicate name - please select another");
							dup = true;
						}
					}

					if (!dup)
					{
						Rectangle[] rect = new Rectangle[3];
						rect[0] = new Rectangle(000, 000, 999, 499);
						rect[1] = new Rectangle(000, 499, 499, 499);
						rect[2] = new Rectangle(499, 499, 499, 499);
						_viewAndLayoutItem = (ViewAndLayoutItem) _groupItem.AddChild(textBoxLayout.Text, Kind.View, FolderType.No);
						textBoxCount.Text = "3";
						_viewAndLayoutItem.Icon = Properties.Resources.UnknownLayout;
						_viewAndLayoutItem.Layout = rect;
						MakeIXList(3);
					}
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.ExceptionDialog("OnCreate2x1", ex);
			}

		}

		/// <summary>
		/// Fill the correct number of dropdowns, each containing a number.
		/// </summary>
		/// <param name="count"></param>
		private void MakeIXList(int count)
		{
			comboBoxViewItemIx.Items.Clear();
			for (int ix = 0; ix < count; ix++)
				comboBoxViewItemIx.Items.Add("" + ix);
			comboBoxViewItemIx.SelectedIndex = 0;
			buttonCreateView.Enabled = true;
			comboBoxViewItemIx.Enabled = true;
			comboBoxViewItemType.Enabled = true;
		}

		/// <summary>
		/// When the index is changed, try to find what is selected on that index and show in the viewitem type dropdown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnIndexChanged(object sender, EventArgs e)
		{
			ignoreSetting = true;
			int ix = comboBoxViewItemIx.SelectedIndex;
			if (_viewAndLayoutItem != null)
			{
				if (ix >= 0 && ix < _viewAndLayoutItem.Layout.Length)
				{
					bool found = false;
					Guid viewItemIdType = _viewAndLayoutItem.ViewItemId(ix);
					foreach (GuidTag gt in comboBoxViewItemType.Items)
					{
						if (gt.Guid == viewItemIdType)
						{
							comboBoxViewItemType.SelectedItem = gt;
							found = true;
							break;
						}
					}
					if (!found)
						comboBoxViewItemType.SelectedIndex = 0;
				}
			}
			ignoreSetting = false;

		}
		private bool ignoreSetting = false;

		/// <summary>
		/// When a viewitem has been selected for a given index, store in the viewlayout.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnViewItemChanged(object sender, EventArgs e)
		{
			if (ignoreSetting==false)
			{
				int ix = comboBoxViewItemIx.SelectedIndex;
				if (ix < 0)
				{
					MessageBox.Show("Select the index first");
				}
				else
				{
					GuidTag tag = (GuidTag) comboBoxViewItemType.SelectedItem;
					if (tag != null)
					{
						if (tag.Builtin)
						{
							Dictionary<String, String> properties = new Dictionary<string, string>();
							properties.Add("CameraId", Guid.NewGuid().ToString()); // add an id (Invalid right now) - For CametaViewItem
							properties.Add("URL", "www.google.com"); // Next 4 for a HTML item
							properties.Add("HideNavigationbar", "true");
							properties.Add("Scaling", "3"); // fit in 800x600
							//properties.Add("Addscript", "");
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

		internal class GuidTag
		{
			public Guid Guid { get; set; }
			public String Name { get; set; }
			public bool Builtin { get; set; }
			public override string ToString()
			{
				return Name;
			}
		}

		private void OnCreate(object sender, EventArgs e)
		{
			if (_viewAndLayoutItem.Name == "")
			{
				MessageBox.Show("Please supply a name before Creating");
			}
			else
			{
				_viewAndLayoutItem.Name = textBoxLayout.Text;
				_viewAndLayoutItem.Save();
				if (_VGitem != null)
					_VGitem.PropertiesModified();

				buttonCreateView.Enabled = false;
				buttonCreateTemp.Enabled = true;
				buttonSelectGroup.Enabled = true;
				buttonCreateGroup.Enabled = false;

				comboBoxViewItemIx.Items.Clear();
				comboBoxViewItemIx.Enabled = false;
				comboBoxViewItemType.Enabled = false;
				textBoxGroupName.Enabled = false;
				textBoxGroupName.Text = "";
				textBoxSelectedGroup.Text = "";
				textBoxVGNewName.Text = "";
				textBoxLayout.Text = "";
				textBoxCount.Text = "";
				groupBoxView.Enabled = false;
			}
		}

	}

}
