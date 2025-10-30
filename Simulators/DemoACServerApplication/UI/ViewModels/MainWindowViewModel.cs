using System.Collections.ObjectModel;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for MainWindow.
    /// </summary>
    public sealed class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModelBase>
            {
                new SwipeCardsTabViewModel("Swipe cards"),
                new AdministrationTabViewModel("Administration")
            };
        }

        public ObservableCollection<TabViewModelBase> Tabs { get; private set; }
    }
}
