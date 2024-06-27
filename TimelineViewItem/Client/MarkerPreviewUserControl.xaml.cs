using System;
using System.Windows.Controls;

namespace TimelineViewItem.Client
{
    public partial class MarkerPreviewUserControl : UserControl
    {
        private Guid _id;

        public MarkerPreviewUserControl()
        {
            InitializeComponent();
        }

        public Guid Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                _idTextBlock.Text = _id.ToString();
            }
        }
    }
}
