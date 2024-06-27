using System;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI.Controls;
using static VideoOS.Platform.Messaging.MessageId;

namespace SCClientAction.Client
{
    internal class Rewind15SecondsClientAction : ClientAction
    {
        public override Guid Id { get; } = new Guid("26B2EA75-3A51-433B-BAE5-E8FFF00DF903");

        public override string Name { get; } = "Rewind 15 seconds"; //Note that in a production plug-in the action name should be localized.

        public override VideoOSIconSourceBase Icon { get; } = new VideoOSIconBuiltInSource() { Icon = VideoOSIconBuiltInSource.Icons.Minus };

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        public override void Activated()
        {
            ImageViewerAddOn imageViewerAddOn = ImageViewerHelper.GetGlobalSelectedImageViewer();
            if (imageViewerAddOn != null)
            {
                DateTime rewindTime;
                if (imageViewerAddOn.IndependentPlaybackEnabled)
                {
                    rewindTime = imageViewerAddOn.IndependentPlaybackController.PlaybackTime.AddSeconds(-15);
                }
                else 
                {
                    if (imageViewerAddOn.InLiveMode)
                    {
                        rewindTime = DateTime.UtcNow.AddSeconds(-15);
                    }
                    else
                    {
                        FQID playbackControllerFqid = ClientControl.Instance.GetPlaybackController(imageViewerAddOn.WindowInformation);
                        if (playbackControllerFqid != null)
                        {
                            PlaybackController playbackController = ClientControl.Instance.GetPlaybackController(playbackControllerFqid);
                            rewindTime = playbackController.PlaybackTime.AddSeconds(-15);
                        }
                        else
                        {
                            DateTime playbackTime = (DateTime)EnvironmentManager.Instance.SendMessage(new Message(SmartClient.GetCurrentPlaybackTimeRequest))[0];
                            rewindTime = playbackTime.AddSeconds(-15);
                        }
                    }
                    imageViewerAddOn.IndependentPlaybackEnabled = true;
                }
                imageViewerAddOn.IndependentPlaybackController.PlaybackTime = rewindTime;
            }
        }
    }
}
