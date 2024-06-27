using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    public class BackgroundColorViewItemManager : ViewItemManager
	{
        public BackgroundColorViewItemManager()
            : base("BackgroundColorViewItemManager")
        {
        }

		public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
		{
            return new BackgroundColorViewItemWpfUserControl(this);
		}

		public override PropertiesWpfUserControl GeneratePropertiesWpfUserControl()
		{
			return new PropertiesWpfUserControl(); //no special properties
		}
    }
}
