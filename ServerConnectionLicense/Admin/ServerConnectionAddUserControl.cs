using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServerConnection.Admin
{
	/// <summary>
	/// This control should contain the fields required for generating a valid Item.
	/// The template only has a Name field, but other fields should be added as required.
	/// 
	/// This dialog will be opened in a new form when user is selecting "New... " on the context menu in the administrator.
	/// When user presses "OK" the ItemManager.ValidateAddUserControl is called for validation, and if correct,
	/// the ItemManager.CreateItem is called with this class as the 3rd parameter.
	/// </summary>
	public partial class ServerConnectionAddUserControl : UserControl
	{
		public ServerConnectionAddUserControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// The name entered by the user
		/// </summary>
		public string ItemName
		{
			set { textBoxName.Text = value; }
			get { return textBoxName.Text; }
		}

        public bool IsValid { get { return !string.IsNullOrWhiteSpace(textBoxName.Text); } }
	}
}
