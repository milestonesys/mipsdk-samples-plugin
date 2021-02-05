using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace SCToolbarPlugin.Client
{
	public class BackgroundColorViewItemPlugin : ViewItemPlugin
	{
        public static readonly Guid PluginId = new Guid("D4ACB8A8-85F0-417D-B3CD-891502873A28");

        internal protected static Bitmap _icon;

        public BackgroundColorViewItemPlugin()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            _icon = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(_icon))
            {
                g.FillRectangle(Brushes.Red, 0, 0, _icon.Width, _icon.Height);
            } 
        }

		public override Guid Id
		{
			get { return PluginId; }
		}

		public override System.Drawing.Image Icon
		{
            get { return _icon; }
		}

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
