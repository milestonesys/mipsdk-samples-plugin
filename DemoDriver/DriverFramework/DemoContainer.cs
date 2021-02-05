using DemoDriver.DriverFramework;
using VideoOS.Platform.DriverFramework;

namespace DemoDriver
{
    public class DemoContainer : Container
    {
        public new DemoConnectionManager ConnectionManager => base.ConnectionManager as DemoConnectionManager;
        public new DemoStreamManager StreamManager => base.StreamManager as DemoStreamManager;

        public DemoContainer(DriverDefinition definition) 
            : base(definition)
        {
            base.StreamManager = new DemoStreamManager(this);
            base.PtzManager = new DemoPtzManager(this);
            base.OutputManager = new DemoOutputManager(this);
            base.SpeakerManager = new DemoSpeakerManager(this);
            base.PlaybackManager = new DemoPlaybackManager(this);
            base.ConnectionManager = new DemoConnectionManager(this);
            base.ConfigurationManager = new DemoConfigurationManager(this);

            CommandManager.RegisterCommand(DemoCommand.Id, new DemoCommand(ConnectionManager));
        }   
    }
}
