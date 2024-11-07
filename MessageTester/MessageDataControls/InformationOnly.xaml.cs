namespace MessageTester.MessageDataControls
{
    /// <summary>
    /// Interaction logic for InformationOnly.xaml
    /// </summary>
    public partial class InformationOnlyUserControl : MessageDataSuper
    {

        public InformationOnlyUserControl(string informationText)
        {
            InitializeComponent();
            _text.Text = informationText;
            IsReadyToSend = true;
        }
    }
}
