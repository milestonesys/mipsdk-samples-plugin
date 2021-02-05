using System;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform.Client;

namespace ServiceTest.Client
{
	public class ServiceTestViewItemPlugin : ViewItemPlugin
	{

		public ServiceTestViewItemPlugin()
		{
		}

		public override Guid Id
		{
			get { return new Guid("9d278b39-0b1d-4186-9a95-a0ffedf8fe94"); }
		}

		public override System.Drawing.Image Icon
		{
			get { return ServiceTestDefinition._treeNodeImage; }
		}

		public override string Name
		{
			get { return "ServiceTest"; }
		}

		public override ViewItemManager GenerateViewItemManager()
		{
			return new ServiceTestViewItemManager();
		}

		public override void Init()
		{
		}

		public override void Close()
		{
		}

	}

}
