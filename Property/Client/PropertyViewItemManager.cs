using System;
using VideoOS.Platform.Client;

namespace Property.Client
{
    public class PropertyViewItemManager : ViewItemManager
    {
        private string _myProp;
        private string _gloProp;
        private string _priProp;

        public PropertyViewItemManager()
            : base("PropertyViewItemManager")
        {
        }

        #region Methods overridden

        /// <summary>
        /// The properties for this ViewItem is now loaded into the base class and can be accessed via 
        /// GetProperty(key) and SetProperty(key,value) methods
        /// </summary>
        public override void PropertiesLoaded()
        {
            _myProp = GetProperty("MyProp");
        }

        /// <summary>
        /// Generate the UserControl containing the Actual ViewItem Content
        /// </summary>
        /// <returns></returns>
        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new PropertyViewItemWpfUserControl(this);
        }

        /// <summary>
        /// Generate the UserControl containing the property configuration.
        /// </summary>
        /// <returns></returns>
        public override PropertiesWpfUserControl GeneratePropertiesWpfUserControl()
        {
            return new PropertyPropertiesWpfUserControl(this);
        }

        #endregion


        public string MyPropValue
        {
            set
            {
                _myProp = value ?? "";
                SetProperty("MyProp", _myProp);
                SaveProperties();
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
    }
}
