using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoOS.Platform;
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
