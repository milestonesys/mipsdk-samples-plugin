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
	public partial class ViewCommandUserControl : UserControl
	{
		public event EventHandler ClickEvent;

		public ViewCommandUserControl()
		{
			InitializeComponent();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			if (!this.DesignMode)	
			{
				Type msgType = typeof(ViewItemControlCommandData);
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
			}

		}

		private void OnClick(object sender, EventArgs e)
		{
			if (ClickEvent != null)
				ClickEvent(this, new EventArgs());
		}

		private void OnFireIndicator(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.ViewItemControlCommand, listBoxIndicator.SelectedItem),
				null,
				null);

		}

        private void buttonUp_Click(object sender, EventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand, 
                    new SetSelectedViewItemData(){ MoveCommand=MoveCommand.MoveUp}));
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand,
                    new SetSelectedViewItemData() { MoveCommand = MoveCommand.MoveLeft }));
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand,
                    new SetSelectedViewItemData() { MoveCommand = MoveCommand.MoveRight }));
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand,
                    new SetSelectedViewItemData() { MoveCommand = MoveCommand.MoveDown }));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            EnvironmentManager.Instance.SendMessage(
                new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.SetSelectedViewItemCommand,
                    new SetSelectedViewItemData() { MoveCommand = MoveCommand.MoveIndex, LayoutIndex=1 }));
        }
	}
}
