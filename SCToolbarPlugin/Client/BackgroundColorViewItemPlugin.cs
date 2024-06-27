using System;
using System.Reflection;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace SCToolbarPlugin.Client
{
    public class BackgroundColorViewItemPlugin : ViewItemPlugin
	{
		public static readonly Guid PluginId = new Guid("D4ACB8A8-85F0-417D-B3CD-891502873A28");
		private readonly static VideoOSIconSourceBase _icon;

		static BackgroundColorViewItemPlugin()
		{
            var packString = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/RedSquare.png");
            _icon = new VideoOSIconUriSource() { Uri = new Uri(packString) };
        }

        public BackgroundColorViewItemPlugin()
		{
		}

		public override Guid Id
		{
			get { return PluginId; }
		}

        public override VideoOSIconSourceBase IconSource { get => _icon; protected set => base.IconSource = value; }

        public override bool HideSetupItem
		{
			get
			{
				return false;
			}
		}

		public override string Name
		{
			get { return "Background color View Item"; }
		}

		public override ViewItemManager  GenerateViewItemManager()
		{
			return new BackgroundColorViewItemManager();
		}

		public override void Init()
		{
		}

		public override void Close()
		{
		}
	}
}
