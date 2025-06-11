using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

using VideoOS.Platform.UI.ItemPicker.Utilities;

namespace SCViewAndWindow.Client
{
    public partial class ViewEditWpfUserControl : System.Windows.Controls.UserControl
    {
        private Dictionary<Guid, ViewItemPlugin> viewItemPlugins = new Dictionary<Guid, ViewItemPlugin>();
        private ViewAndLayoutItem _viewAndLayoutItem;
        private ConfigItem _groupItem;
        private Item _selectedCameraItem;
        private MapResponseDataInstance _selectedMap;
        private Item _selectedViewItem = null;
        private bool _inLoad = true;
        private MessageCommunication _client;
        private object _msgObject;

        public ViewEditWpfUserControl()
        {
            InitializeComponent();
            InitializeMaps();
            PopulateViewItemTypes();
            HideAllSelection();
        }

        public void InitializeMaps()
        {
            try
            {
                MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
                _client = MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);

                bool x = _client.IsConnected;
                _msgObject = _client.RegisterCommunicationFilter(MapResponseHandler,
                    new CommunicationIdFilter(MessageId.Server.GetMapResponse));

                MapRequestData data = new MapRequestData()
                {
                    MapGuid = "",
                };
                _client.TransmitMessage(new VideoOS.Platform.Messaging.Message(MessageId.Server.GetMapRequest, data),
                    null, null, null);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Map Select issue:" + ex.Message);
            }
        }

        /// <summary>
        /// Populate ComboBoxViewItemTypes with view item types and view item plugins.
        /// </summary>
        void PopulateViewItemTypes()
        {
            _inLoad = true;
            comboBoxViewItemType.Items.Clear();
            comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = ViewAndLayoutItem.EmptyBuiltinId, Name = "Empty", Builtin = true });
            comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = ViewAndLayoutItem.CameraBuiltinId, Name = "Camera", Builtin = true });
            comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = ViewAndLayoutItem.HotspotBuiltinId, Name = "Hotspot", Builtin = true });
            comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = ViewAndLayoutItem.HTMLBuiltinId, Name = "HTML", Builtin = true });
            comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = ViewAndLayoutItem.MapBuiltinId, Name = "Map", Builtin = true });
            comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = ViewAndLayoutItem.TextBuiltInId, Name = "Text", Builtin = true });
            comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = ViewAndLayoutItem.ImageBuiltInId, Name = "Image", Builtin = true });
            comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = ViewAndLayoutItem.MatrixBuiltinId, Name = "Matrix", Builtin = true });
            comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = ViewAndLayoutItem.GisMapBuiltinId, Name = "SmartMap", Builtin = true });

            viewItemPlugins = new Dictionary<Guid, ViewItemPlugin>();
            foreach (PluginDefinition pd in EnvironmentManager.Instance.AllPluginDefinitions)
            {
                if (pd.ViewItemPlugins != null)
                {
                    foreach (ViewItemPlugin vi in pd.ViewItemPlugins)
                    {
                        comboBoxViewItemType.Items.Add(new ViewCreateWpfUserControl.GuidTag() { Guid = vi.Id, Name = vi.Name, Builtin = false });
                        viewItemPlugins.Add(vi.Id, vi);
                    }
                }
            }
            _inLoad = false;
        }

        /// <summary>
        /// Select group to delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectGroup(object sender, RoutedEventArgs e)
        {
            try
            {
                var form = new ItemPickerWpfWindow()
                {
                    AllowGroupSelection = true,
                    Items = ClientControl.Instance.GetViewGroupItems(),
                    SelectionMode = SelectionModeOptions.SingleSelect
                };
                form.ShowDialog();
                if(form.SelectedItems != null && form.SelectedItems.Any())
                {
                    _groupItem = (ConfigItem)form.SelectedItems.First();
                    buttonSelectGroup.Content = _groupItem.Name;
                }
                else
                {
                    buttonSelectGroup.Content = "Select View or Group";
                    _groupItem = null;
                }
                form.IsValidSelectionCallback = null;
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("OnSelectGroup", ex);
            }

        }

        /// <summary>
        /// Delete selected view or group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteViewOrGroup(object sender, RoutedEventArgs e)
        {
            if (_groupItem!=null)
            {
                ((ConfigItem)_groupItem.GetParent()).RemoveChild(_groupItem);
                buttonSelectGroup.Content = "Select View or Group";
            }
        }

        List<MapResponseDataInstance> mapList = new List<MapResponseDataInstance>();
        /// <summary>
        /// Add maps to combobox and mapList.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        private object MapResponseHandler(VideoOS.Platform.Messaging.Message message, FQID to, FQID from)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new MessageReceiver(MapResponseHandler), new object[] { message, to, from });
                return null;
            }
            else
            {
                MapResponseData data = message.Data as MapResponseData;
                if (data != null)
                {
                    comboMaps.Items.Clear();
                    mapList.Clear();
                    if (String.IsNullOrEmpty(data.ErrorText))
                    {
                        foreach (var entry in data.MapCollection)
                        {
                            if (!entry.RecursiveMap)
                            {
                                mapList.Add(entry);
                                var comboItem = new ComboBoxItem();
                                comboItem.Content = entry.DisplayName;
                                comboItem.Tag = entry.Id;
                                comboMaps.Items.Add(comboItem);
                            }
                        }
                    }
                    else
                    {
                        labelMapError.Content = data.ErrorText;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// View index is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnViewIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            {
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
                            comboBoxViewItemType.IsEnabled = true;
                        }
                        else
                        {
                            foreach (ViewCreateWpfUserControl.GuidTag tag in comboBoxViewItemType.Items)
                            {
                                if (tag.Guid.ToString() == _selectedViewItem.Properties["ViewItemId"])
                                {
                                    _inLoad = true;
                                    if (tag.Guid == ViewAndLayoutItem.CameraBuiltinId && _selectedViewItem.Properties.ContainsKey("CameraId"))
                                    {
                                        string cameraGuid = _selectedViewItem.Properties["CameraId"];
                                        if (cameraGuid != null)
                                        {
                                            IsCameraPartVisible(true);
                                            _selectedCameraItem = Configuration.Instance.GetItem(new Guid(cameraGuid), Kind.Camera);
                                            if (_selectedCameraItem != null)
                                            {
                                                buttonSelectCamera.Content = _selectedCameraItem.Name;
                                            }
                                        }
                                    }
                                    if (tag.Guid == ViewAndLayoutItem.HTMLBuiltinId)
                                    {
                                        IsHTMLPartVisible(true);
                                        if (_selectedViewItem.Properties.ContainsKey("URL"))
                                            textBoxURL.Text = _selectedViewItem.Properties["URL"];
                                        if (_selectedViewItem.Properties.ContainsKey("HideNavigationBar"))
                                            checkBoxNavigationBar.IsChecked = _selectedViewItem.Properties["HideNavigationBar"]=="false";
                                        if (_selectedViewItem.Properties.ContainsKey("Addscript"))
                                            checkBoxAddScript.IsChecked = _selectedViewItem.Properties["Addscript"] == "true";
                                    }
                                    if (tag.Guid == ViewAndLayoutItem.TextBuiltInId)
                                    {
                                        IsTextPartVisible(true);
                                        _selectedViewItem.Properties["Text"] = textBoxText.Text;
                                    }
                                    if (tag.Guid == ViewAndLayoutItem.ImageBuiltInId)
                                    {
                                        // Below line not yet implemented in Smart Client
                                        //_selectedViewItem.Properties["EmbeddedImage"] = ToBase64(someImage);
                                    }
                                    if (tag.Guid == ViewAndLayoutItem.MapBuiltinId)
                                    {
                                        IsMapPartVisible(true);
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

        public static string ToBase64(System.Drawing.Image image)
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

        /// <summary>
        /// View item for specified view index is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedViewItemChanged(object sender, RoutedEventArgs e)
        {
            if (!_inLoad)
            {
                int ix = comboBoxViewItemIx.SelectedIndex;
                if (ix < 0)
                {
                    buttonSave.IsEnabled = false;
                }
                else
                {
                    ViewCreateWpfUserControl.GuidTag tag = (ViewCreateWpfUserControl.GuidTag)comboBoxViewItemType.SelectedItem;
                    if (tag != null)
                    {
                        if (tag.Builtin)
                        {
                            HideAllSelection();
                            Dictionary<String, String> properties = new Dictionary<string, string>();
                            if (tag.Guid == ViewAndLayoutItem.HTMLBuiltinId)
                            {
                                if (textBoxURL.Text == "")
                                {
                                    textBoxURL.Text = "http://www.google.com";
                                }
                                IsHTMLPartVisible(true);
                            }

                            if (tag.Guid == ViewAndLayoutItem.CameraBuiltinId)
                            {
                                Guid cameraId = _selectedCameraItem == null ? Guid.Empty : _selectedCameraItem.FQID.ObjectId;
                                properties.Add("CameraId", cameraId.ToString());
                                IsCameraPartVisible(true);
                            }
                            properties.Add("URL", textBoxURL.Text);
                            properties.Add("Scaling", "4"); // fit in 640x480
                            properties.Add("Addscript", checkBoxAddScript.IsChecked.ToString());
                            properties.Add("HideNavigationBar", (!checkBoxNavigationBar.IsChecked).ToString());

                            if (tag.Guid == ViewAndLayoutItem.MapBuiltinId)
                            {
                                IsMapPartVisible(true);
                                if (_selectedMap == null)
                                {
                                    _selectedMap = mapList[0];
                                    comboMaps.SelectedIndex = 0;
                                }
                                String selectedMapGuid = _selectedMap.Id;
                                if (selectedMapGuid != null)
                                {
                                    properties.Add("mapguid", selectedMapGuid);
                                }
                            }
                            if (tag.Guid == ViewAndLayoutItem.TextBuiltInId)
                            {
                                properties.Add("Text", textBoxText.Text);
                                IsTextPartVisible(true);
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

        #region Visibility

        private void IsHTMLPartVisible(bool isVisible)
        {
            if (isVisible)
            {
                labelHTMLViewItem.Visibility = Visibility.Visible;
                labelURL.Visibility = Visibility.Visible;
                textBoxURL.Visibility = Visibility.Visible;
                checkBoxAddScript.Visibility = Visibility.Visible;
                checkBoxNavigationBar.Visibility = Visibility.Visible;
                IsCameraPartVisible(false);
                IsTextPartVisible(false);
                IsMapPartVisible(false);

            }
            else
            {
                labelHTMLViewItem.Visibility = Visibility.Collapsed;
                labelURL.Visibility = Visibility.Collapsed;
                textBoxURL.Visibility = Visibility.Collapsed;
                checkBoxAddScript.Visibility = Visibility.Collapsed;
                checkBoxNavigationBar.Visibility= Visibility.Collapsed;
            }
        }

        private void IsCameraPartVisible(bool isVisible)
        {
            if (isVisible)
            {
                labelCameraViewItem.Visibility = Visibility.Visible;
                buttonSelectCamera.Visibility = Visibility.Visible;
                IsHTMLPartVisible(false);
                IsTextPartVisible(false);
                IsMapPartVisible(false);

            }
            else
            {
                labelCameraViewItem.Visibility = Visibility.Collapsed;
                buttonSelectCamera.Visibility = Visibility.Collapsed;
            }
        }

        private void IsTextPartVisible(bool isVisible)
        {
            if (isVisible)
            {
                textBoxText.Visibility = Visibility.Visible;
                labelTextViewItem.Visibility = Visibility.Visible;
                IsHTMLPartVisible(false);
                IsCameraPartVisible(false);
                IsMapPartVisible(false);

            }
            else
            {
                textBoxText.Visibility = Visibility.Collapsed;
                labelTextViewItem.Visibility = Visibility.Collapsed;
            }
        }

        private void IsMapPartVisible(bool isVisible)
        {
            if (isVisible)
            {
                labelSelectMap.Visibility = Visibility.Visible;
                labelMapError.Visibility = Visibility.Visible;
                comboMaps.Visibility = Visibility.Visible;
                IsHTMLPartVisible(false);
                IsCameraPartVisible(false);
                IsTextPartVisible(false);

            }
            else
            {
                labelSelectMap.Visibility = Visibility.Collapsed;
                labelSelectMap.Visibility = Visibility.Collapsed;
                comboMaps.Visibility = Visibility.Collapsed;
            }
        }

        private void HideAllSelection()
        {
            IsHTMLPartVisible(false);
            IsCameraPartVisible(false);
            IsTextPartVisible(false);
            IsMapPartVisible(false);
        }
        #endregion


        private bool _ignoreChanged = false;

        /// <summary>
        /// Edit view and change fields to blank.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSave(object sender, RoutedEventArgs e)
        {
            if (_viewAndLayoutItem != null)
            {
                if (_viewAndLayoutItem.Name == "")
                {
                    System.Windows.MessageBox.Show("Please supply a name before Saving");
                }
                else
                {
                    _viewAndLayoutItem.Name = (string)buttonSelectView.Content;
                    try
                    {
                        _viewAndLayoutItem.Save();
                        _ignoreChanged = true;
                        _viewAndLayoutItem = null;
                        _selectedViewItem = null;
                        _selectedCameraItem = null;
                        _groupItem = null;
                        buttonSelectView.Content = "Select View";
                        comboBoxViewItemIx.Items.Clear();
                        buttonSelectCamera.Content = "Select Camera...";
                        buttonSelectGroup.Content = "Select View or Group";
                        textBoxURL.Text = "";
                        buttonSave.IsEnabled = false;
                        _ignoreChanged = false;
                        textBlockCount.Text = "";
                        HideAllSelection();
                    }
                    catch (Exception ex)
                    {
                        EnvironmentManager.Instance.ExceptionDialog("Save", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Select View to edit or delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectView(object sender, RoutedEventArgs e)
        {
            try
            {
                var form = new ItemPickerWpfWindow()
                {
                    Items = ClientControl.Instance.GetViewGroupItems(),
                    KindsFilter = new List<Guid>() { Kind.View },
                    SelectionMode = SelectionModeOptions.AutoCloseOnSelect
                };
                form.ShowDialog();
                if(form.SelectedItems != null && form.SelectedItems.Any())
                {
                    _viewAndLayoutItem = (ViewAndLayoutItem)form.SelectedItems.First();
                    buttonSelectView.Content = _viewAndLayoutItem.Name;
                    textBlockCount.Text = _viewAndLayoutItem.Layout.Length.ToString();
                    _inLoad = true;
                    MakeIXList(_viewAndLayoutItem.Layout.Length);
                    _inLoad = false;
                    OnViewIndexChanged(null, null);
                    buttonSave.IsEnabled = true;
                }
                else
                {
                    buttonSelectView.Content = "Select View";
                    _viewAndLayoutItem = null;
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("OnSelectGroup", ex);
            }
        }

        /// <summary>
        /// Create list of indexes.
        /// </summary>
        /// <param name="count"></param>
        private void MakeIXList(int count)
        {
            comboBoxViewItemIx.Items.Clear();
            for (int ix = 0; ix < count; ix++)
                comboBoxViewItemIx.Items.Add("" + ix);
            comboBoxViewItemIx.SelectedIndex = 0;
        }


        #region Property Changes

        private void OnSelectCamera(object sender, RoutedEventArgs e)
        {
            try
            {
                var form = new ItemPickerWpfWindow()
                {
                    Items = Configuration.Instance.GetItemsByKind(Kind.Camera),
                    KindsFilter = new List<Guid>() { Kind.Camera },
                    SelectionMode = SelectionModeOptions.SingleSelect
                };
                form.ShowDialog();
                if(form.SelectedItems != null && form.SelectedItems.Any())
                {
                    _selectedCameraItem = form.SelectedItems.First();
                    buttonSelectCamera.Content = _selectedCameraItem.Name;
                    OnSelectedViewItemChanged(null, null); //Store new Camera Selection
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("OnSelectCamera", ex);
            }
        }

        private void OnNavigationCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (_ignoreChanged == false)
                OnSelectedViewItemChanged(null, null);
        }

        private void OnURLChanged(object sender, TextChangedEventArgs e)
        {
            if (_ignoreChanged == false)
                OnSelectedViewItemChanged(null, null);
        }

        private void OnAddScriptCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (_ignoreChanged == false)
                OnSelectedViewItemChanged(null, null);
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_ignoreChanged == false)
                OnSelectedViewItemChanged(null, null);
        }

        private void OnMapSelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedMapComboItem = (ComboBoxItem)comboMaps.SelectedItem;
            if (selectedMapComboItem != null)
            {
                foreach (MapResponseDataInstance map in mapList)
                {
                    if ((string)selectedMapComboItem.Content == map.DisplayName)
                    {
                        _selectedMap = map;
                    }
                }
            }
            if (_ignoreChanged == false)
                OnSelectedViewItemChanged(null, null);
        }

        #endregion

    }
}

