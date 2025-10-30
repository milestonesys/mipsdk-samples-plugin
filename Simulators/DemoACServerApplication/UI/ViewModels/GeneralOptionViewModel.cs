namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for general option.
    /// </summary>
    public class GeneralOptionViewModel : OptionViewModelBase
    {
        public GeneralOptionViewModel(string optionName)
            : base(optionName)
        { }

        public string WebServicePortNumber { get { return "8732"; } }
    }
}
