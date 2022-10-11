using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DynamicView.Background;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;

namespace DynamicView
{
    /// <summary>
    /// This sample listens to the MIP message sent when an alarm is selected in the alarm list.
    /// It then creates a new floating window with a layout containing the cameras in the alarm.
    /// </summary>
    public class DynamicViewDefinition : PluginDefinition
    {
        private static System.Drawing.Image _treeNodeImage;

        internal static Guid DynamicViewPluginId = new Guid("5A428269-B268-421E-BA94-FB2E941CCCDF");
        internal static Guid DynamicViewBackgroundPlugin = new Guid("216CC523-CA0A-4D14-BCE9-4089BE64D374");

        #region Private fields
        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static DynamicViewDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.DynamicView.bmp");
            if (pluginStream != null)
            {
                _treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
            }
        }


        /// <summary>
        /// Get the icon for the plugin
        /// </summary>
        internal static Image TreeNodeImage
        {
            get { return _treeNodeImage; }
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
                return DynamicViewPluginId;
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
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "Dynamic View"; }
        }

        /// <summary>
        /// Top level name
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
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

        /// <summary>
        /// Icon to be used on top level - e.g. a product or company logo
        /// </summary>
        public override System.Drawing.Image Icon
        {
            get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx]; }
        }

        #endregion
        
        #region Administration properties

        /// <summary>
        /// A list of server side configuration items in the administrator
        /// </summary>
        public override List<ItemNode> ItemNodes
        {
            get { return null; }
        }

        /// <summary>
        /// A user control to display when the administrator clicks on the top TreeNode
        /// </summary>
        public override UserControl GenerateUserControl()
        {
            return null;
        }

        /// <summary>
        /// This property can be set to true, to be able to display your own help UserControl on the entire panel.
        /// When this is false - a standard top and left side is added by the system.
        /// </summary>
        public override bool UserControlFillEntirePanel
        {
            get { return false; }
        }
        #endregion

        #region Client related methods and properties

        /// <summary>
        /// Create and returns the background task.
        /// </summary>
        public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
        {
            get { return new List<VideoOS.Platform.Background.BackgroundPlugin>() { new DynamicViewBackgroundPlugin() }; }
        }

        #endregion

    }
}
