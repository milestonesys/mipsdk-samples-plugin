using SCSearchAgent.SCPeopleWithAccessoriesSearchAgent.SearchAgent;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VideoOS.Platform.Search;

namespace SCSearchAgent.SCPeopleWithAccessoriesSearchAgent.SearchUserControl
{
    /// <summary>
    /// Interaction logic for SCPeopleWithAccessoriesSearchResultUserControl.xaml
    /// </summary>
    public partial class SCPeopleWithAccessoriesSearchResultUserControl : SearchResultUserControl
    {
        /// <summary>
        /// Image to display in the result user control.
        /// </summary>
        public ImageSource Image { get; set; }

        /// <summary>
        /// The accessory that the person has in the search result.
        /// </summary>
        public string Accessory { get; set; }

        private static Random _rnd = new Random();

        public SCPeopleWithAccessoriesSearchResultUserControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// Initializes the user control with the provided VideoOS.Platform.Search.SearchResultData.
        /// </summary>
        /// <param name="searchResultData">Search result to visualize.</param>
        public override void Init(SearchResultData searchResultData)
        {
            var result = (SCPeopleWithAccessoriesSearchResultData)searchResultData;
            Accessory = result.Accessory;

            // Load a random image for the given accessory type
            Image = LoadBitmapImageResource($"{Accessory}{_rnd.Next(1, 3).ToString()}.jpg");
            
            OnPropertyChanged(nameof(Image));
            OnPropertyChanged(nameof(Accessory));
        }

        private static BitmapImage LoadBitmapImageResource(string resourceName)
        {
            try
            {
                var s = $"pack://application:,,,/SCSearchAgent;component/SCPeopleWithAccessoriesSearchAgent/SearchUserControl/Images/{resourceName}";
                return new BitmapImage(new Uri(s));
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
