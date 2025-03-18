using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.Client.Hid;

namespace SCHidPlugin.Client
{
    /// <summary>
    /// A manually-discoverable HID plugin is a plugin that needs operator assistance in order to connect to a device.
    /// That are non-plug-and-play devices, like network-connected joysticks, etc.
    /// Set <see cref="IsManualConnectSupported"/> to <c>true</c> to enable manual discovery.
    ///
    /// In the typical scenario, the operator adds a new manually-discoverable HID device in the Smart Client though Settings->Joysticks tab.
    /// That triggers following actions:
    ///  - the Smart Client requests the plugin to provide a list of manual connection properties (see <see cref="GetManualConnectProperties"/>).
    ///  - the Smart Client displays dialog with the properties to the operator.
    ///  - after connection properties collected, the Smart Client calls <see cref="GenerateHidInstance"/> and then
    ///  - <see cref="HidInstance.ConnectAsync"/> to establish connection with the device (connection properties are supplied as arguments in the call).
    /// After that sequence, the new <c>HidInstance</c> is ready to act as a bridge between the device and the Smart Client.
    /// </summary>
    public class ManualDiscoveryHidPlugin : HidPlugin
    {
        private readonly SCHidPluginDefinition _host;
        private static readonly Guid _propIdString = new Guid("8C6445D0-17D1-4EDA-B1FB-22C2339C9F6F");

        public readonly Dictionary<Guid, ManualDiscoveryInstance> HidInstances = new Dictionary<Guid, ManualDiscoveryInstance>();

        public ManualDiscoveryHidPlugin(SCHidPluginDefinition host)
        {
            _host = host;
        }

        public override HidInstance GenerateHidInstance(Guid id)
        {
            if(HidInstances.TryGetValue(id, out var result))
                return result;
            ManualDiscoveryInstance inst = new ManualDiscoveryInstance(this, id); 
            HidInstances.Add(id, inst);
            return inst;
        }

        /// <summary>
        /// Return list of properties, the operator must fill in to connect to the device.
        /// </summary>
        public override IEnumerable<HidConfigurationProperty> GetManualConnectProperties()
        {
            var properties = new List<HidConfigurationProperty>
            {
                new HidStringConfigurationProperty(_propIdString, "Device initialization string")
                {
                    PlaceholderText = "ip=192.168.0.10;timeout=1000;...",
                    ToolTip = "Enter a string to initialize the device."
                },
                // you can request more properties here
            };
            return properties;
        }

        public override Guid Id { get; } = new Guid("E22B9755-612D-49E7-A18E-E4DEB34F387E");
        public override string Name { get; } = "Sample manual discovery HID Plugin";

        public override bool IsManualConnectSupported { get; } = true;

        public override void Close()
        {
            foreach (var instance in HidInstances.Values) 
                instance.Close();

            HidInstances.Clear();
        }
    }

    /// <summary>
    /// Represents a manually-discovered HID instance. Works very similar to <see cref="AutodiscoveryHidInstance"/>.
    /// </summary>
    public class ManualDiscoveryInstance : HidInstanceBase
    {
        private readonly ManualDiscoveryHidPlugin _host;

        public ManualDiscoveryInstance(ManualDiscoveryHidPlugin host, Guid id) : base(id)
        {
            _host = host;
            _inputValueStep = 0.1;
            HidInputs.AddRange(new HidInput[]
            {
                _inputX = new HidAxisInput(new Guid("75DA3F1D-336D-4E7F-8F31-08294E1AE83F"), "AXIS_X", HidAxisInput.ActionPan),

                new HidButtonInput(new Guid("415E0282-3208-495A-A0A2-5CBEECE1D72D"), "Button A", HidButtonInput.ActionPlayForward),
                new HidButtonInput(new Guid("D014C590-969A-48B8-8645-A427FE718637"), "Button B", HidButtonInput.ActionStopPlayback),
                new HidButtonInput(new Guid("E01A1255-41C3-4220-9483-4453BF6B3B72"), "Button C")
            });
        }

        public override Task DisconnectAsync(CancellationToken cancellationToken)
        {
            _host.HidInstances.Remove(Id);
            return base.DisconnectAsync(cancellationToken);
        }
    }
}