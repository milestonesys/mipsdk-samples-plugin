using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SequenceViewer.Client
{
	public class SequenceViewerViewItemManager : ViewItemManager
	{
		private Guid _someid;
		private string _someName;
		private List<Item> _configItems;

		public SequenceViewerViewItemManager()
			: base("DataSourceViewItemManager")
		{
		}

		#region Methods overridden
		public override void PropertiesLoaded()
		{
			String someid = GetProperty("SelectedGUID");
			_configItems = Configuration.Instance.GetItemConfigurations(SequenceViewerDefinition.DataSourcePluginId, null, SequenceViewerDefinition.DataSourceKind);
			if (someid != null && _configItems != null)
			{
				SomeId = new Guid(someid);
			}
		}

		public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
		{
			return new SequenceViewerViewItemWpfUserControl(this);
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
					foreach (Item item in _configItems)
					{
						if (item.FQID.ObjectId == _someid)
						{
							SomeName = item.Name;
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
