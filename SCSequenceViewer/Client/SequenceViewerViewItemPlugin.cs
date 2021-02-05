using System;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform.Client;

namespace SequenceViewer.Client
{
	public class SequenceViewerViewItemPlugin : ViewItemPlugin
	{

		public SequenceViewerViewItemPlugin()
		{
		}

		public override Guid Id
		{
			get { return new Guid("a5833d9c-1fa3-421a-ad92-a86ab3de923d"); }
		}

		public override System.Drawing.Image Icon
		{
			get { return SequenceViewerDefinition._treeNodeImage; }
		}

		public override string Name
		{
			get { return "SequenceViewer"; }
		}

		public override ViewItemManager GenerateViewItemManager()
		{
			return new SequenceViewerViewItemManager();
		}

		public override void Init()
		{			
		}

		public override void Close()
		{
		}
	}

}
