using DemoServerApplication.Configuration;

namespace DemoServerApplication.UI.ViewModels
{
    /// <summary>
    /// The MVVM ViewModel for AddMultipleDoorsWindow.
    /// </summary>
    public sealed class AddMultipleDoorsWindowViewModel : AddingDoorsViewModelBase
    {
        private int _doorCount;
        private bool _isRandomCoordinatesSelected;

        public AddMultipleDoorsWindowViewModel()
            : base()
        {
            // Setting initial value.
            DoorName = "Door ";
            DoorCount = 100;
        }

        public int DoorCount 
        {
            get { return _doorCount; }
            set
            {
                if (_doorCount != value)
                {
                    _doorCount = value;
                    OnPropertyChanged("DoorCount");
                }
            }
        }

        public bool IsRandomCoordinatesSelected
        {
            get { return _isRandomCoordinatesSelected; }
            set
            {
                if (_isRandomCoordinatesSelected != value)
                {
                    _isRandomCoordinatesSelected = value;
                    OnPropertyChanged("IsRandomCoordinatesSelected");
                    if (value)
                    {
                        Latitude = string.Empty;
                        Longitude = string.Empty;
                    }
                }
            }
        }

        protected override void OK()
        {
            ConfigurationManager.Instance.AddDoors(_door, _doorCount, setRandomCoordinates: _isRandomCoordinatesSelected);
            Close();
        }
    }
}
