using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VideoOS.Platform.Client;

namespace Property.Client
{
    /// <summary>
    /// This UserControl is created by the PluginDefinition and placed on the Smart Client's options dialog when the user selects the options icon.<br/>
    /// The UserControl will be added to the owning parent UserControl and docking set to Fill.
    /// </summary>
    public partial class PropertyOptionsDialogUserControl : OptionsDialogUserControl
    {
        public PropertyOptionsDialogUserControl()
        {
            InitializeComponent();
            
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }


        /// <summary>
        /// Get and set the property value - controlled from the OptionDialogPlugin class
        /// </summary>
        public string MyPropValue
        {
            set { textBoxPropValue.Text = value??""; }
            get { return textBoxPropValue.Text; }
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
    }
}
