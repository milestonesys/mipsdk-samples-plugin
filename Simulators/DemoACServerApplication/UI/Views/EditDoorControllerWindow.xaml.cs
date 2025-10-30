using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for EditDoorControllerWindow.xaml
    /// </summary>
    public partial class EditDoorControllerWindow : Window
    {
        public EditDoorControllerWindow(DoorController selectedDoorController)
        {
            InitializeComponent();
            EditDoorControllerWindowViewModel viewModel = new EditDoorControllerWindowViewModel(selectedDoorController);
            viewModel.CancelRequest += (sender, e) => this.Close();
            DataContext = viewModel;
        }
    }
}
