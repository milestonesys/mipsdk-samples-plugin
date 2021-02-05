using System;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace Property.Client
{
    /// <summary>
    /// The ViewItemUserControl is instantiated for every position it is created on the current visible view. When a user select another view or viewlayout, this class will be disposed.  No permanent settings can be saved in this class.
    /// The Init() method is called when the class is initiated and handle has been created for the UserControl. Please perform resource initialization in this method.
    /// <br/>
    /// If Message communication is performed, register the MessageReceivers during the Init() method and UnRegister the receivers during the Close() method.
    /// <br/>
    /// The Close() method can be used to Dispose resources in a controlled manor.
    /// <br/>
    /// Mouse events not used by this control, should be passed on to the Smart Client by issuing the following methods:<br/>
    /// FireClickEvent() for single click<br/>
    ///	FireDoubleClickEvent() for double click<br/>
    /// The single click will be interpreted by the Smart Client as a selection of the item, and the double click will be interpreted to expand the current viewitem to fill the entire View.
    /// </summary>
    public partial class PropertyViewItemUserControl : ViewItemUserControl
    {
        #region Component private class variables

        private PropertyViewItemManager _viewItemManager;
        private object _themeChangedReceiver;

        #endregion

        #region Component constructors + dispose

        /// <summary>
        /// Constructs a PropertyViewItemUserControl instance
        /// </summary>
        public PropertyViewItemUserControl(PropertyViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;

            InitializeComponent();

            ClientControl.Instance.RegisterUIControlForAutoTheming(panelMain);

            panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
            panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
            
        }

        private void SetUpApplicationEventListeners()
        {
            //set up ViewItem event listeners
            _themeChangedReceiver = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ThemeChangedIndicationHandler),
                                             new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication));

            PropertyDefinition.SharedPropertyChanged += PropertyDefinition_SharedPropertyChanged;
        }

        private void RemoveApplicationEventListeners()
        {
            //remove ViewItem event listeners
            EnvironmentManager.Instance.UnRegisterReceiver(_themeChangedReceiver);
            _themeChangedReceiver = null;

            PropertyDefinition.SharedPropertyChanged -= PropertyDefinition_SharedPropertyChanged;
        }

        /// <summary>
        /// Method that is called immediately after the view item is displayed.
        /// </summary>
        public override void Init()
        {
            textBoxPropValue.Text = _viewItemManager.MyPropValue;
            textBoxShareGlobal.Text = _viewItemManager.MyPropShareGlobal;
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
            _viewItemManager.MyPropShareGlobal = textBoxShareGlobal.Text;
            _viewItemManager.MyPropSharePrivate = textBoxSharedUser.Text;
            // Note when you also have a PropertiesUserControl that update the same properties
            // that one will have no effect. This one will overrule the other.
            // You must choose to have either the properties maintained in the 
            // ViewItemUserControl or the PropertiesUserControl
        }

        #endregion

        #region Print method
        /// <summary>
        /// Method that is called when print is activated while the content holder is selected.
        /// Base implementation calls 'DrawToBitmap" to get a bitmap from the entire UserControl,
        /// and pass on to Print method.
        /// <code>
        /// Bitmap bitmap = new Bitmap(this.Width, this.Height);
        /// this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
        /// ClientControl.Instance.Print(bitmap, this.Name, "");
        /// bitmap.Dispose();
        /// </code>
        /// This implementation does not work for usercontrols with embedded ImageViewerControl.cs
        /// </summary>
        public override void Print()
        {
            Print("Name of this item", "Some extra information");
        }

        #endregion


        #region Component events

        private void PropertyDefinition_SharedPropertyChanged(object sender, System.EventArgs e)
        {
            textBoxShareGlobal.Text = _viewItemManager.MyPropShareGlobal;
            textBoxSharedUser.Text = _viewItemManager.MyPropSharePrivate;
        }

        private void ViewItemUserControlMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FireClickEvent();
            }
            else if (e.Button == MouseButtons.Right)
            {
                FireRightClickEvent(e);
            }
        }

        private void ViewItemUserControlMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FireDoubleClickEvent();
            }
        }


        /// <summary>
        /// Signals that the form is right clicked
        /// </summary>
        public event EventHandler RightClickEvent;

        /// <summary>
        /// Activates the RightClickEvent
        /// </summary>
        /// <param name="e">Event args</param>
        protected virtual void FireRightClickEvent(EventArgs e)
        {
            if (RightClickEvent != null)
            {
                RightClickEvent(this, e);
            }
        }

        private object ThemeChangedIndicationHandler(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            this.Selected = _selected;
            return null;
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

        /// <summary>
        /// Make support for Theme colors to show if this ViewItem is selected or not.
        /// </summary>
        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                if (value)
                {
                    panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderColor;
                    panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemSelectedHeaderTextColor;
                }
                else
                {
                    panelHeader.BackColor = ClientControl.Instance.Theme.ViewItemHeaderColor;
                    panelHeader.ForeColor = ClientControl.Instance.Theme.ViewItemHeaderTextColor;
                }
            }
        }




        #endregion

        private void textBoxShareGlobal_TextChanged(object sender, EventArgs e)
        {
            _viewItemManager.MyPropShareGlobal = textBoxShareGlobal.Text;
        }

        private void textBoxSharedUser_TextChanged(object sender, EventArgs e)
        {
            _viewItemManager.MyPropSharePrivate = textBoxSharedUser.Text;
        }

        private void OnClick(object sender, EventArgs e)
        {
            FireClickEvent();
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            FireDoubleClickEvent();
        }
    }
}
