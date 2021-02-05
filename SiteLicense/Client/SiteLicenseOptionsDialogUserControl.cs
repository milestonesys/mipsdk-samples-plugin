using VideoOS.Platform.Client;

namespace SiteLicense.Client
{
	/// <summary>
	/// This UserControl is created by the PluginDefinition and placed on the Smart Client's options dialog when the user selects the options icon.<br/>
	/// The UserControl will be added to the owning parent UserControl and docking set to Fill.
	/// </summary>
	public partial class SiteLicenseOptionsDialogUserControl : OptionsDialogUserControl
	{
		public SiteLicenseOptionsDialogUserControl()
		{
			InitializeComponent();
		}

		public override void Init()
		{
			// There is no setup in this sample - just a display of the SiteLicense information

			label1.Text = Admin.SiteLicenseHandler.LicenseInfo();
		}

		public override void Close()
		{
		}

	}
}
