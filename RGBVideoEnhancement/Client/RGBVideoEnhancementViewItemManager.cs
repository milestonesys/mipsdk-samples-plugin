using System;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace RGBVideoEnhancement.Client
{
    /// <summary>
    /// The ViewItemManager contains the configuration for the ViewItem. <br/>
    /// When the class is initiated it will automatically recreate relevant ViewItem configuration saved in the properties collection from earlier.
    /// Also, when the viewlayout is saved the ViewItemManager will supply current configuration to the SmartClient to be saved on the server.<br/>
    /// This class is only relevant when executing in the Smart Client.
    /// </summary>
    public class RGBVideoEnhancementViewItemManager : ViewItemManager
    {
        private Item _selectedCamera;

        public RGBVideoEnhancementViewItemManager()
            : base("RGBVideoEnhancementViewItemManager")
        {
        }

        #region Methods overridden
        /// <summary>
        /// The properties for this ViewItem is now loaded into the base class and can be accessed via 
        /// </summary>
        public override void PropertiesLoaded()
        {
            String fqidString = GetProperty(ClientControl.EmbeddedCameraFQIDProperty);
            if (!string.IsNullOrEmpty(fqidString))
            {
                FQID cameraFQID = new FQID(fqidString);
                _selectedCamera = Configuration.Instance.GetItem(cameraFQID);
            }
        }

        /// <summary>
        /// Generate the UserControl containing the actual ViewItem content.
        /// </summary>
        /// <returns></returns>
        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new RGBVideoEnhancementViewItemWpfUserControl(this);
        }

        /// <summary>
        /// Generate the UserControl containing the property configuration.
        /// </summary>
        public override PropertiesWpfUserControl GeneratePropertiesWpfUserControl()
        {
            return new RGBVideoEnhancementPropertiesWpfUserControl(this);
        }

        #endregion

        public Item SelectedCamera
        {
            get { return _selectedCamera; }
            set
            {
                _selectedCamera = value;
                SetProperty(ClientControl.EmbeddedCameraFQIDProperty, _selectedCamera.FQID.ToXmlNode().OuterXml);
                SaveProperties();
            }
        }

    }
}
