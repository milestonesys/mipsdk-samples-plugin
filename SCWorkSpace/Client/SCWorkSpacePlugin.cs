using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Threading;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;
using Message = VideoOS.Platform.Messaging.Message;

namespace SCWorkSpace.Client
{
    public class SCWorkSpacePlugin : WorkSpacePlugin
    {
        public static Guid SCWorkSpacePluginId = new Guid("D347FA08-E941-4C64-B0F1-CED8E98DC564");

        private static readonly int MAX_MAX_CAMERA_COUNT = 10;

        private List<object> _messageRegistrationObjects = new List<object>();

        private DispatcherTimer _updateInformationTimer;

        private bool _workSpaceSelected = false;
        private bool _workSpaceViewSelected = false;

        bool _viewInitialized = false;

        public override Guid Id
        {
            get { return SCWorkSpacePluginId; }
        }

        public override string Name
        {
            get { return "WorkSpace Plugin"; }
        }

        public override bool IsSetupStateSupported
        {
            get { return true; }
        }

        public override void Init()
        {
            LoadProperties(true);

            //add message listeners
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(ShuffleCamerasReceiver, new MessageIdFilter(SCWorkSpaceDefinition.ShuffleCamerasMessage)));
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(MaxCamerasChangedReceiver, new MessageIdFilter(SCWorkSpaceDefinition.MaxCamerasChangedMessage)));
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(ShownWorkSpaceChangedReceiver, new MessageIdFilter(MessageId.SmartClient.ShownWorkSpaceChangedIndication)));
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(WorkSpaceStateChangedReceiver, new MessageIdFilter(MessageId.SmartClient.WorkSpaceStateChangedIndication)));
            _messageRegistrationObjects.Add(EnvironmentManager.Instance.RegisterReceiver(SelectedViewChangedReceiver, new MessageIdFilter(MessageId.SmartClient.SelectedViewChangedIndication)));

            //build view layout
            List<Rectangle> rectangles = new List<Rectangle>();
            for (int i = 0; i < MAX_MAX_CAMERA_COUNT; i++)
            {
                rectangles.Add(new Rectangle(1000 / MAX_MAX_CAMERA_COUNT * i, 1000 / MAX_MAX_CAMERA_COUNT * i, 1000 / MAX_MAX_CAMERA_COUNT, 1000 / MAX_MAX_CAMERA_COUNT));
            }
            rectangles.Add(new Rectangle(700, 0, 300, 300));
            rectangles.Add(new Rectangle(0, 600, 400, 400));
            ViewAndLayoutItem.Layout = rectangles.ToArray();
            ViewAndLayoutItem.Name = Name;

            //add viewitems to view layout
            List<Item> cameraItems = new List<Item>();
            for (int i = 0; i < MAX_MAX_CAMERA_COUNT; i++)
            {
                string configurationString = GetProperty("Camera" + i);
                if (String.IsNullOrWhiteSpace(configurationString))
                {
                    Dictionary<String, String> properties = new Dictionary<string, string>();
                    FindAllCameras(Configuration.Instance.GetItemsByKind(Kind.Camera), cameraItems);
                    properties.Add("CameraId", Guid.Empty.ToString());
                    ViewAndLayoutItem.InsertBuiltinViewItem(i, ViewAndLayoutItem.CameraBuiltinId, properties);
                }
                else
                {
                    ViewAndLayoutItem.InsertViewItem(i, configurationString);
                }
            }
            Dictionary<String, String> cameraDictionary = new Dictionary<string, string>();

            ViewAndLayoutItem.InsertViewItemPlugin(rectangles.Count - 2, new SCWorkSpaceViewItemPlugin(), new Dictionary<string, string>());
            ViewAndLayoutItem.InsertViewItemPlugin(rectangles.Count - 1, new SCWorkSpaceViewItemPlugin2(), new Dictionary<string, string>());

            //create and start _updateInformationTimer
            _updateInformationTimer = new DispatcherTimer();
            _updateInformationTimer.Interval = new TimeSpan(0, 0, 1);
            _updateInformationTimer.Tick += new EventHandler(_updateInformationTimer_Tick);
            _updateInformationTimer.Start();
        }

        public override void Close()
        {
            _updateInformationTimer.Stop();
            _updateInformationTimer = null;

            foreach (object messageRegistrationObject in _messageRegistrationObjects)
            {
                EnvironmentManager.Instance.UnRegisterReceiver(messageRegistrationObject);
            }
            _messageRegistrationObjects.Clear();
        }

        public override void ViewItemConfigurationModified(int index)
        {
            base.ViewItemConfigurationModified(index);

            if (ViewAndLayoutItem.ViewItemId(index) == ViewAndLayoutItem.CameraBuiltinId)
            {
                SetProperty("Camera" + index, ViewAndLayoutItem.ViewItemConfigurationString(index));
                SaveProperties(true);
            }
        }

        private object ShownWorkSpaceChangedReceiver(Message message, FQID sender, FQID related)
        {
            if (message.Data is Item && ((Item)message.Data).FQID.ObjectId == Id)
            {
                _workSpaceSelected = true;
                Notification = null;
                _updateInformationTimer.Stop();
            }
            else
            {
                _workSpaceSelected = false;
                _updateInformationTimer.Start();
            }
            return null;
        }

        private object WorkSpaceStateChangedReceiver(Message message, FQID sender, FQID related)
        {
            if (_workSpaceSelected && ((WorkSpaceState)message.Data) == WorkSpaceState.Normal)
            {
                UpdateCameras();
            }
            return null;
        }


        private object SelectedViewChangedReceiver(Message message, FQID sender, FQID related)
        {
            if (message.Data is Item && ((Item)message.Data).FQID.ObjectId == ViewAndLayoutItem.FQID.ObjectId)
            {
                _workSpaceViewSelected = true;
                if (!_viewInitialized)
                {
                    _viewInitialized = true;
                    UpdateCameras();
                }
            }
            else
            {
                _workSpaceViewSelected = false;
            }
            return null;
        }


        private object ShuffleCamerasReceiver(Message message, FQID sender, FQID related)
        {
            if (_workSpaceViewSelected)
            {
                UpdateCameras();
            }
            return null;
        }

        private object MaxCamerasChangedReceiver(Message message, FQID sender, FQID related)
        {
            if (_workSpaceViewSelected)
            {
                UpdateCameras();
            }
            return null;
        }

        void _updateInformationTimer_Tick(object sender, EventArgs e)
        {
            int severity;
            int count;
            string text;

            count = (Notification == null) ? 0 : Notification.Count + 1;

            if (count < 25)
            {
                severity = WorkSpaceNotification.SEVERITY_LOW;
            }
            else if (count < 50)
            {
                severity = WorkSpaceNotification.SEVERITY_MEDIUM;
            }
            else
            {
                severity = WorkSpaceNotification.SEVERITY_HIGH;
            }

            text = "Notification level: " + severity + ", Count: " + count;

            Notification = new WorkSpaceNotification(severity, count, text);
        }

        private void UpdateCameras()
        {
            try
            {
                List<Item> cameraItems = new List<Item>();
                FindAllCameras(Configuration.Instance.GetItemsByKind(Kind.Camera), cameraItems);

                cameraItems = ShuffleList(cameraItems);

                for (int i = 0; i < MAX_MAX_CAMERA_COUNT; i++)
                {
                    Message setCameraMessage;
                    if (i < SCWorkSpaceDefinition.MaxCameras && i < cameraItems.Count)
                    {
                        setCameraMessage = new Message(MessageId.SmartClient.SetCameraInViewCommand,
                                                       new SetCameraInViewCommandData() { Index = i, CameraFQID = cameraItems[i].FQID });
                    }
                    else
                    {
                        setCameraMessage = new Message(MessageId.SmartClient.SetCameraInViewCommand,
                                                       new SetCameraInViewCommandData() { Index = i, CameraFQID = null });
                    }
                    EnvironmentManager.Instance.SendMessage(setCameraMessage);
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("SCWorkSpace:UpdateCameras", ex);
            }
        }

        private void FindAllCameras(List<Item> searchItems, List<Item> foundCameraItems)
        {
            foreach (Item searchItem in searchItems)
            {
                if (searchItem.FQID.Kind == Kind.Camera && searchItem.FQID.FolderType == FolderType.No)
                {
                    bool cameraAlreadyFound = false;
                    foreach (Item foundCameraItem in foundCameraItems)
                    {
                        if (foundCameraItem.FQID.Equals(searchItem.FQID))
                        {
                            cameraAlreadyFound = true;
                            break;
                        }
                    }
                    if (!cameraAlreadyFound)
                    {
                        foundCameraItems.Add(searchItem);
                    }
                }
                else
                {
                    FindAllCameras(searchItem.GetChildren(), foundCameraItems);
                }
            }
        }

        private List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>(); Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count);       //Choose a random object in the list
                randomList.Add(inputList[randomIndex]);         //add it to the new, random list
                inputList.RemoveAt(randomIndex);                //remove to avoid duplicates
            }
            return randomList;                                  //return the new random list
        }
    }
}
