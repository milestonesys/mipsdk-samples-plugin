using System;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

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

        public override VideoOSIconSourceBase IconSource { get => VideoReplayDefinition.PluginIcon; protected set => base.IconSource = value; }

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
