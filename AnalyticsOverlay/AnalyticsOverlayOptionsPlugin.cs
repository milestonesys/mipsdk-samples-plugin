using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace AnalyticsOverlay
{
	public class AnalyticsOverlayOptionsPlugin : OptionsDialogPlugin
	{
		private Item _tempSelectedCameraItem;
		private OptionsUserControl _userControl;

		public override OptionsDialogUserControl GenerateUserControl()
		{
			_userControl = new OptionsUserControl();
			_userControl.SelectedItem = _tempSelectedCameraItem;
			return _userControl;
		}

		public override Guid Id
		{
			get { return AnalyticsOverlayDefinition.AnalyticsOverlayOptionDialog; }
		}

		public override string Name
		{
			get { return "Overlay setup"; }
		}

		public override void Init()
		{
		    LoadProperties(true);
            String id = GetProperty("WatchCameraFQID");
		    if (id != null)
		    {
		        Guid guid = new Guid(id);
		        Item camera = Configuration.Instance.GetItem(guid, Kind.Camera);
		        if (camera != null)
		        {
		            BackgroundOverlayPlugin.WatchCameraFQID = camera.FQID;
		            _tempSelectedCameraItem = camera;
		        }
		    } else
			if (BackgroundOverlayPlugin.WatchCameraFQID !=null)
				_tempSelectedCameraItem = Configuration.Instance.GetItem(BackgroundOverlayPlugin.WatchCameraFQID);
		}

		public override void Close()
		{			
		}

		public override bool SaveChanges()
		{
			if (_userControl != null)
				_tempSelectedCameraItem = _userControl.SelectedItem;

			if (_tempSelectedCameraItem != null)
			{
				BackgroundOverlayPlugin.WatchCameraFQID = _tempSelectedCameraItem.FQID;
				SetProperty("WatchCameraFQID", _tempSelectedCameraItem.FQID.ObjectId.ToString());
			    SaveProperties(true);
			}
			_userControl = null;
			return true;
		}
	}
}
