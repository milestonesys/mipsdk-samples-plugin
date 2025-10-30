namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel base for administration tab options.
    /// </summary>
    public abstract class OptionViewModelBase : ViewModelBase
    {
        protected OptionViewModelBase(string optionName)
        {
            OptionName = optionName;
        }

        public string OptionName { get; private set; }
    }
}
