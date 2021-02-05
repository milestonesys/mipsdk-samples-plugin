using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using System.Drawing;
using VideoOS.Platform.Messaging;
using System.Linq;

namespace DynamicView.Background
{
    /// <summary>
    /// A background plugin will be started during application start and be running until the user logs off or application terminates.<br/>
    /// The Environment will call the methods Init() and Close() when the user login and logout, 
    /// so the background task can flush any cached information.<br/>
    /// The base class implementation of the LoadProperties can get a set of configuration, 
    /// e.g. the configuration saved by the Options Dialog in the Smart Client or a configuration set saved in one of the administrators.  
    /// Identification of which configuration to get is done via the GUID.<br/>
    /// The SaveProperties method can be used if updating of configuration is relevant.
    /// <br/>
    /// The configuration is stored on the server the application is logged into, and should be refreshed when the ApplicationLoggedOn method is called.
    /// Configuration can be user private or shared with all users.<br/>
    /// <br/>
    /// This plugin could be listening to the Message with MessageId == Server.ConfigurationChangedIndication to when when to reload its configuration.  
    /// This event is send by the environment within 60 second after the administrator has changed the configuration.
    /// </summary>
    public class DynamicViewBackgroundPlugin : BackgroundPlugin
    {
        private static int _viewCounter = 0;
        // Object handle for message listener
        private object _cameraListReceiver;

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return DynamicViewDefinition.DynamicViewBackgroundPlugin; }
        }

        /// <summary>
        /// The name of this background plugin
        /// </summary>
        public override String Name
        {
            get { return "DynamicView BackgroundPlugin"; }
        }

        /// <summary>
        /// Called by the Environment when the user has logged in.
        /// </summary>
        public override void Init()
        {
            // Register message receiver for alarm list message.
            _cameraListReceiver = EnvironmentManager.Instance.RegisterReceiver(
				new MessageReceiver(CameraListReceiver), 
				new MessageIdFilter("AlarmList.ObjectSelected.Information*"));
        }

        /// <summary>
        /// Called by the Environment when the user log's out.
        /// You should close all remote sessions and flush cache information, as the
        /// user might logon to another server next time.
        /// </summary>
        public override void Close()
        {
            EnvironmentManager.Instance.UnRegisterReceiver(_cameraListReceiver);
        }

        /// <summary>
        /// Define in what Environments the current background task should be started.
        /// </summary>
        public override List<EnvironmentType> TargetEnvironments
        {
            get { return new List<EnvironmentType>() { EnvironmentType.SmartClient }; }
        }

        /// <summary>
        /// Handles messages from the Alarm List.
        /// Creates a new view and displays it in a floating window each time a message is sent from the Alarm List.
        /// </summary>
        /// <param name="message">The message contains a list of alarms.</param>
        /// <param name="destination">Ignored since we want all messages from the Alarm List.</param>
        /// <param name="source">Ignored since we want messages from any Alarm List.</param>
        /// <returns>null - since the Alarm List is indifferent of who listens to its messages.</returns>
        private Object CameraListReceiver(VideoOS.Platform.Messaging.Message message, FQID destination, FQID source)
        {
            List<Alarm> alarms = message.Data as List<Alarm>;
            if (alarms != null)
            {
                int viewNumber = _viewCounter + 1;
                // Make a new Temporary view group
                ConfigItem tempGroupItem = (ConfigItem)ClientControl.Instance.CreateTemporaryGroupItem("Temporary" + viewNumber);
                // Make a group 
                ConfigItem groupItem = tempGroupItem.AddChild("DynamicViewGroup" + viewNumber, Kind.View, FolderType.UserDefined);
                // Build a layout with wide ViewItems at the top and buttom, and 5 small ones in the middle
                Rectangle[] rect = new Rectangle[7];
                rect[0] = new Rectangle(000, 000, 999, 399);
                rect[1] = new Rectangle(000, 399, 199, 199);
                rect[2] = new Rectangle(199, 399, 199, 199);
                rect[3] = new Rectangle(399, 399, 199, 199);
                rect[4] = new Rectangle(599, 399, 199, 199);
                rect[5] = new Rectangle(799, 399, 199, 199);
                rect[6] = new Rectangle(000, 599, 999, 399);
                int capacity = rect.Length;
                string viewName = "DynamicView" + viewNumber;
                ViewAndLayoutItem viewAndLayoutItem = groupItem.AddChild(viewName, Kind.View, FolderType.No) as ViewAndLayoutItem;
                viewAndLayoutItem.Layout = rect;

                // Insert cameras
                int index = 0;
                foreach (Alarm alarm in alarms)
                {
                    // Add camera from the triggering event
                    // Note: EventHeader.Source cannot be null because an alarm must have a source
                    if (index < capacity && alarm.EventHeader?.Source.FQID.Kind == Kind.Camera)
                    {
                        viewAndLayoutItem.InsertBuiltinViewItem(index++, 
                                                                ViewAndLayoutItem.CameraBuiltinId, 
                                                                new Dictionary<string, string>()
                                                                {
                                                                    { "CameraId", alarm.EventHeader.Source.FQID.ObjectId.ToString() }
                                                                });
                    }

                    if (alarm.ReferenceList == null)
                        continue;

                    // Add related cameras from the alarm
                    foreach (Reference rel in alarm.ReferenceList)
                    {
                        if (index < capacity && rel.FQID.Kind == Kind.Camera)
                        {
                            viewAndLayoutItem.InsertBuiltinViewItem(index++, 
                                                                    ViewAndLayoutItem.CameraBuiltinId, 
                                                                    new Dictionary<string, string>()
                                                                    {
                                                                        { "CameraId", rel.FQID.ObjectId.ToString() }
                                                                    });
                        }
                    }
                }

                if (index == 0)
                {
                    // Exit here, since no cameras were found in the alarm list
                    return null;
                }

                viewAndLayoutItem.Save();
                tempGroupItem.PropertiesModified();

                Item screen = Configuration.Instance.GetItemsByKind(Kind.Screen).FirstOrDefault();
                Item window = Configuration.Instance.GetItemsByKind(Kind.Window).FirstOrDefault();
                Item view = groupItem.GetChildren().FirstOrDefault(v => v.Name.Equals(viewName));
                if (screen == null || window == null || view == null)
                {
                    return null;
                }

                // Create floating window
                MultiWindowCommandData data = new MultiWindowCommandData();
                data.Screen = screen.FQID;
                data.Window = window.FQID;
                data.View = view.FQID;
                data.X = 200;
                data.Y = 200;
                data.Height = 500;
                data.Width = 500;
                data.MultiWindowCommand = "OpenFloatingWindow";
                data.PlaybackSupportedInFloatingWindow = true;
                EnvironmentManager.Instance.SendMessage(new VideoOS.Platform.Messaging.Message(MessageId.SmartClient.MultiWindowCommand, data), null, null);
                _viewCounter++;
            }
            return null;
        }
    }
}
