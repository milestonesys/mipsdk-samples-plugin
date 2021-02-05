using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCWorkSpace.Client
{
	public class SCWorkSpaceViewItemManager : VideoOS.Platform.Client.ViewItemManager
	{
        public SCWorkSpaceViewItemManager()
            : base("SCWorkSpaceViewItemManager")
        {
        }

		public override ViewItemUserControl GenerateViewItemUserControl()
		{
            return new SCWorkSpaceViewItemUserControl();
		}

		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
			return new PropertiesUserControl(); //no special properties
		}

    }
}
