using System;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.License;
using VideoOS.Platform.UI;

namespace ServerConnection.Admin
{
	/// <summary>
	/// This UserControl only contains a configuration of the Name for the Item.
	/// The methods and properties are used by the ItemManager, and can be changed as you see fit.
	/// </summary>
	public partial class ServerConnectionUserControl : UserControl
	{
		internal event EventHandler ConfigurationChangedByUser;


		public ServerConnectionUserControl()
		{
			InitializeComponent();
		}

		internal String DisplayName
		{
			get { return textBoxName.Text; }
			set { textBoxName.Text = value; }
		}

		/// <summary>
		/// Ensure that all user entries will call this method to enable the Save button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void OnUserChange(object sender, EventArgs e)
		{
			if (ConfigurationChangedByUser != null)
				ConfigurationChangedByUser(this, new EventArgs());
		}

		internal void FillContent(Item item)
		{
			textBoxName.Text = item.Name;
			LicenseItem li = null;
			LicenseInformation licenseInformation =
				EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.GetLicenseInformation(
					ServerConnectionDefinition.ServerConnectionPluginId, ServerConnectionDefinition.ServerLicenseId);
			if (licenseInformation!=null)
				li = licenseInformation.GetConfiguredLicenseItem(item.FQID.ObjectId.ToString());

			if (li != null)
			{
				if (licenseInformation.Expire.Year!=9999 && licenseInformation.Expire < li.ItemTrialEnd)
				{
					// Check for entire licens to expire
					if (licenseInformation.Expire < DateTime.UtcNow)
						textBoxLicenseInfo.Text = "License expired on " + licenseInformation.Expire.ToLongDateString();
					else
						textBoxLicenseInfo.Text = "License will expire on " + licenseInformation.Expire.ToLongDateString();
				}
				else
				{
					// Check for each item to expire
					if (li.ItemTrial == false)
						textBoxLicenseInfo.Text = "Licensed";
					else
						textBoxLicenseInfo.Text = "Trial until: " + li.ItemTrialEnd.ToLongDateString();
				}
			}

		}

		internal void UpdateItem(Item item)
		{
			item.Name = DisplayName;
			// Fill in any propertuies that should be saved:
			//item.Properties["AKey"] = "some value";
		}

		internal void ClearContent()
		{
			textBoxName.Text = "";
		}

	}
}
