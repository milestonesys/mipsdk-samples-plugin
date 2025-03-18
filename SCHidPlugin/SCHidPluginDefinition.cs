using System;
using System.Collections.Generic;
using SCHidPlugin.Client;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Client.Hid;
using Image = System.Drawing.Image;

namespace SCHidPlugin
{
    public partial class SCHidPluginDefinition : PluginDefinition
    {
        internal static Guid SCHidPluginPluginId = new Guid("99d3e88c-7f23-4880-86ae-591e14155929");

        #region Private fields
        private readonly List<WorkSpaceToolbarPlugin> _workSpaceToolbarPlugins = new List<WorkSpaceToolbarPlugin>();
        private readonly List<HidPlugin> _hidPlugins = new List<HidPlugin>();

        #endregion

        /// <summary>
        /// This method is called when the environment is up and running.
        /// Register all your auto-discoverable HIDs here.
        /// Usually auto-discoverable HID plugins wrap a plug-and-play devices: USB, Bluetooth, etc.
        /// In a contrast, manual discovery HID plugins are used for devices that require manual configuration.
        /// </summary>
        public override void Init()
        {
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                // HidPlugins-property tells the environment that this MIP plugin hosts 2 HID plugins
                _hidPlugins.Add(new AutodiscoveryHidPlugin(this)); 
                _hidPlugins.Add(new ManualDiscoveryHidPlugin(this));

                _workSpaceToolbarPlugins.Add(new RootWorkspaceToolbarPlugin(_hidPlugins));
            }
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _workSpaceToolbarPlugins.ForEach(i => i.Close());
            _workSpaceToolbarPlugins.Clear();
            _hidPlugins.ForEach(i => i.Close());
            _hidPlugins.Clear();
        }
        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id => SCHidPluginPluginId;

        /// <summary>
        /// This Guid can be defined on several different IPluginDefinitions with the same value,
        /// and will result in a combination of this top level ProductNode for several plugins.
        /// Set to Guid.Empty if no sharing is enabled.
        /// </summary>
        public override Guid SharedNodeId => Guid.Empty;

        public override Image Icon { get; } = null;

        /// <summary>
        /// A product name
        /// </summary>
        public override string Name => "SCSampleHidPlugin";

        /// <summary>
        /// Company name
        /// </summary>
        public override string Manufacturer => "Milestone Systems";

        /// <summary>
        /// Version of this plugin.
        /// </summary>
        public override string VersionString => "1.0.0.0";
        #endregion


        public override List<WorkSpaceToolbarPlugin> WorkSpaceToolbarPlugins => _workSpaceToolbarPlugins;

        /// <summary>
        /// List of all Human Interface Device plugins, hosted by this MIP plugin.
        /// </summary>
        public override List<HidPlugin> HidPlugins => _hidPlugins;
    }
}
