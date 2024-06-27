using System;
using System.Reflection;
using VideoOS.Platform.Client;
using VideoOS.Platform.UI.Controls;

namespace Property.Client
{
    public class PropertyWorkSpaceViewItemPlugin : ViewItemPlugin
    {
        private static VideoOSIconSourceBase _treeNodeImage;

        public PropertyWorkSpaceViewItemPlugin()
        {
            var packString = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/PropertyWorkSpace.bmp");
            _treeNodeImage = new VideoOSIconUriSource() { Uri = new Uri(packString) };
        }

        public override Guid Id
        {
            get { return PropertyDefinition.PropertyWorkSpaceViewItemPluginId; }
        }

        public override VideoOSIconSourceBase IconSource { get => _treeNodeImage; protected set => base.IconSource = value; }

        public override string Name
        {
            get { return "WorkSpace Plugin View Item"; }
        }

        public override bool HideSetupItem
        {
            get
            {
                return false;
            }
        }

        public override ViewItemManager GenerateViewItemManager()
        {
            return new PropertyWorkSpaceViewItemManager();
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }
    }
}
