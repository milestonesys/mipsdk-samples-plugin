using System;
using System.Collections;
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
using VideoOS.Platform.UI;

namespace MessageTester.MessageDataControls
{
    /// <summary>
    /// Interaction logic for DestinationUserControl.xaml
    /// </summary>
    public partial class DestinationUserControl : MessageDataSuper
    {
        public DestinationUserControl()
        {
            InitializeComponent();
        }
        public DestinationUserControl(Guid kind)
        {
            Kind = kind;
            InitializeComponent();
            UpdateUIWithKind();          
        }


        public Guid Kind { get; set; }

        public override FQID Destination { get { return _selectedItemFQID; } }
        
        private FQID _selectedItemFQID;

        private void UpdateUIWithKind()
        {
            string kindName = GetName(Kind);
            if (kindName != String.Empty)
            {
                _destinationKindLabel.Content = $"Destination must be of kind: { kindName } \nThe ItemPicker can be used to find an item of that kind.";
            }
        }

        private string GetName(Guid kind)
        {
            string name = "";
            Hashtable table = VideoOS.Platform.Kind.DefaultTypeToNameTable;
            if (table.ContainsKey(kind))
            {
                name = (string)table[kind];
            }
            return name;
        }

        private void OnPicker(object sender, RoutedEventArgs e)
        {
            Item selectedItem;
            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = Kind;
            form.AutoAccept = true;
            form.Init(Configuration.Instance.GetItems());
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedItem = form.SelectedItem;
                _selectedItemFQID = selectedItem.FQID;
                _destinationFQIDLabel.Content = $"The selected destination is \"{selectedItem.Name}\"";
                IsReadyToSend = true;
            }
        }
    }
}
