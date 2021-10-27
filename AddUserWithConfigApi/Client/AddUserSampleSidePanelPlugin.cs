using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace AddUserSample.Client
{
    /// <summary>
    /// The SidePanelPlugin defines a plugin that resides in the live or playback side panel in the Smart Client.
    /// It is only created once during startup/login and never disposed.
    /// </summary>
    public class AddUserSampleSidePanelPlugin : SidePanelPlugin
    {
        /// <summary>
        /// This method is called when the Environment is ready and configuration is loaded.
        /// This method is called once every time the user logins in.
        /// </summary>
        public override void Init()
        {
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
        public override SidePanelUserControl GenerateUserControl()
        {
            return new AddUserSampleSidePanelUserControl();
        }

        /// <summary>
        /// Identification of this SidePanel
        /// </summary>
        public override Guid Id
        {
            get { return AddUserSampleDefinition.AddUserSampleSidePanel; }
        }

        /// <summary>
        /// Name of panel - displayed on top of user control
        /// </summary>
        public override string Name
        {
            get { return "Add User Side panel"; }
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
