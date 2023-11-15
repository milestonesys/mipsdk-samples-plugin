using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;

namespace DemoDriver
{
    /// <summary>
    /// Simulates that configuration is built with information from the device
    /// </summary>
    public class DemoConfigurationManager: ConfigurationManager
    {
        private const string _firmware = "DemoFirmware";
        private const string _firmwareVersion = "1.0";
        private const string _hardwareName = "Machine name";
        private const string _serialNumber = "M10S25X20";

        private new DemoContainer Container => base.Container as DemoContainer;

        public DemoConfigurationManager(DemoContainer container) : base(container)
        {
        }

        protected override ProductInformation FetchProductInformation()
        {
            if (!Container.ConnectionManager.IsConnected)
            {
                throw new ConnectionLostException("Connection not established");
            }

            var driverInfo = Container.Definition.DriverInfo;
            var product = driverInfo.SupportedProducts.FirstOrDefault();
            var macAddress = Container.ConnectionManager.GetMacAddress();

            return new ProductInformation
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductVersion = driverInfo.Version,
                MacAddress = macAddress,
                FirmwareVersion = _firmwareVersion,
                Firmware = _firmware,
                HardwareName = _hardwareName,
                SerialNumber = _serialNumber
            };
        }

        protected override IDictionary<string, string> BuildHardwareSettings()
        {
            return new Dictionary<string, string>()
            {
                { Constants.BandwidthLimit, "5000" },
                { Setting.FirmwareUpgradeSupported, Setting.SettingValueYes } // will tell the VMS that firmware upgrade is supported. If set, then ConnectionManager.StartFirmwareUpgrade and ConnnectionManager.GetFirmwareUpgradeStatus must be implemented as well
            };
        }

        protected override ICollection<ISetupField> BuildFields()
        {
            var f = new List<ISetupField>();
            f.Add(new NumberSetupField()
            {
                Key = Constants.BandwidthLimit,
                DisplayName = "Bandwith limit (number sample)",
                DisplayNameReferenceId = Guid.Empty,
                IsReadOnly = false,
                ReferenceId = Constants.BandwidthLimitRefId,
                MinValue = 0,
                MaxValue = 20000,
                Resolution = 1,
                DefaultValue = 5001,
            });

            f.Add(new StringSetupField()
            {
                Key = Constants.SomeField,
                DisplayName = "Some string (string sample)",
                DisplayNameReferenceId = Guid.Empty,
                IsReadOnly = false,
                ReferenceId = Constants.StringFieldRefId,
                DefaultValue = "Some value",
            });
            f.Add(new BoolSetupField()
            {
                Key = Constants.BoolField,
                DisplayName = "Enable a feature (bool sample)",
                DisplayNameReferenceId = Guid.Empty,
                IsReadOnly = false,
                ReferenceId = Constants.BoolFieldRefId,
                DefaultValue = true,
            });
            f.Add(new EnumSetupField()
            {
                Key = Constants.Rotation,
                DisplayName = "Rotation (enum sample)",
                DisplayNameReferenceId = Guid.Empty,
                IsReadOnly = false,
                ReferenceId = Constants.RotationFieldRefId,
                EnumList = new[]
                {
                    new StringSetupField(){ Key = "0", DefaultValue = "0", DisplayName = "0", DisplayNameReferenceId = Guid.Empty, ReferenceId = Constants.RotationFieldRefId0 },
                    new StringSetupField(){ Key = "90", DefaultValue = "90", DisplayName = "90", DisplayNameReferenceId = Guid.Empty, ReferenceId = Constants.RotationFieldRefId90 },
                    new StringSetupField(){ Key = "180", DefaultValue = "180", DisplayName = "180", DisplayNameReferenceId = Guid.Empty, ReferenceId = Constants.RotationFieldRefId180 },
                    new StringSetupField(){ Key = "270", DefaultValue = "270", DisplayName = "270", DisplayNameReferenceId = Guid.Empty, ReferenceId = Constants.RotationFieldRefId270 },
                },
                DefaultValue = "0",
            });
            f.Add(new EnumSetupField()
            {
                Key = Constants.BoundingBoxColor,
                DisplayName = "Bounding box color",
                DisplayNameReferenceId = Guid.Empty,
                IsReadOnly = false,
                ReferenceId = Constants.MetadataColorRefId,
                EnumList = new[]
                {
                    new StringSetupField(){ Key = "C" , DefaultValue = "C", DisplayName = "Colors", DisplayNameReferenceId = Guid.Empty, ReferenceId = Constants.MetadataColorCRefId },
                    new StringSetupField(){ Key = "BW", DefaultValue = "BW", DisplayName = "Black/White", DisplayNameReferenceId = Guid.Empty, ReferenceId = Constants.MetadataColorBWRefId },
                },
                DefaultValue = "C",
            });
            f.Add(new DoubleSetupField()
            {
                Key = Constants.FPS,
                DisplayName = "FPS",
                DisplayNameReferenceId = Guid.Empty,
                MinValue = 0.00028,
                MaxValue = 30.0,
                IsReadOnly = false,
                ReferenceId = Constants.FPSRefId,
                Resolution = 0.0,
                DefaultValue = 8.0,
            });
            f.Add(new EnumSetupField()
            {
                Key = "Codec",
                DisplayName = "Codec",
                DisplayNameReferenceId = Guid.Empty,
                IsReadOnly = false,
                ReferenceId = Constants.CodecRefId,
                EnumList = new[]
                {
                    new StringSetupField {Key = "h264", DefaultValue = "h264", DisplayName = "H.264", ReferenceId = Constants.CodecH264ReferenceId, DisplayNameReferenceId = Constants.CodecH264DisplayNameReferenceId },
                    new StringSetupField {Key = "jpeg", DefaultValue = "jpeg", DisplayName = "MJPEG", ReferenceId = Constants.CodecMjpegReferenceId, DisplayNameReferenceId = Constants.CodecMjpegDisplayNameReferenceId },
                },
                DefaultValue = "jpeg",
            });
            f.Add(new NumberSetupField()
            {
                Key = Constants.InputGain,
                DisplayName = "Input gain (number sample)",
                DisplayNameReferenceId = Guid.Empty,
                MinValue = 0,
                MaxValue = 100,
                IsReadOnly = false,
                ReferenceId = Constants.InputGainRefId,
                DefaultValue = 25,
            });
            f.Add(new NumberSetupField()
            {
                Key = Constants.OutputGain,
                DisplayName = "Output gain (number sample)",
                DisplayNameReferenceId = Guid.Empty,
                MinValue = 0,
                MaxValue = 100,
                IsReadOnly = false,
                ReferenceId = Constants.OutputGainRefId,
                DefaultValue = 25,
            });

            return f;
        }

        protected override ICollection<EventDefinition> BuildHardwareEvents()
        {
            var hardwareEvents = new List<EventDefinition>();
            hardwareEvents.Add(new EventDefinition()
            {
                DisplayName = "Rebooting soon (Demo)",
                ReferenceId = Constants.RebootReferenceId,
                NameReferenceId = Constants.RebootReferenceId,
                CounterReferenceId = Guid.Empty,
            });
            hardwareEvents.Add(new EventDefinition()
            {
                DisplayName = "CPU or Memory critical (Demo)",
                ReferenceId = Constants.ResourceIssueReferenceId,
                NameReferenceId = Constants.ResourceIssueReferenceId,
                CounterReferenceId = Guid.Empty,
            });
            return hardwareEvents;
        }

        protected override ICollection<DeviceDefinitionBase> BuildDevices()
        {
            var devices = new List<DeviceDefinitionBase>();

            devices.Add(new CameraDeviceDefinition()
            {
                DisplayName = "First camera",
                DeviceId = Constants.Camera1.ToString(),
                DeviceEvents = BuildCameraEvents(),
                Settings = new Dictionary<string, string>()
                {
                    { Constants.SomeField, "has value 25" },
                    { Constants.BoolField, "false" },
                    { Constants.Rotation, "0" }
                },
                Streams = BuildCameraStreams(),
                // Leave PtzSupport set to null to disable PTZ support
                PtzSupport = BuildPtzSupport(),
            });
            devices.Add(new CameraDeviceDefinition()
            {
                DisplayName = "Second camera",
                DeviceId = Constants.Camera2.ToString(),
                DeviceEvents = BuildCameraEvents(),
                Settings = new Dictionary<string, string>()
                {
                    { Constants.SomeField, "has value 20" },
                    { Constants.BoolField, "true" },
                    { Constants.Rotation, "180" }
                },
                Streams = BuildCameraStreams(),
                // Leave PtzSupport set to null to disable PTZ support
                PtzSupport = null
            });

            devices.Add(new MetadataDeviceDefinition()
            {
                DisplayName = "Bounding box & GPS",
                DeviceId = Constants.Metadata1.ToString(),
                Streams = BuildMetadataStreams(),
                DeviceEvents = BuildMetadataEvents(),
            });
            devices.Add(new MetadataDeviceDefinition()
            {
                DisplayName = "Bounding box & GPS (2)",
                DeviceId = Constants.Metadata2.ToString(),
                Streams = BuildMetadataStreams(),
                DeviceEvents = BuildMetadataEvents()
            });

            devices.Add(new MicrophoneDeviceDefinition()
            {
                DisplayName = "Built-in microphone",
                DeviceId = Constants.Audio1.ToString(),
                Streams = BuildAudioStream(),
                DeviceEvents = BuildMicrophoneEvents()
            });

            devices.Add(new OutputDeviceDefinition()
            {
                DisplayName = "Output 1",
                DeviceId = Constants.Output1.ToString(),
                SupportSetState = true,
                SupportTrigger = true,
            });

            devices.Add(new InputDeviceDefinition()
            {
                DisplayName = "Input 1",
                DeviceId = Constants.Input1.ToString(),
            });
            devices.Add(new InputDeviceDefinition()
            {
                DisplayName = "Input 2",
                DeviceId = Constants.Input2.ToString(),
            });

            devices.Add(new SpeakerDeviceDefinition()
            {
                DisplayName = "Speaker",
                DeviceId = Constants.Speaker1.ToString(),
                Streams = BuildSpeakerStream()
            });

            return devices;
        }

        private static ICollection<StreamDefinition> BuildCameraStreams()
        {
            ICollection<StreamDefinition> streams = new List<StreamDefinition>();
            streams.Add(new StreamDefinition()
            {
                DisplayName = "Stream 1 (stream sample)",
                ReferenceId = Constants.Stream1RefId.ToString(),
                Settings = new Dictionary<string, string>()
                {
                    {Constants.Codec, "MJPEG" },
                    {Constants.FPS, "8.0" },
                },
                RemotePlaybackSupport = true,
            });
            streams.Add(new StreamDefinition()
            {
                DisplayName = "Stream 2 (stream sample)",
                ReferenceId = Constants.Stream2RefId.ToString(),
                Settings = new Dictionary<string, string>()
                {
                    {Constants.Codec, "MJPEG" },
                    {Constants.FPS, "8.0" },
                },
                RemotePlaybackSupport = true,

            });

            return streams;
        }

        private static ICollection<StreamDefinition> BuildAudioStream()
        {
            ICollection<StreamDefinition> streams = new List<StreamDefinition>();
            streams.Add(new StreamDefinition()
            {
                DisplayName = "Audio stream 1",
                ReferenceId = Constants.AudioStream1RefId.ToString(),
                Settings = new Dictionary<string, string>()
                {
                    {Constants.InputGain, "25" },
                },
            });
            return streams;
        }

        private static ICollection<StreamDefinition> BuildSpeakerStream()
        {
            ICollection<StreamDefinition> streams = new List<StreamDefinition>();
            streams.Add(new StreamDefinition()
            {
                DisplayName = "Speaker stream 1",
                ReferenceId = Constants.SpeakerStream1RefId.ToString(),
                Settings = new Dictionary<string, string>()
                {
                    {Constants.OutputGain, "25" },
                },
            });
            return streams;
        }

        private static ICollection<StreamDefinition> BuildMetadataStreams()
        {
            ICollection<StreamDefinition> streams = new List<StreamDefinition>();
            streams.Add(new StreamDefinition()
            {
                DisplayName = "Metadata Stream 1",
                ReferenceId = MetadataType.BoundingBoxDisplayId.ToString(),
                MetadataTypes = new List<MetadataTypeDefinition>()
                {
                    new MetadataTypeDefinition()
                    {
                        DisplayName = "My bounding boxes",
                        DisplayNameId = MetadataType.BoundingBoxDisplayId,
                        MetadataType = MetadataType.BoundingBoxTypeId,
                        ValidTime = TimeSpan.FromSeconds(5),
                        Settings = new Dictionary<string, string>()
                        {
                            { Constants.BoundingBoxColor, "C" },
                        },
                    },
                    new MetadataTypeDefinition()
                    {
                        DisplayName = "My GPS positions (GPS metadata sample)",
                        DisplayNameId = MetadataType.GpsDisplayId,
                        MetadataType = MetadataType.GpsTypeId,
                        ValidTime = TimeSpan.FromSeconds(30),
                    }
                },
                //Fields = common fields for stream,
            });
            return streams;
        }


        private static ICollection<EventDefinition> BuildMicrophoneEvents()
        {
            //Note - microphone events are not triggered in this sample. They are just a demonstration
            var metaDataEvents = new List<EventDefinition>();

            metaDataEvents.Add(new EventDefinition()
            {
                ReferenceId = Constants.SoundActivatedEventReferenceId,
                DisplayName = "Sound activated (Demo)",
                NameReferenceId = Constants.ResourceSoundActivatedEventNameReferenceId,
                CounterReferenceId = Constants.SoundDeactivatedEventReferenceId,
            });
            metaDataEvents.Add(new EventDefinition()
            {
                ReferenceId = Constants.SoundDeactivatedEventReferenceId,
                DisplayName = "Sound deactivated (Demo)",
                NameReferenceId = Constants.ResourceSoundDeactivatedEventNameReferenceId,
                CounterReferenceId = Constants.SoundActivatedEventReferenceId,
            });
            return metaDataEvents;

        }

        private static ICollection<EventDefinition> BuildMetadataEvents()
        {
            //Note - metadata events are not triggered in this sample. They are just a demonstration
            var metaDataEvents = new List<EventDefinition>();

            metaDataEvents.Add(new EventDefinition()
            {
                ReferenceId = Constants.MetadataInputActivated,
                DisplayName = "Metadata input activated (Demo)",
                NameReferenceId = Constants.ResourceInputActivatedEventNameReferenceId,
                CounterReferenceId = Constants.MetadataInputDeactivated,
               
            });
            metaDataEvents.Add(new EventDefinition()
            {
                ReferenceId = Constants.MetadataInputDeactivated,
                DisplayName = "Metadata input deactivated (Demo)",
                NameReferenceId = Constants.ResourceInputDeactivatedEventNameReferenceId,
                CounterReferenceId = Constants.MetadataInputActivated,
            });
            return metaDataEvents;
        }

        private static ICollection<EventDefinition> BuildCameraEvents()
        {
            var deviceEvents = new List<EventDefinition>();

            deviceEvents.Add(new EventDefinition()
            {
                ReferenceId = EventId.MotionStartedDriver,
                DisplayName = "Motion Started (Demo)",
                NameReferenceId = Constants.MotionStartedEventNameReferenceId,
                CounterReferenceId = EventId.MotionStoppedDriver,
            });
            deviceEvents.Add(new EventDefinition()
            {
                ReferenceId = EventId.MotionStoppedDriver,
                DisplayName = "Motion Stopped (Demo)",
                NameReferenceId = Constants.MotionStoppedEventNameReferenceId,    
                CounterReferenceId = EventId.MotionStartedDriver,
            });
            deviceEvents.Add(new EventDefinition()
            {
                ReferenceId = Constants.AnalyticsEventReferenceId,
                DisplayName = "Analytics Event LPR (Demo)",
                NameReferenceId = Constants.ResourceAnalyticsEventNameReferenceId,   
                CounterReferenceId = Guid.Empty,
            });
            return deviceEvents;
        }

        private static PtzSupport BuildPtzSupport()
        {
            PtzMoveSupport moveSupport = new PtzMoveSupport()
            {
                AbsoluteSupport = true,
                AutomaticSupport = false,
                RelativeSupport = true,
                SpeedSupport = true,
                StartSupport = true,
                StopSupport = true,
            };

            PtzMoveSupport moveSupportZoom = new PtzMoveSupport()
            {
                AbsoluteSupport = true,
                AutomaticSupport = false,
                RelativeSupport = true,
                SpeedSupport = false,
                StartSupport = true,
                StopSupport = true,
            };

            PresetSupport presetSupport = new PresetSupport()
            {
                AbsoluteSpeedSupport = false,
                LoadFromDeviceSupport = true,
                QueryAbsolutePositionSupport = true,
                SetPresetSupport = true,
                SpeedSupport = true,
            };

            PtzSupport ptzSupport = new PtzSupport()
            {
                CenterSupport = true,
                DiagonalSupport = true,
                HomeSupport = true,
                RectangleSupport = true,
                PanSupport = moveSupport,
                TiltSupport = moveSupport,
                ZoomSupport = moveSupportZoom,
                PresetSupport = presetSupport,
            };

            return ptzSupport;
        }
    }
}
