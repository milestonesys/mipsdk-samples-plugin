using System;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    class SetViewItemBackgroundColorWorkspaceToolbarPluginGroup : WorkSpaceToolbarPluginGroup
    {
        public static readonly Guid PluginGroupId = new Guid("1E504014-2349-4B0D-BDBE-286BA23B383B");

        public SetViewItemBackgroundColorWorkspaceToolbarPluginGroup()
        {
            // In a real-world scenario, localized strings based on CultureInfo.CurrentUICulture
            // should be provided here.
            Title = "Colors";
            Tooltip = "Change background color of all the view items.";
        }

        public override Guid Id
        {
            get { return PluginGroupId; }
        }
    }
}
