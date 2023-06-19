using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Platform;

namespace ServerSideCarrousel.Admin
{
    public partial class CarrouselUserControl : UserControl
    {

        #region private fields

        private ImageList _imageList;
        private int ServerIconIx = 0;
        private int FolderIconIx = 1;
        private int PresetIconIx = 2;
        private int CameraIconIx = 3;
        private Hashtable _itemTypeToIx = new Hashtable();
        private Hashtable _selectedItems = new Hashtable();

        private int _defaultSeconds = 10;
        private int _maxSortIx = 1;

        #endregion

        #region internal fields

        internal event EventHandler ConfigurationChangedByUser;

        #endregion


        #region Initialization

        public CarrouselUserControl()
        {
            InitializeComponent();

            _imageList = new ImageList();
            this.treeViewAvailable.ImageList = _imageList;
            this.treeViewSelected.ImageList = _imageList;

            _imageList.ImageSize = new Size(16, 16);
            _imageList.Images.Add(VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.ServerIconIx]);
            _imageList.Images.Add(VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.FolderIconIx]);
            _imageList.Images.Add(VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PresetIconIx]);
            _imageList.Images.Add(VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.CameraIconIx]);
            _itemTypeToIx[Kind.Server] = ServerIconIx;
            _itemTypeToIx[Kind.Folder] = FolderIconIx;
            _itemTypeToIx[Kind.Preset] = PresetIconIx;
            _itemTypeToIx[Kind.Camera] = CameraIconIx;

            textBoxSeconds.Text = "";
        }

        internal async void FillContent()
        {
            treeViewAvailable.Nodes.Clear();

            List<Item> items = Configuration.Instance.GetItemsByKind(Kind.Camera);
            foreach (Item item in items)
            {
                if (item.FQID.Kind == Kind.Camera ||
                    item.FQID.Kind == Kind.Server) 
                {
                    TreeNode tn = new TreeNode(item.Name);
                    tn.Tag = item;
                    if (_itemTypeToIx[item.FQID.Kind] != null)
                        tn.ImageIndex = tn.SelectedImageIndex = (int)_itemTypeToIx[item.FQID.Kind];
                    tn.Nodes.Add("...building");
                    treeViewAvailable.Nodes.Add(tn);
                    treeViewAvailable.Sort();
                }
            }
            textBoxSeconds.Text = "";
        }

        internal void ClearUserControl()
        {
            treeViewAvailable.Nodes.Clear();
            treeViewSelected.Nodes.Clear();
            textBoxName.Text = "";
            textBoxDefaultSeconds.Text = "10";
            textBoxSeconds.Text = "";
            _selectedItems.Clear();
            buttonAdd.Enabled = false;
            buttonRemove.Enabled = false;
            _maxSortIx = 1;
        }
        #endregion


        #region Properties

        internal String CarrouselName
        {
            get { return textBoxName.Text; }
            set { textBoxName.Text = value; }
        }

        internal string DefaultSeconds
        {
            get { return ""+_defaultSeconds; }
            set {
                Int32.TryParse(value, out _defaultSeconds);
                textBoxDefaultSeconds.Text = ""+_defaultSeconds;
            }
        }

        internal List<CarrouselTreeNode> ItemsSelected
        {
            set
            {
                _selectedItems = new Hashtable();

                treeViewSelected.Nodes.Clear();
                if (value != null)
                {
                    foreach (CarrouselTreeNode tn in value)
                    {
                        tn.Tag = tn.Item;
                        if (_itemTypeToIx[tn.Item.FQID.Kind] != null)
                            tn.ImageIndex = tn.SelectedImageIndex = (int)_itemTypeToIx[tn.Item.FQID.Kind];
                        treeViewSelected.Nodes.Add(tn);

                        _selectedItems.Add(tn.Item.FQID, tn);
                        if (tn.Sortix >= _maxSortIx)
                            _maxSortIx = tn.Sortix+1;
                    }
                }
            }

            get
            {
                List<CarrouselTreeNode> items = new List<CarrouselTreeNode>();
                foreach (CarrouselTreeNode item in _selectedItems.Values)
                    items.Add(item);
                return items;
            }
        }
        #endregion


        #region Event handling

        private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag is Item)
            {
                Item node = e.Node.Tag as Item;

                buttonRemove.Enabled = false;
                buttonAdd.Enabled = false;

                if (e.Node.Nodes.Count == 0 || e.Node.Nodes[0].Text == "...building")
                    node.GetChildrenAsync(OnBeforeExpandHandler, this, e.Node);
            }
        }

        private void OnBeforeExpandHandler(List<Item> children, object callerReference)
        {
            TreeNode node = (TreeNode)callerReference;
            node.Nodes.Clear();
            if (children != null && children.Count > 0)
            {
                foreach (Item child in children)
                {
                    if (child.FQID.Kind == Kind.Camera || 
                        child.FQID.Kind == Kind.Server)
                    {
                        TreeNode tn = new TreeNode(child.Name);
                        tn.Tag = child;
                        if (_itemTypeToIx[child.FQID.Kind] != null)
                            tn.ImageIndex = tn.SelectedImageIndex = (int) _itemTypeToIx[child.FQID.Kind];
                        if (child.FQID.FolderType != FolderType.No)
                        {
                            tn.Nodes.Add("...building");
                        }
                        else if (child.FQID.Kind == Kind.Camera)
                        {
                            List<Item> presets = child.GetChildren();
                            if (presets != null)
                            {
                                foreach (Item preset in presets)
                                {
                                    TreeNode tnPreset = new TreeNode(preset.Name);
                                    tnPreset.Tag = preset;
                                    if (_itemTypeToIx[preset.FQID.Kind] != null)
                                        tnPreset.ImageIndex = tnPreset.SelectedImageIndex = (int) _itemTypeToIx[preset.FQID.Kind];
                                    tn.Nodes.Add(tnPreset);
                                }
                            }
                        }
                        node.Nodes.Add(tn);
                    }
                }

            }
        }

        private void OnNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag is Item)
            {
                Item node = e.Node.Tag as Item;

                buttonRemove.Enabled = false;
                buttonAdd.Enabled = (_selectedItems[node.FQID] == null) &&
                                    ((node.FQID.ObjectId != Guid.Empty) || (node.FQID.ObjectIdString != null)) &&
                                    (node.FQID.Kind == Kind.Camera || node.FQID.Kind == Kind.Preset) &&
                                    node.FQID.FolderType == FolderType.No;
            }
        }

        private void OnAdd(object sender, EventArgs e)
        {
            if (treeViewAvailable.SelectedNode != null && treeViewAvailable.SelectedNode.Tag is Item)
            {
                Item toSelect = (Item)treeViewAvailable.SelectedNode.Tag;
                TreeNode tn = new CarrouselTreeNode(toSelect, _defaultSeconds, _maxSortIx++);
                _selectedItems[toSelect.FQID] = tn;
                tn.Tag = toSelect;
                tn.ImageIndex = tn.SelectedImageIndex = treeViewAvailable.SelectedNode.ImageIndex;
                treeViewSelected.Nodes.Add(tn);
            }
            buttonAdd.Enabled = false;
            OnUserChange(null,null);
        }

        private void OnRemove(object sender, EventArgs e)
        {
            if (treeViewSelected.SelectedNode != null)
            {
                CarrouselTreeNode selectedItem = (CarrouselTreeNode)treeViewSelected.SelectedNode;
                foreach (CarrouselTreeNode carrouselItem in _selectedItems.Values)
                    if (carrouselItem.Sortix > selectedItem.Sortix)
                        carrouselItem.Sortix--;

                _maxSortIx--;
                Item item = (Item)treeViewSelected.SelectedNode.Tag;
                if (item != null && _selectedItems[item.FQID] != null)
                    _selectedItems.Remove(item.FQID);
                treeViewSelected.Nodes.Remove(treeViewSelected.SelectedNode);
                //if (treeViewSelected.Nodes.Count == 0)
                buttonRemove.Enabled = false;
                treeViewSelected.Sort();
            }
            OnUserChange(null, null);
        }

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            buttonAdd.Enabled = false;
            buttonRemove.Enabled = true;
            buttonUp.Enabled = true;
            buttonDown.Enabled = true;
            textBoxSeconds.Enabled = true;
            if (treeViewSelected.SelectedNode!=null)
                textBoxSeconds.Text = "" + ((CarrouselTreeNode)treeViewSelected.SelectedNode).Seconds;
        }

        private void OnUserChange(object sender, EventArgs e)
        {
            if (ConfigurationChangedByUser != null)
                ConfigurationChangedByUser(this, new EventArgs());
        }

        private void OnUp(object sender, EventArgs e)
        {
            if (treeViewSelected.SelectedNode!=null)
            {
                CarrouselTreeNode item = (CarrouselTreeNode)treeViewSelected.SelectedNode;
                if (item.Sortix>1)
                {
                    int prevSortIx = item.Sortix - 1;
                    foreach (CarrouselTreeNode checkItem in _selectedItems.Values)
                        if (checkItem.Sortix==prevSortIx)
                        {
                            checkItem.Sortix++;
                            item.Sortix--;
                            break;
                        }
                    treeViewSelected.Sort();
                    treeViewSelected.SelectedNode = item;
                    treeViewSelected.Focus();
                    OnUserChange(sender, e);
                }
            }
        }

        private void OnDown(object sender, EventArgs e)
        {
            if (treeViewSelected.SelectedNode != null)
            {
                CarrouselTreeNode item = (CarrouselTreeNode)treeViewSelected.SelectedNode;
                int nextSortIx = item.Sortix + 1;
                foreach (CarrouselTreeNode checkItem in _selectedItems.Values)
                    if (checkItem.Sortix == nextSortIx)
                    {
                        checkItem.Sortix--;
                        item.Sortix++;
                        break;
                    }
                treeViewSelected.Sort();
                treeViewSelected.SelectedNode = item;
                treeViewSelected.Focus();
                OnUserChange(sender, e);
            }
        }

        private void OnSecondsChanged(object sender, EventArgs e)
        {
            if (treeViewSelected.SelectedNode != null)
            {
                CarrouselTreeNode item = (CarrouselTreeNode) treeViewSelected.SelectedNode;
                int newSeconds = 10;
                if (Int32.TryParse(textBoxSeconds.Text, out newSeconds))
                {
                    item.Seconds = newSeconds;
                    treeViewSelected.SelectedNode = item;
                    OnUserChange(sender, e);
                }
            }
        }

        private void OnDefaultSecondsChanged(object sender, EventArgs e)
        {
            int newDefault;
            if (Int32.TryParse(textBoxDefaultSeconds.Text, out newDefault))
                _defaultSeconds = newDefault;
            OnUserChange(sender, e);
        }

        #endregion
    }

    #region internal classes

    internal class CarrouselTreeNode : TreeNode
    {
        private int _sortix = 1;
        private int _seconds = 10;

        internal Item Item { get; private set; }
        internal int Seconds
        {
            get { return _seconds; }  
            set
            {
                _seconds = value;
                base.Text = ToString(); 
            }
        }
        internal int Sortix
        {
            get { return _sortix; }
            set {
                _sortix = value;  
                base.Text = ToString(); 
            }
        }

        internal CarrouselTreeNode(Item item, int seconds, int sortix)
        {
            Item = item;
            Seconds = seconds;
            Sortix = sortix;
            base.Text = ToString();
        }

        public override string ToString()
        {
            return String.Format("{0:000} : {1:00} seconds - {2}", Sortix, Seconds, Item.Name);
        }
    }
    #endregion
}
