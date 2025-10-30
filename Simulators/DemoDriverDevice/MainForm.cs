using DemoDriverDevice.WebServer;
using MulticastV2;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DemoDriverDevice
{
    public partial class MainForm : Form
    {
        private delegate bool HandleLoginDelegate(string username, string password);

        private readonly ServiceProvider _serviceProvider = new ServiceProvider();
        private readonly string defaultMacAddress = "DEADC0DE5678";

        private DeviceDiscovery _deviceDiscovery;

        private const string NoLoginText = "No logins";
        private const string LoginAcceptedText = "Login accepted";
        private const string LoginRejectedText = "Login rejected";

        private const string CannotStartServiceText = "Service cannot be started";
        private const string InvalidPortText = "Invalid port";
        private const string InvalidMacText = "Invalid MAC address";

        public MainForm()
        {
            InitializeComponent();

            WebServiceHandler.WorkPanel = workPanel;
            WebServiceHandler.MainForm = this;
            cbScheme.SelectedIndex = 0;
            
            _serviceProvider.OnClosed += OnServiceClosed;
        }

        public MainForm(string port, string username, string password, string macAddress, bool startService = false) : this()
        {
            textBoxPort.Text = port;
            textBoxUser.Text = username;
            textBoxPassword.Text = password;

            if (!string.IsNullOrEmpty(macAddress))
            {
                maskedTextBoxMac.Text = macAddress;
            }

            if (startService && !(this.StartService(cbScheme.SelectedItem.ToString(), int.Parse(port), checkBoxEnableDiscovery.Checked)))
            {
                MessageBox.Show(CannotStartServiceText);
            }
        }        

        public string GetMacAddress()
        {
            return maskedTextBoxMac.Text;
        }

        public bool IncludeTextOverlay
        {
            get { return checkBoxIncludeOverlay.Checked; }
        }

        public bool HandleLogin(string username, string password)
        {
            if (InvokeRequired)
            {
                return (bool)this.Invoke(new HandleLoginDelegate(HandleLoginThread), new string[] { username, password });
            }
            return HandleLoginThread(username, password);
        }

        public int ChangePassword(string username, string password)
        {
            if (password.Length < 4)
            {
                return 10; // PasswordTooShortError
            }
            Invoke(new Action(() => { ChangePasswordInternal(password); }));
            return 1; // success
        }

        private void ChangePasswordInternal(string password)
        {
            textBoxPassword.Text = password;
        }

        private void OnServiceClosed()
        {
            Invoke(new MethodInvoker(delegate { SetControlsStop(); }));
        }

        private bool HandleLoginThread(string username, string password)
        {
            try
            {
                bool ok = username == textBoxUser.Text && password == textBoxPassword.Text;
                string currentDateTime = DateTime.Now.ToString("T");
                labelLoginState.Text = ok ? $"{LoginAcceptedText} from '{username}' at {currentDateTime}" : $"{LoginRejectedText} from '{username}' at {currentDateTime}";
                return ok;
            }
            catch { return false; }
        }

        private void ButtonCloseClick(object sender, EventArgs e)
        {
            workPanel.Close();
            this.Close();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            int port;
            if (!int.TryParse(textBoxPort.Text, out port))
            {
                MessageBox.Show(InvalidPortText);
                return;
            }

            if (IsEmptyMacAddress(maskedTextBoxMac.Text))
            {
                SetDefaultMacAddress();
            }

            string macAddress = maskedTextBoxMac.Text;
            if (!this.IsValidMacAddress(macAddress))
            {
                MessageBox.Show(InvalidMacText);
                return;
            }

            if (!this.StartService(cbScheme.SelectedItem.ToString(), port, checkBoxEnableDiscovery.Checked))
            {
                MessageBox.Show(CannotStartServiceText);
                return;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.SetControlsDisableAll();
            if (_deviceDiscovery != null)
            {
                _deviceDiscovery.LeaveMulticastGroup();
                _deviceDiscovery = null;
            }
            _serviceProvider.Close();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _serviceProvider.OnClosed -= OnServiceClosed;
            if(_deviceDiscovery != null)
            {
                _deviceDiscovery.LeaveMulticastGroup();
                _deviceDiscovery = null;
            }
            _serviceProvider.Close();
        }

        private void buttonOutOfCpuHardwareEvent_Click(object sender, EventArgs e)
        {
            this.workPanel.AddEvent(DemoDeviceConstants.HardwareEventOutOfCpuOrMemory);
        }

        private void buttonRebootHardwareEvent_Click(object sender, EventArgs e)
        {
            this.workPanel.AddEvent(DemoDeviceConstants.HardwareEventReboot);
        }

        private void SetControlsStart()
        {
            UpdateAllControls(true);

            groupBoxLoginData.Enabled = false;
            buttonStart.Enabled = false;
            labelLoginState.Text = NoLoginText;
            workPanel.Init();
        }

        private void SetControlsStop()
        {
            UpdateAllControls(true);

            buttonStop.Enabled = false;
            workPanel.Enabled = false;
            labelLoginState.Text = NoLoginText;
            buttonOutOfCpuHardwareEvent.Enabled = false;
            buttonRebootHardwareEvent.Enabled = false;
        }

        private void SetControlsDisableAll()
        {
            UpdateAllControls(false);
        }

        private void UpdateAllControls(bool isEnabled)
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = isEnabled;
            }
        }

        private bool IsValidMacAddress(string macAddress)
        {
            var r = new Regex("^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$");
            return r.IsMatch(macAddress);
        }

        private void SetDefaultMacAddress()
        {
            maskedTextBoxMac.Text = defaultMacAddress;
        }

        private bool IsEmptyMacAddress(string macAddress)
        {
            return string.IsNullOrWhiteSpace(maskedTextBoxMac.Text.Replace(":", string.Empty));
        }

        private bool StartService(string scheme, int port, bool enableDiscovery)
        {
            if (_serviceProvider.Init(scheme, port))
            {
                if (enableDiscovery)
                {
                    try
                    {
                        _deviceDiscovery = new DeviceDiscovery(3702, "239.255.255.250", "[ff02::c]", port, scheme);
                        _deviceDiscovery.JoinMulticastGroup();
                        _deviceDiscovery.UDPListener();
                    }
                    catch(Exception ex) 
                    {
                        MessageBox.Show("Failed to enable device discovery. Exception: " + ex.Message, "Device discovery", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                this.SetControlsStart();
                return true;
            }
            return false;
        }
    }
}
