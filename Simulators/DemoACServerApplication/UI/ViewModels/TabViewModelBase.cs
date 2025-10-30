namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel base class for MainWindow tabs.
    /// </summary>
    public abstract class TabViewModelBase : ViewModelBase
    {
        protected TabViewModelBase(string tabName)
        {
            TabName = tabName;
        }

        public string TabName { get; private set; }
    }
}
