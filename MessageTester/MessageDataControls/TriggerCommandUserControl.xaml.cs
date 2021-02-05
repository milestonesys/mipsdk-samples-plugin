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
using VideoOS.Platform.UI;

namespace MessageTester.MessageDataControls
{
    /// <summary>
    /// Interaction logic for TriggerCommandUserControl.xaml
    /// </summary>
    public partial class TriggerCommandUserControl : MessageDataSuper
    {
        public TriggerCommandUserControl()
        {
            InitializeComponent();
        }

        public override FQID Destination { get { return _selectedDestinationItemFQID; } }
        public override FQID Related { get { return _selectedRelatedItemFQID; } }

        
        private FQID _selectedDestinationItemFQID;
        private FQID _selectedRelatedItemFQID = null;
        
        private void OnDestPicker(object sender, RoutedEventArgs e)
        {
            Item selectedItem;
            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = Kind.TriggerEvent;
            form.AutoAccept = true;
            form.Init(Configuration.Instance.GetItems());
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedItem = form.SelectedItem;
                _selectedDestinationItemFQID = selectedItem.FQID;
                _destinationFQIDLabel.Content = $"The selected destination is \"{selectedItem.Name}\"";
            }
            if (_selectedDestinationItemFQID != null)
            {
                IsReadyToSend = true;
            }
        }

        private void OnRelatedPicker(object sender, RoutedEventArgs e)
        {
            Item selectedItem;
            ItemPickerForm form = new ItemPickerForm();
            form.KindFilter = Kind.Camera;
            form.AutoAccept = true;
            form.Init(Configuration.Instance.GetItems());
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedItem = form.SelectedItem;
                _selectedRelatedItemFQID = selectedItem.FQID;
                _relatedFQIDLabel.Content = $"The selected related item is \"{selectedItem.Name}\"";
            }
        }
    }
}
