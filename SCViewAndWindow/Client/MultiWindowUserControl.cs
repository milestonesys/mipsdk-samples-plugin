using System;
using System.Collections.Generic;		
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;			

namespace SCViewAndWindow.Client
{
	public partial class MultiWindowUserControl : UserControl
	{
		#region Component private class variables

		private Item _selectedView;

		#endregion

		#region Component constructors + dispose

		/// <summary>
		/// Constructs a SCViewAndWindowViewItemUserControl instance
		/// </summary>
		public MultiWindowUserControl()
		{
			InitializeComponent();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			if (!this.DesignMode)	
			{
				List<Item> config = Configuration.Instance.GetItemConfigurations(
					SCViewAndWindowDefinition.SCViewAndWindowPluginId, null,SCViewAndWindowDefinition.SCViewAndWindowKind);
				if (config != null)
				{
				}
				List<Item> list = Configuration.Instance.GetItemsByKind(Kind.Screen);
				foreach (Item item in list)
					comboBoxScreens.Items.Add(new TaggedItem(item));


				Type msgType = typeof (MultiWindowCommand);
				FieldInfo[] info = msgType.GetFields();
				foreach (FieldInfo type in info)
				{
					if (type.IsLiteral)
					{
						String name = type.ToString();
						name = name.Substring(name.LastIndexOf(" ") + 1);
						listBox1.Items.Add(name);
					}
				}
			}
			
		}

		#endregion

		#region Component events



		private void OnSelectView(object sender, EventArgs e)
		{
			ItemPickerForm form = new ItemPickerForm();
			form.AutoAccept = false;
			form.KindFilter = Kind.View;
			form.Init(ClientControl.Instance.GetViewGroupItems());
			if (form.ShowDialog() == DialogResult.OK)
			{
				_selectedView = form.SelectedItem;
				button1.Text = _selectedView.Name;
			}

		}

		private void OnFireCommand(object sender, EventArgs e)
		{
			TaggedItem screenItem = (TaggedItem)comboBoxScreens.SelectedItem;
			TaggedItem windowItem = (TaggedItem)comboBoxWindows.SelectedItem;
			//if (screenItem == null || windowItem == null || _selectedView == null || listBox1.SelectedItem==null)
			//{
			//	MessageBox.Show("Please select Screen/Window in above dropdown boxes", "Error");
			//} else
			{
				MultiWindowCommandData data = new MultiWindowCommandData();
				data.Screen = screenItem!=null? screenItem.Item.FQID : null;
				data.Window = windowItem!=null? windowItem.Item.FQID : null;
				data.View = _selectedView!=null? _selectedView.FQID : null;
				data.X = 200;
				data.Y = 200;
				data.Height = 400;
				data.Width = 400;
				data.MultiWindowCommand = (string)listBox1.SelectedItem;
			    data.PlaybackSupportedInFloatingWindow = checkBoxPlayback.Checked;

                if (data.MultiWindowCommand == MultiWindowCommand.SetNextViewInWindow ||
                    data.MultiWindowCommand == MultiWindowCommand.SetPreviousViewInWindow)
                {
                    data.MultiWindowCommand = MultiWindowCommand.SelectWindow;
                    EnvironmentManager.Instance.SendMessage(
                        new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.MultiWindowCommand, data));
                    data.MultiWindowCommand = (string)listBox1.SelectedItem;
                }
				EnvironmentManager.Instance.SendMessage(
					new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.MultiWindowCommand, data), 
					null, 
					null);
			}
		}

		private void OnEnterWindows(object sender, EventArgs e)
		{
			comboBoxWindows.Items.Clear();
			List<Item> list = Configuration.Instance.GetItemsByKind(Kind.Window);
			foreach (Item item in list)
				comboBoxWindows.Items.Add(new TaggedItem(item));
		}

		#endregion

	}

}

