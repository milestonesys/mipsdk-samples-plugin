using VideoOS.Platform.Client;

namespace SCTheme.Client
{
    public class SCThemeViewItemManager : VideoOS.Platform.Client.ViewItemManager
	{
        public SCThemeViewItemManager()
            : base("SCThemeViewItemManager")
        {
        }

		public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
		{
            return new SCThemeViewItemWpfUserControl();
		}
    }
}
