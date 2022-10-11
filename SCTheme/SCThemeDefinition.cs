using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using SCTheme.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;

namespace SCTheme
{
    public class SCThemeDefinition : PluginDefinition
    {
        #region Private fields

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
            _viewItemPlugins.Add(new SCThemeViewItemPlugin());
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _viewItemPlugins.Clear();
        }

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return new Guid("70A438F8-EB18-419A-A87B-D8DAA52478D4"); }
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
            get { return "SCTheme"; }
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
            get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx]; }
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
            get { return null; }
        }

        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get { return _viewItemPlugins; }
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
