using System;
using VideoOS.Platform.Client;

namespace Property.Client
{
    /// <summary>
    /// </summary>
    public class PropertyOptionsDialogPlugin : OptionsDialogPlugin
    {
        private PropertyOptionsDialogUserControl _myUserControl;

        /// <summary>
        /// </summary>
        public override void Init()
        {            
        }

        /// <summary>
        /// Called by the Environment when the user log's off
        /// </summary>
        public override void Close()
        {
            if (_myUserControl!=null)
                _myUserControl.Dispose();
            _myUserControl = null;
        }

        /// <summary>
        /// Create a UserControl to place on the options dialog.
        /// </summary>
        /// <returns></returns>
        public override OptionsDialogUserControl GenerateUserControl()
        {
            // Get hold of the default set of properties for this plugin (Saved under Id='PropertyDefinition.PropertyOptionsDialog')
            LoadProperties(true);
            _myUserControl = new PropertyOptionsDialogUserControl() {MyPropValue = GetProperty("MyProp")};

            // The following lines show how to access other set of properties, saved under id='PropertyDefinition.MyPropertyId'

            // GetOptionsConfiguration - Global
            System.Xml.XmlNode result = VideoOS.Platform.Configuration.Instance.GetOptionsConfiguration(PropertyDefinition.MyPropertyId, false);
            _myUserControl.MyPropShareGlobal = Utility.GetInnerText(result, "Empty");

            // GetOptionsConfiguration - Private
            result = VideoOS.Platform.Configuration.Instance.GetOptionsConfiguration(PropertyDefinition.MyPropertyId, true);
            _myUserControl.MyPropSharePrivate = Utility.GetInnerText(result, "Empty");
            return _myUserControl;
        }

        /// <summary>
        /// A unique ID of this plugin
        /// </summary>
        public override Guid Id
        {
            get { return PropertyDefinition.PropertyOptionsDialog; }
        }

        /// <summary>
        /// The name displayed on the side selection.
        /// </summary>
        public override string Name
        {
            get { return "Property Options"; }
        }

        /// <summary>
        /// Method called when you need to save the user changes.
        /// Return true for OK, and false if the "GetLastSaveError" contains an error message.
        /// </summary>
        /// <returns></returns>
        public override bool SaveChanges()
        {
            if (_myUserControl == null) return true;   

            // SetProperty - maintains a dictionary of properties (default implementation)
            SetProperty("MyProp", _myUserControl.MyPropValue);
            SaveProperties(true);

            // Below is for sample only.  Normally the above default properties should be used.
            // SaveOptionsConfiguration - Global
            VideoOS.Platform.Configuration.Instance.SaveOptionsConfiguration(
                Property.PropertyDefinition.MyPropertyId, false, Utility.ToXml("SharedProperty", _myUserControl.MyPropShareGlobal));

            // SaveOptionsConfiguration - Private
            VideoOS.Platform.Configuration.Instance.SaveOptionsConfiguration(
                Property.PropertyDefinition.MyPropertyId, true, Utility.ToXml("SharedProperty", _myUserControl.MyPropSharePrivate));

            PropertyDefinition.OnSharedPropertyChange(this, EventArgs.Empty);

            return true;
        }

        /// <summary>
        /// Returns the last save error. An empty string is returned if no error is available.
        /// </summary>
        /// <returns>The last save error</returns>
        public override string GetLastSaveError()
        {
            return "";
        }

    }
}
