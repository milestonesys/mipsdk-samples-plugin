using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
    public partial class PlaybackWpfUserControl : UserControl
    {

        public PlaybackWpfUserControl()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            Type msgType = typeof(PlaybackData);
            FieldInfo[] info = msgType.GetFields();
            foreach (FieldInfo type in info)
            {
                if (type.IsLiteral)
                {
                    String name = type.ToString();
                    name = name.Substring(name.LastIndexOf(" ") + 1);
                    if (name!="Speed" && name!="Play" && name!="PlayMode")
                        listBoxIndicator.Items.Add(name);
                }
            }
            EnvironmentManager.Instance.RegisterReceiver(PlaybackTimeHandler,
                                                            new MessageIdFilter(MessageId.SmartClient.PlaybackCurrentTimeIndication));
        }

        private object PlaybackTimeHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            DateTime dt = (DateTime)message.Data;
            // Convert to user selected time
            DateTime convertedTime = TimeZoneInfo.ConvertTime(dt, ClientControl.Instance.SelectedTimeZoneInfo);
            textBoxTime.Text = convertedTime.ToLongTimeString();
            return null;
        }

        private void OnFireIndicator(object sender, System.Windows.RoutedEventArgs e)
        {
            string command = (string)listBoxIndicator.SelectedItem;
            // The entered datetime is here consider within the time zone selected by the Smart Client.
            // Convert from selected time zone to UTC

            // First change the entered datatime to be unspecified
            DateTime unspecifiedTime = new DateTime(dateTimePicker.Value.Ticks, DateTimeKind.Unspecified);

            // Now convert according to selected timezoneinfo (In UTC)
            DateTime dateTime = TimeZoneInfo.ConvertTime(unspecifiedTime, ClientControl.Instance.SelectedTimeZoneInfo, TimeZoneInfo.Utc);

            double speed = 1.0;
            try
            {
                speed = Double.Parse(textBoxSpeed.Text, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                textBoxSpeed.Text = "1.0";
            }

            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(
                    MessageId.SmartClient.PlaybackCommand,
                    new PlaybackCommandData()
                    {
                        Command=command,
                        DateTime = dateTime,
                        Speed = speed
                    }));
        }
    }
}
