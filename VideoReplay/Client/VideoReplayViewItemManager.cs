using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace VideoReplay.Client
{
	public class VideoReplayViewItemManager : ViewItemManager
	{
		private Guid _someid;
		private string _someName;
		private List<Item> _configItems;

		public VideoReplayViewItemManager()
			: base("VideoReplayViewItemManager")
		{
		}

		#region Methods overridden
		public override void PropertiesLoaded()
		{
			String someid = GetProperty("SelectedGUID");
			_configItems = Configuration.Instance.GetItemConfigurations(VideoReplayDefinition.VideoReplayPluginId, null, VideoReplayDefinition.VideoReplayKind);
			if (someid != null && _configItems != null)
			{
				SomeId = new Guid(someid);	// Set as last selected
			}
		}


		public override ViewItemUserControl GenerateViewItemUserControl()
		{
			return new VideoReplayViewItemUserControl(this);
		}

		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
			return new VideoReplayPropertiesUserControl(this);
		}
	
		#endregion

		public List<Item> ConfigItems
		{
			get { return _configItems; }
		}

		public Guid SomeId
		{
			get { return _someid; }
			set
			{
				_someid = value;
				SetProperty("SelectedGUID", _someid.ToString());
				if (_configItems != null)
				{
					foreach (Item item in _configItems)
					{
						if (item.FQID.ObjectId == _someid)
						{
							SomeName = item.Name;
						}
					}
				}
				SaveProperties();
			}
		}

		public String SomeName
		{
			get { return _someName; }
			set { _someName = value; }
		}
	}
}
