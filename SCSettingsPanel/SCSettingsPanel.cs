using System;
using System.Windows.Controls;
using VideoOS.Platform.Client;

namespace SCSettingsPanel
{
    public class SCSettingsPanel : SettingsPanelPlugin
    {
        private UserControl _userControl;

        public override Guid Id { get { return new Guid("F2F511B1-808D-47D7-98E0-C2580A32B01B"); } }

        public override string Title { get { return "SettingsPanel sample"; } }

        public override void Close()
        { }

        public override void CloseUserControl()
        {
            _userControl = null;
        }

        public override UserControl GenerateUserControl()
        {
            _userControl = new SCSettingsPanelControl();
            return _userControl;
        }

        public override void Init()
        { }

        public override bool TrySaveChanges(out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}
