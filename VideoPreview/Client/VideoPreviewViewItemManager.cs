using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace VideoPreview.Client
{
	public class VideoPreviewViewItemManager : ViewItemManager
	{
		private Guid _someid;
		private string _someName;
		private List<Item> _config;

		public VideoPreviewViewItemManager()
			: base("VideoPreviewViewItemManager")
		{
		}

		public override void PropertiesLoaded()
		{
			_config = Configuration.Instance.GetItemConfigurations(VideoPreviewDefinition.VideoPreviewPluginId, null,
																  VideoPreviewDefinition.VideoPreviewKind);
			string someid = GetProperty("SelectedGUID");
			if (someid != null)
			{
				if (someid != null && _config != null)
				{
					SomeId = new Guid(someid);
				}
			}
		}

		public override ViewItemUserControl GenerateViewItemUserControl()
		{
			return new VideoPreviewViewItemUserControl(this);
		}

		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
			return new VideoPreviewPropertiesUserControl(this);
		}

		public List<Item> Config
		{
			get { return _config; }
		}

		public Guid SomeId
		{
			get { return _someid; }
			set
			{
				_someid = value;
				SetProperty("SelectedGUID", _someid.ToString());
				if (_config != null)
					foreach (Item item in _config)
					{
						if (item.FQID.ObjectId == _someid)
						{
							SomeName = item.Name;
						}
					}
				SaveProperties();
			}
		}

		public void SaveAllProperties()
		{
			SaveProperties();
		}

		public String SomeName
		{
			get { return _someName; }
			set { _someName = value; }
		}
	}
}
