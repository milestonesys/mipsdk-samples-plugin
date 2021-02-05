using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
	public partial class IndicatorAndAppUserControl : UserControl
	{
		public IndicatorAndAppUserControl()
		{
			InitializeComponent();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			if (!this.DesignMode)	//Add to template
			{
				Type msgType = typeof(ClearIndicatorCommandData);
				FieldInfo[] info = msgType.GetFields();
				foreach (FieldInfo type in info)
				{
					if (type.IsLiteral)
					{
						String name = type.ToString();
						name = name.Substring(name.LastIndexOf(" ") + 1);
						listBoxIndicator.Items.Add(name);
					}
				}

				msgType = typeof(ApplicationControlCommandData);
				info = msgType.GetFields();
				foreach (FieldInfo type in info)
				{
					if (type.IsLiteral)
					{
						String name = type.ToString();
						name = name.Substring(name.LastIndexOf(" ") + 1);
						listBoxApp.Items.Add(name);
					}
				}
			}

		}

		private void OnFireIndicator(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.ClearIndicatorCommand, listBoxIndicator.SelectedItem),
				null,
				null);

		}

		private void OnFireApp(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.ApplicationControlCommand, listBoxApp.SelectedItem),
				null,
				null);

		}

		private void OnLiveMode(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.ChangeModeCommand, Mode.ClientLive),
                GetWindow());
		}

		private void OnPlaybackMode(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.ChangeModeCommand, Mode.ClientPlayback),
                GetWindow());
		}

        private FQID GetWindow()
        {
            TaggedItem windowItem = (TaggedItem)comboBoxWindows.SelectedItem;
            if (windowItem != null)
                return windowItem.Item.FQID;
            return null;
        }
		private void OnMakeMessage(object sender, EventArgs e)
		{
			SmartClientMessageData data = new SmartClientMessageData();
			data.Message = "Test message with " + ((Button) sender).Tag + " priority";
			data.Priority = SmartClientMessageDataPriority.Normal;
			if ((string)(((Button)sender).Tag) == "HIGH")
				data.Priority = SmartClientMessageDataPriority.High;
			if ((string)(((Button)sender).Tag) == "LOW")
				data.Priority = SmartClientMessageDataPriority.Low;
			data.MessageId = Guid.NewGuid();
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SmartClientMessageCommand, data));
		}

        private void OnEnterWindows(object sender, EventArgs e)
        {
            comboBoxWindows.Items.Clear();
            List<Item> list = Configuration.Instance.GetItemsByKind(Kind.Window);
            foreach (Item item in list)
                comboBoxWindows.Items.Add(new TaggedItem(item));
        }

	}
}
