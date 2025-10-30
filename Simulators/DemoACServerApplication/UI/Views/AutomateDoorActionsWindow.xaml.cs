using DemoServerApplication.UI.ViewModels;
using System.Windows;

namespace DemoServerApplication.UI.Views
{
    /// <summary>
    /// Interaction logic for AutomateDoorActionsWindow.xaml
    /// </summary>
    public partial class AutomateDoorActionsWindow : Window
    {
        public static AutomateDoorActionsWindow Current;
        public int EventFrequency
        {
            get { return (DataContext as AutomateDoorActionsWindowViewModel).EventFrequency; }
            set { (DataContext as AutomateDoorActionsWindowViewModel).EventFrequency = value; }
        }

        public AutomateDoorActionsWindow()
        {
            InitializeComponent();
            AutomateDoorActionsWindowViewModel viewModel = new AutomateDoorActionsWindowViewModel();
            viewModel.CancelRequest += (sender, e) => this.Close();
            DataContext = viewModel;
            this.Closing += (sender, e) =>
            {
                viewModel.StopActionThread();
            };
            this.Closed += (sender, e) => { Current = null; };

            Current = this;
        }
    }
}
