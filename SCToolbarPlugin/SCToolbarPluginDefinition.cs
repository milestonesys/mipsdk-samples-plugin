using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SCToolbarPlugin.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin
{
    public class SCToolbarPluginDefinition : PluginDefinition
    {
        #region Private fields

        public class ColorMessageData
        {
            public Color Color { get; set; }
            public FQID ViewItemInstanceFQID { get; set; }
            public FQID WindowFQID { get; set; }
        }

        internal static String SetViewItemBackgroundColor = "SCToolbarPluginSample.SetViewItemBackgroundColor";
        internal static String ViewItemBackgroundColorChanged = "SCToolbarPluginSample.ViewItemBackgroundColorChanged";

        //
        // Note that all the plugin are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.

        private List<ViewItemToolbarPlugin> _viewItemToolbarPlugins = new List<ViewItemToolbarPlugin>();
        private List<WorkSpaceToolbarPlugin> _workSpaceToolbarPlugins = new List<WorkSpaceToolbarPlugin>();
        private List<ViewItemPlugin> _viewItemPlugins = new List<ViewItemPlugin>();

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
                _viewItemToolbarPlugins.Add(new ShowCameraNameViewItemToolbarPlugin());
                _viewItemToolbarPlugins.Add(new SetViewItemBackgroundColorViewItemToolbarPlugin(Color.Red));
                _viewItemToolbarPlugins.Add(new SetViewItemBackgroundColorViewItemToolbarPlugin(Color.Green));
                _viewItemToolbarPlugins.Add(new SetViewItemBackgroundColorViewItemToolbarPlugin(Color.Blue));

                _workSpaceToolbarPlugins.Add(new SetViewItemBackgroundColorWorkSpaceToolbarPlugin(Color.Red));
                _workSpaceToolbarPlugins.Add(new SetViewItemBackgroundColorWorkSpaceToolbarPlugin(Color.Green));
                _workSpaceToolbarPlugins.Add(new SetViewItemBackgroundColorWorkSpaceToolbarPlugin(Color.Blue));

                _viewItemPlugins.Add(new BackgroundColorViewItemPlugin());
            }
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _viewItemToolbarPlugins.Clear();
            _workSpaceToolbarPlugins.Clear();
            _viewItemPlugins.Clear();
        }

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return new Guid("5D5BA2B0-DE37-4DF4-937D-729E780D5082"); }
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
            get { return "SCToolbar"; }
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

        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get { return _viewItemPlugins; }
        }

        public override List<ViewItemToolbarPlugin> ViewItemToolbarPlugins
        {
            get { return _viewItemToolbarPlugins; }
        }

        public override List<WorkSpaceToolbarPlugin> WorkSpaceToolbarPlugins
        {
            get { return _workSpaceToolbarPlugins; }
        }

        #endregion

        public override UserControl GenerateUserControl()
        {
            return null;
        }

        public override List<ItemNode> ItemNodes
        {
            get { return new List<ItemNode>(); }
        }
    }
}
