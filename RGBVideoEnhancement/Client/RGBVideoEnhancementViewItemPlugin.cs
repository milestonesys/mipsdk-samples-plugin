using System;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace RGBVideoEnhancement.Client
{
    /// <summary>
    /// Class defining one Smart Client ViewItem.<br/>
    /// The ViewItemPlugin is only constructed one time.  
    /// For each ViewItem defined on any view layout, a call to GenerateViewItemManager is performed to create a ViewItemManager. 
    /// This instance of the ViewItemManager is then responsible for the configuration in relation to one viewitem.<br/>
    /// The ViewItemManager will again generate ViewItemUserControl and PropertiesUserControl as appropriate for this viewitem.<br/>
    /// e.g. if a given viewlayout is shown once, then only one ViewItemUserControl is generated, and only one PropertiesUserControl
    /// is generated when selected in setup mode.<br/>
    /// Now, if multiple floating windows are opened with SAME viewlayout, and thereby same configuration of a viewitem,
    /// then multiple ViewItemUserControl's may be requested via the same ViewItemManager.<br/>
    /// The properties part of the class is used to build the tree node in the Smart Client.
    /// </summary>
    public class RGBVideoEnhancementViewItemPlugin : ViewItemPlugin
	{
	    /// <summary>
		/// Called by the Environment when the user has logged in and has received configuration from the server.
		/// </summary>
		public override void Init()
	    {
	        EnvironmentManager.Instance.EnvironmentOptions[EnvironmentOptions.HardwareDecodingMode] = "auto";
	    }

		/// <summary>
		/// Called by the Environment when the user logs out.
		/// You should close all remote sessions and flush cache information, as the
		/// user might logon to another server next time.
		/// </summary>
		public override void Close()
		{
		}

		/// <summary>
		/// Gets the unique id identifying this ViewItem
		/// </summary>
		public override Guid Id
		{
			get { return RGBVideoEnhancementDefinition.RGBVideoEnhancementViewItemPlugin; }
		}

        public override VideoOSIconSourceBase IconSource { get => RGBVideoEnhancementDefinition.PluginIcon; protected set => base.IconSource = value; }

        /// <summary>
        /// The text used for a single ViewItem
        /// </summary>
        public override string Name
		{
			get { return "RGBVideoEnhancement"; }
		}

		/// <summary>
		/// Generates a ViewItemManager for managing one ViewItem. A ViewItemManager is generated for each ViewItem defined.
		/// </summary>
		/// <returns></returns>
		public override ViewItemManager GenerateViewItemManager()
		{
			return new RGBVideoEnhancementViewItemManager();
		}
	}
}
