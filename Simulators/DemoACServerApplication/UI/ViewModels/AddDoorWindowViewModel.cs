using DemoServerApplication.Configuration;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for AddDoorWindow.
    /// </summary>
    public sealed class AddDoorWindowViewModel : AddingDoorsViewModelBase
    {
        public AddDoorWindowViewModel()
            : base()
        {
            // Setting initial value.
            DoorName = ConfigurationManager.Instance.GenerateDoorName("Door ");
        }

        protected override void OK()
        {
            ConfigurationManager.Instance.AddDoor(_door);
            Close();
        }
    }   
}
