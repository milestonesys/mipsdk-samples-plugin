using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoOS.Platform;
using VideoOS.Platform.Admin;

namespace SensorMonitor.Admin
{
	/// <summary>
	/// The ControllerItem needs to return the Sensors as its Children, and any ContextMenu actions that should be available as Triggers
	/// </summary>
	public class ControllerItem : Item
	{
		public ControllerItem(Item item)
			: base(item.FQID, item.Name)
		{
			foreach (string key in item.Properties.Keys)
				base.Properties[key] = item.Properties[key];
		}

		/// <summary>
		/// Build a list of Children.  It contains both Sensor and Triggeble Items - the ContextMenu Items
		/// </summary>
		/// <returns></returns>
		public override List<Item> GetChildren()
		{
			List<Item> items = Configuration.Instance.GetItemConfigurations(SensorMonitorDefinition.SensorMonitorPluginId, this, SensorMonitorDefinition.SensorMonitorSensorKind);
			foreach (MapAlarmContextMenu contextMenu in SensorMonitorDefinition.MapAlarmContextMenuList)
			{
				items.Add(new MapAlarmContextMenuItem(this.FQID.ServerId, this.FQID.ObjectId, contextMenu.Command, contextMenu.DisplayName, this));
			}
			return items;
		}
	}
}
