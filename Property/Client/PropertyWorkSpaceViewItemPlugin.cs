using System;
using System.Reflection;
using VideoOS.Platform.Client;

namespace Property.Client
{
    public class PropertyWorkSpaceViewItemPlugin : ViewItemPlugin
    {
        private static System.Drawing.Image _treeNodeImage;
        public PropertyWorkSpaceViewItemPlugin()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            _treeNodeImage = System.Drawing.Image.FromStream(assembly.GetManifestResourceStream(name + ".Resources.PropertyWorkSpace.bmp"));
        }

        public override Guid Id
        {
            get { return PropertyDefinition.PropertyWorkSpaceViewItemPluginId; }
        }

        public override System.Drawing.Image Icon
        {
            get { return _treeNodeImage; }
        }

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
