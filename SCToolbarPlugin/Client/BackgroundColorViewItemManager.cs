using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
	public class BackgroundColorViewItemManager : ViewItemManager
	{
        public BackgroundColorViewItemManager()
            : base("BackgroundColorViewItemManager")
        {
        }

		public override ViewItemUserControl GenerateViewItemUserControl()
		{
            return new BackgroundColorViewItemUserControl(this);
		}

		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
			return new PropertiesUserControl(); //no special properties
		}

    }
}
