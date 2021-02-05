using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCViewAndWindow.Client
{
	public class SCViewAndWindowViewItemManager : ViewItemManager
	{
		private Guid _someid;
		private string _someName;
		private List<Item> _config;

		public SCViewAndWindowViewItemManager()
			: base("SCViewAndWindowViewItemManager")
		{
		}

		public override void PropertiesLoaded()
		{
			string someid = GetProperty("SelectedGUID");
			_config = Configuration.Instance.GetItemConfigurations(SCViewAndWindowDefinition.SCViewAndWindowPluginId, null, SCViewAndWindowDefinition.SCViewAndWindowKind);
			if (someid != null && _config != null)
			{
				SomeId = new Guid(someid);
			}
		}

		public override ViewItemUserControl GenerateViewItemUserControl()
		{
			return new SCViewAndWindowViewItemUserControl(this);
		}

		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
			return new SCViewAndWindowPropertiesUserControl(this);
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
				{
					foreach (Item item in _config)
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
