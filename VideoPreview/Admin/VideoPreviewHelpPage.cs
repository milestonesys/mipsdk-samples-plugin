using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform.Admin;

namespace VideoPreview.Admin
{
	public partial class HelpPage : ItemNodeUserControl
	{
		internal event EventHandler ConfigurationChangedByUser;

		public HelpPage()
		{
			InitializeComponent();
		}

		private void OnChange(object sender, EventArgs e)
		{
			if (ConfigurationChangedByUser != null)
				ConfigurationChangedByUser(this, new EventArgs());
		}
	}
}
