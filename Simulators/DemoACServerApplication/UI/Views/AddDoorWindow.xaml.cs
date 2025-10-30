using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for AddDoorView.xaml
    /// </summary>
    public partial class AddDoorWindow : Window
    {
        public AddDoorWindow()
        {
            InitializeComponent();
            AddDoorWindowViewModel viewModel = new AddDoorWindowViewModel();
            viewModel.CancelRequest += (sender, e) => this.Close();
            DataContext = viewModel;
        }
    }
}
