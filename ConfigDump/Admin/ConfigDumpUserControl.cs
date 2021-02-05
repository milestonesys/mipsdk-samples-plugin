using System;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace ConfigDump.Admin
{
	public partial class ConfigDumpUserControl : ItemNodeUserControl
	{
		internal event EventHandler ConfigurationChangedByUser;


		public ConfigDumpUserControl()
		{
			InitializeComponent();
		}


		internal void OnUserChange(object sender, EventArgs e)
		{
			if (ConfigurationChangedByUser != null)
				ConfigurationChangedByUser(this, new EventArgs());
		}

		internal void FillContent(Item item)
		{
			dumpConfigurationUserControl1.FillContent();
		}

		internal void ClearContent()
		{
		}

	}
}
