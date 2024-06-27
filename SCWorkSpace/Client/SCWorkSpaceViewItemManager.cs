using VideoOS.Platform.Client;

namespace SCWorkSpace.Client
{
    public class SCWorkSpaceViewItemManager : VideoOS.Platform.Client.ViewItemManager
    {
        public SCWorkSpaceViewItemManager()
            : base("SCWorkSpaceViewItemManager")
        {
        }

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new SCWorkSpaceViewItemWpfUserControl();
        }
    }
}
