using System;
using System.Xml;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace VideoPreview.Client
{
	public class VideoPreviewViewItemPlugin : ViewItemPlugin
	{

		public VideoPreviewViewItemPlugin()
		{
		}

		public override Guid Id
		{
			get { return new Guid("5bcf8eac-0520-44d3-b11b-a0eeba0d0162"); }
		}

		public override VideoOSIconSourceBase IconSource
		{
			get { return new VideoOSIconUriSource { Uri = new Uri("pack://application:,,,/VideoPreview;component/Resources/VideoPreviewWpf.png") }; }
		}

		public override string Name
		{
			get { return "VideoPreview"; }
		}

		public override ViewItemManager GenerateViewItemManager()
		{
			return new VideoPreviewViewItemManager();
		}

		public override void Init()
		{			
		}

		public override void Close()
		{
		}
	}

}
