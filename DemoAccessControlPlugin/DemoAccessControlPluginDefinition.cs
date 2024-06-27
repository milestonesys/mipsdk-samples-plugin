using System;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin
{
    public class DemoAccessControlPluginDefinition : ACPluginDefinition
    {
        private List<ACPlugin> _accessControlPlugins = new List<ACPlugin>();

        public override Guid Id
        {
            get { return new Guid("9B1DA9B8-5922-4346-BFEB-9ABD4CE46D77"); }
        }

        public override string Name
        {
            get { return "Demo Access Control Plug-in"; }
        }

        public override string Manufacturer
        { 
            get { return PluginSamples.Common.ManufacturerName; }
        }

        public override string VersionString
        {
            get { return "2.0"; }
        }

        public override List<ACPlugin> ACPlugins
        {
            get { return _accessControlPlugins; }
        }

        public override void Init()
        {
            _accessControlPlugins.Add(new DemoAccessControlPlugin());
        }

        public override void Close()
        {
            _accessControlPlugins.Clear();
        }
    }
}
