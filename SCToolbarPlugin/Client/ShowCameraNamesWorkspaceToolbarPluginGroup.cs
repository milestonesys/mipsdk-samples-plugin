using System;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    class ShowCameraNamesWorkspaceToolbarPluginGroup : WorkSpaceToolbarPluginGroup
    {
        public static readonly Guid PluginGroupId = new Guid("09AB51AD-6F9E-4954-9940-28FC98A771BA");

        public ShowCameraNamesWorkspaceToolbarPluginGroup()
        {
            // In a real-world scenario, localized strings based on CultureInfo.CurrentUICulture
            // should be provided here.
            Title = "Camera names";
            Tooltip = "Show the names of the cameras in the view.";
        }

        public override Guid Id
        {
            get { return PluginGroupId; }
        }
    }
}
