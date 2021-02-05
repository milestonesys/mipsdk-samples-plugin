using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCAudioExport.Client
{
    /// <summary>
    /// The SidePanelPlugin defines a plugin that resides in the live or playback side panel in the Smart Client.
    /// Is it only created once during startup/login and never disposed.
    /// This class can be instantiated directly without making your own inherited class
    /// </summary>
    public class SCAudioExportSidePanelPlugin : SidePanelPlugin
    {
        private List<SidePanelPlaceDefinition> _sidePanelPlaceDefinitions = new List<SidePanelPlaceDefinition>();

        /// <summary>
        /// This method is called when the Environment is up and configuration is loaded.
        /// This method is called once every time the user logins in.
        /// </summary>
        public override void Init()
        {
            _sidePanelPlaceDefinitions.Add(new SidePanelPlaceDefinition()
            {
                WorkSpaceId = VideoOS.Platform.ClientControl.PlaybackBuildInWorkSpaceId,
                WorkSpaceStates = new List<WorkSpaceState>() { VideoOS.Platform.WorkSpaceState.Normal }
            });
        }

        /// <summary>
        /// Flush any configuration or dynamic resources.
        /// </summary>
        public override void Close()
        {
        }

        /// <summary>
        /// Creates a new UserControl to be placed on the specified panel place.
        /// Size of this panel is limited and can not be wider than 188 pixels.
        /// </summary>
        /// <returns></returns>
        public override SidePanelWpfUserControl GenerateWpfUserControl()
        {
            return new SCAudioExportSidePanelWpfUserControl();
        }

        /// <summary>
        /// Identification of this SidePanel
        /// </summary>
        public override Guid Id
        {
            get { return SCAudioExportDefinition.SCAudioExportSidePanel; }
        }

        /// <summary>
        /// Name of panel - displayed on top of user control
        /// </summary>
        public override string Name
        {
            get { return "SCAudioExport Side panel"; }
        }

        /// <summary>
        /// Where to place this panel.
        /// </summary>
        public override List<SidePanelPlaceDefinition> SidePanelPlaceDefinitions
        {
            get
            {
                return _sidePanelPlaceDefinitions;
            }
        }

    }
}
