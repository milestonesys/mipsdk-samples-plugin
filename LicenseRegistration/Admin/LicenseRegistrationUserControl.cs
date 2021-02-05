using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.License;
using VideoOS.Platform.UI;

namespace LicenseRegistration.Admin
{
	/// <summary>
	/// This UserControl only contains a configuration of the Name for the Item.
	/// The methods and properties are used by the ItemManager, and can be changed as you see fit.
	/// </summary>
	public partial class LicenseRegistrationUserControl : UserControl
	{
		internal event EventHandler ConfigurationChangedByUser;


		public LicenseRegistrationUserControl()
		{
			InitializeComponent();
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

		internal void FillContent()
		{
			List<PluginDefinition> plugins = EnvironmentManager.Instance.AllPluginDefinitions;
			foreach (PluginDefinition plugin in plugins)
			{
				Collection<VideoOS.Platform.License.LicenseInformation> lics = plugin.PluginLicenseRequest;
				if (lics!=null && lics.Count>0)
				{
					comboBoxPlugins.Items.Add(plugin);
				}
			}

		}

		internal void UpdateItem(Item item)
		{
		}

		internal void ClearContent()
		{
		}

		private void OnSelect(object sender, EventArgs e)
		{
			PluginDefinition plugin = (PluginDefinition)comboBoxPlugins.SelectedItem;
			Collection<VideoOS.Platform.License.LicenseInformation> lics = plugin.PluginLicenseRequest;

			StringBuilder sb = new StringBuilder();
			XmlWriter xw = XmlWriter.Create(sb);
			xw.WriteStartDocument();
			xw.WriteStartElement("MIPLicenseInformation");

			xw.WriteStartElement("PluginId");
			xw.WriteString(plugin.Id.ToString());
			xw.WriteEndElement();
			xw.WriteElementString("Name", plugin.Name);

			xw.WriteStartElement("Licenses");
			foreach (LicenseInformation lic in lics)
			{
				xw.WriteStartElement("License");
				xw.WriteElementString("Name", lic.Name);
				xw.WriteElementString("Type", lic.LicenseType);
				xw.WriteEndElement();
			}
			xw.WriteEndElement();

			xw.WriteEndElement();
			xw.WriteEndDocument();
			xw.Flush();

			textBoxXML.Text = sb.ToString();
			xw.Close();

		}

		private void OnCopy(object sender, EventArgs e)
		{
			Clipboard.SetText(textBoxXML.Text);
		}

	}
}
