using System;
using VideoOS.Platform.Client;

namespace ServerSideCarrousel.Client
{
	public class CarrouselViewItemPlugin : ViewItemPlugin
	{

		public CarrouselViewItemPlugin()
		{
		}

		public override Guid Id
		{
			get
			{
				return new Guid("53940325-6988-4ca5-A215-824686235C83");
			}
		}

		public override System.Drawing.Image Icon
		{
			get { return ServerSideCarrouselDefinition._treeNodeImage; }
		}

		public override string Name
		{
			get { return "Server Side Carrousel"; }
		}

		public override VideoOS.Platform.Client.ViewItemManager  GenerateViewItemManager()
		{
			return new CarrouselViewItemManager();
		}

		public override void Init()
		{
		}

		public override void Close()
		{
		}
	}
}
