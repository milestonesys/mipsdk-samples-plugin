namespace MessageTester.MessageDataControls
{
    public class NoSupportUserControl : InformationOnlyUserControl
    {
        public NoSupportUserControl(string note = "No suppport for this message is currently in this sample, please consult documentation on how to use this message.") 
            : base(note)
        {
            IsReadyToSend = false;
        }
    }
}
