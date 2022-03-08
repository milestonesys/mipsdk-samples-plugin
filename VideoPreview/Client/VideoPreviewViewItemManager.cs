using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace VideoPreview.Client
{
	public class VideoPreviewViewItemManager : ViewItemManager
	{
		public VideoPreviewViewItemManager()
			: base("VideoPreviewViewItemManager")
		{
		}

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
			return new VideoPreviewWpfUserControl(this);
		}

		public void SaveAllProperties()
		{
			SaveProperties();
		}
	}
}
