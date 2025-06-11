using AdminMultiTab.Admin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace AdminMultiTab
{
    /// <summary>
    /// The PluginDefinition for our admin plugin
    /// </summary>
    public class AdminMultiTabDefinition : PluginDefinition
		{
				private static readonly Image _treeNodeImage;
				private static readonly Image _topTreeNodeImage;

				internal static readonly Uri DummyImagePackUri;

				internal static Guid AdminMultiTabPluginId = new Guid("d3f67746-c64e-4555-ab5a-db033b66bd1a");
				internal static Guid AdminMultiTabKind = new Guid("1e2ce2c9-a0f7-4945-b390-8bf6ab3f9bca");
				internal static Guid AdminMultiTabItemId = new Guid("672626C9-2B8B-4053-AC3F-5F6D9BAA757C");

        #region Private fields

				private List<ItemNode> _itemNodes = new List<ItemNode>();				
			
				#endregion

				#region Initialization

				/// <summary>
				/// Load resources 
				/// </summary>
				static AdminMultiTabDefinition()
				{
						_topTreeNodeImage = Properties.Resources.Server;
						DummyImagePackUri = new Uri(string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/DummyItem.png"));						
						_treeNodeImage = ResourceToImage(DummyImagePackUri);						
        }

				/// <summary>
				/// WPF requires resources to be stored with Build Action=Resource, which unfortunately cannot easily be read for WinForms controls, so we use this small
				/// utility method
				/// </summary>
				/// <param name="imageUri">Pack URI pointing to the image <seealso cref="https://learn.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf"/></param>
				/// <returns></returns>
				private static Image ResourceToImage(Uri imageUri)
				{
						var bitmapImage = new BitmapImage(imageUri);
						var encoder = new PngBitmapEncoder();
						encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
						using (var stream = new MemoryStream())
						{
								encoder.Save(stream);
								stream.Flush();
								return new Bitmap(stream);
						}
				}

				/// <summary>
				/// Get the icon for the plugin in WinForms format
				/// </summary>
				internal static Image TreeNodeImage => _treeNodeImage;

				#endregion

				/// <summary>
				/// This method is called when the environment is up and running.				
				/// </summary>
				public override void Init()
				{						
						_itemNodes.Add(new ItemNode(AdminMultiTabKind, Guid.Empty,
																 "Admin Multi Tab", _treeNodeImage,
																 "Admin Multi Tabs", _treeNodeImage,
																 Category.Text, true,
																 ItemsAllowed.One, 
																 new AdminMultiTabItemManager(),
																 null
																 ));
				}

				public override void Close()
				{
						_itemNodes.Clear();
				}

				#region Identification Properties

				/// <summary>
				/// Gets the unique id identifying this plugin component
				/// </summary>
				public override Guid Id
				{
						get
						{
								return AdminMultiTabPluginId;
						}
				}

				/// <summary>
				/// We are here using the common node for samples.
				/// </summary>
				public override Guid SharedNodeId
				{
						get
						{
								return PluginSamples.Common.SampleTopNode;
            }
				}

        /// <summary>
        /// Top level name for common node for samples
        /// </summary>
        public override string SharedNodeName
        {
            get { return PluginSamples.Common.SampleNodeName; }
        }

        /// <summary>
        /// Define name of top level Tree node
        /// </summary>
        public override string Name
				{
						get { return "Admin Multi Tab"; }
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
						get { return _topTreeNodeImage; }
				}

				#endregion

				#region Administration properties

				/// <summary>
				/// A list of server side configuration items in the administrator
				/// </summary>
				public override List<ItemNode> ItemNodes
				{
						get { return _itemNodes; }
				}
				#endregion
		}
}
