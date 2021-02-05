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
		/// GetProperty(key) and SetProperty(key,value) methods
		/// </summary>
		public override void PropertiesLoaded()
		{
			//String fqidString = GetProperty("SelectedFQID");
			String fqidString = GetProperty(ClientControl.EmbeddedCameraFQIDProperty);
			if (!string.IsNullOrEmpty(fqidString))
			{
				FQID cameraFQID = new FQID(fqidString);
				_selectedCamera = Configuration.Instance.GetItem(cameraFQID);
			}
		}

		/// <summary>
		/// Generate the UserControl containing the Actual ViewItem Content
		/// </summary>
		/// <returns></returns>
		public override ViewItemUserControl GenerateViewItemUserControl()
		{
			return new RGBVideoEnhancementViewItemUserControl(this);
		}

		/// <summary>
		/// Generate the UserControl containing the property configuration.
		/// </summary>
		/// <returns></returns>
		public override PropertiesUserControl GeneratePropertiesUserControl()
		{
			return new RGBVideoEnhancementPropertiesUserControl(this);
		}

		#endregion

		public Item SelectedCamera
		{
			get { return _selectedCamera; }
			set
			{
				_selectedCamera = value;
				//SetProperty("SelectedFQID", _selectedCamera.FQID.ToXmlNode().OuterXml);  // Changed to below to support TimeLine and Export process
				SetProperty(ClientControl.EmbeddedCameraFQIDProperty, _selectedCamera.FQID.ToXmlNode().OuterXml);
				SaveProperties();
			}
		}

	}
}
