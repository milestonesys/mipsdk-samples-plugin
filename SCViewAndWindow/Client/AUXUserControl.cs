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

namespace SCViewAndWindow.Client
{
	public partial class AUXUserControl : UserControl
	{
		public AUXUserControl()
		{
			InitializeComponent();

			comboBoxAUX.Items.Clear();
			for (int ix = 1; ix < 9; ix++ )
				comboBoxAUX.Items.Add("" + ix);

			comboBoxOnOff.Items.Clear();
			comboBoxOnOff.Items.Add("On");
			comboBoxOnOff.Items.Add("Off");
		}

		private void OnFireIndicator(object sender, EventArgs e)
		{
			if (comboBoxOnOff.SelectedIndex==-1 || comboBoxAUX.SelectedIndex==-1)
			{
				MessageBox.Show("Please select above...","Missing selection(s)");
				return;
			}
			PTZAUXCommandData data = new PTZAUXCommandData();
			data.AuxNumber = comboBoxAUX.SelectedIndex+1;
			data.On = (string)comboBoxOnOff.SelectedItem == "On";
			EnvironmentManager.Instance.SendMessage(
				new VideoOS.Platform.Messaging.Message(MessageId.Control.PTZAUXCommand, data),
				null,
				null);

		}


	}
}
