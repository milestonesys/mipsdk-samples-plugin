using System;

using VideoOS.Platform.Admin;

namespace Property.Admin
{
    class PropertyToolsOptionDialogPlugin : VideoOS.Platform.Admin.ToolsOptionsDialogPlugin 
    {
        private PropertyToolsOptionDialogUserControl _myUserControl;

        public override void Init()
        {            
            //Note: Do not try to get option settings here!
        }
        
        public override void Close()
        {
            //Note: Do not try to save option settings here!
        }

        /// <summary>
        /// saving the changes
        /// </summary>
        /// <returns></returns>
        public override bool SaveChanges()
        {
            if (_myUserControl == null) return true;
            VideoOS.Platform.Configuration.Instance.SaveOptionsConfiguration(Property.PropertyDefinition.MyPropertyId, true, Utility.ToXml("ToolOptions", _myUserControl.MyPropValue));
            return true;
        }

        public override string Name
        {
            get { return "Property";  }
        }

        public override Guid Id
        {
            get { return Property.PropertyDefinition.PropertyToolsOptionsDialog; }
        }

        public override ToolsOptionsDialogUserControl GenerateUserControl()
        {
            _myUserControl = new Property.Admin.PropertyToolsOptionDialogUserControl();
            System.Xml.XmlNode result = VideoOS.Platform.Configuration.Instance.GetOptionsConfiguration(PropertyDefinition.MyPropertyId, true);
            _myUserControl.MyPropValue = Utility.GetInnerText(result, "Empty");
            return _myUserControl;
        }
        
    }


}
