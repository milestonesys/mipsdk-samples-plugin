using SCWorkSpace.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCWorkSpace
{
    public class SCWorkSpaceDefinition : PluginDefinition
    {
        #region Private fields

        internal static string ShuffleCamerasMessage = "SCWorkSpacePlugin.ShuffleCameras";
        internal static string MaxCamerasChangedMessage = "SCWorkSpacePlugin.MaxCamerasChanged";
        internal static string MoveViewItemMessage = "SCWorkSpacePlugin.MoveViewItemMessage";


        internal static int MaxCameras = 10;

        //
        // Note that all the plugin are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.

        private List<WorkSpacePlugin> _workSpacePlugins = new List<WorkSpacePlugin>();
        private List<ViewItemPlugin> _workSpaceViewItemPlugins = new List<ViewItemPlugin>();
        private List<SidePanelPlugin> _workSpaceSidePanelPlugins = new List<SidePanelPlugin>();

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
                _workSpacePlugins.Add(new SCWorkSpacePlugin());
                _workSpaceViewItemPlugins.Add(new SCWorkSpaceViewItemPlugin());
                _workSpaceViewItemPlugins.Add(new SCWorkSpaceViewItemPlugin2());
                _workSpaceSidePanelPlugins.Add(new SCWorkSpaceSidePanelPlugin());
            }
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _workSpacePlugins.Clear();
            _workSpaceViewItemPlugins.Clear();
            _workSpaceSidePanelPlugins.Clear();
        }

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return new Guid("6BB5B0AD-D567-4060-B806-5C3C807B500C"); }
        }

        public override Guid SharedNodeId
        {
            get { return PluginSamples.Common.SampleTopNode; }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "SCWorkSpace"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        public override Image Icon
        {
            get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.SDK_VSIx]; }
        }

        /// <summary>
        /// Your company name
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

        #endregion


        #region Client related methods and properties

        public override List<WorkSpacePlugin> WorkSpacePlugins
        {
            get { return _workSpacePlugins; }
        }

        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get { return _workSpaceViewItemPlugins; }
        }

        public override List<SidePanelPlugin> SidePanelPlugins
        {
            get { return _workSpaceSidePanelPlugins; }
        }

        #endregion

    }
}
