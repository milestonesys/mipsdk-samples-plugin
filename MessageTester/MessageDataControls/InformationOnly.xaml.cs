using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MessageTester.MessageDataControls
{
    /// <summary>
    /// Interaction logic for InformationOnly.xaml
    /// </summary>
    public partial class InformationOnlyUserControl : MessageDataSuper
    {
        public InformationOnlyUserControl()
        {
            InitializeComponent();
        }

        public InformationOnlyUserControl(string informationText)
        {
            InitializeComponent();
            _text.Text = informationText;
            IsReadyToSend = true;
        }
    }
}
