using System.Windows.Controls;

namespace SiteLicense.Client
{
    public partial class SiteLicenseSettingsPanelWpfUserControl : UserControl
    {
        public SiteLicenseSettingsPanelWpfUserControl()
        {
            InitializeComponent();
            labelLicence.Content = Admin.SiteLicenseHandler.LicenseInfo();
        }

        public void Init()
        {
            // There is no setup in this sample - just a display of the SiteLicense information
        }
    }
}
