using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid>() { Kind },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
                Items = Configuration.Instance.GetItems()
            };
            form.ShowDialog();
            if(form.SelectedItems != null && form.SelectedItems.Any())
            {
                var item = form.SelectedItems.First();
                _selectedItemFQID = item.FQID;
                _destinationFQIDLabel.Content = $"The selected destination is \"{ item.Name }\"";
                IsReadyToSend = true;
            }
        }
    }
}
