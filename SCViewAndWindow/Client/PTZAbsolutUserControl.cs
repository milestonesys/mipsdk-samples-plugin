using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCViewAndWindow.Client
{
	public partial class PTZAbsolutUserControl : UserControl
	{
		public PTZAbsolutUserControl()
		{
			InitializeComponent();
		}

		void OnFirePTZ(object sender, EventArgs e)
		{
			PTZMoveAbsoluteCommandData data = new PTZMoveAbsoluteCommandData();
			Double.TryParse(maskedTextBoxPan.Text, out data.Pan);
			Double.TryParse(maskedTextBoxTilt.Text, out data.Tilt);
			Double.TryParse(maskedTextBoxZoom.Text, out data.Zoom);
		    data.AllowRepeats = _allowRepeatsCheckBox.Checked;
			data.Speed = 1.0;
			EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveAbsoluteCommand, data));
		}

		private void OnGetCurrent(object sender, EventArgs e)
		{
			Collection<object> result = EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZGetAbsoluteRequest, null));
			if (result.Count > 0)
			{
				PTZGetAbsoluteRequestData data = (PTZGetAbsoluteRequestData) result[0];
				maskedTextBoxPan.Text = data.Pan.ToString();
				maskedTextBoxTilt.Text = data.Tilt.ToString();
				maskedTextBoxZoom.Text = data.Zoom.ToString();
			}
		}

		private void OnMoveStart(object sender, EventArgs e)
		{
			PTZMoveStartCommandData data = new PTZMoveStartCommandData();
			Double.TryParse(maskedTextBoxPan.Text, out data.Pan);
			Double.TryParse(maskedTextBoxTilt.Text, out data.Tilt);
			Double.TryParse(maskedTextBoxZoom.Text, out data.Zoom);
			data.Speed = 1.0;
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveStartCommand, data));
		}

		private void OnMoveStop(object sender, EventArgs e)
		{
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZMoveStopCommand));
		}
	}
}
