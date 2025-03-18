using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using SCHidPlugin.Client;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Client.Hid;
using VideoOS.Platform.UI.Controls;

namespace SCHidPlugin
{
    /// <summary>
    /// The Smart Client workspace toolbar plugin: this is the toolbar that is shown in the Smart Client
    /// and allow interaction with the sample HID plugins.
    /// Not needed in real-life scenarios as HID-devices will rise hardware events and send them into the Smart Client.
    /// </summary>
    internal class RootWorkspaceToolbarPlugin : WorkSpaceToolbarPlugin
    {
        private readonly List<HidPlugin> _plugins;

        public RootWorkspaceToolbarPlugin(List<HidPlugin> plugins)
        {
            _plugins = plugins;
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        public override WorkSpaceToolbarPluginInstance GenerateWorkSpaceToolbarPluginInstance()
        {
            return new RootToolbarInstance(_plugins);
        }

        public override Guid Id { get; } = new Guid("F160C043-00A2-4842-9D02-3F7F769266E3");
        public override string Name { get; } = "Sample virtual HID";

        /// <summary>
        /// Instance of the toolbar plugin: allows to interact with sample HID plugins through the toolbar.
        /// <remarks>
        /// About implementation:
        ///   since number of manually-discovered HID devices is unknown (operator can make many of them),
        ///   we create menu items dynamically.
        /// </remarks> 
        /// </summary>
        private class RootToolbarInstance : WorkSpaceToolbarPluginInstance
        {
            private readonly List<HidPlugin> _plugins;

            public RootToolbarInstance(List<HidPlugin> plugins)
            {
                _plugins = plugins;

                Title = "Sample virtual HID";
                Tooltip = "Control sample virtual HIDs";
            }

            public override void Init(Item window)
            {
            }

            public override void Close()
            {
            }

            /// <summary>
            /// Creates a toolbar menu with items for each HID device.
            /// </summary>
            public override void Activate()
            {
                var menu = new VideoOSContextMenuSmall() { IsOpen = true };
                var plugin = _plugins.OfType<AutodiscoveryHidPlugin>().First();

                AddMenuForHidInstance(menu, plugin.Instance);

                var manualPlugin = _plugins.OfType<ManualDiscoveryHidPlugin>().First();
                foreach (var instance in manualPlugin.HidInstances.Values)
                {
                    menu.Items.Add(new Separator());

                    AddMenuForHidInstance(menu, instance);
                }
            }

            private static void AddMenuForHidInstance(VideoOSContextMenuSmall menu, HidInstanceBase instance)
            {
                var manualItem = new VideoOSContextMenuItem()
                {
                    Data = $"Move '{instance.Name}'",
                    IsChecked = instance.Enabled,
                    IsCheckable = true,
                };
                manualItem.Click += (sender, args) =>
                {
                    instance.Enabled = !instance.Enabled;
                };
                menu.Items.Add(manualItem);

                foreach (var input in instance.HidInputs.OfType<HidButtonInput>())
                {
                    var item = new VideoOSContextMenuItem()
                    {
                        Data = $"Click '{input.Name}'",
                    };
                    item.Click += (sender, args) => { instance.Press(input.Name); };
                    menu.Items.Add(item);
                }
            }
        }
    }
}