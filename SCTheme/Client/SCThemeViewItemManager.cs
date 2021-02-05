using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCTheme.Client
{
	public class SCThemeViewItemManager : VideoOS.Platform.Client.ViewItemManager
	{
        public SCThemeViewItemManager()
            : base("SCThemeViewItemManager")
        {
        }

		public override ViewItemUserControl GenerateViewItemUserControl()
		{
            return new SCThemeViewItemUserControl();
		}

		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
			return new PropertiesUserControl();
		}

    }
}
