using VideoOS.Platform.Client;

namespace VideoReplay.Client
{
    public class VideoReplayViewItemManager : ViewItemManager
    {

        public VideoReplayViewItemManager()
            : base("VideoReplayViewItemManager")
        {
        }

        #region Methods overridden

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new VideoReplayViewItemWpfUserControl(this);
        }

        #endregion

    }
}
