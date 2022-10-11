using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace ConfigDump.Client
{
    public class ConfigDumpViewItemManager : ViewItemManager
    {
        private Guid _someid;
        private string _someName;
        private List<Item> _config;

        public ConfigDumpViewItemManager()
            : base("ConfigDumpViewItemManager2")
        {
        }

        public override void PropertiesLoaded()
        {
            string someid = GetProperty("SelectedGUID");
            _config = Configuration.Instance.GetItemConfigurations(ConfigDumpDefinition.ConfigDumpPluginId, null, ConfigDumpDefinition.ConfigDumpKind);
            if (someid != null && _config != null)
            {
                SomeId = new Guid(someid);
            }
        }

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new ConfigDumpViewItemWpfUserControl(this);
        }

        public List<Item> Config
        {
            get { return _config; }
        }

        public Guid SomeId
        {
            get { return _someid; }
            set
            {
                _someid = value;
                SetProperty("SelectedGUID", _someid.ToString());
                if (_config != null)
                {
                    foreach (Item item in _config)
                    {
                        if (item.FQID.ObjectId == _someid)
                        {
                            SomeName = item.Name;
                        }
                    }
                }
                SaveProperties();
            }
        }

        public String SomeName
        {
            get { return _someName; }
            set { _someName = value; }
        }
    }
}
