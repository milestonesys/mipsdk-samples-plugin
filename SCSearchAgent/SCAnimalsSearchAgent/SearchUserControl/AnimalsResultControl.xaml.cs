using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SCSearchAgent.SCAnimalsSearchAgent.SearchAgent;
using VideoOS.Platform.Search;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchUserControl
{
    /// <summary>
    /// Interaction logic for AnimalsResultControl.xaml
    /// </summary>
    public partial class AnimalsResultControl : SearchResultUserControl
    {
        public AnimalsResultControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ImageSource Image { get; set; }

        public string Species { get; set; }

        static Random _rnd = new Random();

        public override void Init(SearchResultData searchResultData)
        {
            var result = (AnimalsSearchResultData)searchResultData;
            Species = result.Species;

            // Load a random image for the given species
            Image = LoadBitmapImageResource($"{Species}{_rnd.Next(1,3).ToString()}.jpg");

            OnPropertyChanged(nameof(Image));
            OnPropertyChanged(nameof(Species));
        }

        private static BitmapImage LoadBitmapImageResource(string resourceName)
        {
            try
            {
                var s = $"pack://application:,,,/SCSearchAgent;component/SCAnimalsSearchAgent/SearchUserControl/Images/{resourceName}";
                return new BitmapImage(new Uri(s));
            }
            catch
            {
                return null;
            }
        }
    }
}
