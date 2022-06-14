using System;
using System.Windows.Controls;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace AnalyticsOverlay
{
    public class AnalyticsOverlaySettingsPanelPlugin : SettingsPanelPlugin
    {
        private Item _tempSelectedCameraItem;
        private AnalyticsOverlaySettingsPanelWpfUserControl _userControl;

        public override UserControl GenerateUserControl()
        {
            _userControl = new AnalyticsOverlaySettingsPanelWpfUserControl();
            _userControl.SelectedItem = _tempSelectedCameraItem;
            return _userControl;
        }

        public override Guid Id
        {
            get { return AnalyticsOverlayDefinition.AnalyticsOverlaySettingsPanel; }
        }

        public override string Title
        {
            get { return "AnalyticsOverlay sample"; }
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

        public override void CloseUserControl()
        {
            _userControl = null;
        }

        public override bool TrySaveChanges(out string errorMessage)
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
            errorMessage = "Error occurred";

            return true;
        }
    }
}
