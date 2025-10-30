using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for DoorControllersOptionsWindow.xaml
    /// </summary>
    public partial class DoorControllersOptionsWindow : Window
    {
        public DoorControllersOptionsWindow()
        {
            InitializeComponent();
            DataContext = new DoorControllersOptionsWindowViewModel();
        }
    }
}
