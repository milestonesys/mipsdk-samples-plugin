using System;
using System.Reflection;
using VideoOS.Platform.Client;

namespace SCWorkSpace.Client
{
	public class SCWorkSpaceViewItemPlugin2 : ViewItemPlugin
	{
        internal protected static System.Drawing.Image _treeNodeImage;

        public SCWorkSpaceViewItemPlugin2()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;
            _treeNodeImage = System.Drawing.Image.FromStream(assembly.GetManifestResourceStream(name + ".Resources.WorkSpace.ico"));
        }

		public override Guid Id
		{
			get { return new Guid("CCD3FD1E-FD65-4F3D-9012-1BE73D44D188"); }
		}

		public override System.Drawing.Image Icon
		{
            get { return _treeNodeImage; }
		}

		public override string Name
		{
			get { return "WorkSpace Plugin View Item2"; }
		}

        public override bool HideSetupItem
        {
            get
            {
                return false;
            }
        }

		public override ViewItemManager  GenerateViewItemManager()
		{
            return new SCWorkSpaceViewItemManager2();
		}

		public override void Init()
		{
		}

		public override void Close()
		{
		}

    }

}
