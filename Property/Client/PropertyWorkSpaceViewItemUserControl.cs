using System;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace Property.Client
{
    public partial class PropertyWorkSpaceViewItemUserControl : ViewItemUserControl
    {
        private PropertyWorkSpaceViewItemManager _wsViewItemMgr;
        
        public PropertyWorkSpaceViewItemUserControl(PropertyWorkSpaceViewItemManager wsViewItemMgr)
        {
            _wsViewItemMgr = wsViewItemMgr;
            InitializeComponent();

            ClientControl.Instance.RegisterUIControlForAutoTheming(this);
        }

        public override void Init()
        {
            textBoxShareGlobal.Text = _wsViewItemMgr.MyPropShareGlobal;
            textBoxSharedUser.Text = _wsViewItemMgr.MyPropSharePrivate;
            PropertyDefinition.SharedPropertyChanged += PropertyDefinition_SharedPropertyChanged;
        }


        public override void Close()
        {
            PropertyDefinition.SharedPropertyChanged -= PropertyDefinition_SharedPropertyChanged;
            _wsViewItemMgr.MyPropShareGlobal = textBoxShareGlobal.Text;
            _wsViewItemMgr.MyPropSharePrivate = textBoxSharedUser.Text;
        }

        /// <summary>
        /// Do not show the sliding toolbar!
        /// </summary>
        public override bool ShowToolbar
        {
            get { return false; }
        }

        private void PropertyDefinition_SharedPropertyChanged(object sender, System.EventArgs e)
        {
            textBoxShareGlobal.Text = _wsViewItemMgr.MyPropShareGlobal;
            textBoxSharedUser.Text = _wsViewItemMgr.MyPropSharePrivate;
        }

        private void ViewItemUserControlClick(object sender, EventArgs e)
        {
            FireClickEvent();
        }

        private void ViewItemUserControlDoubleClick(object sender, EventArgs e)
        {
            FireDoubleClickEvent();
        }

        public string MyPropShareGlobal
        {
            set { textBoxShareGlobal.Text = value ?? ""; }
            get { return textBoxShareGlobal.Text; }
        }
        
        public string MyPropSharePrivate
        {
            set { textBoxSharedUser.Text = value ?? ""; }
            get { return textBoxSharedUser.Text; }
        }

        private void textBoxShareGlobal_TextChanged(object sender, EventArgs e)
        {
            _wsViewItemMgr.MyPropShareGlobal = textBoxShareGlobal.Text;
        }

        private void textBoxSharedUser_TextChanged(object sender, EventArgs e)
        {
            _wsViewItemMgr.MyPropSharePrivate = textBoxSharedUser.Text;
        }
    }
}
