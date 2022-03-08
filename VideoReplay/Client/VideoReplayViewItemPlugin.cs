using System;
using VideoOS.Platform.Client;

namespace VideoReplay.Client
{
    public class VideoReplayViewItemPlugin : ViewItemPlugin
    {

        public VideoReplayViewItemPlugin()
        {
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        public override Guid Id
        {
            get { return VideoReplayDefinition.VideoReplayViewItemPlugin; }
        }

        public override System.Drawing.Image Icon
        {
            get { return VideoReplayDefinition.TreeNodeImage; }
        }

        public override string Name
        {
            get { return "VideoReplay"; }
        }

        public override ViewItemManager GenerateViewItemManager()
        {
            return new VideoReplayViewItemManager();
        }
    }
}
