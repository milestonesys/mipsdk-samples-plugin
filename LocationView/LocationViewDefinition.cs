using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using LocationView.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace LocationView
{
	public class LocationViewDefinition : PluginDefinition
	{
		private readonly static VideoOSIconSourceBase _treeNodeImage;

        internal static Guid LocationViewPluginId = new Guid("527F292F-B321-4487-9493-03DBD51D4231");
        internal static Guid LocationViewKind = new Guid("41E20EC4-2798-486B-B4E2-97ED81E10BEC");

	    internal static string GMapProviderType = "GMapProviderType";

        static LocationViewDefinition()
		{
            var packString = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/LocationView.png");
            _treeNodeImage = new VideoOSIconUriSource() { Uri = new Uri(packString) };
        }

        internal static VideoOSIconSourceBase TreeNodeImage => _treeNodeImage;

        public override Guid Id
		{
			get
			{
				return LocationViewPluginId;
			}
		}

		public override Guid SharedNodeId
		{
			get
			{
				return PluginSamples.Common.SampleTopNode;
			}
		}

		public override string Name
		{
            get { return "LocationView"; }
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
				return "3.0.0.0";
			}
		}

		public override System.Drawing.Image Icon
		{
			get { return VideoOS.Platform.UI.Util.ImageList.Images[VideoOS.Platform.UI.Util.PluginIx]; }
		}

		public override UserControl GenerateUserControl()
		{
		    return null;
		}

        public override bool IncludeInExport
        {
            get
            {
                return true;
            }
        }

		public override List<ViewItemPlugin> ViewItemPlugins
		{
			get
			{
				return new List<ViewItemPlugin> { new LocationViewViewItemPlugin() };
			}
		}

		public override List<ItemNode> ItemNodes
		{
			get
			{
			    return null;
			}
		}
	}
}
