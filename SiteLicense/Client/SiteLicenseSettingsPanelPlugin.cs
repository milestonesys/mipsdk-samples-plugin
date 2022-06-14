using System;
using System.Windows.Controls;
using VideoOS.Platform.Client;

namespace SiteLicense.Client
{

    public class SiteLicenseSettingsPanelPlugin : SettingsPanelPlugin
    {
        private UserControl _userControl;

        public override Guid Id { get { return SiteLicenseDefinition.SiteLicensePluginId; } }

        public override string Title { get { return "SiteLicense sample"; } }

        public override void Close()
        {
        }

        public override void CloseUserControl()
        {
            _userControl = null;
        }

        public override UserControl GenerateUserControl()
        {
            return new SiteLicenseSettingsPanelWpfUserControl();
        }

        public override void Init()
        {
        }

        public override bool TrySaveChanges(out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}
