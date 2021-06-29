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
