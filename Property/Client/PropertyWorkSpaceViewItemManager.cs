using System;
using VideoOS.Platform.Client;

namespace Property.Client
{
    public class PropertyWorkSpaceViewItemManager : ViewItemManager
    {
        internal const string WorkspacePropertyKey = "MyProp";
        private string _gloProp;
        private string _priProp;

        public PropertyWorkSpaceViewItemManager()
            : base("PropertyWorkSpaceViewItemManager")
        {
        }
        public override void PropertiesLoaded()
        {
        }

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new PropertyWorkSpaceViewItemWpfUserControl(this);
        }

        public string MyPropValue
        {
            set { SetProperty(WorkspacePropertyKey, value ?? ""); SaveProperties(); }
            get { return GetProperty(WorkspacePropertyKey); }
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
    }
}
