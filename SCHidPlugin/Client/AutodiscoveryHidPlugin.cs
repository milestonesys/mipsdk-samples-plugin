using System;
using System.Windows.Forms;
using VideoOS.Platform.Client.Hid;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace SCHidPlugin.Client
{
    /// <summary>
    /// An auto-discoverable HID plugin is a plugin capable of automatically establishing a connection to devices 
    /// without requiring user intervention, similar to plug-and-play devices (e.g., USB or Bluetooth).
    /// When the plugin detects a new device, it registers the device by invoking <see cref="HidPlugin.InstanceDiscovered"/>.
    /// This triggers the hosting environment to request the plugin to create a new instance of the HID device wrapper 
    /// (see <see cref="GenerateHidInstance"/>).
    /// </summary>
    public class AutodiscoveryHidPlugin : HidPlugin
    {
        internal AutodiscoveryHidInstance Instance;

        public AutodiscoveryHidPlugin(SCHidPluginDefinition host)
        {
        }

        private static Guid _autodiscoveryInstanceId = new Guid("8461C29A-BC76-4849-BFDC-AE58E500DA4A");
        private object _directInputRequestMessageReceiver = null;
        private int _companyVendorId = int.MaxValue; // Todo: Replace this value with the vendor id of the device that the plugin should take ownership of.
        private int _deviceProductId = int.MaxValue; // Todo: Replave this value with the product id of the device that the plugin should take ownership of.

        public override HidInstance GenerateHidInstance(Guid id)
        {
            return Instance = new AutodiscoveryHidInstance(id);
        }

        /// <summary>The unique ID identifying this plug-in component</summary>
        public override Guid Id { get; } = new Guid("99d3e88c-7f23-4880-86ae-591e14155929");
        /// <summary>Name of the plugin.</summary>
        public override string Name => "Sample autodiscovery HID Plugin";

        public override void Init()
        {
            // Notify the environment about discovered instance: 
            // plug-and-play devices are discovered by plugins without users' assistance.
            // That is why they are "auto-discoverable".

            // In the Init-override plugin can register such instances.
            // Another scenario might be to register a listener for the PnP device and register the instance when the device is detected.

            // For simplicity, in this example, we register the instance right away.
            this.InstanceDiscovered(_autodiscoveryInstanceId);

            // If the plugin will handle devices that also registers themself as DirectInput devices, the plugin needs to respond to the
            // IsDirectInputHIDHandledByPlugInRequestHandler message for the device not to be listed by the build-in DirectInput Hid
            // plugin of the Smart Client. 
            _directInputRequestMessageReceiver = EnvironmentManager.Instance.RegisterReceiver(IsDirectInputHIDHandledByPlugInRequestHandler, new MessageIdFilter(MessageId.SmartClient.IsDirectInputHIDHandledByPlugInRequest));
        }

        private object IsDirectInputHIDHandledByPlugInRequestHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            IsDirectInputHIDHandledByPlugInRequestData data = message.Data as IsDirectInputHIDHandledByPlugInRequestData;
            if (data != null)
            {
                // If the device matches the vendor id and the product id this plugin decides to take ownership of the DirectInput device which
                // is done by returning true. 
                // Note that it's possible to identify the device to take ownership of through a series of information in the
                // IsDirectInputHIDHandledByPlugInRequestData object, including InstanceId, InstanceName and ProductGuid
                if (data.VendorId == _companyVendorId && data.ProductId == _deviceProductId)
                    return true;
                else
                    return false;
            }
            return false;
        }


        public override void Close()
        {
            // Unregister the receiver
            EnvironmentManager.Instance.UnRegisterReceiver(_directInputRequestMessageReceiver);
        }
    }


    /// <summary>
    /// Represents an auto-discovered HID instance. Class responsibilities:
    /// <para>- "describe" HID device features to the hosting environment (Smart Client) - how many axes, buttons, sliders, etc. it has.</para>
    /// <para>- handle HID device events and update associated HidInputs accordingly.</para>
    /// </summary>
    public class AutodiscoveryHidInstance : HidInstanceBase
    {
        /// <summary>
        /// Name of the device that operator will see in the Smart Client->Settings->Joysticks tab
        /// </summary>
        public override string Name { get; protected set; } = "Autodiscovery joystick";

        public AutodiscoveryHidInstance(Guid id) : base(id)
        {
            // In this example, we construct a fake joystick with only X-axis and 3 buttons.
            // In a real-life scenario, plugin can read device features from the device itself.

            // NOTE: it is important to keep HidInputs id same (constant) as the Smart Client uses these ids to associate HID inputs with the actions assigned to them.
            // I.e. IDs must be unique, but same between SC sessions.
            HidInputs.AddRange(new HidInput[]
            {
                //When constructing a HID input, you can specify what environment action to associate with the input by default.
                //In this case, we instruct environment to associate "AXIS_X" input with the "Pan" action:
                //  that should simplify device configuration for the operator.
                _inputX = new HidAxisInput(Guid.Parse("CF42F239-CB29-4822-B0FF-4C64808E9327"), "AXIS_X", HidAxisInput.ActionPan),

                new HidButtonInput(Guid.Parse("61522A52-45EA-4618-87A8-8DDCD4E2ECF3"), "Button 1", HidButtonInput.ActionStartRecording),
                new HidButtonInput(Guid.Parse("341B0569-66B8-48E1-8CCE-C16D8EC40499"), "Button 2", HidButtonInput.ActionStopRecording),
                new HidButtonInput(Guid.Parse("766CEEC7-8AC7-42D8-A8DC-8ED1CE8EB4F2"), "Button 3"),
            });
        }
    }
}