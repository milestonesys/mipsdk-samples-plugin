using VideoOS.Platform.AccessControl.Alarms;
using DemoAccessControlPlugin.Configuration;
using VideoOS.Platform.AccessControl.Plugin;

namespace DemoAccessControlPlugin.Managers
{
    internal class SystemManagers
    {
        public CommandManager CommandManager { get; private set; }
        public ConfigurationManager ConfigurationManager { get; private set; }
        public ConnectionManager ConnectionManager { get; private set; }
        public CredentialHolderManager CredentialHolderManager { get; private set; }
        public EventManager EventManager { get; private set; }
        public StateManager StateManager { get; private set; }

        public SystemManagers(SystemProperties systemProperties, ConfigurationCache configurationCache, ACConfiguration configuration, IACAlarmRepository alarmRepository)
        {
            CommandManager = new CommandManager(this, configurationCache);
            ConfigurationManager = new ConfigurationManager(this, systemProperties, configurationCache, configuration);
            ConnectionManager = new ConnectionManager(this, alarmRepository, systemProperties);
            CredentialHolderManager = new CredentialHolderManager(this, systemProperties);
            EventManager = new EventManager(configurationCache);
            StateManager = new StateManager(this, configurationCache);
        }

        public void Init()
        {
            StateManager.Init();
        }

        public void Close()
        {
            ConfigurationManager.Close();
            ConnectionManager.Close();
            StateManager.Close();
        }
    }
}
