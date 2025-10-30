using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for AddCredentialHolderWindow.xaml
    /// </summary>
    public partial class AddCredentialHolderWindow : Window
    {
        public AddCredentialHolderWindow()
        {
            InitializeComponent();
            AddCredentialHolderWindowViewModel viewModel = new AddCredentialHolderWindowViewModel();
            viewModel.CancelRequest += (sender, e) => this.Close();
            DataContext = viewModel;
        }
    }
}
