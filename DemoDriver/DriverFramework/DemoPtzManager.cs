using System;
using System.Collections.Generic;
using VideoOS.Platform.DriverFramework.Data.Ptz;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;

namespace DemoDriver
{
    public class DemoPtzManager : PtzManager
    {
        private List<string> _presets = new List<string>() { "Preset-3", "Preset-27" };

        private new DemoContainer Container => base.Container as DemoContainer;

        public DemoPtzManager(DemoContainer container) : base(container)
        {
        }

        public override ICollection<string> GetPresets(string deviceId)
        {
            if (new Guid(deviceId) == Constants.Camera1)
                return _presets;
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void ActivatePreset(string deviceId, string presetId)
        {
            
            WriteLine(deviceId, string.Format("{0}:{1}", nameof(ActivatePreset), presetId));
            if (new Guid(deviceId) == Constants.Camera1)
                return;
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void AddPreset(string deviceId, string presetId)
        {
            WriteLine(deviceId, string.Format("{0}:{1}", nameof(AddPreset), presetId));
            _presets.Add(presetId);
        }

        public override void UpdatePreset(string deviceId, string presetId)
        {

            WriteLine(deviceId, string.Format("{0}:{1}", nameof(UpdatePreset), presetId));
            if (!_presets.Contains(presetId))
                throw new ArgumentException("Preset not found", "presetId");
        }

        public override void DeletePreset(string deviceId, string presetId)
        {
            WriteLine(deviceId, string.Format("{0}:{1}", nameof(DeletePreset), presetId));
            if (!_presets.Contains(presetId))
                throw new ArgumentException("Preset not found", "presetId");
            _presets.Remove(presetId);
        }

        public override void MoveStart(string deviceId, PtzMoveStartData ptzargs)
        {
            WriteLine(deviceId, nameof(MoveStart));
            if (new Guid(deviceId) == Constants.Camera1)
                return;
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void MoveStop(string deviceId)
        {
            WriteLine(deviceId, nameof(MoveStop));
            if (new Guid(deviceId) == Constants.Camera1)
                return;
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void MoveAbsolute(string deviceId, PtzMoveAbsoluteData ptzargs)
        {
            WriteLine(deviceId, nameof(MoveAbsolute));
            if (new Guid(deviceId) == Constants.Camera1)
                return;
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void MoveRelative(string deviceId, string direction)
        {
            WriteLine(deviceId, string.Format("{0} - {1}", nameof(MoveRelative), direction));
            if (new Guid(deviceId) == Constants.Camera1)
                return;
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void MoveHome(string deviceId)
        {
            WriteLine(deviceId, nameof(MoveHome));
            if (new Guid(deviceId) == Constants.Camera1)
                return;
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void CenterAndZoomToRectangle(string deviceId, PtzRectangleData data)
        {
            WriteLine(deviceId, nameof(CenterAndZoomToRectangle));
            if (new Guid(deviceId) == Constants.Camera1)
                return;
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void CenterOnPositionInView(string deviceId, PtzCenterData data)
        {
            WriteLine(deviceId, nameof(CenterOnPositionInView));
            if (new Guid(deviceId) == Constants.Camera1)
                return;
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override PtzGetAbsoluteData GetAbsolutePosition(string deviceId)
        {
            WriteLine(deviceId, nameof(GetAbsolutePosition));
            if (new Guid(deviceId) == Constants.Camera1)
                return new PtzGetAbsoluteData() { Pan = 0.5, Tilt = 0.6, Zoom = 1.0 };
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void LensCommand(string deviceId, string command)
        {
            WriteLine(deviceId, string.Format("{0} - {1}", nameof(LensCommand), command));
            if (new Guid(deviceId) == Constants.Camera1)
                return;
            throw new MIPDriverException("Device does not support PTZ");
        }

        private void WriteLine(string deviceId, string info)
        {
            Container.ConnectionManager.SendInfo(deviceId, "PTZ "+info);
        }
    }
}
