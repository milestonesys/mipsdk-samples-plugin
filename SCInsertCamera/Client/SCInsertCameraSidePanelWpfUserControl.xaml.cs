using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace SCInsertCamera.Client
{
    /// <summary>
    /// Interaction logic for SCInsertCameraSidePanelWpfUserControl.xaml
    /// </summary>
    public partial class SCInsertCameraSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        private object _selectedViewChanged;
        private SCInsertCameraSidePanelPlugin _sCInsertCameraSidePanelPlugin;
        private Item _selectedCamera;
        private const string _serializedMSLogo = "Qk02DAAAAAAAADYAAAAoAAAAIAAAACAAAAABABgAAAAAAAAMAADDDgAAww4AAAAAAAAAAAAA/////////////////////////////////////////////////////////////Pfr////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////+vHb3qQb68l3////////////////////////////////////////////////////////////////////////////////////////////////////////////////+vHb3qQb2pkA2pkA68l3////////////////////////////////////////////////////////////////////////////////////////////////////////+vHb3qQb2pkA2pkA2pkA2pkA68h2///+////////////////////////////////////////////////////////////////////////////////////////////+O3S3aEU2pkA2pkA2pkA2pkA2pkA2pkA6cNo///+////////////////////////////////////////////////////////////////////////////////////+O3S3aEU2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA6cNo///+////////////////////////////////////////////////////////////////////////////+O3S3aEU2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA6cNo///+////////////////////////////////////////////////////////////////////+O3S3aEU2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA6cNo///+////////////////////////////////////////////////////////////9+nI3aEU2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA575d//78////////////////////////////////////////////////////9+nI3J8O2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA571a//78////////////////////////////////////////////9+nI3J8O2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA571a//78////////////////////////////////////9+nI3J8O2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA571a//78////////////////////////////9ufD3J8O2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA5rtW/v35////////////////////9eW9250J2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA5bhN/v35////////////9eW9250J2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA5bhN/v35////9eW9250J2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA5bhN/v3589+u25sF2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA47I//fnx////9OCx25sF2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA47NB/vv1////////////9OCx25sF2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA47NB/vv1////////////////////9OCx25sF2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA47NB/vv1////////////////////////////9OGz250J2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA5LVF/vv1////////////////////////////////////9eW9250J2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA5bhN/v35////////////////////////////////////////////9eW9250J2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA5bhN/v35////////////////////////////////////////////////////9eW9250J2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA5bhN/v35////////////////////////////////////////////////////////////9eW9250K2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA5bhN/v35////////////////////////////////////////////////////////////////////9+nI3J8O2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA571a/v36////////////////////////////////////////////////////////////////////////////9+nI3J8O2pkA2pkA2pkA2pkA2pkA2pkA2pkA2pkA571a//78////////////////////////////////////////////////////////////////////////////////////9+nI3J8O2pkA2pkA2pkA2pkA2pkA2pkA571a//78////////////////////////////////////////////////////////////////////////////////////////////9+nI3J8O2pkA2pkA2pkA2pkA571a//78////////////////////////////////////////////////////////////////////////////////////////////////////+OvN3aEU2pkA2pkA6MFj//78////////////////////////////////////////////////////////////////////////////////////////////////////////////+O3S3aEU6cNo///+////////////////////////////////////////////////////////////////////////////////////////////////////////////////////+/Pi///+////////////////////////////////////////////////////////////";
        public SCInsertCameraSidePanelWpfUserControl(SCInsertCameraSidePanelPlugin sCInsertCameraSidePanelPlugin)
        {
            _sCInsertCameraSidePanelPlugin = sCInsertCameraSidePanelPlugin;
            InitializeComponent();
        }

        public override void Init()
        {
            _selectedViewChanged =
                EnvironmentManager.Instance.RegisterReceiver(ViewChangedHandler, new MessageIdFilter(MessageId.SmartClient.SelectedViewChangedIndication));
            UpdateIndexes();
        }
        public override void Close()
        {
            EnvironmentManager.Instance.UnRegisterReceiver(_selectedViewChanged);
        }

        /// <summary>
        /// This method keeps an eye with what view layout the user is selecting, 
        /// stores it under _currentView, and show the name in the label textBoxLayoutName.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="s"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private object ViewChangedHandler(Message message, FQID s, FQID r)
        {
            _sCInsertCameraSidePanelPlugin.CurrentView = message.Data as ViewAndLayoutItem;

            UpdateIndexes();
            return null;
        }

        /// <summary>
        /// To Select a single camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectCameraClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid>() { Kind.Camera },
                Items = Configuration.Instance.GetItemsByKind(Kind.Camera),                
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect
            };
            form.ShowDialog();
            if(form.SelectedItems != null && form.SelectedItems.Any())
            {
                _selectedCamera = form.SelectedItems.First();
                buttonSelect.Content = _selectedCamera.Name;
                buttonInsert.IsEnabled = true;

                comboBoxStream.Items.Clear();
                comboBoxStream.SelectedIndex = 0;

                var streamDataSource = new StreamDataSource(_selectedCamera);
                var streams = streamDataSource.GetTypes();
                foreach(var stream in streams)
                {
                    comboBoxStream.Items.Add(stream);
                }
                comboBoxStream.IsEnabled = streams.Any();
            }
        }

        /// <summary>
        /// This method does all what is required to create a group and viewlayout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreateClick(object sender, System.Windows.RoutedEventArgs e)
        {
            List<Item> groups = ClientControl.Instance.GetViewGroupItems();
            if (groups.Count < 1)
                throw new MIPException("You do not have access to any groups?");

            // Use the first one (Private group)
            ConfigItem topGroupItem = (ConfigItem)groups[0];

            ConfigItem groupItem = (ConfigItem)FindName("ScInsertGroup", topGroupItem.GetChildren());
            if (groupItem == null)
            {
                // Make a group 
                groupItem = topGroupItem.AddChild("ScInsertGroup", Kind.View, FolderType.UserDefined);
            }

            // Figure out a name that does not exist
            List<Item> currentViews = groupItem.GetChildren();
            int ix = 0;
            while (FindName("ScInsertLayout-" + ix, currentViews) != null)
                ix++;

            // Build a layout with a wide ViewItem at top and bottom, and 5 small ones in the middle
            Rectangle[] rect = new Rectangle[7];
            rect[0] = new Rectangle(000, 000, 999, 399);
            rect[1] = new Rectangle(000, 399, 199, 199);
            rect[2] = new Rectangle(199, 399, 199, 199);
            rect[3] = new Rectangle(399, 399, 199, 199);
            rect[4] = new Rectangle(599, 399, 199, 199);
            rect[5] = new Rectangle(799, 399, 199, 199);
            rect[6] = new Rectangle(000, 599, 999, 399);
            ViewAndLayoutItem viewAndLayoutItem =
                (ViewAndLayoutItem)groupItem.AddChild("ScInsertLayout-" + ix, Kind.View, FolderType.No);
            viewAndLayoutItem.Layout = rect;
            viewAndLayoutItem.Properties["Created"] = DateTime.Now.ToLongDateString();
            viewAndLayoutItem.Icon = SCInsertCameraDefinition.PluginImage;
            
            // Insert an HTML ViewItem on top
            Dictionary<String, String> properties = new Dictionary<string, string>();
            properties.Add("URL", "www.google.com");		// Next 4 for a HTML item
            properties.Add("HideNavigationbar", "true");
            properties.Add("Scaling", "3");					// fit in 800x600
            viewAndLayoutItem.InsertBuiltinViewItem(0, ViewAndLayoutItem.HTMLBuiltinId, properties);

            // Find any camera and insert in index 1
            properties = new Dictionary<string, string>();
            properties.Add("CameraId", JustGetAnyCamera(Configuration.Instance.GetItems(ItemHierarchy.SystemDefined)));
            viewAndLayoutItem.InsertBuiltinViewItem(1, ViewAndLayoutItem.CameraBuiltinId, properties);

            // Insert a text ViewItem at index 2
            properties = new Dictionary<string, string>();
            properties.Add("Text", "Hello world");
            viewAndLayoutItem.InsertBuiltinViewItem(2, ViewAndLayoutItem.TextBuiltInId, properties);

            // Insert a carrousel ViewItem with two cameras at index 3
            properties = new Dictionary<string, string>();
            properties.Add("interval", "15");
            properties.Add("carousel-item", string.Format($"<carousel-items><carousel-item><device-id>{JustGetAnyCamera(Configuration.Instance.GetItems(ItemHierarchy.SystemDefined))}</device-id><show-time /></carousel-item><carousel-item><device-id>{JustGetAnyCamera(Configuration.Instance.GetItems(ItemHierarchy.UserDefined))}</device-id><show-time>8</show-time></carousel-item></carousel-items>"));
            viewAndLayoutItem.InsertBuiltinViewItem(3, ViewAndLayoutItem.CarrouselBuiltinId, properties);

            // Insert an image ViewItem at index 4
            properties = new Dictionary<string, string>();
            properties.Add("EmbedImage", "True");
            properties.Add("EmbeddedImage", _serializedMSLogo);
            viewAndLayoutItem.InsertBuiltinViewItem(4, ViewAndLayoutItem.ImageBuiltInId, properties);

            // Insert a hotspot ViewItem at bottom -- index 6
            properties = new Dictionary<string, string>();
            viewAndLayoutItem.InsertBuiltinViewItem(6, ViewAndLayoutItem.HotspotBuiltinId, properties);
            
            viewAndLayoutItem.Save();
            topGroupItem.PropertiesModified();
            List<Item> windows = Configuration.Instance.GetItemsByKind(Kind.Window);
            FQID mainWindowFQID = new FQID(new ServerId("SC", "", 0, Guid.Empty));
            if (windows.Count > 0)
                mainWindowFQID = windows[0].FQID;
            EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.MultiWindowCommand,
                                                                new MultiWindowCommandData()
                                                                {
                                                                    MultiWindowCommand = MultiWindowCommand.SetViewInWindow,
                                                                    View = viewAndLayoutItem.FQID,
                                                                    Window = mainWindowFQID
                                                                }));
        }

        private Item FindName(String name, List<Item> items)
        {
            foreach (Item item in items)
                if (item.Name == name)
                    return item;
            return null;
        }

        private void UpdateIndexes()
        {
            if (_sCInsertCameraSidePanelPlugin.CurrentView != null)
            {
                textBoxLayoutName.Text = _sCInsertCameraSidePanelPlugin.CurrentView.Name;
                comboBoxIndex.Items.Clear();

                for (int ix = 0; ix < _sCInsertCameraSidePanelPlugin.CurrentView.Layout.Length; ix++)
                {
                    if (!(bool)_temporaryInsertCheckBox.IsChecked || _sCInsertCameraSidePanelPlugin.CurrentView.ViewItemId(ix) == ViewAndLayoutItem.CameraBuiltinId)
                    {
                        comboBoxIndex.Items.Add("Index " + ix);
                    }
                }
                if (comboBoxIndex.Items.Count > 0)
                {
                    comboBoxIndex.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// This method will just find the first camera and return it's ObjectId as a string
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string JustGetAnyCamera(List<Item> items)
        {
            foreach (Item item in items)
            {
                if (item.FQID.Kind == Kind.Camera && item.FQID.FolderType == FolderType.No)
                {
                    return item.FQID.ObjectId.ToString();
                }
                if (item.FQID.Kind == Kind.Server || item.FQID.Kind == Kind.Camera || item.FQID.Kind == Kind.Folder)
                {
                    string child = JustGetAnyCamera(item.GetChildren());
                    if (child != "")
                        return child;
                }
            }
            return "";
        }

        /// <summary>
        /// Insert the selected camera in the selected index of the current view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInsert(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_selectedCamera != null && _sCInsertCameraSidePanelPlugin.CurrentView != null && comboBoxIndex.SelectedItem != null)
            {
                // Make sure we are inside array boundary
                string itemString = comboBoxIndex.SelectedItem.ToString();
                int ix = -1;
                int.TryParse(itemString.Substring(itemString.LastIndexOf(" ")), out ix);
                Guid streamId = Guid.Empty;
                DataType dataType = comboBoxStream.SelectedItem as DataType;
                if (dataType != null)
                    streamId = dataType.Id;

                if (ix >= 0 && ix < _sCInsertCameraSidePanelPlugin.CurrentView.Layout.Length)
                {
                    if ((bool)_temporaryInsertCheckBox.IsChecked)
                    {
                        //Change the camera temporarily in the camera view item. Just change the id.
                        EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetCameraInViewCommand, new SetCameraInViewCommandData()
                        {
                            Index = ix,
                            CameraFQID = _selectedCamera.FQID,
                            StreamId = streamId
                        }));
                    }
                    else
                    {
                        // Create, insert and save a new CameraViewItem at selected index. This insert is stored permanently on the view
                        Dictionary<string, string> properties = new Dictionary<string, string>();
                        properties.Add("CameraId", _selectedCamera.FQID.ObjectId.ToString());
                        properties.Add("StreamId", streamId.ToString());
                        _sCInsertCameraSidePanelPlugin.CurrentView.InsertBuiltinViewItem(ix, ViewAndLayoutItem.CameraBuiltinId, properties);
                        _sCInsertCameraSidePanelPlugin.CurrentView.Save();
                    }
                }
            }
        }

        private void OnClearClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (comboBoxIndex.SelectedItem == null)
                return;
            // Make sure we are inside array boundary
            string itemString = comboBoxIndex.SelectedItem.ToString();
            int ix = -1;
            int.TryParse(itemString.Substring(itemString.LastIndexOf(" ")), out ix);
            if (ix >= 0 && ix < _sCInsertCameraSidePanelPlugin.CurrentView.Layout.Length)
            {
                if ((bool)_temporaryInsertCheckBox.IsChecked)
                {
                    //Change the camera temporarily in the camera view item. Just change the id.
                    EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetCameraInViewCommand, new SetCameraInViewCommandData() { Index = ix, CameraFQID = null }));
                }
                else
                {
                    // Create, insert and save a new CameraViewItem at selected index. This insert is stored permanently on the view
                    Dictionary<string, string> properties = new Dictionary<string, string>();
                    _sCInsertCameraSidePanelPlugin.CurrentView.InsertBuiltinViewItem(ix, ViewAndLayoutItem.EmptyBuiltinId, properties);
                    _sCInsertCameraSidePanelPlugin.CurrentView.Save();
                }
            }
        }
    }
}
