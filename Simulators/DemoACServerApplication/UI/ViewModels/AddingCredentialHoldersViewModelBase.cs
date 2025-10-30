using DemoServerApplication.Configuration;
using DemoServerApplication.UI.ViewModels.Commands;
using System;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel base class for add credential holder(s) classes.
    /// </summary>
    public abstract class AddingCredentialHoldersViewModelBase : ViewModelBase
    {
        protected AddingCredentialHoldersViewModelBase()
        {
            OKCommand = new DelegateCommand(_ => this.OK());
            CancelCommand = new DelegateCommand(_ => this.Close());
        }

        public DelegateCommand OKCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public event EventHandler CancelRequest;
        protected void Close()
        {
            EventHandler handler = CancelRequest;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected abstract void OK();

    }
}
