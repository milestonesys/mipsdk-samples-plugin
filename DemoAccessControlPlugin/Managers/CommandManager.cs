using DemoAccessControlPlugin.Client;
using DemoAccessControlPlugin.Configuration;
using DemoAccessControlPlugin.Constants;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Managers
{
    /// <summary>
    /// The CommandManager is responsible for executing commands towards the access control system (ExecuteCommand).
    /// </summary>
    internal class CommandManager : ACCommandManager
    {
        private SystemProperties _systemProperties;
        private DemoClient _client;

        public CommandManager(SystemProperties systemProperties, DemoClient client)
        {
            _systemProperties = systemProperties;
            _client = client;
        }

        public void Close()
        {
        }

        /// <summary>
        /// Execute a command. Used when personalized log-in is not enabled.
        /// </summary>
        public override ACCommandResult ExecuteCommand(string operationableInstance, string commandType, string vmsUsername)
        {
            // Execute the command using the admin user.
            return ExecuteCommand(operationableInstance, commandType, _systemProperties.AdminUser, _systemProperties.AdminPassword, vmsUsername);
        }

        /// <summary>
        /// Execute a command. Used when personalized log-in is enabled.
        /// </summary>
        public override ACCommandResult ExecuteCommand(string operationableInstance, string commandType, string username, string password, string vmsUsername)
        {
            try
            {
                if (commandType == CommandTypes.DoorLock.Id)
                {
                    _client.LockDoor(operationableInstance, username, password, vmsUsername);
                }
                else if (commandType == CommandTypes.DoorUnlock.Id)
                {
                    _client.UnlockDoor(operationableInstance, username, password, vmsUsername);
                }
                else
                {
                    return new ACCommandResult(false, "Invalid command.");
                }
            }
            catch (DemoApplicationClientException ex)
            {
                ACUtil.Log(true, "DemoACPlugin.CommandManager", "Error executing command " + commandType + " on door " + operationableInstance + ": " + ex.Message);
                return new ACCommandResult(false, ex.Message);
            }
            return new ACCommandResult(true);
        }
    }
}
