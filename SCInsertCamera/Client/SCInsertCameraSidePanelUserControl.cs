using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.UI;
using Message = VideoOS.Platform.Messaging.Message;

namespace SCInsertCamera.Client
{
	/// <summary>
	/// This UserControl to be displayed in a side panel in the Smart Client in live mode.<br/>
	/// The user can create a view formatted defined by this plug-in, and continue to update it with new cameras.
	/// </summary>
	public partial class SCInsertCameraSidePanelUserControl : SidePanelUserControl
	{

		private object _selectedViewChanged;
		private ViewAndLayoutItem _currentView;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private Item _selectedCamera;

		public SCInsertCameraSidePanelUserControl()
		{
			InitializeComponent();
		}

		public override void Init()
		{
			_selectedViewChanged =
				EnvironmentManager.Instance.RegisterReceiver(ViewChangedHandler, new MessageIdFilter(MessageId.SmartClient.SelectedViewChangedIndication));
		}
		public override void Close()
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_selectedViewChanged);
		}

		/// <summary>
		/// This method keeps an eye with what view layout the user is selecting, 
		/// stores it under _currentView, and show the name in the read-only textBoxLayoutName textbox.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="s"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		private object ViewChangedHandler(Message message, FQID s, FQID r)
		{
			_currentView = message.Data as ViewAndLayoutItem;

		    UpdateIndexes();
			return null;
		}

	    /// <summary>
		/// To Select a single camera
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSelectCameraClick(object sender, EventArgs e)
		{
			ItemPickerForm form = new ItemPickerForm();
			form.AutoAccept = true;
			form.KindFilter = Kind.Camera;
			form.Init();
			if (form.ShowDialog() == DialogResult.OK)
			{
				_selectedCamera = form.SelectedItem;
				buttonSelect.Text = _selectedCamera.Name;

				buttonInsert.Enabled = true;

				StreamDataSource streamDataSource = new StreamDataSource(_selectedCamera);
				List<DataType> streams = streamDataSource.GetTypes();
	            foreach (DataType stream in streams)
	            {
		            comboBoxStream.Items.Add(stream);
	            }
				comboBoxStream.Enabled = streams.Count > 0;
			}
		}

		/// <summary>
		/// This method does all what is required to create a group and viewlayout.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCreateClick(object sender, EventArgs e)
		{
			List<Item> groups = ClientControl.Instance.GetViewGroupItems();
			if (groups.Count < 1)
				throw new MIPException("You do not have access to any groups?");

			// Use the first one (Private group)
			ConfigItem topGroupItem = (ConfigItem) groups[0];

			ConfigItem groupItem = (ConfigItem) FindName("ScInsertGroup", topGroupItem.GetChildren());
			if (groupItem == null)
			{
				// Make a group 
				groupItem = topGroupItem.AddChild("ScInsertGroup", Kind.View, FolderType.UserDefined);
			}

			// Figure out a name that does not exist
			List<Item> currentViews = groupItem.GetChildren();
			int ix = 0;
			while (FindName("ScInsertLayout-" + ix, currentViews) != null)
				ix++;

			// Build a layout with one wide ViewItem at top and buttom, and 6 small once in the middle
			Rectangle[] rect = new Rectangle[7];
			rect[0] = new Rectangle(000, 000, 999, 399);
			rect[1] = new Rectangle(000, 399, 199, 199);
			rect[2] = new Rectangle(199, 399, 199, 199);
			rect[3] = new Rectangle(399, 399, 199, 199);
			rect[4] = new Rectangle(599, 399, 199, 199);
			rect[5] = new Rectangle(799, 399, 199, 199);
			rect[6] = new Rectangle(000, 599, 999, 399);
			ViewAndLayoutItem viewAndLayoutItem =
				(ViewAndLayoutItem) groupItem.AddChild("ScInsertLayout-" + ix, Kind.View, FolderType.No);
			viewAndLayoutItem.Layout = rect;
			viewAndLayoutItem.Properties["Created"] = DateTime.Now.ToLongDateString();

			// Insert a HTML ViewItem at top
			Dictionary<String, String> properties = new Dictionary<string, string>();
			properties.Add("URL", "www.google.com");		// Next 4 for a HTML item
			properties.Add("HideNavigationbar", "true");
			properties.Add("Scaling", "3");					// fit in 800x600
			//properties.Add("Addscript", "");				// In case you need to add script
			viewAndLayoutItem.InsertBuiltinViewItem(0, ViewAndLayoutItem.HTMLBuiltinId, properties);

			// Find any camera and insert in index 1
			properties = new Dictionary<string, string>();
			properties.Add("CameraId", JustGetAnyCamera(Configuration.Instance.GetItems(ItemHierarchy.SystemDefined)));
			viewAndLayoutItem.InsertBuiltinViewItem(1, ViewAndLayoutItem.CameraBuiltinId, properties);

			// Insert a hotspot ViewItem at bottom -- index 6
			properties = new Dictionary<string, string>();
			viewAndLayoutItem.InsertBuiltinViewItem(6, ViewAndLayoutItem.HotspotBuiltinId, properties);

			viewAndLayoutItem.Save();
			topGroupItem.PropertiesModified();
			List<Item> windows = Configuration.Instance.GetItemsByKind(Kind.Window);
			FQID mainWindowFQID = new FQID(new ServerId("SC", "", 0, Guid.Empty));
			if (windows.Count > 0)
				mainWindowFQID = windows[0].FQID;
			EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.MultiWindowCommand,
																new MultiWindowCommandData()
																{
																	MultiWindowCommand = MultiWindowCommand.SetViewInWindow,
																	View = viewAndLayoutItem.FQID,
																	Window = mainWindowFQID
																}));			
		}

		private Item FindName(String name, List<Item> items)
		{
			foreach (Item item in items)
				if (item.Name == name)
					return item;
			return null;
		}

        private void _temporaryInsertCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateIndexes();
        }

        private void UpdateIndexes()
        {
            if (_currentView != null)
            {
                textBoxLayoutName.Text = _currentView.Name;
                comboBoxIndex.Items.Clear();

                for (int ix = 0; ix < _currentView.Layout.Length; ix++)
                {
                    if (!_temporaryInsertCheckBox.Checked || _currentView.ViewItemId(ix) == ViewAndLayoutItem.CameraBuiltinId)
                    {
                        comboBoxIndex.Items.Add("Index " + ix);
                    }
                }
                if (comboBoxIndex.Items.Count > 0)
                {
                    comboBoxIndex.SelectedIndex = 0;
                }
            }
        }

		/// <summary>
		/// This method will just find the first camera and return it's ObjectId as a string
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		private string JustGetAnyCamera(List<Item> items)
		{
			foreach (Item item in items)
			{
				if (item.FQID.Kind == Kind.Camera && item.FQID.FolderType == FolderType.No)
				{
					return item.FQID.ObjectId.ToString();
				}
				if (item.FQID.Kind == Kind.Server || item.FQID.Kind == Kind.Camera || item.FQID.Kind == Kind.Folder)
				{
					string child = JustGetAnyCamera(item.GetChildren());
					if (child != "")
						return child;
				}
			}
			return "";
		}

		/// <summary>
		/// Insert the selected camera in the select index of the current view
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnInsert(object sender, EventArgs e)
		{
			if (_selectedCamera!=null && _currentView!=null && comboBoxIndex.SelectedItem!=null)
			{
				// Make sure we are inside array boundary
				string itemString = comboBoxIndex.SelectedItem.ToString();
			    int ix = -1;
			    int.TryParse(itemString.Substring(itemString.LastIndexOf(" ")), out ix);
				Guid streamId = Guid.Empty;
				DataType dataType = comboBoxStream.SelectedItem as DataType;
				if (dataType != null)
					streamId = dataType.Id;

				if (ix>=0 && ix<_currentView.Layout.Length)
				{
                    if(_temporaryInsertCheckBox.Checked)
                    {
                        //Change the camera temporary in the camera view item. Just change the id.
                        EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.SetCameraInViewCommand, new SetCameraInViewCommandData()
                        {
	                        Index = ix, 
							CameraFQID = _selectedCamera.FQID,
							StreamId = streamId
                        }));
                    }
                    else
                    {
                        // Create, insert and save a new CameraViewItem at selected index. This insert is stored permenantly on the view
                        Dictionary<string, string> properties = new Dictionary<string, string>();
                        properties.Add("CameraId", _selectedCamera.FQID.ObjectId.ToString());
						properties.Add("StreamId", streamId.ToString());
						_currentView.InsertBuiltinViewItem(ix, ViewAndLayoutItem.CameraBuiltinId, properties);
                        _currentView.Save();
                    }
				}
			}
		}

        private void buttonClear_Click(object sender, EventArgs e)
        {
            // Make sure we are inside array boundary
            string itemString = comboBoxIndex.SelectedItem.ToString();
            int ix = -1;
            int.TryParse(itemString.Substring(itemString.LastIndexOf(" ")), out ix);
            if (ix >= 0 && ix < _currentView.Layout.Length)
            {
                if (_temporaryInsertCheckBox.Checked)
                {
                    //Change the camera temporary in the camera view item. Just change the id.
                    EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.SetCameraInViewCommand, new SetCameraInViewCommandData() { Index = ix, CameraFQID = null }));
                }
                else
                {
                    // Create, insert and save a new CameraViewItem at selected index. This insert is stored permenantly on the view
                    Dictionary<string, string> properties = new Dictionary<string, string>();
                    _currentView.InsertBuiltinViewItem(ix, ViewAndLayoutItem.EmptyBuiltinId, properties);
                    _currentView.Save();
                }
            }
        }

	}
}
