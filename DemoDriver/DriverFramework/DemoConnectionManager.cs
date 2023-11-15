using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using VideoOS.Platform.DriverFramework.Data;
using VideoOS.Platform.DriverFramework.Data.Settings;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;


namespace DemoDriver
{
    public class DemoConnectionManager : ConnectionManager
    {
        private DeviceDriverProxy _proxy;
        private InputPoller _inputPoller;

        private Uri _uri;
        private DateTime _lastConnectCheck = DateTime.MinValue;
        private static readonly TimeSpan ConnectionCheckPeriod = TimeSpan.FromSeconds(15);
        private readonly object _isConnectedLock = new object();

        private string _userName;
        private SecureString _scrambledPassword;
        private string _connectMessage = string.Empty;
        private bool _connected;
        private bool _authenticationIssue;

        public DemoConnectionManager(DemoContainer container) : base(container)
        {
        }

        /// <summary>
        /// Implementation of the DFW platform method.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="hardwareSettings"></param>
        public override void Connect(Uri uri, string userName, SecureString password, ICollection<HardwareSetting> hardwareSettings)
        {
            _connected = false;
            _uri = uri;
            _userName = userName;
            _scrambledPassword = password;
            BuildBaseProxy();
            ConnectToServer();
            if (_authenticationIssue)
            {
                throw new AuthenticationException();
            }
            else if (!_connected)
            {
                throw new ConnectionLostException(_connectMessage);
            }
            _inputPoller = new InputPoller(Container.EventManager, this);
            _inputPoller.Start();
        }

        /// <summary>
        /// Implementation of the DFW platform method.
        /// </summary>
        public override void Close()
        {
            _connected = false;
            _lastConnectCheck = DateTime.MinValue;
        }

        /// <summary>
        /// Implementation of the DFW platform property.
        /// NOTE: It will perform a new login if it hasn't been accessed recently.
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                lock (_isConnectedLock)
                {
                    if (_uri != null && _lastConnectCheck + ConnectionCheckPeriod < DateTime.UtcNow)
                    {
                        ConnectToServer();
                    }
                    return _connected;
                }
            }
        }

        /// <summary>
        /// Is called when a firmware upgrade is started. 
        /// 
        /// <see cref="VideoOS.Platform.DriverFramework.Definitions.Setting.FirmwareUpgradeSupported"/> must be added to Dictionary
        /// returned by <see cref="ConfigurationManager.BuildHardwareSettings"/> with value <see cref="VideoOS.Platform.DriverFramework.Definitions.Setting.SettingValueYes"/>
        /// for this to be enabled.
        /// </summary>
        /// <param name="firmwarePath"></param>
        /// <returns></returns>
        public override Guid StartFirmwareUpgrade(string firmwarePath)
        {
            ThrowIfNotConnected();
            if (_proxy != null)
            {
                var firmwareContent = File.ReadAllText(firmwarePath); // in this sample only a text file is supported, but for normal firmware files it is of course more likely to be in byte format

                return _proxy.Client.StartFirmwareUpgrade(firmwareContent);
            }
            return Guid.Empty;
        }

        public override FirmwareUpgradeStatus GetFirmwareUpgradeStatus(Guid upgradeSessionId)
        {
            var progress = _proxy.Client.GetFirmwareUpgradeProgress(upgradeSessionId);
            FirmwareUpgradeStatus status = new FirmwareUpgradeStatus();
            if (progress == 100)
            {
                status.NewFirmwareVersion = "1.upgraded"; // normally this would be read from the device
                status.CompletionPercentage = progress;
                status.UpgradeStatusCode = UpgradeStatusCode.Completed;
            }
            else if (progress >= 0)
            {
                status.CompletionPercentage = progress;
                status.UpgradeStatusCode = UpgradeStatusCode.InProgress;
            }
            else if (progress == -1)
            {
                status.UpgradeStatusCode = UpgradeStatusCode.ErrorFirmwareRejected;
            }
            else
            {
                status.UpgradeStatusCode = UpgradeStatusCode.ErrorUnknown;
            }
            return status;
        }

        public void StartLiveStream(int channel)
        {
            ThrowIfNotConnected();
            DeviceDriverProxy proxy = _proxy;
            if (proxy == null)
            {
                proxy = new DeviceDriverProxy(_uri.ToString(), _userName, _scrambledPassword);
            }
            proxy.Client.StartLiveStream(channel, new Dictionary<string, string>());
            _proxy = proxy;
        }

        public void StopLiveStream(int channel)
        {
            if (_proxy != null)
            {
                try
                {
                    _proxy.Client.StopLiveStream(channel);
                }
                catch { }
            }
        }

        public byte[] GetLiveFrame(int channel, bool color)
        {
            ThrowIfNotConnected();
            if (_proxy != null)
            {
                return _proxy.Client.GetLiveFrame(channel, color);
            }
            return null;
        }

        public byte[] GetPlaybackFrame(int channel, DateTime dt)
        {
            ThrowIfNotConnected();
            if (_proxy != null)
            {
                return _proxy.Client.GetPlaybackFrame(channel, dt);
            }
            return null;
        }

        public string GetMacAddress()
        {
            if (_proxy != null)
            {
                return _proxy.Client.GetMacAddress();
            }
            return null;
        }

        public string[] GetEvents(int channel)
        {
            ThrowIfNotConnected();
            if (_proxy != null)
            {
                return _proxy.Client.GetEvents(channel);
            }
            return null;
        }

        public void SendSpeakerData(byte[] data)
        {
            ThrowIfNotConnected();
            if (_proxy != null)
            {
                _proxy.Client.SendSpeakerData(data);
            }
        }

        public void SendInfo(string deviceId, string info)
        {
            ThrowIfNotConnected();
            Guid dev = new Guid(deviceId);
            string device = (dev == Constants.Camera1 || dev == Constants.Camera2) ? "Camera" : "Other device";
            string msg = string.Format("{0}: {1}", device, info);
            Toolbox.Log.Trace(msg);
            _proxy.Client.SendToDemoDeviceLog(msg);
        }

        public string SendCommand(string deviceId, string command, string parameter)
        {
            ThrowIfNotConnected();
            Guid dev = new Guid(deviceId);
            string device = (dev == Constants.Camera1 || dev == Constants.Camera2) ? "Camera" : "Other device";
            string msg = string.Format("{0} on {1} with {2}", command, device, parameter);
            Toolbox.Log.Trace(msg);
            return _proxy.Client.SendCommand(msg);
        }

        public void ChangeSetting(int channel, string key, string data)
        {
            ThrowIfNotConnected();
            if (_proxy != null)
            {
                _proxy.Client.ChangeSetting(channel, key, data);
            }
        }

        private void BuildBaseProxy()
        {
            if (_proxy != null)
            {
                try
                {
                    _proxy.Client.Close();
                }
                catch (Exception) { }
            }
            string uriString = new UriBuilder(_uri.Scheme, _uri.DnsSafeHost, _uri.Port, "DeviceService").ToString();

            DeviceDriverProxy proxy = new DeviceDriverProxy(uriString, _userName, _scrambledPassword);
            _proxy = proxy;
        }

        /// <summary>
        /// Creates a connection.
        /// </summary>
        private void ConnectToServer()
        {
            if (_proxy == null)
            {
                BuildBaseProxy();
            }

            try
            {
                _lastConnectCheck = DateTime.UtcNow;
                // accept all SSL certificates - for testing purposes only!
                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                _connected = _proxy.Client.Login(_userName, SecureStringToString(_scrambledPassword));
                _connectMessage = string.Empty;
                _authenticationIssue = _connected == false;
            }
            catch (Exception ex)
            {
                _connected = false;
                _authenticationIssue = false;
                _connectMessage = ex.Message;
                BuildBaseProxy();
            }
        }

        /// <summary>
        /// Decode a secure password to plain password.
        /// This code makes sure that memory used to hold the decoded password gets explicitly deallocated.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Reference to unsecured password string</returns>
        private static string SecureStringToString(SecureString password)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(password);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// If not connected, this will attempt to connect/login or fail with an exception.
        /// </summary>
        private void ThrowIfNotConnected()
        {
            if (!IsConnected)
            {
                if (_authenticationIssue)
                {
                    throw new AuthenticationException("Login credentials not accepted");
                }
                throw new ConnectionLostException(_connectMessage);
            }
        }
    }
}
