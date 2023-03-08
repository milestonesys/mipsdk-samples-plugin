using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows.Shapes;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.Data;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Platform.Proxy.AlarmClient;

namespace SCOverlayGraphOnEvent.Background
{

    public class OverlayGraphOnEventBackgroundPlugin : BackgroundPlugin
    {
        #region private fields

        private List<ImageViewerAddOn> _activeImageViewerAddOns = new List<ImageViewerAddOn>();

        private const double _interval = 1000;                // check whether an overlay should be cleared every 1000 milliseconds 
        private const double _keepOverlayPeriod = 20000;      // keep the graph overlay for 20 seconds (20000 milliseconds)
        private const double _startCache = 10000;
        private const double _endCache = 60000;

        private IDictionary<ImageViewerAddOn, OverlayObject> _dicAddOnOverlayObjects =
            new Dictionary<ImageViewerAddOn, OverlayObject>();

        private IDictionary<ImageViewerAddOn, CachedOverlayObjects> _dicAddOnOverlayObjectsPlayback =
            new Dictionary<ImageViewerAddOn, CachedOverlayObjects>();

        private static System.Timers.Timer timerRemoveOverlays;
        private Dictionary<ImageViewerAddOn, Guid> _dicShapes = new Dictionary<ImageViewerAddOn, Guid>();

        public static MessageCommunication _messageCommunication;
        private object _obj1;
        private object _obj2;

        #endregion

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get { return OverlayGraphOnEventDefinition.OverlayGraphOnEventBackgroundPlugin; }
        }

        /// <summary>
        /// The name of this background plugin
        /// </summary>
        public override String Name
        {
            get { return "OverlayGraphOnEvent BackgroundPlugin"; }
        }

        /// <summary>
        /// Called by the Environment when the user has logged in.
        /// </summary>
        public override void Init()
        {
            ClientControl.Instance.NewImageViewerControlEvent += new ClientControl.NewImageViewerControlHandler(NewImageViewerControlEvent);
            MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
            _messageCommunication = MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);

            _obj1 = _messageCommunication.RegisterCommunicationFilter(NewEventIndicationMessageHandler,
                new CommunicationIdFilter(MessageId.Server.NewEventIndication));

            _obj2 = _messageCommunication.RegisterCommunicationFilter(NewAlarmMessageHandler,
                new CommunicationIdFilter(MessageId.Server.NewAlarmIndication));
            timerRemoveOverlays = new System.Timers.Timer(_interval);
            timerRemoveOverlays.Elapsed += new ElapsedEventHandler(TimedRefresh);

            timerRemoveOverlays.Enabled = true;
        }

        #region Messaging eventhandlers

        ///<summary>
        /// Whenever an event has been happened this handler is run
        /// We choose to ignore all events that are not AnalyticsEvent
        /// You can send the message.Data as BaseEvent for all sorts of events
        /// An easy test whether the event is AnalyticsEvent is if message.Data is AnalyticsEvent.
        /// </summary>
        private object NewEventIndicationMessageHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            try
            {
                AnalyticsEvent evAnalyt = message.Data as AnalyticsEvent;
                if (evAnalyt != null)
                {
                    DrawGraphOverlays(evAnalyt);
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("NewEventMessageHandler", ex);
            }
            return null;
        }

        /// <summary>
        /// Handling of alarms is not implemeted, only subscribing to events, the alarms where properly created based on an event
        /// and is therefor already displayed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dest"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private object NewAlarmMessageHandler(VideoOS.Platform.Messaging.Message message, FQID dest, FQID source)
        {
            try
            {
                Alarm al = message.Data as Alarm;
                if (al != null)
                {
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("NewAlarmMessageHandler", ex);
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Called by the Environment when the user log's out.
        /// You should close all remote sessions and flush cache information, as the
        /// user might logon to another server next time.
        /// </summary>
        public override void Close()
        {
            ClientControl.Instance.NewImageViewerControlEvent -= new ClientControl.NewImageViewerControlHandler(NewImageViewerControlEvent);
            if (_messageCommunication != null)
            {
                _messageCommunication.UnRegisterCommunicationFilter(_obj1);
                _messageCommunication.UnRegisterCommunicationFilter(_obj2);
                _messageCommunication = null;
            }
            timerRemoveOverlays.Stop();
            timerRemoveOverlays.Elapsed -= new ElapsedEventHandler(TimedRefresh);
        }

        /// <summary>
        /// Define in what Environments the current background task should be started.
        /// </summary>
        public override List<EnvironmentType> TargetEnvironments
        {
            get { return new List<EnvironmentType>() { EnvironmentType.SmartClient }; }	// will run in the Smart Client
        }


        #region Event registration on/off
        /// <summary>
        /// Register all the events we need
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void RegisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.CloseEvent += new EventHandler(ImageViewerAddOn_CloseEvent);
            imageViewerAddOn.StartLiveEvent += new PassRequestEventHandler(ImageViewerAddOn_StartLiveEvent);
            imageViewerAddOn.StopLiveEvent += new PassRequestEventHandler(ImageViewerAddOn_StopLiveEvent);
            imageViewerAddOn.RecordedImageReceivedEvent += new RecordedImageReceivedHandler(ImageViewerAddOn_RecordedImageReceivedEvent);
        }

        /// <summary>
        /// Unhook all my event handlers
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void UnregisterEvents(ImageViewerAddOn imageViewerAddOn)
        {
            imageViewerAddOn.CloseEvent -= new EventHandler(ImageViewerAddOn_CloseEvent);
            imageViewerAddOn.StartLiveEvent -= new PassRequestEventHandler(ImageViewerAddOn_StartLiveEvent);
            imageViewerAddOn.StopLiveEvent -= new PassRequestEventHandler(ImageViewerAddOn_StopLiveEvent);
            imageViewerAddOn.RecordedImageReceivedEvent -= new RecordedImageReceivedHandler(ImageViewerAddOn_RecordedImageReceivedEvent);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// This routine runs through the imagevieweraddons, checks whether they are in the dictionary of overlay objects,
        /// if so checks whether it is time to clear the overlay.
        /// The timer is not part of the UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimedRefresh(object sender, ElapsedEventArgs e)
        {
            if (_activeImageViewerAddOns.Count > 0)
            {
                try
                {
                    // Copy array to avoid deadlocks
                    ImageViewerAddOn[] tempList;
                    lock (_activeImageViewerAddOns)
                    {
                        tempList = new ImageViewerAddOn[_activeImageViewerAddOns.Count];
                        _activeImageViewerAddOns.CopyTo(tempList, 0);
                    }

                    //Go through all registered AddOns and identify the one we are looking for
                    foreach (ImageViewerAddOn addOn in tempList)
                    {
                        OverlayObject oo;
                        if (_dicAddOnOverlayObjects.TryGetValue(addOn, out oo))
                        {
                            DateTime endTime = oo.StartTime.AddMilliseconds(_keepOverlayPeriod);
                            if (endTime < DateTime.UtcNow)
                            {
                                ClearOverlay(addOn);
                                _dicAddOnOverlayObjects.Remove(addOn);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    EnvironmentManager.Instance.ExceptionDialog("Background Overlay", ex);
                }
            }

        }
        /// <summary>
        /// A new ImageViewer has been created
        /// </summary>
        /// <param name="imageViewerAddOn"></param>
        void NewImageViewerControlEvent(ImageViewerAddOn imageViewerAddOn)
        {
            lock (_activeImageViewerAddOns)
            {
                RegisterEvents(imageViewerAddOn);
                _activeImageViewerAddOns.Add(imageViewerAddOn);
            }
        }

        /// <summary>
        /// The smart client is now going into setup or playback mode (Or just this one camera is in instant playback mode)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageViewerAddOn_StopLiveEvent(object sender, PassRequestEventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null)
            {
                ClearOverlay(imageViewerAddOn);
            }
        }

        /// <summary>
        /// The Smart Client is now going into live mode.  We would overtake or reject that this item is going into live.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageViewerAddOn_StartLiveEvent(object sender, PassRequestEventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;  // todo does this do anything (is the obects always null)?
            if (imageViewerAddOn != null)
            {
                DrawGraphOverlay(imageViewerAddOn);
            }
        }

        void ImageViewerAddOn_RecordedImageReceivedEvent(object sender, RecordedImageReceivedEventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn == null)
            {
                return;
            }

            CreatePlaybackDictionary(imageViewerAddOn, e.DateTime);

            if (IsDisplay(e.DateTime))
            {
                DrawGraphOverlayPlayback(imageViewerAddOn, e.DateTime);
            }
            else
            {
                ClearOverlay(imageViewerAddOn);
            }
        }

        private void CreatePlaybackDictionary(ImageViewerAddOn imageViewerAddOn, DateTime videoTime)
        {
            CachedOverlayObjects coo;
            if (!_dicAddOnOverlayObjectsPlayback.TryGetValue(imageViewerAddOn, out coo) || coo.StartTime > videoTime || coo.EndTime < videoTime)
            {
                DateTime startDatetime = videoTime.AddMilliseconds(-_startCache);
                DateTime endDatetime = videoTime.AddMilliseconds(_endCache);
                if (endDatetime > DateTime.Now)
                {
                    endDatetime = DateTime.Now;
                }

                EventLine[] analyticsEventLines = GetAnalyticsEventList(startDatetime, endDatetime);
                if (analyticsEventLines.Length == 0)
                {
                    return;
                }

                CachedOverlayObjects overlayObjectList = new CachedOverlayObjects();
                overlayObjectList.OverlayObjectList = new List<OverlayObject>();
                overlayObjectList.StartTime = startDatetime;
                overlayObjectList.EndTime = endDatetime;

                AlarmClientManager _alarmClientManager = new AlarmClientManager();
                IAlarmClient alarmClient = _alarmClientManager.GetAlarmClient(EnvironmentManager.Instance.MasterSite.ServerId);

                foreach (EventLine ev in analyticsEventLines)
                {
                    BaseEvent baseevent = alarmClient.GetEvent(ev.Id);
                    overlayObjectList.OverlayObjectList.Add(new OverlayObject(baseevent, ev.Timestamp));
                }
                if (_dicAddOnOverlayObjectsPlayback.ContainsKey(imageViewerAddOn))
                {
                    _dicAddOnOverlayObjectsPlayback.Remove(imageViewerAddOn);
                }
                _dicAddOnOverlayObjectsPlayback.Add(imageViewerAddOn, overlayObjectList);
            }
        }

        private bool IsDisplay(DateTime videoTime)
        {
            foreach (var item in _dicAddOnOverlayObjectsPlayback.Values)
            {
                foreach (var obj in item.OverlayObjectList)
                {
                    if (obj.StartTime <= videoTime && videoTime < obj.StartTime.AddMilliseconds(_keepOverlayPeriod))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// One of the ImageViewer has been closed / Removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ImageViewerAddOn_CloseEvent(object sender, EventArgs e)
        {
            ImageViewerAddOn imageViewerAddOn = sender as ImageViewerAddOn;
            if (imageViewerAddOn != null)
            {
                UnregisterEvents(imageViewerAddOn);
                ClearOverlay(imageViewerAddOn);
                if (_dicAddOnOverlayObjects.ContainsKey(imageViewerAddOn))
                {
                    _dicAddOnOverlayObjects.Remove(imageViewerAddOn);
                }
                lock (_activeImageViewerAddOns)
                {
                    // Remove from list
                    if (_activeImageViewerAddOns.Contains(imageViewerAddOn))
                        _activeImageViewerAddOns.Remove(imageViewerAddOn);
                }
            }
        }
        #endregion

        #region Drawing the overlay

        private void ClearOverlay(ImageViewerAddOn imageViewerAddOn)
        {
            try
            {
                // Clear the overlay
                Guid shapeID;
                if (_dicShapes.TryGetValue(imageViewerAddOn, out shapeID))
                {
                    ClientControl.Instance.CallOnUiThread(() => imageViewerAddOn.ShapesOverlayRemove(shapeID));
                    _dicShapes.Remove(imageViewerAddOn);
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("ImageViewerAddOn_ClearOverlay", ex);
            }
        }

        /// <summary>
        /// Draw the overlay on one specific imagevieweraddon
        /// </summary>
        /// <param name="addOn"></param>
        private void DrawGraphOverlay(ImageViewerAddOn addOn)
        {
            if (addOn.PaintSize.Width == 0 || addOn.PaintSize.Height == 0)
            {
                return;
            }
            OverlayObject oo;
            if (!_dicAddOnOverlayObjects.TryGetValue(addOn, out oo))
            {
                return;
            }
            ClientControl.Instance.CallOnUiThread(() =>
            {
                List<Shape> shapes = VideoOS.Platform.Util.AnalyticsOverlayBuilder.BuildShapeOverlay(addOn.PaintSize.Width, addOn.PaintSize.Height, (BaseEvent)oo.EventObject);
                UpdateShapeOverlay(addOn, shapes);
            });
        }

        private void DrawGraphOverlayPlayback(ImageViewerAddOn addOn, DateTime playbackDatetime)
        {
            if (addOn.PaintSize.Width == 0 || addOn.PaintSize.Height == 0)
            {
                return;
            }
            CachedOverlayObjects coo;
            if (!_dicAddOnOverlayObjectsPlayback.TryGetValue(addOn, out coo))
            {
                return;
            }

            List<Shape> shapes = new List<Shape>();
            foreach (OverlayObject oo in coo.OverlayObjectList)
            {
                if (oo.StartTime <= playbackDatetime && oo.StartTime.AddMilliseconds(_keepOverlayPeriod) >= playbackDatetime)
                {
                    shapes.AddRange(VideoOS.Platform.Util.AnalyticsOverlayBuilder.BuildShapeOverlay(addOn.PaintSize.Width, addOn.PaintSize.Height, (BaseEvent)oo.EventObject));
                }
            }

            ClientControl.Instance.CallOnUiThread(() =>
            {
                UpdateShapeOverlay(addOn, shapes);
            });
        }

        private void UpdateShapeOverlay(ImageViewerAddOn addOn, List<Shape> shapes)
        {
            if (!_dicShapes.ContainsKey(addOn))
            {
                _dicShapes.Add(addOn, addOn.ShapesOverlayAdd(shapes, new ShapesOverlayRenderParameters() { ZOrder = 100 }));
            }
            else
            {
                addOn.ShapesOverlayUpdate(_dicShapes[addOn], shapes, new ShapesOverlayRenderParameters() { ZOrder = 100 });
            }
        }

        private void DrawGraphOverlays(BaseEvent evData)
        {
            if (_activeImageViewerAddOns.Count <= 0)
            {
                return;
            }

            try
            {
                // Copy array to avoid deadlocks
                ImageViewerAddOn[] tempList = new ImageViewerAddOn[_activeImageViewerAddOns.Count];
                lock (_activeImageViewerAddOns)
                {
                    _activeImageViewerAddOns.CopyTo(tempList, 0);
                }

                //Go through all registered AddOns and identify the one we are looking for
                foreach (ImageViewerAddOn addOn in tempList)
                {
                    if (addOn.CameraFQID != null && addOn.CameraFQID.ObjectId != Guid.Empty)
                    {
                        if (addOn.CameraFQID.ObjectId == evData.EventHeader.Source.FQID.ObjectId)
                        {
                            //if the dictionary already has the key remove it, in effect make sure we use the newer event data
                            if (_dicAddOnOverlayObjects.ContainsKey(addOn))
                            {
                                _dicAddOnOverlayObjects.Remove(addOn);
                            }
                            _dicAddOnOverlayObjects.Add(addOn, new OverlayObject(evData, DateTime.UtcNow));
                            //Only draw the ones in Live mode
                            if (addOn.InLiveMode)
                            {
                                DrawGraphOverlay(addOn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionDialog("Draw Overlay", ex);
            }
        }

        private EventLine[] GetAnalyticsEventList(DateTime periodStart, DateTime periodEnd)
        {
            AlarmClientManager _alarmClientManager = new AlarmClientManager();

            int eventCount = (int)(_startCache + _endCache) / 500;
            try
            {
                IAlarmClient alarmClient = _alarmClientManager.GetAlarmClient(EnvironmentManager.Instance.MasterSite.ServerId);
                EventLine[] events = alarmClient.GetEventLines(0, eventCount, new VideoOS.Platform.Proxy.Alarm.EventFilter()
                {
                    Conditions = new Condition[] { new Condition() { Operator = Operator.NotEquals, Target = Target.Type, Value = "System Event" },
                                                   new Condition() { Operator=Operator.LessThan, Target = Target.Timestamp, Value=periodEnd },
                                                   new Condition() { Operator=Operator.GreaterThan, Target = Target.Timestamp, Value=periodStart }
                                                 }
                });
                return events;
            }
            catch (Exception ex)
            {
                // this will happen if for instance the Event Server cannot be contacted. Showing error each time is quite intrusive, so let's just log.
                EnvironmentManager.Instance.Log(true, nameof(OverlayGraphOnEventBackgroundPlugin), "Exception thrown: " + ex.Message);
                return new EventLine[] { };
            }
        }
        #endregion
    }
}
