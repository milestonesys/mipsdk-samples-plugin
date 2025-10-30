using DemoServerApplication.Configuration;
using System.Windows.Media.Imaging;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for credential holder.
    /// </summary>
    public class CredentialHolderViewModel : ViewModelBase
    {
        public CredentialHolderViewModel(CredentialHolder credentialHolder)
        {
            CredentialHolder = credentialHolder;
        }

        public CredentialHolder CredentialHolder { get; private set; }

        public BitmapImage Picture { get { return CredentialHolder.GetPicture(); } }
    }
}
