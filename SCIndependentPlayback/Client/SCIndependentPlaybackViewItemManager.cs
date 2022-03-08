using System;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCIndependentPlayback.Client
{
	/// <summary>
	/// The ViewItemManager contains the configuration for the ViewItem. <br/>
	/// When the class is initiated it will automatically recreate relevant ViewItem configuration saved in the properties collection from earlier.
	/// Also, when the viewlayout is saved the ViewItemManager will supply current configuration to the SmartClient to be saved on the server.<br/>
	/// This class is only relevant when executing in the Smart Client.
	/// </summary>
	public class SCIndependentPlaybackViewItemManager : ViewItemManager
	{
		private Item _selectedCamera;

		public SCIndependentPlaybackViewItemManager()
			: base("SCIndependentPlaybackViewItemManager")
		{
		}

		#region Methods overridden
		/// <summary>
		/// The properties for this ViewItem is now loaded into the base class and can be accessed via 
		/// GetProperty(key) and SetProperty(key,value) methods
		/// </summary>
		public override void PropertiesLoaded()
		{
			String fqidString = GetProperty("SelectedFQID");
			if (fqidString != null && fqidString != "")
			{
				FQID cameraFQID = new FQID(fqidString);
				_selectedCamera = Configuration.Instance.GetItem(cameraFQID);
			}
		}

		/// <summary>
		/// Generate the UserControl containing the Actual ViewItem Content
		/// </summary>
		/// <returns></returns>
		public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
		{
			return new SCIndependentPlaybackViewItemWpfUserControl(this);
		}

		/// <summary>
		/// Generate the UserControl containing the property configuration.
		/// </summary>
		/// <returns></returns>
		public override PropertiesWpfUserControl GeneratePropertiesWpfUserControl()
		{
			return new SCIndependentPlaybackPropertiesWpfUserControl(this);
		}

		#endregion

		public Item SelectedCamera
		{
			get { return _selectedCamera; }
			set
			{
				_selectedCamera = value;
				SetProperty("SelectedFQID", _selectedCamera.FQID.ToXmlNode().OuterXml);
				SaveProperties();
			}
		}
	}
}
