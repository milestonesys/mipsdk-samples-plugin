using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCWorkSpace.Client
{
	public class SCWorkSpaceViewItemManager2 : VideoOS.Platform.Client.ViewItemManager
	{
        public SCWorkSpaceViewItemManager2()
            : base("SCWorkSpaceViewItemManager2")
        {
        }

		public override ViewItemUserControl GenerateViewItemUserControl()
		{
            return new SCWorkSpaceViewItemUserControl2();
		}

    }
}
