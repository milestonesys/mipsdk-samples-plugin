using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

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

        public override VideoOSIconSourceBase IconSource { get => SequenceViewerDefinition.TreeNodeImage; protected set => base.IconSource = value; }

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
