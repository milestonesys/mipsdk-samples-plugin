using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace SCViewAndWindow.Client
{
	public class SCViewAndWindowViewItemPlugin : ViewItemPlugin
	{

		public SCViewAndWindowViewItemPlugin()
		{
		}

		public override Guid Id
		{
			get { return new Guid("3a3e4a7f-8661-4021-8106-86898c1dbe89"); }
		}
		
        public override VideoOSIconSourceBase IconSource { get => SCViewAndWindowDefinition.PluginIcon; protected set => base.IconSource = value; }

        public override string Name
		{
			get { return "View And Window Tool"; }
		}

		public override ViewItemManager GenerateViewItemManager()
		{
			return new SCViewAndWindowViewItemManager();
		}

		public override void Init()
		{
		}

		public override void Close()
		{
		}
	}
}
