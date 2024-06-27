using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCMessageAreaMessageTester.Client
{
    class SCMessageAreaMessageTesterSidePanelPlugin : SidePanelPlugin
    {
        List<SidePanelPlaceDefinition> _sidePanelPlaceDefinitions = null;

        public override Guid Id
        {
            get
            {
                return new Guid("FA3100D8-2C43-41A7-96EF-DC5BFCB3A6B0");
            }
        }

        public override string Name
        {
            get { return "Message Area Message Tester Plugin Side Panel"; }
        }

        public override void Init()
        {
            _sidePanelPlaceDefinitions = new List<SidePanelPlaceDefinition>();
            _sidePanelPlaceDefinitions.Add(new SidePanelPlaceDefinition() { WorkSpaceId = ClientControl.LiveBuildInWorkSpaceId, WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal } });
            _sidePanelPlaceDefinitions.Add(new SidePanelPlaceDefinition() { WorkSpaceId = ClientControl.PlaybackBuildInWorkSpaceId, WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal } });
        }

        public override void Close()
        {
        }

        public override SidePanelWpfUserControl GenerateWpfUserControl()
        {
            return new SCMessageAreaMessageTesterSidePanelWpfUserControl() { };
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
