using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
	public partial class PlaybackUControl : UserControl
	{
		public event EventHandler ClickEvent;

		public PlaybackUControl()
		{
			InitializeComponent();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			if (!this.DesignMode)
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

		}

		private object PlaybackTimeHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
		{
			DateTime dt = (DateTime)message.Data;
			// Convert to user selected time!
			DateTime convertedTime = TimeZoneInfo.ConvertTime(dt, ClientControl.Instance.SelectedTimeZoneInfo);
			textBoxTime.Text = convertedTime.ToLongTimeString();
			return null;
		}

		private void OnClick(object sender, EventArgs e)
		{
			if (ClickEvent != null)
				ClickEvent(this, new EventArgs());
		}

		private void OnFireIndicator(object sender, EventArgs e)
		{
			string command = (string) listBoxIndicator.SelectedItem;
			// The entered datetime is here consider within the time zone selected by the Smart Client.
			// Convert from selected time zone to UTC

			// First change the entered datatime to be unspecified
			DateTime unspecifiedTime = new DateTime(dateTimePicker1.Value.Ticks, DateTimeKind.Unspecified);

			// Now convert according to selected timezoneinfo (In UTC)
			DateTime dateTime = TimeZoneInfo.ConvertTime(unspecifiedTime, ClientControl.Instance.SelectedTimeZoneInfo, TimeZoneInfo.Utc);

		    double speed = 1.0;
		    try
		    {
		        speed = Double.Parse(_speedTextBox.Text, CultureInfo.InvariantCulture);
		    }
		    catch (Exception)
		    {
                _speedTextBox.Text = "1.0";
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
