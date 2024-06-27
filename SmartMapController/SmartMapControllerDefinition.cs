using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using SmartMapController.Client;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace SmartMapController
{
    /// <summary>
    /// This sample is a Smart Client view item plugin. It demonstrates how to use Message Communication to navigate
    /// Smart Map and listen to the information of the Smart Map position changes.
    /// </summary>
    public class SmartMapControllerDefinition : PluginDefinition
    {
        private static readonly VideoOSIconSourceBase _pluginIcon;
        internal static Guid SmartMapControllerPluginId = new Guid("4b8e9e95-1443-4193-9332-619a9ecfb0e9");
        internal static Guid SmartMapControllerKind = new Guid("49eaf8d2-eb12-4b2c-9197-82ff42fb5b90");
        internal static Guid SmartMapControllerViewItemPlugin = new Guid("b4957ad3-edc3-4c67-b8ff-66ae05e2bb98");

        #region Private fields

        //
        // Note that all the plugin are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.
        private List<ViewItemPlugin> _viewItemPlugins = new List<ViewItemPlugin>();

        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static SmartMapControllerDefinition()
        {
            var packString = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/DummyItem.bmp");
            _pluginIcon = new VideoOSIconUriSource() { Uri = new Uri(packString) };
        }

        internal static VideoOSIconSourceBase PluginIcon => _pluginIcon;

        /// <summary>
        /// This method is called when the environment is up and running.
        /// Registration of Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {
            // Populate all relevant lists with your plugins etc.
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                _viewItemPlugins.Add(new SmartMapControllerViewItemPlugin());
            }
        }
        #endregion

        #region Cleanup
        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _viewItemPlugins.Clear();
        }
        #endregion

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get
            {
                return SmartMapControllerPluginId;
            }
        }

        /// <summary>
        /// This Guid can be defined on several different IPluginDefinitions with the same value,
        /// and will result in a combination of this top level ProductNode for several plugins.
        /// Set to Guid.Empty if no sharing is enabled.
        /// </summary>
        public override Guid SharedNodeId
        {
            get
            {
                return PluginSamples.Common.SampleTopNode;
            }
        }

        /// <summary>
        /// Name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "SmartMapController"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        /// <summary>
        /// Company name
        /// </summary>
        public override string Manufacturer
        {
            get
            {
                return PluginSamples.Common.ManufacturerName;
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

        /// <summary>
        /// Icon to be used on top level - e.g. a product or company logo
        /// </summary>
        public override System.Drawing.Image Icon
        {
            get { return null; }
        }
        #endregion

        #region Client related methods and properties

        /// <summary>
        /// A list of Client side definitions for Smart Client
        /// </summary>
        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get { return _viewItemPlugins; }
        }
        #endregion

    }
}
