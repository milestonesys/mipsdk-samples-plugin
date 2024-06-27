using System;
using System.Windows.Controls;
using VideoOS.Platform.Client;

namespace Property.Client
{
    public class PropertySettingsPanelPlugin : SettingsPanelPlugin
    {
        private PropertySettingsPanelUserControl _myUserControl;

        public override void Init()
        {
        }

        /// <summary>
        /// Called by the Environment when the user log's off
        /// </summary>
        public override void Close()
        {
            if (_myUserControl!=null)
                _myUserControl = null;
        }

        /// <summary>
        /// Create a UserControl to place on the settings panel.
        /// </summary>
        public override UserControl GenerateUserControl()
        {
            // Get hold of the default set of properties for this plugin (Saved under Id='PropertyDefinition.PropertyOptionsDialog')
            LoadProperties(true);
            _myUserControl = new PropertySettingsPanelUserControl() { MyPropValue = GetProperty("MyProp") };

            // The following lines show how to access other set of properties, saved under id='PropertyDefinition.MyPropertyId'

            // GetOptionsConfiguration - Global
            System.Xml.XmlNode result = VideoOS.Platform.Configuration.Instance.GetOptionsConfiguration(PropertyDefinition.MyPropertyId, false);
            _myUserControl.MyPropSharedGlobal = Utility.GetInnerText(result, "Empty");

            // GetOptionsConfiguration - Private
            result = VideoOS.Platform.Configuration.Instance.GetOptionsConfiguration(PropertyDefinition.MyPropertyId, true);
            _myUserControl.MyPropSharedPrivate = Utility.GetInnerText(result, "Empty");
            return _myUserControl;
        }

        /// <summary>
        /// A unique ID of this plugin
        /// </summary>
        public override Guid Id
        {
            get { return PropertyDefinition.PropertySettingsPanel; }
        }

        /// <summary>
        /// The name displayed on the side selection.
        /// </summary>
        public override string Title
        {
            get { return "Property Settings"; }
        }

        public override void CloseUserControl()
        {
            _myUserControl = null;
        }

        public override bool TrySaveChanges(out string errorMessage)
        {
            if (_myUserControl == null)
            {
                errorMessage = "";
                return true;
            }

            try
            {
            // SetProperty - maintains a dictionary of properties (default implementation)
            SetProperty("MyProp", _myUserControl.MyPropValue);
            SaveProperties(true);

            // Below is for sample only.  Normally the above default properties should be used.
            // SaveOptionsConfiguration - Global
            VideoOS.Platform.Configuration.Instance.SaveOptionsConfiguration(
                Property.PropertyDefinition.MyPropertyId, false, Utility.ToXml("SharedProperty", _myUserControl.MyPropSharedGlobal));
            
            // SaveOptionsConfiguration - Private
            VideoOS.Platform.Configuration.Instance.SaveOptionsConfiguration(
                Property.PropertyDefinition.MyPropertyId, true, Utility.ToXml("SharedProperty", _myUserControl.MyPropSharedPrivate));
            }
            catch (Exception) 
            {
                errorMessage = "Error occurred";
                return false;
            }
            
            PropertyDefinition.OnSharedPropertyChange(this, EventArgs.Empty);
            errorMessage = "";
            return true;
        }
    }
}
