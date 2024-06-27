using System;
using System.Reflection;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace SCTheme.Client
{
	public class SCThemeViewItemPlugin : ViewItemPlugin
	{
        private readonly static VideoOSIconSourceBase _treeNodeImage;

		static SCThemeViewItemPlugin()
		{
            var packString = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/SCTheme.png");
            _treeNodeImage = new VideoOSIconUriSource() { Uri = new Uri(packString) };
        }

        public SCThemeViewItemPlugin()
        {
        }

		public override Guid Id
		{
			get
			{
				return new Guid("65C216DA-9CB3-4E03-B652-2452BB69AAE4");
			}
		}

        public override VideoOSIconSourceBase IconSource { get => _treeNodeImage; protected set => base.IconSource = value; }

		public override string Name
		{
			get { return "SCTheme View Item"; }
		}

        public override bool HideSetupItem
        {
            get
            {
                return false;
            }
        }

		public override ViewItemManager  GenerateViewItemManager()
		{
            return new SCThemeViewItemManager();
		}

		public override void Init()
		{
		}

		public override void Close()
		{
		}

    }

}
