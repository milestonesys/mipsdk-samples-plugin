using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace LocationView.Client
{
	public class LocationViewViewItemManager 
        : ViewItemManager
	{
        private Config.Config _config;

		public LocationViewViewItemManager()
			: base("LocationViewViewItemManager")
		{
		}

		public override void PropertiesLoaded()
		{
		}

		public override ViewItemUserControl GenerateViewItemUserControl()
		{
			return new LocationViewViewItemUserControl(this);
		}

		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
            return null;
		}

		internal Config.Config Config
		{
			get
            {
                if (_config == null)
                {
                    _config = new Config.Config();
                    _config.ReadConfiguration(GetProperty);
                }
                return _config;
            }
		}

		public void SaveAllProperties()
		{          
            Config.WriteConfiguration(SetProperty);
            SaveProperties();
		}
	}
}
