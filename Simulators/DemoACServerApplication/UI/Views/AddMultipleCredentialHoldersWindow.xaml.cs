using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for AddMultipleCredentialHoldersWindow.xaml
    /// </summary>
    public partial class AddMultipleCredentialHoldersWindow : Window
    {
        public AddMultipleCredentialHoldersWindow()
        {
            InitializeComponent();
            AddMultipleCredentialHoldersWindowViewModel viewModel = new AddMultipleCredentialHoldersWindowViewModel();
            viewModel.CancelRequest += (sender, e) => this.Close();
            DataContext = viewModel;
        }
    }
}
