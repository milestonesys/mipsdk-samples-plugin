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

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new LocationViewViewItemWpfUserControl(this);
        }

        public Config.Config Config
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
