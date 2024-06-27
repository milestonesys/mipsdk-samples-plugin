using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using VideoOS.Platform.Util;

namespace MessageTester.MessageDataControls
{
    /// <summary>
    /// Interaction logic for AddToExportCommandUserControl.xaml
    /// </summary>
    public partial class AddToExportCommandUserControl : MessageDataSuper
    {
        public AddToExportCommandUserControl()
        {
            InitializeComponent();
        }

        public IList<ExportRow> ExportRows { get; } = new ObservableCollection<ExportRow>();

        public ExportRow SelectedRow { get; set; }

        public bool ShowConfirmationToasts { get; set; } = true;

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            var item = ShowItemPickerForm();

            if (item != null)
            {
                ExportRows.Add(new ExportRow(item));
                IsReadyToSend = true;
            }
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (SelectedRow != null)
            {
                ExportRows.Remove(SelectedRow);
                IsReadyToSend = ExportRows.Any();
            }
        }

        private static Item ShowItemPickerForm()
        {
            var form = new ItemPickerWpfWindow
            {
                Items = Configuration.Instance.GetItemsByKind(Kind.Camera),
                KindsFilter = new List<Guid> { Kind.Camera },
                SelectionMode = SelectionModeOptions.AutoCloseOnSelect
            };
            if(form.ShowDialog().Value)
            {
                return form.SelectedItems.First();
            }
            return null;
        }

        public override object Data => new AddToExportCommandData
        {
            ItemsToExport = ExportRows.Select(x => (x.Item.FQID, new TimeInterval(x.StartTime.ToUniversalTime(), x.EndTime.ToUniversalTime()))),
            ShowConfirmationToasts = ShowConfirmationToasts
        };
    }

    public class ExportRow
    {
        public ExportRow(Item item)
        {
            Item = item;
            StartTime = DateTime.Now.Subtract(TimeSpan.FromHours(1));
            EndTime = DateTime.Now;
        }

        public Item Item { get; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
