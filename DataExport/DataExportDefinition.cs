using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using DataExport.Client;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace DataExport
{
    public class DataExportDefinition : PluginDefinition
    {
        private static readonly Image _treeNodeImage;
        private Image _topTreeNodeImage;

        internal static Guid DataExportPluginId = new Guid("bd4ef35e-995e-4718-a3be-c2f006133f51");
        internal static Guid DataExportKind = new Guid("49c46102-9c13-4db9-8345-3a1f57f19450");
        internal static Guid DataExportViewItemPlugin = new Guid("6fe9607b-9efa-405c-a15e-d229435d3f0e");

        internal static string NoteNamePropertyName = "NoteName";
        internal static string ViewItemName = "DataExport";
        internal static string ExportFileExtension = ".note";

        #region Private fields

        private List<ViewItemPlugin> _viewItemPlugin = new List<ViewItemPlugin>();

        #endregion

        #region Initialization

        static DataExportDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.DataExport.bmp");
            if (pluginStream != null)
                _treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
        }


        internal static Image TreeNodeImage
        {
            get { return _treeNodeImage; }
        }

        #endregion

        public override void Init()
        {
            _topTreeNodeImage = VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx];
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                _viewItemPlugin.Add(new DataExportViewItemPlugin());
            }
        }

        public override void Close()
        {
            _viewItemPlugin.Clear();
        }

        #region Identification Properties

        public override Guid Id
        {
            get
            {
                return DataExportPluginId;
            }
        }

        public override Guid SharedNodeId
        {
            get
            {
                return PluginSamples.Common.SampleTopNode;
            }
        }

        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        public override string Name
        {
            get { return "Data Export"; }
        }

        public override string Manufacturer
        {
            get
            {
                return PluginSamples.Common.ManufacturerName;
            }
        }

        public override string VersionString
        {
            get
            {
                return "1.0.0.0";
            }
        }

        public override System.Drawing.Image Icon
        {
            get { return _topTreeNodeImage; }
        }

        #endregion

        #region Client related methods and properties

        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get { return _viewItemPlugin; }
        }

        #endregion

        #region Export related methods and properties
        public override bool IncludeInExport
        {
            get
            {
                return true;
            }
        }

        public override ExportManager GenerateExportManager(ExportParameters exportParameters)
        {
            return new DataExportManager(exportParameters);
        }
        #endregion

        /// <summary>
        /// As we have no real external data source for this sample we just use a simple dictionary for 
        /// containing our data, so that it is available for the ExportManager
        /// </summary>
        internal static Dictionary<string, string> SampleDataProvider = new Dictionary<string, string>();
    }
}
