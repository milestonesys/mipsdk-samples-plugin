using System;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace ServiceTest.Client
{
	public class ServiceTestViewItemManager : ViewItemManager
	{

		public ServiceTestViewItemManager()
			: base("ServiceTestViewItemManager")
		{
		}

		public override ViewItemUserControl GenerateViewItemUserControl()
		{
			return new ServiceTestViewItemUserControl(this);
		}

		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
			return new ServiceTestPropertiesUserControl(this);
		}

	}
}
