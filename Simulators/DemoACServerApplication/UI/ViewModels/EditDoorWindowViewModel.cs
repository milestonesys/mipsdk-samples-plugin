using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for EditDoorWindow.
    /// </summary>
    public sealed class EditDoorWindowViewModel : DoorViewModelBase
    {
        public EditDoorWindowViewModel(DoorViewModel selectedDoorViewModel)
            : base(selectedDoorViewModel)
        {
            RemoveDoorControllerCommand = new DelegateCommand(_ => this.RemoveDoorController());
        }

        public DelegateCommand RemoveDoorControllerCommand { get; private set; }

        private void RemoveDoorController()
        {
            SelectedDoorController = null;
        }

        protected override void OK()
        {
            ConfigurationManager.Instance.UpdateDoor(_door);
            Close();
        }
    }
}
