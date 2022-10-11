using PluginSamples;
using SCMessageAreaMessageTester.Client;
using System;
using System.Collections.Generic;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCMessageAreaMessageTester
{
    public class SCMessageAreaMessageTesterDefinition : PluginDefinition
	{
        #region Private fields

        private List<SidePanelPlugin> _sidePanelPlugins = new List<SidePanelPlugin>();

		#endregion

		#region Initialization

		#endregion

		/// <summary>
		/// This method is called when the environment is up and running.
		/// Registration of Messages via RegisterReceiver can be done at this point.
		/// </summary>
		public override void Init()
		{
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
			{
                _sidePanelPlugins.Add(new SCMessageAreaMessageTesterSidePanelPlugin());
			}
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
            get { return new Guid("1584246E-B496-4F1C-991D-F503B188EE87"); }
		}

	    public override Guid SharedNodeId
	    {
            get { return Common.SampleTopNode; }
	    }

	    /// <summary>
		/// Define name of top level Tree node - e.g. A product name
		/// </summary>
		public override string Name
		{
            get { return "SCMessageAreaMessageTester"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return Common.SampleNodeName; }
		}

	    public override System.Drawing.Image Icon
	    {
			get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx]; }
	    }

	    /// <summary>
		/// Your company name
		/// </summary>
		public override string Manufacturer
		{
			get
			{
				return Common.ManufacturerName;
            }
		}

	    /// <summary>
		/// Version of this plugin.
		/// </summary>
		public override string VersionString
		{
			get
			{
				return "1.0.0.0";
			}
		}

		#endregion


		#region Client related methods and properties

        public override List<SidePanelPlugin> SidePanelPlugins
        {
            get { return _sidePanelPlugins; }
        }

        #endregion
    }
}
