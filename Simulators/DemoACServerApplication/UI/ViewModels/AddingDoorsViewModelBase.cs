using DemoServerApplication.Configuration;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel base for AddDoor(s) Window.
    /// </summary>
    public abstract class AddingDoorsViewModelBase : DoorViewModelBase
    {
        protected AddingDoorsViewModelBase()
            :base(Door.CreateTemplate())
        { }
    }
}
