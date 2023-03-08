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
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, string.Format("{0}:{1}", nameof(ActivatePreset), presetId));
        }

        public override void AddPreset(string deviceId, string presetId)
        {
            WriteLine(deviceId, string.Format("{0}:{1}", nameof(AddPreset), presetId));
            _presets.Add(presetId);
        }

        public override void UpdatePreset(string deviceId, string presetId)
        {
            if (!_presets.Contains(presetId))
                throw new ArgumentException("Preset not found", "presetId");
            WriteLine(deviceId, string.Format("{0}:{1}", nameof(UpdatePreset), presetId));
        }

        public override void DeletePreset(string deviceId, string presetId)
        {
            if (!_presets.Contains(presetId))
                throw new ArgumentException("Preset not found", "presetId");
            _presets.Remove(presetId);
            WriteLine(deviceId, string.Format("{0}:{1}", nameof(DeletePreset), presetId));
        }

        public override void MoveStart(string deviceId, PtzMoveStartData ptzargs)
        {
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, nameof(MoveStart));
        }

        public override void MoveStop(string deviceId)
        {
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, nameof(MoveStop));
        }

        public override void MoveAbsolute(string deviceId, PtzMoveAbsoluteData ptzargs)
        {
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, nameof(MoveAbsolute));
        }

        public override void MoveRelative(string deviceId, string direction)
        {
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, string.Format("{0} - {1}", nameof(MoveRelative), direction));
        }

        public override void MoveHome(string deviceId)
        {
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, nameof(MoveHome));
        }

        public override void CenterAndZoomToRectangle(string deviceId, PtzRectangleData data)
        {
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, nameof(CenterAndZoomToRectangle));
        }

        public override void CenterOnPositionInView(string deviceId, PtzCenterData data)
        {
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, nameof(CenterOnPositionInView));
        }

        public override PtzGetAbsoluteData GetAbsolutePosition(string deviceId)
        {
            if (new Guid(deviceId) == Constants.Camera1)
            {
                WriteLine(deviceId, nameof(GetAbsolutePosition));
                return new PtzGetAbsoluteData() { Pan = 0.5, Tilt = 0.6, Zoom = 1.0 };
            }
            throw new MIPDriverException("Device does not support PTZ");
        }

        public override void LensCommand(string deviceId, string command)
        {
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, string.Format("{0} - {1}", nameof(LensCommand), command));
        }

        public override void SetAux(string deviceId, int auxNumber, bool enable)
        {
            if (new Guid(deviceId) != Constants.Camera1)
                throw new MIPDriverException("Device does not support PTZ");
            WriteLine(deviceId, string.Format("Aux {0} - {1}", auxNumber, enable?"enabled":"disabled"));
        }

        private void WriteLine(string deviceId, string info)
        {
            Container.ConnectionManager.SendInfo(deviceId, "PTZ "+info);
        }
    }
}
