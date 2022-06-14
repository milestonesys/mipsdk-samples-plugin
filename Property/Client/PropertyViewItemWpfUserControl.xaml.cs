using System.Windows.Controls;
using VideoOS.Platform.Client;

namespace Property.Client
{
    /// <summary>
    /// The ViewItemUserControl is instantiated for every position it is created on the current visible view. When a user select another view or viewlayout, this class will be disposed.  No permanent settings can be saved in this class.
    /// The Init() method is called when the class is initiated and handle has been created for the UserControl. Please perform resource initialization in this method.
    /// The Close() method can be used to Dispose resources in a controlled manor.
    /// Mouse events not used by this control, should be passed on to the Smart Client by issuing the following methods:
    /// FireClickEvent() for single click
    ///FireDoubleClickEvent() for double click
    /// The single click will be interpreted by the Smart Client as a selection of the item, and the double click will be interpreted to expand the current viewitem to fill the entire View.
    /// </summary>
    public partial class PropertyViewItemWpfUserControl : ViewItemWpfUserControl
    {
        #region Component private class variables

        private PropertyViewItemManager _viewItemManager;

        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a PropertyViewItemUserControl instance
        /// </summary>
        public PropertyViewItemWpfUserControl(PropertyViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        private void SetUpApplicationEventListeners()
        {
            //set up ViewItem event listeners
            PropertyDefinition.SharedPropertyChanged += PropertyDefinition_SharedPropertyChanged;
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ViewItem event listeners
            PropertyDefinition.SharedPropertyChanged -= PropertyDefinition_SharedPropertyChanged;
        }

        /// <summary>
        /// Method that is called immediately after the view item is displayed.
        /// </summary>
        public override void Init()
        {
            textBoxPropValue.Text = _viewItemManager.MyPropValue;
            textBoxSharedGlobal.Text = _viewItemManager.MyPropShareGlobal;
            textBoxSharedUser.Text = _viewItemManager.MyPropSharePrivate;
            SetUpApplicationEventListeners();
        }

        /// <summary>
        /// Method that is called when the view item is closed. The view item should free all resources when the method is called.
        /// Is called when userControl is not displayed anymore. Either because of 
        /// user clicking on another View or Item has been removed from View.
        /// </summary>
        public override void Close()
        {
            RemoveApplicationEventListeners();
            _viewItemManager.MyPropValue = textBoxPropValue.Text;
            _viewItemManager.MyPropShareGlobal = textBoxSharedGlobal.Text;
            _viewItemManager.MyPropSharePrivate = textBoxSharedUser.Text;
        }

        #endregion

        #region Print method

        public override void Print()
        {
            Print("Name of this item", "Some extra information");
        }

        #endregion


        #region Component events

        private void PropertyDefinition_SharedPropertyChanged(object sender, System.EventArgs e)
        {
            textBoxSharedGlobal.Text = _viewItemManager.MyPropShareGlobal;
            textBoxSharedUser.Text = _viewItemManager.MyPropSharePrivate;
        }

        private void OnMouseLeftUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireClickEvent();
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FireDoubleClickEvent();
        }
        private void TextBoxSharedGlobal_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewItemManager.MyPropShareGlobal = textBoxSharedGlobal.Text;
        }

        private void TextBoxSharedUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewItemManager.MyPropSharePrivate = textBoxSharedUser.Text;
        }

        #endregion

        #region Component properties

        /// <summary>
        /// Gets boolean indicating whether the view item can be maximized or not. <br/>
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

