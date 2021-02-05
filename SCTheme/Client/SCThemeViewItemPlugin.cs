using System;
using System.Reflection;
using VideoOS.Platform.Client;

namespace SCTheme.Client
{
	public class SCThemeViewItemPlugin : ViewItemPlugin
	{
        internal protected static System.Drawing.Image _treeNodeImage;

        public SCThemeViewItemPlugin()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            _treeNodeImage = System.Drawing.Image.FromStream(assembly.GetManifestResourceStream(name + ".Resources.SCTheme.ico"));
        }

		public override Guid Id
		{
			get
			{
				return new Guid("65C216DA-9CB3-4E03-B652-2452BB69AAE4");
			}
		}

		public override System.Drawing.Image Icon
		{
            get { return _treeNodeImage; }
		}

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
