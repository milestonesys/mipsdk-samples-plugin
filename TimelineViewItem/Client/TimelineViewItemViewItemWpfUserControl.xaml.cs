using VideoOS.Platform.Client;

namespace TimelineViewItem.Client
{
    /// <summary>
    /// The ViewItemUserControl is instantiated for every position it is created on the current visible view. When a user selects another view or viewlayout, this class will be disposed.  No permanent settings can be saved in this class.
    /// The Init() method is called when the class is initiated and handle has been created for the UserControl. Please perform resource initialization in this method.
    ///
    /// If Message communication is performed, register the MessageReceivers during the Init() method and UnRegister the receivers during the Close() method.
    /// 
    /// The Close() method can be used to Dispose resources in a controlled manner.
    /// 
    /// Mouse events not used by this control, should be passed on to the Smart Client by issuing the following methods:
    /// FireClickEvent() for single click
    ///	FireDoubleClickEvent() for double click
    /// The single click will be interpreted by the Smart Client as a selection of the item, and the double click will be interpreted to expand the current viewitem to fill the entire View.
    /// </summary>
    public partial class TimelineViewItemViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private TimelineViewItemViewItemManager _viewItemManager;
        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a TimelineViewItemViewItemUserControl instance
        /// </summary>
        public TimelineViewItemViewItemWpfUserControl(TimelineViewItemViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        /// <summary>
        /// Method that is called immediately after the view item is displayed.
        /// </summary>
        public override void Init()
        {
        }

        /// <summary>
        /// Method that is called when the view item is closed. The view item should free all resources when the method is called.
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
        }

        #endregion

        #region Print method
        
        public override void Print()
        {
            Print("Name of this item", "Some extra information");
        }

        #endregion


        #region Component events

        private void OnMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }

        #endregion

        #region Component properties

        /// <summary>
        /// Gets boolean indicating whether the view item can be maximized or not. 
        /// The content holder should implement the click and double click events even if it is not maximizable. 
        /// </summary>
        public override bool Maximizable
        {
            get { return true; }
        }

        /// <summary>
        /// Tell if ViewItem is selectable
        /// </summary>
        public override bool Selectable
        {
            get { return true; }
        }

        #endregion

    }
}
