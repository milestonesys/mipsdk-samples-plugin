using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCWorkSpace.Client
{
    class SCWorkSpaceSidePanelPlugin : SidePanelPlugin
    {
        List<SidePanelPlaceDefinition> _sidePanelPlaceDefinitions = null;

        public override Guid Id
        {
            get
            {
                return new Guid("EC41997D-843A-44A5-849A-015D8A0E78F8");
            }
        }

        public override string Name
        {
            get { return "WorkSpace Plugin Side Panel"; }
        }

        public override void Init()
        {
            _sidePanelPlaceDefinitions = new List<SidePanelPlaceDefinition>();
            _sidePanelPlaceDefinitions.Add(new SidePanelPlaceDefinition() { WorkSpaceId = SCWorkSpacePlugin.SCWorkSpacePluginId, WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal, WorkSpaceState.Setup } });
            _sidePanelPlaceDefinitions.Add(new SidePanelPlaceDefinition() { WorkSpaceId = ClientControl.LiveBuildInWorkSpaceId, WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal } });
        }

        public override void Close()
        {
        }

        public override SidePanelUserControl GenerateUserControl()
        {
            return new SCWorkSpaceSidePanelUserControl() { };
        }

        public override List<SidePanelPlaceDefinition> SidePanelPlaceDefinitions
        {
            get
            {
                return _sidePanelPlaceDefinitions;
            }
        }
    }
}
