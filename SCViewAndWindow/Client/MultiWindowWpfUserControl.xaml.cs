using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;

namespace SCViewAndWindow.Client
{
    public partial class MultiWindowWpfUserControl : System.Windows.Controls.UserControl
    {

        private Item _selectedView;


        #region Component constructors + dispose

        public MultiWindowWpfUserControl()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            List<Item> config = Configuration.Instance.GetItemConfigurations(
                SCViewAndWindowDefinition.SCViewAndWindowPluginId, null, SCViewAndWindowDefinition.SCViewAndWindowKind);
            if (config != null)
            {
            }
            List<Item> list = Configuration.Instance.GetItemsByKind(Kind.Screen);
            foreach (Item item in list)
                comboBoxScreens.Items.Add(new TaggedItem(item));

            Type msgType = typeof(MultiWindowCommand);
            FieldInfo[] info = msgType.GetFields();
            foreach (FieldInfo type in info)
            {
                if (type.IsLiteral)
                {
                    String name = type.ToString();
                    name = name.Substring(name.LastIndexOf(" ") + 1);
                    listBox.Items.Add(name);
                }
            }
        }


        #endregion


        #region Component events


        private void OnSelectView(object sender, RoutedEventArgs e)
        {
            var form = new ItemPickerWpfWindow()
            {
                KindsFilter = new List<Guid>() { Kind.View },
                SelectionMode = SelectionModeOptions.SingleSelect,
                Items = ClientControl.Instance.GetViewGroupItems()
            };
            form.ShowDialog();
            if(form.SelectedItems != null && form.SelectedItems.Any())
            {
                _selectedView = form.SelectedItems.First();
                buttonSelect.Content = _selectedView.Name;
            }
        }

        private void OnFireCommand(object sender, RoutedEventArgs e)
        {
            TaggedItem screenItem = (TaggedItem)comboBoxScreens.SelectedItem;
            TaggedItem windowItem = (TaggedItem)comboBoxWindows.SelectedItem;
            if (screenItem == null || windowItem == null || _selectedView == null || listBox.SelectedItem==null)
            {
                System.Windows.MessageBox.Show("Please select Screen/Window in above dropdown boxes", "Error");
            }
            else
            {
                MultiWindowCommandData data = new MultiWindowCommandData();
                data.Screen = screenItem!=null ? screenItem.Item.FQID : null;
                data.Window = windowItem!=null ? windowItem.Item.FQID : null;
                data.View = _selectedView!=null ? _selectedView.FQID : null;
                data.X = 200;
                data.Y = 200;
                data.Height = 400;
                data.Width = 400;
                data.MultiWindowCommand = (string)listBox.SelectedItem;
                data.PlaybackSupportedInFloatingWindow = (bool)checkBoxPlayback.IsChecked;

                if (data.MultiWindowCommand == MultiWindowCommand.SetNextViewInWindow ||
                    data.MultiWindowCommand == MultiWindowCommand.SetPreviousViewInWindow)
                {
                    data.MultiWindowCommand = MultiWindowCommand.SelectWindow;
                    EnvironmentManager.Instance.SendMessage(
                        new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.MultiWindowCommand, data));
                    data.MultiWindowCommand = (string)listBox.SelectedItem;
                }
                EnvironmentManager.Instance.SendMessage(
                    new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.MultiWindowCommand, data),
                    null,
                    null);
            }
        }

        #endregion

        /// <summary>
        /// Gets all Smart Client windows and populates comboBoxWindows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadWindows(object sender, EventArgs e)
        {
            comboBoxWindows.Items.Clear();
            List<Item> list = Configuration.Instance.GetItemsByKind(Kind.Window);
            foreach (Item item in list)
                comboBoxWindows.Items.Add(new TaggedItem(item));
        }
    }
}

