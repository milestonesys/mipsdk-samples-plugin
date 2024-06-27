using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace ConfigDump.Client
{
    public class ConfigDumpViewItemPlugin : ViewItemPlugin
    {

        public ConfigDumpViewItemPlugin()
        {
        }

        public override Guid Id
        {
            get { return new Guid("3c72c67b-2501-4d6e-8984-9d0143bfa299"); }
        }

        public override VideoOSIconSourceBase IconSource { get => ConfigDumpDefinition.PluginIcon; protected set => base.IconSource = value; }

        public override string Name
        {
            get { return "ConfigDump"; }
        }

        public override ViewItemManager GenerateViewItemManager()
        {
            return new ConfigDumpViewItemManager();
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }
    }
}
