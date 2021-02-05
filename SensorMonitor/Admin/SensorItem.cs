using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace SensorMonitor.Admin
{
    class SensorItem : Item
    {
        public static Dictionary<Guid, bool> SensorActiveState = new Dictionary<Guid, bool>();

        private Item _parentItem;        

        public SensorItem (FQID fqid, string name, Item parentItem)
			: base(fqid, name)
        {
            _parentItem = parentItem;
        }

        public SensorItem(Item item)
            : base(item.FQID, item.Name)
        {
            _parentItem = item.GetParent();
            foreach (KeyValuePair<string, string> property in item.Properties)
            {
                Properties.Add(property.Key, property.Value);
            }
        }

        public override Item GetParent()
        {
            return _parentItem;
        }

        public override Collection<MapAlarmContextMenu> ContextMenu
        {
            get
            {
                if (!SensorActiveState.ContainsKey(FQID.ObjectId) || SensorActiveState[FQID.ObjectId] == false)
                {
                    return new Collection<MapAlarmContextMenu>
                                                               {
                                                                   new MapAlarmContextMenu("Activate Sensor", "ACTIVATESENSOR", null)
                                                               };
                }
                else
                {
                    return new Collection<MapAlarmContextMenu>
                                                               {
                                                                   new MapAlarmContextMenu("Deactivate Sensor", "DEACTIVATESENSOR", null)
                                                               };

                }
            }
            set { }
        }

        public override Guid MapIconKey
        {
            get
            {
                if (SensorActiveState.ContainsKey(FQID.ObjectId) && SensorActiveState[FQID.ObjectId])
                {
                    return SensorMonitorDefinition.SensorMonitorSensorActivated;
                }
                else
                {
                    return SensorMonitorDefinition.SensorMonitorSensorKind;
                }
            }
            set { } // do nothing
        }
    }
}
