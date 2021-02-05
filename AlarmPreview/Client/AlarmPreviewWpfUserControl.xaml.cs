using System;
using System.Windows.Controls;
using VideoOS.Platform.Data;

namespace AlarmPreview.Client
{
    /// <summary>
    /// Interaction logic for AlarmPreviewWpfUserControl.xaml
    /// </summary>
    public partial class AlarmPreviewWpfUserControl : UserControl
    {
        public AlarmPreviewWpfUserControl(object alarmOrBaseEvent)
        {
            InitializeComponent();

            textBox1.Text = GetContents(alarmOrBaseEvent);
        }

        private string GetContents(object alarmOrBaseEvent)
        {
            Alarm alarm = alarmOrBaseEvent as Alarm;
            if (alarm != null)
            {
                string alarmInText = "Message: {0}" + Environment.NewLine +
                    "Definition {8}" + Environment.NewLine +
                    "Type: {1}" + Environment.NewLine +
                    "Source: {2}" + Environment.NewLine +
                    "CustomTag: {3}" + Environment.NewLine +
                    "Object {4}" + Environment.NewLine +
                    "Vendor {5}" + Environment.NewLine +
                    "Location {6}" + Environment.NewLine +
                    "Description: {7}";

                string alarmDef = alarm.RuleList != null && alarm.RuleList.Count > 0 ? alarm.RuleList[0].Name : "";
                string alarmObj = alarm.ObjectList != null && alarm.ObjectList.Count > 0 ? alarm.ObjectList[0].Value : "";
                string alarmVendorName = alarm.Vendor != null && alarm.Vendor.Name != null ? alarm.Vendor.Name : "";
                string alarmLocation = alarm.Location ?? "";
                string alarmDescription = alarm.Description ?? "";

                
                return string.Format(alarmInText,
                    alarm.EventHeader.Message,
                    alarm.EventHeader.Type,
                    alarm.EventHeader.Source.Name,
                    alarm.EventHeader.CustomTag,
                    alarmObj,
                    alarmVendorName,
                    alarmLocation,
                    alarmDescription,
                    alarmDef);
            }
            else
            {
                AnalyticsEvent analyticsEvent = alarmOrBaseEvent as AnalyticsEvent;
                if (analyticsEvent != null)
                {
                    string analyticsEventInText = "Message: {0}" + Environment.NewLine +
                        "Type: {1}" + Environment.NewLine +
                        "Source: {2}" + Environment.NewLine +
                        "CustomTag: {3}" + Environment.NewLine +
                        "Object {4}" + Environment.NewLine +
                        "Vendor {5}" + Environment.NewLine +
                        "Location {6}" + Environment.NewLine +
                        "Description: {7}";

                    string analyticsObj = analyticsEvent.ObjectList != null && analyticsEvent.ObjectList.Count > 0 ? analyticsEvent.ObjectList[0].Value : "";
                    string analyticsEventVendorName = analyticsEvent.Vendor != null && analyticsEvent.Vendor.Name != null ? analyticsEvent.Vendor.Name : "";
                    string analyticsEventLocation = analyticsEvent.Location ?? "";
                    string analyticsEventDescription = analyticsEvent.Description ?? "";

                    return string.Format(analyticsEventInText,
                        analyticsEvent.EventHeader.Message,
                        analyticsEvent.EventHeader.Type,
                        analyticsEvent.EventHeader.Source.Name,
                        analyticsEvent.EventHeader.CustomTag,
                        analyticsObj,
                        analyticsEventVendorName,
                        analyticsEventLocation,
                        analyticsEventDescription);
                }
                else
                {
                    BaseEvent baseEvent = alarmOrBaseEvent as BaseEvent;
                    if (baseEvent != null)
                    {
                        string baseEventInText = "Message: {0}" + Environment.NewLine +
                            "Type: {1}" + Environment.NewLine +
                            "Source: {2}" + Environment.NewLine +
                            "CustomTag: {3}";

                        return string.Format(baseEventInText,
                            baseEvent.EventHeader.Message,
                            baseEvent.EventHeader.Type,
                            baseEvent.EventHeader.Source.Name,
                            baseEvent.EventHeader.CustomTag
                            );
                    }
                }
                return "Unknown object";
            }
        }
    }
}
