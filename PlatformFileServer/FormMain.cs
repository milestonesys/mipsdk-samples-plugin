using System;
using System.Windows.Forms;

namespace PlatformFileServer
{
	public partial class FormMain : Form
	{
		private static FormMain _control;

		public FormMain(String openResult)
		{
			InitializeComponent();
			_control = this;

		    labelMessage.Text = openResult;
		}

		//Pass on to correct thread
		internal static void Display(string text)
		{
			_control.BeginInvoke(new DisplayTextDelegate(_control.DisplayText), new object[] {text});
		}

		//Add text to listbox
		private delegate void DisplayTextDelegate(string text);
		private void DisplayText(string text)
		{
			listBoxDebug.Items.Add(text);
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
