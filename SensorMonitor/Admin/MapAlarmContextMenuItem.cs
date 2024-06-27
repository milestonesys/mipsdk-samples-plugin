using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoOS.Platform;

namespace SensorMonitor.Admin
{
	/// <summary>
	/// This class has been implemented to ensure the "GetParent" method will return something useful.<br/>
	/// When not implemented - the default behaviour of looking for a parent item of same kind will not work.<br/>
	/// The usage of "GetParent" is not important for the MIP Environment.
	/// </summary>
	public class MapAlarmContextMenuItem : Item
	{
		private Item _parentItem;

		public MapAlarmContextMenuItem(ServerId serverId, Guid parentId, string objectIdString, string name, Item parentItem) : 
			base(serverId, parentId, objectIdString, name, FolderType.No, Kind.TriggerEvent)
		{
			_parentItem = parentItem;
			_hasRelated = HasRelated.No;
			_anyChildren = HasChildren.No;
		}

		public override Item GetParent()
		{
			return _parentItem;
		}
	}
}
