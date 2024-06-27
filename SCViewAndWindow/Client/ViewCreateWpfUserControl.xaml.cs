using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI;

namespace SCViewAndWindow.Client
{
    public partial class ViewCreateWpfUserControl : System.Windows.Controls.UserControl
    {
        private ConfigItem _VGitem;                                         // The View Group (Top Level)
        private ConfigItem _groupItem;                                  // The Group
        private ViewAndLayoutItem _viewAndLayoutItem;   // The ViewLayout
        private Dictionary<Guid, ViewItemPlugin> viewItemPlugins = new Dictionary<Guid, ViewItemPlugin>();
        private static readonly System.Drawing.Image _unknownLayoutIcon;

        static ViewCreateWpfUserControl()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.UnknownLayout.png");
            if (pluginStream != null)
                _unknownLayoutIcon = System.Drawing.Image.FromStream(pluginStream);
        }

        public ViewCreateWpfUserControl()
        {
            InitializeComponent();
            PopulateViewItemComboBox();
        }

        private void PopulateViewItemComboBox()
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

        private void OnCreateTempVG(object sender, System.Windows.RoutedEventArgs e)
        {
            if (textBoxVGNewName.Text!="")
            {
                _VGitem = (ConfigItem)ClientControl.Instance.CreateTemporaryGroupItem(textBoxVGNewName.Text);
                buttonCreateGroup.IsEnabled = true;
                textBoxGroupName.IsEnabled = true;

                buttonSelectGroup.Content = "Select Group";
                _groupItem = null;
                textBoxLayout.Text = "";
                groupBoxView.IsEnabled = false;

                buttonSelectGroup.IsEnabled = false;
            }
        }

        private void OnSelectGroup(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var form = new ItemPickerWpfWindow()
                {
                    Items = ClientControl.Instance.GetViewGroupItems(),
                    KindsFilter = new List<Guid>() { Kind.View },
                    SelectionMode = SelectionModeOptions.SingleSelect
                };
                form.IsValidSelectionCallback = ValidateSelectionHandler;
                form.ShowDialog();
                if(form.SelectedItems != null && form.SelectedItems.Any())
                {
                    _groupItem = (ConfigItem)form.SelectedItems.First();
                    buttonSelectGroup.Content = _groupItem.Name;
                    groupBoxView.IsEnabled= true;

                    buttonCreateTemp.IsEnabled= false;
                }
                else
                {
                    buttonSelectGroup.Content = "Select Group";
                    _groupItem = null;
                    groupBoxView.IsEnabled = false;
                }
                form.IsValidSelectionCallback = null;
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("OnSelectGroup", ex);
            }
        }
        private bool ValidateSelectionHandler(Item item)
        {
            return item != null && item.FQID.Kind == Kind.View && item.FQID.FolderType != FolderType.No && item is ConfigItem && item.FQID.ParentId != Guid.Empty;
        }

        private void OnCreateGroup(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_VGitem != null)
            {
                _groupItem = _VGitem.AddChild(textBoxGroupName.Text, Kind.View, FolderType.UserDefined);
                buttonSelectGroup.Content = _groupItem.Name;
                groupBoxView.IsEnabled = true;
            }
            else
                System.Windows.MessageBox.Show("Select where to Create new Group: Either a ViewGroup or Group");
        }

        private void OnCreate2x1(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(textBoxLayout.Text))
                {
                    System.Windows.MessageBox.Show("Please specifiy name of layout before creating");
                }
                else
                {
                    // Check that the name entered is not already used:
                    bool dup = false;
                    foreach (Item currentView in _groupItem.GetChildren())
                    {
                        if (textBoxLayout.Text == currentView.Name)
                        {
                            System.Windows.MessageBox.Show("Duplicate name - please select another");
                            dup = true;
                        }
                    }

                    if (!dup)
                    {
                        System.Drawing.Rectangle[] rect = new System.Drawing.Rectangle[3];
                        rect[0] = new System.Drawing.Rectangle(000, 000, 999, 499);
                        rect[1] = new System.Drawing.Rectangle(000, 499, 499, 499);
                        rect[2] = new System.Drawing.Rectangle(499, 499, 499, 499);
                        _viewAndLayoutItem = (ViewAndLayoutItem)_groupItem.AddChild(textBoxLayout.Text, Kind.View, FolderType.No);
                        _viewAndLayoutItem.Icon = _unknownLayoutIcon;
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
            buttonCreateView.IsEnabled = true;
            comboBoxViewItemIx.IsEnabled = true;
            comboBoxViewItemType.IsEnabled = true;
        }

        /// <summary>
        /// When the index is changed, try to find what is selected on that index and show in the viewitem type dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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
        private void OnViewItemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ignoreSetting==false)
            {
                int ix = comboBoxViewItemIx.SelectedIndex;
                if (ix < 0)
                {
                    System.Windows.MessageBox.Show("Select the index first");
                }
                else
                {
                    GuidTag tag = (GuidTag)comboBoxViewItemType.SelectedItem;
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

        private void OnCreate(object sender, System.Windows.RoutedEventArgs e)
        {
            try { 
            if (_viewAndLayoutItem.Name == "")
            {
                System.Windows.MessageBox.Show("Please supply a name before Creating");
            }
            else
            {
                _viewAndLayoutItem.Name = textBoxLayout.Text;
                _viewAndLayoutItem.Save();
                if (_VGitem != null)
                    _VGitem.PropertiesModified();

                buttonCreateView.IsEnabled = false;
                buttonCreateTemp.IsEnabled = true;
                buttonSelectGroup.IsEnabled = true;
                buttonCreateGroup.IsEnabled = false;

                comboBoxViewItemIx.Items.Clear();
                comboBoxViewItemIx.IsEnabled = false;
                comboBoxViewItemType.IsEnabled = false;
                textBoxGroupName.IsEnabled = false;
                textBoxGroupName.Text = "";
                buttonSelectGroup.Content = "Select Group";
                textBoxVGNewName.Text = "";
                textBoxLayout.Text = "";
                groupBoxView.IsEnabled = false;
            }
            }catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
