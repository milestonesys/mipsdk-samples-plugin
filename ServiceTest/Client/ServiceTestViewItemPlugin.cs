using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace ServiceTest.Client
{
    public class ServiceTestViewItemPlugin : ViewItemPlugin
    {

        public ServiceTestViewItemPlugin()
        {
        }

        public override Guid Id
        {
            get { return new Guid("9d278b39-0b1d-4186-9a95-a0ffedf8fe94"); }
        }

        public override VideoOSIconSourceBase IconSource { get => ServiceTestDefinition.PluginIcon; protected set => base.IconSource = value; }

        public override string Name
        {
            get { return "ServiceTest"; }
        }

        public override ViewItemManager GenerateViewItemManager()
        {
            return new ServiceTestViewItemManager();
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }

    }

}
