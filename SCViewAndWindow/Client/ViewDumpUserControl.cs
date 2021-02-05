using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform;

namespace SCViewAndWindow.Client
{
	public partial class ViewDumpUserControl : UserControl
	{
		public ViewDumpUserControl()
		{
			InitializeComponent();
		}


		private void OnTest(object sender, EventArgs e)
		{
			treeView.Nodes.Clear();
			treeView.ImageList = VideoOS.Platform.UI.Util.ImageListClone;
			TreeNode tn = new TreeNode("Top");
			tn.ImageIndex = tn.SelectedImageIndex = 0;
			treeView.Nodes.Add(tn);
			ItemHierarchy ih = ItemHierarchy.UserDefined;

			List<Item> all = Configuration.Instance.GetItems(ih);
			List<Item> scRelevant = new List<Item>();
			foreach (Item item in all)
			{
				if (item.FQID.Kind == Kind.View)
					scRelevant.Add(item);
			}

			DumpItems(tn, null, scRelevant, 0, true, true, true, true, true);
		}

		private void DumpItems(TreeNode node, Item parentItem, List<Item> items, int level, bool children, bool childrensChildren, bool related, bool relatedsRelated, bool parent)
		{
			if (items != null)
			{
				foreach (Item item in items)
				{
					TreeNode tn = new TreeNode(item.Name);
					node.Nodes.Add(tn);
					tn.ImageIndex = tn.SelectedImageIndex = VideoOS.Platform.UI.Util.KindToImageIndex[item.FQID.Kind];

					// Check for loops
					Item parentItemCheck = item.GetParent();
					while (parentItemCheck != null)
					{
						if (parentItemCheck.FQID.Equals(item.FQID))
						{
							tn.Nodes.Add("Recursive ITEM").BackColor = Color.Red;
							return; //We skip rest in items !
						}
						parentItemCheck = parentItemCheck.GetParent();
					}


					ItemDump(tn, item);

					tn.Nodes.Add("HasRelated: " + item.HasRelated);
					tn.Nodes.Add("HasChildren: " + item.HasChildren);
					TreeNode propNode = new TreeNode("Properties");
					tn.Nodes.Add(propNode);
					foreach (KeyValuePair<string, string> property in item.Properties)
					{
						propNode.Nodes.Add("Property: " + property.Key + "=" + property.Value);
					}

					TreeNode AuthNode = new TreeNode("Authorization");
					tn.Nodes.Add(AuthNode);
					foreach (KeyValuePair<string, string> authorization in item.Authorization)
					{
						AuthNode.Nodes.Add(new string(' ', level * 2) + "Authorization: " + authorization.Key + "=" + authorization.Value);
					}

					if (related)
					{
						TreeNode r = tn.Nodes.Add("Related:");
						DumpItems(r, null, item.GetRelated(), level + 1, childrensChildren, childrensChildren, relatedsRelated,
								  relatedsRelated, true);
					}
					if (children)
					{
						TreeNode c = tn.Nodes.Add("Children:");
						List<Item> childrenList = item.GetChildren();
						DumpItems(c, item, childrenList, level + 1, childrensChildren, childrensChildren, relatedsRelated, relatedsRelated,
								  true);
					}
				}
			}
		}

		private void ItemDump(TreeNode tn, Item item)
		{
			TreeNode fqidNode = new TreeNode("FQID");
			tn.Nodes.Add(fqidNode);
			fqidNode.Nodes.Add("FQID.Kind: " + item.FQID.Kind + " (" + Kind.DefaultTypeToNameTable[item.FQID.Kind] + ")");
			fqidNode.Nodes.Add("FQID.ServerId.ServerType: " + item.FQID.ServerId.ServerType);
			fqidNode.Nodes.Add("FQID.ServerId.Id: " + item.FQID.ServerId.Id);
			fqidNode.Nodes.Add("FQID.ServerId.ServerHostname: " + item.FQID.ServerId.ServerHostname);
			fqidNode.Nodes.Add("FQID.ServerId.Serverport: " + item.FQID.ServerId.Serverport);
			fqidNode.Nodes.Add("FQID.ParentId: " + item.FQID.ParentId);
			fqidNode.Nodes.Add("FQID.ObjectId: " + item.FQID.ObjectId);
			fqidNode.Nodes.Add("FQID.ObjectIdString: " + item.FQID.ObjectIdString);
			fqidNode.Nodes.Add("FQID.FolderType: " + item.FQID.FolderType);
		}


	}
}
