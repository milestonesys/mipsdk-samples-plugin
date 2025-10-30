using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for EditCredentialHolderWindow.xaml
    /// </summary>
    public partial class EditCredentialHolderWindow : Window
    {
        public EditCredentialHolderWindow(CredentialHolderViewModel credentialHolderViewModel)
        {
            InitializeComponent();
            EditCredentialHolderWindowViewModel viewModel = new EditCredentialHolderWindowViewModel(credentialHolderViewModel);
            viewModel.CancelRequest += (sender, e) => this.Close();
            DataContext = viewModel;
        }
    }
}
