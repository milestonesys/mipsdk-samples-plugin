using System;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI.Controls;
using static VideoOS.Platform.Messaging.MessageId;

namespace SCClientAction.Client
{
    internal class CloseAllWindowsClientAction : ClientAction
    {
        public override Guid Id { get; } = new Guid("56E640A0-DAED-4289-9A1B-DA9B026810B9");

        public override string Name { get; } = "Close all windows"; //Note that in a production plug-in the action name should be localized.

        public override VideoOSIconSourceBase Icon { get; } = new VideoOSIconBuiltInSource() { Icon = VideoOSIconBuiltInSource.Icons.Close };

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        public override void Activated()
        {
            MultiWindowCommandData multiWindowCommandData = new MultiWindowCommandData();
            multiWindowCommandData.MultiWindowCommand = MultiWindowCommand.CloseAllWindows;

            EnvironmentManager.Instance.SendMessage(new Message(SmartClient.MultiWindowCommand) { Data = multiWindowCommandData });
        }
    }
}
