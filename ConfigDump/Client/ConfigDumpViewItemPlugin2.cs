using System;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform.Client;

namespace ConfigDump.Client
{
	public class ConfigDumpViewItemPlugin2 : ViewItemPlugin
	{

		public ConfigDumpViewItemPlugin2()
		{
		}

		public override Guid Id
		{
			get { return new Guid("3c72c67b-2501-4d6e-8984-9d0143bfa299"); }
		}

		public override System.Drawing.Image Icon
		{
			get { return ConfigDumpDefinition._treeNodeImage; }
		}

		public override string Name
		{
			get { return "ConfigDump"; }
		}

		public override ViewItemManager GenerateViewItemManager()
		{
			return new ConfigDumpViewItemManager2();
		}

		public override void Init()
		{
		}

		public override void Close()
		{
		}

	}

}
