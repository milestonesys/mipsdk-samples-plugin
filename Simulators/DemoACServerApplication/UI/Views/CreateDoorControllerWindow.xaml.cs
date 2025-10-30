using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for CreateDoorControllerWindow.xaml
    /// </summary>
    public partial class CreateDoorControllerWindow : Window
    {
        public CreateDoorControllerWindow()
        {
            InitializeComponent();
            CreateDoorControllerWindowViewModel viewModel = new CreateDoorControllerWindowViewModel();
            viewModel.CancelRequest += (sender, e) => this.Close();
            DataContext = viewModel;
        }
    }
}
