using System;
using System.Collections.ObjectModel;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCSettingsPanel
{
    public class SCSettingsPanelPluginDefinition : PluginDefinition
    {
        public override System.Drawing.Image Icon { get { return null; } }

        public override Guid Id { get { return new Guid("BCB17495-1B1D-49D0-A304-0B2C7C0E05EC"); } }

        public override string Name { get { return "SCSettingsPanelPluginDefinition"; } }

        public override string Manufacturer { get { return PluginSamples.Common.ManufacturerName; } }

        public override Collection<SettingsPanelPlugin> SettingsPanelPlugins
        {
            get { return new Collection<SettingsPanelPlugin> { new SCSettingsPanel() }; }
        }
    }
}
