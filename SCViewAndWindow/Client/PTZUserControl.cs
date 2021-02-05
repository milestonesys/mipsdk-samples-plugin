using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using Message=System.Windows.Forms.Message;

namespace SCViewAndWindow.Client
{
	public partial class PTZUserControl : UserControl
	{
		Item _selectedCamera = null;

		public PTZUserControl()
		{
			InitializeComponent();
		}

		void OnLoad(object sender, EventArgs e)
		{
			if (!this.DesignMode)
			{
				EnvironmentManager.Instance.RegisterReceiver(SelectedCameraChangedHandler,
				                                             new MessageIdFilter(MessageId.SmartClient.SelectedCameraChangedIndication));
			}
		}

		object SelectedCameraChangedHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
		{
			if (message.RelatedFQID == null)
			{
				button1.Text = "None selected";
			}
			else
			{
				Item item = Configuration.Instance.GetItem(message.RelatedFQID);
				button1.Text = item.Name;
			}
			return null;
		}

		private void OnSelect(object sender, EventArgs e)
		{
			ItemPickerForm form = new ItemPickerForm();
			form.AutoAccept = true;
			form.KindFilter = Kind.Camera;
			form.Init();
			if (form.ShowDialog() == DialogResult.OK)
			{
				_selectedCamera = form.SelectedItem;
				button1.Text = _selectedCamera.Name;
			} else
			{
				_selectedCamera = null;
				button1.Text = "Current";				
			}
		}

		private void OnUpLeft(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.UpLeft),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnUp(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Up),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnUpRight(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.UpRight),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnLeft(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Left),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnHome(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Home),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnRight(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Right),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnDownLeft(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.DownLeft),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnDown(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.Down),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnDownRight(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.DownRight),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnZoomIn(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.ZoomIn),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
		private void OnZoomOut(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveCommand, PTZMoveCommandData.ZoomOut),
				_selectedCamera == null ? null : _selectedCamera.FQID, null);
		}
	}
}
