using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using System.Drawing;
using VideoOS.Platform.Messaging;

namespace SCHotSpot.Background
{
    /// <summary>
    /// </summary>
    public class SCHotSpotBackgroundPlugin : BackgroundPlugin
    {
        private static int _viewCounter = 0;

        private bool _myMessageInProgress = false;
        // Object handle for message listener
        private object _cameraSelect;

		private FQID _myWindowFQID2 = null;
		private bool _top = true;

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return SCHotSpotDefinition.SCHotSpotBackgroundPlugin; }
        }

        /// <summary>
        /// The name of this background plugin
        /// </summary>
        public override String Name
        {
            get { return "SCHotSpot BackgroundPlugin"; }
        }

        /// <summary>
        /// Called by the Environment when the user has logged in.
        /// </summary>
        public override void Init()
        {
			_cameraSelect = EnvironmentManager.Instance.RegisterReceiver(new MessageReceiver(ViewItemSelected),
        	                                                                   new MessageIdFilter(
        	                                                                   	MessageId.SmartClient.
        	                                                                   		SelectedCameraChangedIndication));
        }

        /// <summary>
        /// Called by the Environment when the user log's out.
        /// You should close all remote sessions and flush cache information, as the
        /// user might logon to another server next time.
        /// </summary>
        public override void Close()
        {
			EnvironmentManager.Instance.UnRegisterReceiver(_cameraSelect);
		}

        /// <summary>
        /// Define in what Environments the current background task should be started.
        /// </summary>
        public override List<EnvironmentType> TargetEnvironments
        {
            get { return new List<EnvironmentType>() { EnvironmentType.SmartClient }; }
        }


		/// <summary>
		/// This method is called when the user selects a camera and inserts the selected camera in a floating hotspot window
		/// </summary>
		/// <param name="message"></param>
		/// <param name="dest"></param>
		/// <param name="sender"></param>
		/// <returns></returns>
		private object ViewItemSelected(Message message, FQID dest, FQID sender)
		{
            if (sender != null && _myWindowFQID2 != null && sender.ObjectId == _myWindowFQID2.ObjectId)       // Ignore my own setup messages
                return null;
            if (_myMessageInProgress)
                return null;

			if (message.MessageId == MessageId.SmartClient.SelectedCameraChangedIndication)
			{
				FQID cameraFQID = message.RelatedFQID;

				if (_myWindowFQID2 != null)
				{
                    _myMessageInProgress = true;
                    EnvironmentManager.Instance.SendMessage(new Message(MessageId.SmartClient.SetCameraInViewCommand,
																		new SetCameraInViewCommandData() { Index = _top?0:1, CameraFQID = cameraFQID }), _myWindowFQID2);
                    _myMessageInProgress = false;
					_top = !_top;
					return null;
				}

				if (cameraFQID == null)
					return null;

				_viewCounter++;
				// Make a new Temporary view group
				ConfigItem tempGroupItem = (ConfigItem)ClientControl.Instance.CreateTemporaryGroupItem("Floating Hotspot");
				// Make a group 
				ConfigItem groupItem = tempGroupItem.AddChild("SCHotSpotGroup" + _viewCounter, Kind.View, FolderType.UserDefined);
				// Build a layout with wide ViewItems at the top and buttom, and 5 small ones in the middle
				Rectangle[] rect = new Rectangle[2];
				rect[0] = new Rectangle(000, 000, 999, 499);
				rect[1] = new Rectangle(000, 499, 999, 500);
				string viewName = "SCHotSpot" + _viewCounter;
				ViewAndLayoutItem viewAndLayoutItem = (ViewAndLayoutItem)groupItem.AddChild(viewName, Kind.View, FolderType.No);
				viewAndLayoutItem.Layout = rect;
				// Insert cameras
				Dictionary<String, String> cameraViewItemProperties = new Dictionary<string, string>();
				cameraViewItemProperties.Add("CameraId", cameraFQID!=null?cameraFQID.ObjectId.ToString():Guid.Empty.ToString());
				viewAndLayoutItem.InsertBuiltinViewItem(0, ViewAndLayoutItem.CameraBuiltinId, cameraViewItemProperties);
				viewAndLayoutItem.Save();
				tempGroupItem.PropertiesModified();
				// Create floating window
				MultiWindowCommandData data = new MultiWindowCommandData();
				List<Item> screens = Configuration.Instance.GetItemsByKind(Kind.Screen);
				FQID screenFQID = GetFirstOfKind(screens, Kind.Screen);
				if (screenFQID != null)
				{
					data.Screen = screenFQID;
					List<Item> windows = Configuration.Instance.GetItemsByKind(Kind.Window);
					FQID windowFQID = GetFirstOfKind(windows, Kind.Window);
					if (windowFQID != null)
					{
						data.Window = windowFQID;
						foreach (Item view in groupItem.GetChildren())
						{
							if (view.Name.Equals(viewName))
							{
								data.View = view.FQID;
								data.X = 200;
								data.Y = 200;
								data.Height = 400;
								data.Width = 200;
                                _myMessageInProgress = true;
                                data.MultiWindowCommand = MultiWindowCommand.OpenFloatingWindow;
								Collection<object> result = EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.MultiWindowCommand, data), null, null);
								if (result != null && result.Count > 0)
									_myWindowFQID2 = result[0] as FQID;		// Save the Window FQID for next updates
                                _myMessageInProgress = false;
                                return null;
							}
						}
					}
				}
			}
			return null;
		}

		private FQID GetFirstOfKind(List<Item> items, Guid kind)
		{
			if (items!=null)
			{
				foreach (Item item in items)
				{
					if (item.FQID.Kind == kind)
						return item.FQID;
				}
			}
			return null;
		}
    }

}
