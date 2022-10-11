using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using SCToast.Client;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToast
{
	/// <summary>
	/// The PluginDefinition is the ‘entry’ point to any plugin.  
	/// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
	/// Several PluginDefinitions are allowed to be available within one DLL.
	/// Here the references to all other plugin known objects and classes are defined.
	/// The class is an abstract class where all implemented methods and properties need to be declared with override.
	/// The class is constructed when the environment is loading the DLL.
	/// </summary>
	public class SCToastDefinition : PluginDefinition
	{
		internal static Guid SCToastPluginId = new Guid("{0B27F1FA-4323-4130-AA51-7CC7782B7534}");
		internal static Guid SCToastSidePanel = new Guid("{F226317B-AA64-41C7-BC09-C224F63DAE5D}");

		#region Private fields

		//
		// Note that all the plugin are constructed during application start, and the constructors
		// should only contain code that references their own dll, e.g. resource load.

		private List<SidePanelPlugin> _sidePanelPlugins = new List<SidePanelPlugin>();

		#endregion

		/// <summary>
		/// This method is called when the environment is up and running.
		/// Registration of Messages via RegisterReceiver can be done at this point.
		/// </summary>
		public override void Init()
		{
			_sidePanelPlugins.Add(new SCToastSidePanelPlugin());
		}

		/// <summary>
		/// The main application is about to be in an undetermined state, either logging off or exiting.
		/// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
		/// </summary>
		public override void Close()
		{
			_sidePanelPlugins.Clear();
		}

		#region Identification Properties

		/// <summary>
		/// Gets the unique id identifying this plugin component
		/// </summary>
		public override Guid Id
		{
			get
			{
				return SCToastPluginId;
			}
		}

		/// <summary>
		/// Define name of top level Tree node - e.g. A product name
		/// </summary>
		public override string Name
		{
            get { return "Toast"; }
        }

        /// <summary>
        /// Your company name.
        /// </summary>
        public override string Manufacturer
        {
            get
            {
                return PluginSamples.Common.ManufacturerName;
            }
        }

        /// <summary>
        /// Icon to be used on top level - e.g. a product or company logo
        /// </summary>
        public override Image Icon
		{
			get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx]; }
		}

        #endregion


        #region Client related methods and properties

        /// <summary> 
        /// An extention plugin to add to the side panel of the Smart Client.
        /// </summary>
        public override List<SidePanelPlugin> SidePanelPlugins
		{
			get { return _sidePanelPlugins; }
		}

		#endregion

	}
}
