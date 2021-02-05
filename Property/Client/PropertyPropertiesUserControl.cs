using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace Property.Client
{
    /// <summary>
    /// </summary>
    public partial class PropertyPropertiesUserControl : PropertiesUserControl
    {

        #region private fields

        private PropertyViewItemManager _viewItemManager;

        #endregion

        #region Initialization & Dispose

        /// <summary>
        /// This class is created by the ViewItemManager.  
        /// </summary>
        /// <param name="viewItemManager"></param>
        public PropertyPropertiesUserControl(PropertyViewItemManager viewItemManager)
        {
            _viewItemManager = viewItemManager;
            InitializeComponent();
        }

        /// <summary>
        /// Setup events and message receivers and load stored configuration.
        /// </summary>
        public override void Init()
        {
            textBoxPropValue.Text = _viewItemManager.MyPropValue;
            textBoxShareGlobal.Text = _viewItemManager.MyPropShareGlobal;
            textBoxSharedUser.Text = _viewItemManager.MyPropSharePrivate;
        }

        /// <summary>
        /// Perform any cleanup stuff and event -=
        /// </summary>
        public override void Close()
        {
            _viewItemManager.MyPropValue = textBoxPropValue.Text;
            _viewItemManager.MyPropShareGlobal=textBoxShareGlobal.Text;
            _viewItemManager.MyPropSharePrivate=textBoxSharedUser.Text;

            // Note when you also have a ViewItemUserControl that update the same properties
            // this one will have no effect. You must choose to have either the properties 
            // maintained in the ViewItemUserControl or the PropertiesUserControl
        }

        #endregion
    }

}
