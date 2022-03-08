using SCImageViewerAddOnSample.Background;
using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCImageViewerAddOnSample.Client
{
    /// <summary>
    /// The SidePanelPlugin defines a plugin that resides in the live or playback side panel in the Smart Client.
    /// Is it only created once during startup/login and never disposed.
    /// This class can be instantiated directly without making your own inherited class
    /// </summary>
    public class SCImageViewerAddOnSampleSidePanelPlugin : SidePanelPlugin
    {
        private SCImageViewerAddOnSampleBackgroundPlugin _backgroundPlugin;

        public SCImageViewerAddOnSampleSidePanelPlugin(SCImageViewerAddOnSampleBackgroundPlugin backgroundPlugin)
        {
            _backgroundPlugin = backgroundPlugin;
        }

        /// <summary>
        /// This method is called when the Environment is up and configuration is loaded.
        /// This method is called once every time the user logins in.
        /// </summary>
        public override void Init()
        {
        }

        /// <summary>
        /// Called by the Environment when the user log's out.
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
            return new SCImageViewerAddOnSampleSidePanelWpfUserControl(_backgroundPlugin);
        }

        /// <summary>
        /// Identification of this SidePanel
        /// </summary>
        public override Guid Id
        {
            get { return SCImageViewerAddOnSampleDefinition.SCImageViewerAddOnSampleSidePanel; }
        }


        /// <summary>
        /// Name of panel - displayed on top of user control
        /// </summary>
        public override string Name
        {
            get { return "SCImageViewerAddOnSample"; }
        }

        /// <summary>
        /// Where to place this panel.
        /// </summary>
        public override List<SidePanelPlaceDefinition> SidePanelPlaceDefinitions
        {
            get
            {
                return new List<SidePanelPlaceDefinition>() {
                    new SidePanelPlaceDefinition() {
                        WorkSpaceId = VideoOS.Platform.ClientControl.LiveBuildInWorkSpaceId,
                        WorkSpaceStates = new List<WorkSpaceState>() { VideoOS.Platform.WorkSpaceState.Normal }
                    }
                };
            }
        }

    }
}
