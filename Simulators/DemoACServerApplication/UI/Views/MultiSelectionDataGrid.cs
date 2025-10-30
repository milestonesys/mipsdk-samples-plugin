using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Represents extended DataGrid control supporting multiple items selection.
    /// </summary>
    public class MultiSelectionDataGrid : DataGrid
    {
        public MultiSelectionDataGrid()
        {
            this.SelectionChanged += MultiSelectionDataGrid_SelectionChanged;
        }

        void MultiSelectionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
                DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(MultiSelectionDataGrid), new PropertyMetadata(null));
    }
}
