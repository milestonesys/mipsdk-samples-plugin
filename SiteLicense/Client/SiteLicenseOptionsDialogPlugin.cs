using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoOS.Platform.Client;

namespace SiteLicense.Client
{
	/// <summary>
	/// A class specifying an OptionsDialog plugin.<br/>
	/// <br/>
	/// Each of the OptionsDialogPlugin’s will have a line on the left hand side of the dialog with the Name defined.<br/>
	/// When the Name is selected, the CreateUserControl method is called to create the UserControl for this plugin. 
	/// The plugin should initialize the UI as part of the class construction.  <br/>
	/// The UserControl contains the entire right hand side of the panel, and will be resized to current available size.  <br/>
	/// <br/>
	/// When the SaveChanges method is called, the plugin can use the base method ‘SaveProperties’ to save the configuration, 
	/// if all configuration fields are saved in the Property dictionary.
	/// </summary>
	public class SiteLicenseOptionsDialogPlugin : OptionsDialogPlugin
	{

		/// <summary>
		/// Is method is called when the user has logged in and configuration is accessible.<br/>
		/// If a user logs out and in again, this method will be called at every login.<br/>
		/// This should be used if the plugin is accessing configuration items.
		/// </summary>
		public override void Init()
		{
		}

		/// <summary>
		/// Called by the Environment when the user log's off
		/// </summary>
		public override void Close()
		{
		}

		/// <summary>
		/// Create a UserControl to place on the options dialog.
		/// </summary>
		/// <returns></returns>
		public override OptionsDialogUserControl GenerateUserControl()
		{
			return new SiteLicenseOptionsDialogUserControl();
		}

		/// <summary>
		/// A unique ID of this plugin
		/// </summary>
		public override Guid Id
		{
			get { return SiteLicenseDefinition.SiteLicenseOptionsDialog; }
		}

		/// <summary>
		/// The name displayed on the side selection.
		/// </summary>
		public override string Name
		{
			get { return "SiteLicense Options"; }
		}

		/// <summary>
		/// Method called when you need to save the user changes.
		/// Return true for OK, and false if the "GetLastSaveError" contains an error message.
		/// </summary>
		/// <returns></returns>
		public override bool SaveChanges()
		{
			SaveProperties(true);
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
