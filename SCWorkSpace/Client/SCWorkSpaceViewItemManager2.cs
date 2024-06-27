using VideoOS.Platform.Client;

namespace SCWorkSpace.Client
{
    public class SCWorkSpaceViewItemManager2 : VideoOS.Platform.Client.ViewItemManager
    {
        public SCWorkSpaceViewItemManager2()
            : base("SCWorkSpaceViewItemManager2")
        {
        }

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new SCWorkSpaceViewItemWpfUserControl2();
        }
    }
}
