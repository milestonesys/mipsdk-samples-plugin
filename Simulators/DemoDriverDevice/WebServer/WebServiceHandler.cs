using System;
using System.Collections.Generic;
using System.Globalization;

namespace DemoDriverDevice
{
    public class WebServiceHandler : IDeviceService
    {
        public static WorkPanel WorkPanel;
        public static MainForm MainForm;

        public Guid _speakerStreamId = Guid.NewGuid();
        public byte[] _speakerData = null;

        byte[] IDeviceService.GetPlaybackFrame(int channel, DateTime dateTime)
        {
            return WorkPanel.GetPlaybackFrame(dateTime);
        }

        string[] IDeviceService.GetEvents(int channel)
        {
            if (channel == DemoDeviceConstants.DeviceVideoChannel1)
            {
                return WorkPanel.GetEvents();
            }
            return null;
        }

        bool IDeviceService.Login(string username, string password)
        {
            //WorkPanel.AddDeviceAction("Login attempt: " + username + ", " + password);
            return MainForm.HandleLogin(username, password);
        }

        byte[] IDeviceService.GetLiveFrame(int channel, int stream, bool coloredMetadata)
        {
            if (channel == DemoDeviceConstants.DeviceVideoChannel1 || channel == DemoDeviceConstants.DeviceVideoChannel2)
            {
                return WorkPanel.GetLiveFrame(channel, stream, MainForm.IncludeTextOverlay ? (MainForm.GetMacAddress() + ":" + channel) : null);
            }
            if (channel == DemoDeviceConstants.DeviceAudioChannel)
            {
                return WorkPanel.AudioProvider.GetAudio();
            }
            if (channel == DemoDeviceConstants.DeviceMetadataChannel1)
            {
                return WorkPanel.MetadataProvider1.GetMetadata(coloredMetadata);
            }
            if (channel == DemoDeviceConstants.DeviceMetadataChannel2)
            {
                return WorkPanel.MetadataProvider2.GetMetadata(coloredMetadata);
            }

            throw new ArgumentException("Unsupported channel number: " + channel);
        }

        string IDeviceService.GetMacAddress()
        {
            return MainForm.GetMacAddress();
        }

        Guid IDeviceService.StartLiveStream(int channel, Dictionary<string, string> parameters)
        {
            if (channel == DemoDeviceConstants.DeviceVideoChannel1 || channel == DemoDeviceConstants.DeviceVideoChannel2)
            {
                return WorkPanel.StartSession(channel, parameters);
            }
         
            if (channel == DemoDeviceConstants.DeviceAudioChannel)
            {
                return WorkPanel.AudioProvider.StartAudioSession(parameters);
            }

            if (channel == DemoDeviceConstants.DeviceMetadataChannel1)
            {
                return WorkPanel.MetadataProvider1.StartMetadataSession();
            }
            if (channel == DemoDeviceConstants.DeviceMetadataChannel2)
            {
                return WorkPanel.MetadataProvider2.StartMetadataSession();
            }
                     
            if (channel == DemoDeviceConstants.DeviceSpeakerChannel)
            {
                return _speakerStreamId;
            }

            throw new ArgumentException("Unknown channel number: " + channel);
        }

        void IDeviceService.StopLiveStream(int channel)
        {
            if (channel == DemoDeviceConstants.DeviceVideoChannel1 || channel == DemoDeviceConstants.DeviceVideoChannel2)
            {
                WorkPanel.StopSession(channel);
            }
        }

        void IDeviceService.SendToDemoDeviceLog(string logAction)
        {
            WorkPanel.AddDeviceAction(logAction);
        }

        void IDeviceService.SendSpeakerData(byte[] data)
        {
            _speakerData = data;
            WorkPanel.AddDeviceAction("Speaker data, len=" + data.Length);
        }

        public string SendCommand(string command)
        {
            WorkPanel.AddDeviceAction("Executing command: " + command);
            return command + " executed";
        }

        public void ChangeSetting(int channel, int stream, string key, string data)
        {
            if (channel == DemoDeviceConstants.DeviceVideoChannel1 || channel == DemoDeviceConstants.DeviceVideoChannel2)
            {
                switch (key)
                {
                    case DemoDeviceConstants.DeviceSettingFPS:
                        if (double.TryParse(data, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double fps))
                        {
                            WorkPanel.SetFPS(channel, fps);
                        }
                        break;
                    case DemoDeviceConstants.DeviceSettingResolution:
                        WorkPanel.SetResolution(channel, stream, data);
                        break;
                }
            }
        }

        public Guid StartFirmwareUpgrade(string firmwareContent)
        {
            return WorkPanel.StartFirmwareUpgrade(firmwareContent);
        }

        public int GetFirmwareUpgradeProgress(Guid upgradeSessionId)
        {
            return WorkPanel.GetFirmwareUpgradeProgress(upgradeSessionId);
        }

        public void SetAbsolutePosition(double pan, double tilt, double zoom)
        {
            WorkPanel.SetAbsolutePosition(pan, tilt, zoom);
        }

        public double[] GetAbsolutePosition()
        {
            return WorkPanel.GetAbsolutePosition();
        }

        public int ChangePassword(string targetUsername, string password)
        {
            var result = MainForm.ChangePassword(targetUsername, password);
            if (result == 1)
            {
                WorkPanel.AddDeviceAction("Password changed to: " + password);
            }
            else if (result == 10)
            {
                WorkPanel.AddDeviceAction("Password rejected as it was shorter than the required 4 characters");
            }
            else if (result == 50)
            {
                WorkPanel.AddDeviceAction("Password change rejected as user name does not match current one");
            }
            return result;
        }
    }
}
