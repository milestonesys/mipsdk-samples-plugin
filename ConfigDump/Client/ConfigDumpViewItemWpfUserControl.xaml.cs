using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace ConfigDump.Client
{
    public partial class ConfigDumpViewItemWpfUserControl : ViewItemWpfUserControl
    {
        TreeViewItem selectedItem;
        List<Item> _items = new List<Item>();
        private object _themeChangedHandler;

        private ConfigDumpViewItemManager _viewItemManager;

        public ConfigDumpViewItemWpfUserControl(ConfigDumpViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();

            ItemPickerUserControl itemPickerUserControl = new ItemPickerUserControl();
            itemPickerUserControl.Init();

            _items.Clear();
            _items = Configuration.Instance.GetItems(ItemHierarchy.Both);
            SetUpApplicationEventListeners();
            FillNodes();
            InitialTheme();
        }

        #region Theming

        public void InitialTheme()
        {
            var currentTheme = ClientControl.Instance.Theme.ThemeType.ToString();

            if (currentTheme == "Dark")
            {
                treeViewItems.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor);
                treeViewDetails.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor);
                treeViewSites.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor); ;
            }
            else if (currentTheme == "Light")
            {
                treeViewItems.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor); ;
                treeViewDetails.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor); ;
                treeViewSites.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor); ;
            }
        }

        public static Brush ToMediaBrush(System.Drawing.Color color)
        {

            var mediaColor = Color.FromArgb(color.A, color.R, color.G, color.B);
            Brush mediaBrush = new SolidColorBrush(mediaColor);
            return mediaBrush;
        }

        private object ThemeChangedIndicationHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            InitialTheme();
            return null;
        }


        #endregion


        private void SetUpApplicationEventListeners()
        {
            _themeChangedHandler = EnvironmentManager.Instance.RegisterReceiver(
                                                         new MessageReceiver(ThemeChangedIndicationHandler),
                                                         new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));
        }

        private void RemoveApplicationEventListeners()
        {
            if (_themeChangedHandler != null)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(_themeChangedHandler);
                _themeChangedHandler = null;
            }
        }

        public override void Close()
        {
            RemoveApplicationEventListeners();
        }


        #region Component properties


        public override bool Maximizable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Tell if Item is selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        /// <summary>
        /// Sets the maximized status status of the content holder.
        /// </summary>
        public override bool Maximized
        {
            set
            {
                if (base.Maximized != value)
                {
                    base.Maximized = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the hidden status on the content holder (e.g. the view item is set
        /// to be hidden when another view item is maximized). 
        /// </summary>
        public override bool Hidden
        {
            set
            {
                if (base.Hidden != value)
                {
                    base.Hidden = value;
                }
            }
        }

        #endregion


        /// <summary>
        /// Fill treeViewItems and treeViewSites nodes.
        /// </summary>
        public void FillNodes()
        {
            _items.Clear();
            _items = Configuration.Instance.GetItems(ItemHierarchy.Both);
            MipconfigItem root = new MipconfigItem();

            foreach (Item item in _items)
            {
                ImageSource imageSource = MipconfigItem.GetImage(item);
                MipconfigItem mipconfig = new MipconfigItem() { MipItem = item };
                MipconfigItem child = new MipconfigItem();
                mipconfig.Items.Add(child);
                root.Items.Add(mipconfig);
            }
            treeViewItems.ItemsSource = root.Items;

            treeViewSites.Items.Clear();
            Item site = EnvironmentManager.Instance.GetSiteItem(EnvironmentManager.Instance.MasterSite);
            TreeViewItem tn = new TreeViewItem() { Header = site.Name };
            tn.Tag = site;
            treeViewSites.Items.Add(tn);
            FillSitesTree(site, tn);
        }

        private void FillSitesTree(Item site, TreeViewItem treeViewItem)
        {
            var children = site.GetChildren();
            foreach (var child in children) 
            {
                TreeViewItem tn = new TreeViewItem() { Header = child.Name };
                tn.Tag = child;
                treeViewItem.Items.Add(tn);
                FillSitesTree(child, tn);
            }
        }


        /// <summary>
        /// Get children of the selected TreeViewItem and display them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItemsItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvitem = e.OriginalSource as TreeViewItem;
            if (tvitem == null)
            {
                return;
            }
            var item = tvitem.Header as MipconfigItem;
            if (item != null && item.Items.Any() && item.Items.First().Title == "")
            {
                item.Items.Clear();
                List<Item> subItems = item.MipItem.GetChildren();
                foreach (Item subItem in subItems)
                {
                    MipconfigItem mipconfig = new MipconfigItem() { MipItem = subItem };
                    if (subItem.HasChildren != HasChildren.No)
                    {
                        mipconfig.Items.Add(new MipconfigItem());
                    }
                    item.Items.Add(mipconfig);
                }
            }
        }

        /// <summary>
        /// Display details of selected item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItemsItem_Selected(object sender, RoutedEventArgs e)
        {
            selectedItem = e.OriginalSource as TreeViewItem;
            var item = selectedItem.Header as MipconfigItem;
            treeViewDetails.Items.Clear();
            TreeViewItem tn = new TreeViewItem() { Header = item.MipItem.Name };
            DumpFQID(item, tn);
            DumpFields(item, tn);
            DumpProperties(item, tn);
            DumpAuthorization(item, tn);
            DumpRelated(item, tn);
            DumpDataVersion(item, tn);
            treeViewDetails.Items.Add(tn);
            tn.IsExpanded = true;
            foreach (TreeViewItem child in tn.Items)
            {
                child.IsExpanded = true;
            }
        }

        #region The Detail Dump Methods

        private void DumpDataVersion(MipconfigItem item, TreeViewItem tn)
        {
            long dataVersion = Configuration.Instance.GetItemConfigurationDataVersion(item.MipItem.FQID.Kind);
            TreeViewItem fqidNode = new TreeViewItem() { Header = "DataVersion = "+dataVersion };
            tn.Items.Add(fqidNode);
        }

        private void DumpFQID(MipconfigItem item, TreeViewItem tn)
        {
            TreeViewItem fqidNode = new TreeViewItem() { Header = "FQID" };
            tn.Items.Add(fqidNode);
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.Kind: " + item.MipItem.FQID.Kind + "(" + Kind.DefaultTypeToNameTable[item.MipItem.FQID.Kind] + ")" });
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.ServerId.ServerType: " + item.MipItem.FQID.ServerId.ServerType });
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.ServerId.Id: " + item.MipItem.FQID.ServerId.Id });
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.ServerId.ServerHostname: " + item.MipItem.FQID.ServerId.ServerHostname });
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.ServerId.Serverport: " + item.MipItem.FQID.ServerId.Serverport });
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.ServerId.ServerScheme: " + item.MipItem.FQID.ServerId.ServerScheme });
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.ParentId: " + item.MipItem.FQID.ParentId });
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.ObjectId: " + item.MipItem.FQID.ObjectId });
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.ObjectIdString: " + item.MipItem.FQID.ObjectIdString });
            fqidNode.Items.Add(new TreeViewItem() { Header = "FQID.FolderType: " + item.MipItem.FQID.FolderType });
        }

        private void DumpProperties(MipconfigItem item, TreeViewItem tn)
        {
            TreeViewItem propNode = new TreeViewItem() { Header = "Properties" };
            tn.Items.Add(propNode);
            if (item.MipItem.Properties != null)
            {
                foreach (string key in item.MipItem.Properties.Keys)
                {
                    if (key.ToLower().Contains("password"))
                        propNode.Items.Add(key + " = " + "**********");
                    else
                        propNode.Items.Add(key + " = " + item.MipItem.Properties[key]);
                }
            }
        }

        private void DumpAuthorization(MipconfigItem item, TreeViewItem tn)
        {
            TreeViewItem propNode = new TreeViewItem() { Header = "Authorization" };
            tn.Items.Add(propNode);
            if (item.MipItem.Authorization != null)
            {
                foreach (string key in item.MipItem.Authorization.Keys)
                {
                    propNode.Items.Add(new TreeViewItem() { Header = key + " = " + item.MipItem.Authorization[key] });
                }
            }
        }

        private void DumpRelated(MipconfigItem item, TreeViewItem tn)
        {
            TreeViewItem relatedNode = new TreeViewItem() { Header = "Related" };
            tn.Items.Add(relatedNode);
            List<Item> related = item.MipItem.GetRelated();
            if (related != null)
            {
                foreach (Item rel in related)
                {
                    TreeViewItem relNode = new TreeViewItem() { Header = rel.Name };
                    relatedNode.Items.Add(relNode);
                }
            }
        }

        private void DumpFields(MipconfigItem item, TreeViewItem tn)
        {
            TreeViewItem fields = new TreeViewItem() { Header = "Fields" };
            tn.Items.Add(fields);
            fields.Items.Add("HasRelated : " + item.MipItem.HasRelated);
            fields.Items.Add("HasChildren: " + item.MipItem.HasChildren);
            if (item.MipItem.PositioningInformation == null)
            {
                fields.Items.Add("No PositioningInformation");
            }
            else
            {
                fields.Items.Add("PositioningInformation: Latitude=" + item.MipItem.PositioningInformation.Latitude + ", longitude=" + item.MipItem.PositioningInformation.Longitude);
                fields.Items.Add("PositioningInformation: CoverageDirection=" + item.MipItem.PositioningInformation.CoverageDirection);
                fields.Items.Add("PositioningInformation: CoverageDepth=" + item.MipItem.PositioningInformation.CoverageDepth);
                fields.Items.Add("PositioningInformation: CoverageFieldOfView=" + item.MipItem.PositioningInformation.CoverageFieldOfView);
            }
        }
        #endregion


        internal static Guid CameraPropertiesAnalyticsPluginId = new Guid("e919b772-74ee-43a2-8a56-fe0c08abcc84");
        internal static Guid CameraPropertiesAnalyticsKind = new Guid("dd1152b9-3eec-4dde-ad39-621cbcb96507");

        private void OnShowMultiSite(object sender, RoutedEventArgs e)
        {

            #region Show parent

            listBoxProperties.Items.Clear();
            Item MasterSite = EnvironmentManager.Instance.GetSiteItem(EnvironmentManager.Instance.MasterSite);
            listBoxProperties.Items.Add("ServerName " + MasterSite.Name + " ServerId.Id " + MasterSite.FQID.ServerId.Id);

            List<Item> cameras = Configuration.Instance.GetItemConfigurations(MasterSite.FQID.ServerId, CameraPropertiesAnalyticsPluginId, null,
                                                                    CameraPropertiesAnalyticsKind);
            foreach (Item citem in cameras)
            {
                string tx = citem.Name + ", Address=" + citem.Properties["Address"] + " (" + citem.Properties["UserName"] + " )";
                listBoxProperties.Items.Add(tx);
            }

            #endregion

            #region Show children

            List<Item> sites = MasterSite.GetChildren();
            foreach (var item in sites)
            {
                listBoxProperties.Items.Add("+ServerName " + item.Name + " ServerId.Id " + item.FQID.ServerId.Id);


                cameras = Configuration.Instance.GetItemConfigurations(item.FQID.ServerId, CameraPropertiesAnalyticsPluginId, null,
                                                                                    CameraPropertiesAnalyticsKind);
                foreach (Item citem in cameras)
                {
                    string tx = "+ " + citem.Name + ", Address=" + citem.Properties["Address"] + " (" + citem.Properties["UserName"] + " )";
                    listBoxProperties.Items.Add(tx);
                }
            }

            #endregion

        }

        /// <summary>
        /// Show site properties in listBoxProperties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowProperties(object sender, RoutedEventArgs e)
        {
            listBoxProperties.Items.Clear();
            var tvitem = e.OriginalSource as TreeViewItem;

            if (tvitem != null)
            {
                Item site = EnvironmentManager.Instance.GetSiteItem((tvitem.Tag as Item).FQID.ServerId.Id);
                if (site != null)
                {
                    foreach (String p in site.Properties.Keys)
                    {
                        listBoxProperties.Items.Add(p + " = " + site.Properties[p]);
                    }
                }
            }
            tvitem.IsSelected = false;
        }

        private void OnItemPickerClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ItemPickerForm form = new ItemPickerForm();

            if (selectedItem!=null)
            {
                var item = selectedItem.Header as MipconfigItem;
                form.KindFilter = item.MipItem.FQID.Kind;
            }
            form.Init();
            form.ShowDialog();
        }

        private void OnMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }
    }

    public class MipconfigItem : INotifyPropertyChanged
    {
        public MipconfigItem()
        {
            this.Items = new ObservableCollection<MipconfigItem>();
        }

        public string Title
        {
            get
            {
                if (_mipItem == null) { return ""; };
                return this.MipItem.Name;
            }
        }

        private Item _mipItem;
        public Item MipItem
        {
            get
            {
                return _mipItem;
            }
            set
            {
                _mipItem = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ImageSource"));
                }
            }
        }

        //Get icons for each node in TreeViewItems
        public static ImageSource GetImage(Item item)
        {
            int ix = Util.BuiltInItemToImageIndex(item);
            System.Drawing.Image image = VideoOS.Platform.UI.Util.ImageList.Images[ix];
            ImageSource imageSource = ConvertImage(image);
            return imageSource;
        }

        private static System.Windows.Media.ImageSource ConvertImage(System.Drawing.Image image)
        {
            try
            {
                if (image != null)
                {
                    var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                    bitmap.BeginInit();
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                    bitmap.StreamSource = memoryStream;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            catch { }
            return null;
        }

        public ImageSource ImageSource
        {
            get
            {
                if (_mipItem == null) { return null; };
                return GetImage(_mipItem);
            }
        }



        public ObservableCollection<MipconfigItem> Items { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
