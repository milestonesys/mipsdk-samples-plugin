using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace SCViewAndWindow.Client
{

    public partial class DumpWpfUserControl : UserControl
    {

        TreeViewItem selectedItem;
        List<Item> _items = new List<Item>();
        private object _themeChangedHandler;


        public DumpWpfUserControl()
        {
            InitializeComponent();
            _items.Clear();
            SetUpApplicationEventListeners();
            InitialTheme();
        }

        private void OnDump(object sender, RoutedEventArgs e)
        {
            FillNodes();
        }


        #region Theming

        public void InitialTheme()
        {
            var currentTheme = ClientControl.Instance.Theme.ThemeType.ToString();

            if (currentTheme == "Dark")
            {
                treeViewItems.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor);
                treeViewDetails.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor);
            }
            else if (currentTheme == "Light")
            {
                treeViewItems.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor);
                treeViewDetails.Background = ToMediaBrush(ClientControl.Instance.Theme.BackgroundColor);
            }
        }

        public static Brush ToMediaBrush(System.Drawing.Color color)
        {
            var mediaColor = Color.FromArgb(color.A, color.R, color.G, color.B);
            Brush mediaBrush = new SolidColorBrush(mediaColor);
            return mediaBrush;
        }

        private object ThemeChangedIndicationHandler(Message message, FQID destination, FQID source)
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

        /// <summary>
        /// Fill treeViewItems and treeViewSites nodes.
        /// </summary>
        public void FillNodes()
        {
            _items.Clear();
            var all = Configuration.Instance.GetItems(ItemHierarchy.Both);
            MipconfigItem root = new MipconfigItem();
            foreach (Item item in all)
            {
                if (item.FQID.Kind == Kind.View)
                    _items.Add(item);
            }
                foreach (Item item in _items)
            {
                ImageSource imageSource = MipconfigItem.GetImage(item);
                MipconfigItem mipconfig = new MipconfigItem() { MipItem = item };
                MipconfigItem child = new MipconfigItem();
                mipconfig.Items.Add(child);
                root.Items.Add(mipconfig);
            }
            treeViewItems.ItemsSource = root.Items;
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
