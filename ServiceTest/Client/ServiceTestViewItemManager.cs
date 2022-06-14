using VideoOS.Platform.Client;

namespace ServiceTest.Client
{
    public class ServiceTestViewItemManager : ViewItemManager
    {

        public ServiceTestViewItemManager()
            : base("ServiceTestViewItemManager")
        {
        }

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new ServiceTestViewItemWpfUserControl(this);
        }
    }
}
