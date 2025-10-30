using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for AddMultipleDoorsWindow.xaml
    /// </summary>
    public partial class AddMultipleDoorsWindow : Window
    {
        public AddMultipleDoorsWindow()
        {
            InitializeComponent();
            AddMultipleDoorsWindowViewModel viewModel = new AddMultipleDoorsWindowViewModel();
            viewModel.CancelRequest += (sender, e) => this.Close();
            DataContext = viewModel;
        }
    }
}
