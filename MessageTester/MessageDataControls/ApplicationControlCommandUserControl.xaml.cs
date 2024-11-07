using System;
using System.Reflection;
using System.Windows.Controls;
using VideoOS.Platform.Messaging;

namespace MessageTester.MessageDataControls
{
    /// <summary>
    /// Interaction logic for ApplicationControlCommandUserControl.xaml
    /// </summary>
    public partial class ApplicationControlCommandUserControl : MessageDataSuper
    {
        public ApplicationControlCommandUserControl()
        {
            
            InitializeComponent();
            Type msgType = typeof(ApplicationControlCommandData);
            FieldInfo[] info = msgType.GetFields();
            foreach (FieldInfo type in info)
            {
                if (type.IsLiteral)
                {
                    String name = type.ToString();
                    name = name.Substring(name.LastIndexOf(" ") + 1);  // avoid spaces
                    _commandsCB.Items.Add(name);
                }
            }
        }
        
        public override object Data { get { return _commandsCB.SelectedItem; } }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsReadyToSend = (_commandsCB.SelectedItem != null);
        }
    }
}
