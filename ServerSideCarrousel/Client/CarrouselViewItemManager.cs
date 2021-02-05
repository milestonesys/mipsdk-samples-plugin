using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ServerSideCarrousel.Admin;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace ServerSideCarrousel.Client
{
	public class CarrouselViewItemManager : VideoOS.Platform.Client.ViewItemManager
	{
		private Guid _selectedCarrouselId;
		private int _defaultTime = 10;
		private List<CarrouselTreeNode> _items = new List<CarrouselTreeNode>();

        public CarrouselViewItemManager(): base("ServerSideCarrouselViewItem")
        {
        }

		public override void PropertiesLoaded()
		{
			string guidString = GetProperty("SelectedGUID");
			if (guidString != null)
				SelectedCarrouselId = new Guid(guidString);            
		}

		public override ViewItemUserControl GenerateViewItemUserControl()
		{
			return new CarrouselViewItemUserControl(this);
		}

		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
			return new CarrouselPropertiesUserControl(this);
		}

    	public Guid SelectedCarrouselId
    	{
			get { return _selectedCarrouselId; }
			set
			{
				if (_selectedCarrouselId != value)
				{
					_selectedCarrouselId = value;
					SetProperty("SelectedGUID", _selectedCarrouselId.ToString());

					Item item = Configuration.Instance.GetItemConfiguration(ServerSideCarrouselDefinition.CarrouselPluginId, ServerSideCarrouselDefinition.CarrouselKind, _selectedCarrouselId);
					CarrouselConfigUtil.BuildCarrouselList(item, CarrouselItems);

                    SaveProperties();       // Moved down to after the re-build of camera list
                }
            }
    	}
		private void CarrouselItems(List<CarrouselTreeNode> list)
		{
			_items = list;
		}

		internal List<CarrouselTreeNode> Items
		{
			get { return _items; }
			set { _items = value; }
		}

		public int DefaultTime
		{
			get { return _defaultTime; }
		}
    }
}
