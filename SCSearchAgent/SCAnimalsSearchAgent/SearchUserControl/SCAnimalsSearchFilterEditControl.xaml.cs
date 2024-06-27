using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VideoOS.Platform.Search;
using VideoOS.Platform.Search.FilterValues;
using System.ComponentModel;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchUserControl
{
    /// <summary>
    /// Interaction logic for SCAnimalsSearchFilterEditControl.xaml
    /// </summary>
    public partial class SCAnimalsSearchFilterEditControl : SearchFilterEditControl, INotifyPropertyChanged
    {
        private int _selectedIndex;
        private ObservableCollection<Species> _species = new ObservableCollection<Species>()
        {
            new Species("Any"),
            new Species("Lion"),
            new Species("Elephant"),
            new Species("Giraffe"),
            new Species("Zebra"),
            new Species("Rhino"),
            new Species("Crocodile"),
        };

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Species> Species => _species;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    var av = (StringFilterValue)FilterValue;
                    av.Text = Species[_selectedIndex].Name;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
                }
            }
        }

        public override void Init()
        {
            SelectedIndex = FindIndexOfSpeciesName(((StringFilterValue)FilterValue).Text);
            FilterValue.Changed += FilterValue_Changed;
        }

        public override void Close()
        {
            FilterValue.Changed -= FilterValue_Changed;
        }

        public SCAnimalsSearchFilterEditControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void FilterValue_Changed(object sender, EventArgs e)
        {
            SelectedIndex = FindIndexOfSpeciesName(((StringFilterValue)FilterValue).Text);
        }

        private int FindIndexOfSpeciesName(string name)
        {
            var av = (StringFilterValue)FilterValue;
            Species species = Species.First(x => x.Name == av.Text);
            return Species.IndexOf(species);
        }
    }

    public class Species
    {
        public string Name { get; private set; }
        public ImageSource Image { get; private set; }

        public Species(string name)
        {
            Name = name;
            if(name != "Any")
                Image = LoadBitmapImageResource(name + ".png");
        }

        private static BitmapImage LoadBitmapImageResource(string resourceName)
        {
            try
            {
                var s = $"pack://application:,,,/SCSearchAgent;component/SCAnimalsSearchAgent/SearchUserControl/Images/Thumbnails/{resourceName}";
                return new BitmapImage(new Uri(s));
            }
            catch
            {
                return null;
            }
        }
    }
}
