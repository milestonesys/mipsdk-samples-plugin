using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VideoOS.Platform.Client.Hid;
using Timer = System.Timers.Timer;

namespace SCHidPlugin.Client
{
    /// <summary>
    /// HidInstance - class that handles HID device events and translates them into the environment's events.
    /// Each instance supplied with a set of configuration properties (see <see cref="ConnectAsync"/>) that are
    /// used to communicate with the underlying device.
    /// If connection to the device cannot be established, the <see cref="ConnectAsync"/> must return a <see cref="ConnectResult"/> with an error message.
    /// </summary>
    public class HidInstanceBase : HidInstance
    {
        private readonly Timer _timer = new Timer();
        protected double _inputValueStep = 0.05;

        internal readonly List<HidInput> HidInputs = new List<HidInput>();
        protected HidAxisInput _inputX;

        public Dictionary<Guid, string> Properties { get; } = new Dictionary<Guid, string>();

        protected HidInstanceBase(Guid id) : base(id)
        {
        }

        /// <summary>
        /// Establish connecting with the underlying device here. Remember to check <c>cancellationToken</c>: the Smart Client may cancel the operation.
        /// In such a case, the method must return a <see cref="ConnectResult"/> with an error message.
        /// </summary>
        /// <param name="userDefinedName">For manually discoverable HIDs, this parameter will contain user-defined name of the device.</param>
        /// <param name="configurationProperties">List of properties, associated with this instance. Properties filled by the operator when creating
        /// manually-discoverable HID.</param>
        /// <param name="cancellationToken">Check this cancellation token: if raised, method must stop execution and return ConnectionResult
        /// with appropriate error message.</param>
        /// <returns>List of actually discovered HID input controls: axes, buttons, sliders, etc.</returns>
        public override Task<ConnectResult> ConnectAsync(string userDefinedName, IEnumerable<HidConfigurationProperty> configurationProperties, CancellationToken cancellationToken)
        {
            Name = userDefinedName;
            var props = configurationProperties.ToList();
            foreach (var i in props) 
                Properties[i.Id] = ((HidStringConfigurationProperty)i).Value;
            return Task.FromResult(new ConnectResult(HidInputs));
        }

        /// <summary>
        /// Disconnect from the device here. Remember to check <c>cancellationToken</c>: the Smart Client may cancel the operation.
        /// </summary>
        /// <param name="cancellationToken">Cancel operation and return immediately if cancellationToken is raised.</param>
        /// <returns>Return when device is disconnected.</returns>
        public override Task DisconnectAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #region Joystick simulation
        /// <summary>
        /// Simulate X-axis movement: patrolling horizontally.
        /// </summary>
        public bool Enabled
        {
            get => _timer.Enabled;
            set
            {
                _timer.Enabled = value;
                _inputX.Value = 0.0;
            }
        }

        /// <summary> Simulate button click. </summary>
        /// <param name="buttonName"></param>
        public void Press(string buttonName)
        {
            foreach (var input in HidInputs)
            {
                //look up the button by name, but more confident way is to use the input.Id
                if (input.Name == buttonName && input is HidButtonInput button)
                {
                    button.Value = HidButtonValueData.Pressed;
                    Task.Delay(100).ContinueWith(_ => button.Value = HidButtonValueData.Released);
                    break;
                }
            }
        }

        public override void Init()
        {
            _timer.Interval = 100;
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = false;
        }

        /// <summary>
        /// Simulate X-axis movement.
        /// Notice that the value may vary in the range [-1.0..1.0] and is updated on the UI thread.
        /// </summary>
        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => {
                _inputX.Value += _inputValueStep;
                if (_inputX.Value + _inputValueStep <= -1.0 || _inputX.Value + _inputValueStep >= 1.0)
                    _inputValueStep = -_inputValueStep;
            });
        }
        #endregion

        public override void Close()
        {
            HidInputs.Clear();
            _timer.Close();
        }
    }
}