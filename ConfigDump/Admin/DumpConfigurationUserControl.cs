using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using VideoOS.Platform;
using VideoOS.Platform.UI;

namespace ConfigDump.Admin
{
    public class DumpConfigurationUserControl : UserControl
    {
        private TreeView treeView1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TreeView treeViewDetail;
        private TreeView treeViewItems;
        private Button button1;
        private ListBox listBox1;
        private Button buttonMultiSite;
        private Button button2;
        private Item _selectedItem=null;

        public DumpConfigurationUserControl()
        {
            InitializeComponent();
        }

        public void FillContent()
        {
            try
            {
                treeViewItems.Nodes.Clear();
                treeViewItems.ImageList = VideoOS.Platform.UI.Util.ImageList;
                if (this.IsHandleCreated)
                    Configuration.Instance.GetItemsAsync(AsyncItemsHandler, this, null);
                else
                {
                    FillNodes(Configuration.Instance.GetItems(), treeViewItems.Nodes);
                }

                Item hardwareFolder = Configuration.Instance.GetItem(Kind.Hardware, Kind.Hardware);
                if (hardwareFolder != null)
                { 
                    TreeNode tn2 = new TreeNode(hardwareFolder.Name);
                    tn2.ImageIndex = tn2.SelectedImageIndex = VideoOS.Platform.UI.Util.BuiltInItemToImageIndex(hardwareFolder);
                    tn2.Tag = hardwareFolder;
                    if (hardwareFolder.HasChildren != VideoOS.Platform.HasChildren.No)
                    {
                        tn2.Nodes.Add("...");
                    }
                    treeViewItems.Nodes.Add(tn2);
                }

                treeView1.Nodes.Clear();
                Item site = EnvironmentManager.Instance.GetSiteItem(EnvironmentManager.Instance.MasterSite);
                TreeNode tn = new TreeNode(site.Name);
                tn.Tag = site;
                treeView1.Nodes.Add(tn);
                FillRecursive(tn);
            } catch (Exception e)
            {
                EnvironmentManager.Instance.ExceptionDialog("FillContent", e);
            }
        }

        private void AsyncItemsHandler(List<Item> list, object callerref)
        {
            if (list != null)
                FillNodes(list, treeViewItems.Nodes);

        }

        private void FillNodes(List<Item> list, TreeNodeCollection treeNodeCollection)
        {
            foreach (Item item in list)
            {
                TreeNode tn = new TreeNode(item.Name);
                tn.ImageIndex = tn.SelectedImageIndex = VideoOS.Platform.UI.Util.BuiltInItemToImageIndex(item);
                tn.Tag = item;
                if (item.HasChildren != VideoOS.Platform.HasChildren.No)
                {
                    tn.Nodes.Add("...");
                }
                treeNodeCollection.Add(tn);
            }
        }


        private void FillRecursive(TreeNode tn)
        {
            Item siteItem = (Item)tn.Tag;
            List<Item> childs = siteItem.GetChildren();
            if (childs!=null && childs.Count>0)
            {
                foreach (Item site in childs)
                {
                    TreeNode child = new TreeNode(site.Name);
                    child.Tag = site;
                    tn.Nodes.Add(child);
                    FillRecursive(child);					
                }
            }
        }

        #region Designer generated code

        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.treeViewDetail = new System.Windows.Forms.TreeView();
            this.treeViewItems = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonMultiSite = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(7, 385);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(301, 97);
            this.treeView1.TabIndex = 10;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnNodeClick);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(314, 385);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(369, 95);
            this.listBox1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(351, 368);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Properties for selected Site:";
            this.label1.Click += new System.EventHandler(this.OnClick);
            this.label1.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 368);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Available Sites:";
            this.label2.Click += new System.EventHandler(this.OnClick);
            this.label2.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(346, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Details for the selected Item:";
            this.label3.Click += new System.EventHandler(this.OnClick);
            this.label3.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "List of all defined items:";
            this.label4.Click += new System.EventHandler(this.OnClick);
            this.label4.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // treeViewDetail
            // 
            this.treeViewDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewDetail.Location = new System.Drawing.Point(314, 45);
            this.treeViewDetail.Name = "treeViewDetail";
            this.treeViewDetail.Size = new System.Drawing.Size(369, 272);
            this.treeViewDetail.TabIndex = 15;
            // 
            // treeViewItems
            // 
            this.treeViewItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewItems.Location = new System.Drawing.Point(10, 45);
            this.treeViewItems.Name = "treeViewItems";
            this.treeViewItems.Size = new System.Drawing.Size(298, 272);
            this.treeViewItems.TabIndex = 14;
            this.treeViewItems.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnBeforeExpand);
            this.treeViewItems.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelect);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(325, 331);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(340, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "ItemPicker";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonMultiSite
            // 
            this.buttonMultiSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonMultiSite.Location = new System.Drawing.Point(13, 331);
            this.buttonMultiSite.Name = "buttonMultiSite";
            this.buttonMultiSite.Size = new System.Drawing.Size(129, 23);
            this.buttonMultiSite.TabIndex = 19;
            this.buttonMultiSite.Text = "Multi-Site Properties";
            this.buttonMultiSite.UseVisualStyleBackColor = true;
            this.buttonMultiSite.Click += new System.EventHandler(this.OnShowMultiSite);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(162, 331);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(129, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "Hardware...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DumpConfigurationUserControl
            // 
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonMultiSite);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.treeViewDetail);
            this.Controls.Add(this.treeViewItems);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.treeView1);
            this.Name = "DumpConfigurationUserControl";
            this.Size = new System.Drawing.Size(691, 495);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        #region user click event handling

        private void OnNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            listBox1.Items.Clear();
            Item item = e.Node.Tag as Item;
            if (item != null)
            {
                Item item2 = EnvironmentManager.Instance.GetSiteItem(item.FQID);
                if (item2 != null)
                {
                    foreach (String p in item2.Properties.Keys)
                    {
                        listBox1.Items.Add(p + " = " + item2.Properties[p]);
                    }

                }
            }
        }

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            Item item = e.Node.Tag as Item;
            _selectedItem = item;
            if (item != null)
            {
                treeViewDetail.Nodes.Clear();
                TreeNode tn = new TreeNode(item.Name);
                DumpFQID(item, tn);
                DumpFields(item, tn);
                DumpProperties(item, tn);
                DumpAuthorization(item, tn);
                DumpRelated(item, tn);
                DumpDataVersion(item, tn);
                treeViewDetail.Nodes.Add(tn);
                tn.ExpandAll();
            }
        }

        private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // If this is the first time we expand, get the children
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == null)
            {
                e.Node.Nodes.Clear();					// Skip the dummy entry
                Item item = e.Node.Tag as Item;
                if (item != null)
                {
                    try
                    {
                        FillNodes(item.GetChildren(), e.Node.Nodes);						
                    } catch (Exception ex)
                    {
                        EnvironmentManager.Instance.ExceptionDialog("OnBeforeExpand", ex);
                    }
                }
            }
        }

        #endregion

        #region the detail dump methods

        private void DumpDataVersion(Item item, TreeNode tn)
        {
            long dataVersion = Configuration.Instance.GetItemConfigurationDataVersion(item.FQID.Kind);
            TreeNode fqidNode = new TreeNode("DataVersion = "+dataVersion, 1, 1);
            tn.Nodes.Add(fqidNode);
        }

        private void DumpFQID(Item item, TreeNode tn)
        {
            TreeNode fqidNode = new TreeNode("FQID", 1, 1);
            tn.Nodes.Add(fqidNode);
            fqidNode.Nodes.Add(new TreeNode("FQID.Kind: " + item.FQID.Kind + " (" + Kind.DefaultTypeToNameTable[item.FQID.Kind] + ")", 1, 1));
            fqidNode.Nodes.Add(new TreeNode("FQID.ServerId.ServerType: " + item.FQID.ServerId.ServerType, 1, 1));
            fqidNode.Nodes.Add(new TreeNode("FQID.ServerId.Id: " + item.FQID.ServerId.Id, 1, 1));
            fqidNode.Nodes.Add(new TreeNode("FQID.ServerId.ServerHostname: " + item.FQID.ServerId.ServerHostname, 1, 1));
            fqidNode.Nodes.Add(new TreeNode("FQID.ServerId.Serverport: " + item.FQID.ServerId.Serverport, 1, 1));
            fqidNode.Nodes.Add(new TreeNode("FQID.ServerId.ServerScheme: " + item.FQID.ServerId.ServerScheme, 1, 1));
            fqidNode.Nodes.Add(new TreeNode("FQID.ParentId: " + item.FQID.ParentId, 1, 1));
            fqidNode.Nodes.Add(new TreeNode("FQID.ObjectId: " + item.FQID.ObjectId, 1, 1));
            fqidNode.Nodes.Add(new TreeNode("FQID.ObjectIdString: " + item.FQID.ObjectIdString, 1, 1));
            fqidNode.Nodes.Add(new TreeNode("FQID.FolderType: " + item.FQID.FolderType, 1, 1));
        }

        private void DumpProperties(Item item, TreeNode tn)
        {
            TreeNode propNode = new TreeNode("Properties", 1, 1);
            tn.Nodes.Add(propNode);
            if (item.Properties != null)
            {
                foreach (string key in item.Properties.Keys)
                {
                    if (key.ToLower().Contains("password"))
                        propNode.Nodes.Add(key + " = " + "**********");
                    else
                        propNode.Nodes.Add(key + " = " + item.Properties[key]);
                }
            }
        }

        private void DumpAuthorization(Item item, TreeNode tn)
        {
            TreeNode propNode = new TreeNode("Authorization");
            tn.Nodes.Add(propNode);
            if (item.Authorization != null)
            {
                foreach (string key in item.Authorization.Keys)
                {
                    propNode.Nodes.Add(new TreeNode(key + " = " + item.Authorization[key]));
                }
            }
        }

        private void DumpRelated(Item item, TreeNode tn)
        {
            TreeNode relatedNode = new TreeNode("Related");
            tn.Nodes.Add(relatedNode);
            List<Item> related = item.GetRelated();
            if (related != null)
            {
                foreach (Item rel in related)
                {
                    TreeNode relNode = new TreeNode(rel.Name);
                    relNode.ImageIndex = relNode.SelectedImageIndex = VideoOS.Platform.UI.Util.KindToImageIndex[rel.FQID.Kind];
                    relatedNode.Nodes.Add(relNode);
                }
            }
        }

        private void DumpFields(Item item, TreeNode tn)
        {
            TreeNode fields = new TreeNode("Fields");
            tn.Nodes.Add(fields);
            fields.Nodes.Add("HasRelated : " + item.HasRelated);
            fields.Nodes.Add("HasChildren: " + item.HasChildren);
            if (item.PositioningInformation == null)
            {
                fields.Nodes.Add("No PositioningInformation");
            }
            else
            {
                fields.Nodes.Add("PositioningInformation: Latitude=" + item.PositioningInformation.Latitude + ", longitude=" + item.PositioningInformation.Longitude);
                fields.Nodes.Add("PositioningInformation: CoverageDirection=" + item.PositioningInformation.CoverageDirection);
                fields.Nodes.Add("PositioningInformation: CoverageDepth=" + item.PositioningInformation.CoverageDepth);
                fields.Nodes.Add("PositioningInformation: CoverageFieldOfView=" + item.PositioningInformation.CoverageFieldOfView);
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            ItemPickerForm form = new ItemPickerForm();
            if (_selectedItem!=null)
                form.KindFilter = _selectedItem.FQID.Kind;
            form.Init();
            form.ShowDialog();
        }

        internal static Guid CameraPropertiesAnalyticsPluginId = new Guid("e919b772-74ee-43a2-8a56-fe0c08abcc84");
        internal static Guid CameraPropertiesAnalyticsKind = new Guid("dd1152b9-3eec-4dde-ad39-621cbcb96507");

        private void OnShowMultiSite(object sender, EventArgs e)
        {
            #region show master
            listBox1.Items.Clear();
            Item MasterSite = EnvironmentManager.Instance.GetSiteItem(EnvironmentManager.Instance.MasterSite);
            listBox1.Items.Add("ServerName " + MasterSite.Name + " ServerId.Id " + MasterSite.FQID.ServerId.Id);

            List<Item> cameras = Configuration.Instance.GetItemConfigurations(MasterSite.FQID.ServerId, CameraPropertiesAnalyticsPluginId, null,
                                                                    CameraPropertiesAnalyticsKind);
            foreach (Item citem in cameras)
            {
                string tx = citem.Name + ", Address=" + citem.Properties["Address"] + " (" + citem.Properties["UserName"] +
                            "/" +
                            citem.Properties["Password"] + " )";
                listBox1.Items.Add(tx);
            }
            #endregion
            #region show slaves
            List<Item> sites = MasterSite.GetChildren();
            foreach (var item in sites)
            {
                listBox1.Items.Add("+ServerName " + item.Name + " ServerId.Id " + item.FQID.ServerId.Id);


                cameras = Configuration.Instance.GetItemConfigurations(item.FQID.ServerId, CameraPropertiesAnalyticsPluginId, null,
                                                                                    CameraPropertiesAnalyticsKind);
                foreach (Item citem in cameras)
                {
                    string tx = "+ " + citem.Name + ", Address=" + citem.Properties["Address"] + " (" + citem.Properties["UserName"] +
                                "/" +
                                citem.Properties["Password"] + " )";
                    listBox1.Items.Add(tx);
                }
            }
            #endregion

        }

        public bool ShowHardware
        {
            get { return button2.Visible; }
            set
            {
                button2.Visible = value;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = Kind.Hardware;
            form.Init();
            form.ShowDialog();

        }

        private void OnClick(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            OnDoubleClick(e);
        }
    }

}
