using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace Property.Client
{
    public class PropertySidePanelPlugin : SidePanelPlugin
    {
        private string _myProp;
        private string _priProp;
        private string _gloProp;

        /// <summary>
        /// This method is called when the Environment is up and configuration is loaded.
        /// This method is called once every time the user logins in.
        /// </summary>
        public override void Init()
        {
            LoadProperties(true);
            MyPropValue = GetProperty("MyProp");
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
            return new PropertySidePanelWpfUserControl(this);
        }

        /// <summary>
        /// Identification of this SidePanel
        /// </summary>
        public override Guid Id
        {
            get { return PropertyDefinition.PropertySidePanel; }
        }

        /// <summary>
        /// Name of panel - displayed on top of user control
        /// </summary>
        public override string Name
        {
            get { return "Property Side panel"; }
        }

        public string MyPropValue
        {
            set
            {
                _myProp = value ?? "";
                SetProperty("MyProp", _myProp);
                SaveProperties(true);
            }
            get { return _myProp; }
        }
        public string MyPropShareGlobal
        {
            set
            {
                _gloProp = value ?? "";
                // SaveOptionsConfiguration - Global
                VideoOS.Platform.Configuration.Instance.SaveOptionsConfiguration(
                    Property.PropertyDefinition.MyPropertyId, false, Utility.ToXml("SharedProperty", _gloProp));
                PropertyDefinition.OnSharedPropertyChange(this, EventArgs.Empty);
            }
            get
            {
                System.Xml.XmlNode result = VideoOS.Platform.Configuration.Instance.GetOptionsConfiguration(PropertyDefinition.MyPropertyId, false);
                _gloProp = Utility.GetInnerText(result, "Empty");
                return _gloProp;
            }
        }

        public string MyPropSharePrivate
        {
            set
            {
                _priProp = value ?? "";
                // SaveOptionsConfiguration - Private
                VideoOS.Platform.Configuration.Instance.SaveOptionsConfiguration(
                    Property.PropertyDefinition.MyPropertyId, true, Utility.ToXml("PrivateProperty", _priProp));
                PropertyDefinition.OnSharedPropertyChange(this, EventArgs.Empty);
            }
            get
            { // GetOptionsConfiguration - Private
                System.Xml.XmlNode result = VideoOS.Platform.Configuration.Instance.GetOptionsConfiguration(PropertyDefinition.MyPropertyId, true);
                _priProp = Utility.GetInnerText(result, "Empty");
                return _priProp;
            }
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
                    } ,
                    new SidePanelPlaceDefinition() {
                        WorkSpaceId = VideoOS.Platform.ClientControl.PlaybackBuildInWorkSpaceId,
                        WorkSpaceStates = new List<WorkSpaceState>() { VideoOS.Platform.WorkSpaceState.Normal }
                    }
                };
            }
        }
    }
}
