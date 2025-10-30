using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for EditDoorWindow.xaml
    /// </summary>
    public partial class EditDoorWindow : Window
    {
        public EditDoorWindow(DoorViewModel selectedDoorViewModel)
        {
            InitializeComponent();
            EditDoorWindowViewModel viewModel = new EditDoorWindowViewModel(selectedDoorViewModel);
            viewModel.CancelRequest += (sender, e) => this.Close();
            DataContext = viewModel;
        }
    }
}
