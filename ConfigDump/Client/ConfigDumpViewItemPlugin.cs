using System;
using VideoOS.Platform.Client;

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

        public override System.Drawing.Image Icon
        {
            get { return ConfigDumpDefinition._treeNodeImage; }
        }

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
