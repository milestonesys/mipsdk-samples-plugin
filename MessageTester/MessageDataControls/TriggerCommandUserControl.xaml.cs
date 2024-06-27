using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid> { Kind.TriggerEvent },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
                Items = Configuration.Instance.GetItems()
            };
            form.ShowDialog();
            if(form.SelectedItems != null && form.SelectedItems.Any())
            {
                var item = form.SelectedItems.First();
                _selectedDestinationItemFQID = item.FQID;
                _destinationFQIDLabel.Content = $"The selected destination is \"{item.Name}\"";
            }
            if(_selectedDestinationItemFQID != null)
            {
                IsReadyToSend = true;
            }
        }

        private void OnRelatedPicker(object sender, RoutedEventArgs e)
        {
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid>() { Kind.Camera },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect,
                Items = Configuration.Instance.GetItems()
            };
            form.ShowDialog();
            if(form.SelectedItems == null || form.SelectedItems.Any())
            {
                var item = form.SelectedItems.First();
                _selectedDestinationItemFQID = item.FQID;
                _relatedFQIDLabel.Content = $"The selected related item is \"{ item.Name }\"";
            };
        }
    }
}
