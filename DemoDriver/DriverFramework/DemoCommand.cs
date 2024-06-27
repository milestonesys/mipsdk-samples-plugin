using VideoOS.Platform.DriverFramework.Managers;

namespace DemoDriver.DriverFramework
{
    public class DemoCommand : BaseCommand
    {
        public static readonly string Id = "DemoCommand";

        private readonly DemoConnectionManager _connectionManager;

        public DemoCommand(DemoConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override string Execute(string deviceId, string parameter)
        {
            return _connectionManager.SendCommand(deviceId, Id, parameter);
        }
    }
}
