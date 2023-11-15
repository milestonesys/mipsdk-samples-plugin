using System.Windows.Forms;
using VideoOS.Platform.Admin;

namespace ChatWithWebsockets.Admin
{
    public class ChatWithWebsocketsItemManager : ItemManager
    {
        #region Constructors

        public ChatWithWebsocketsItemManager()
        {			
        }

        public override void Init()
        {
        }

        public override void Close()
        {
        }

        #endregion

        #region UserControl Methods

        /// <summary>
        /// Generate the UserControl for configuring a type of item that this ItemManager manages.
        /// </summary>
        /// <returns></returns>
        public override UserControl GenerateDetailUserControl()
        {
            return null;
        }

        /// <summary>
        /// A user control to display when the administrator clicks on the treeNode.
        /// This can be a help page or a status over of the configuration
        /// </summary>
        public override ItemNodeUserControl GenerateOverviewUserControl()
        {
            return new ChatWithWebsocketsItemNodeUserControl();
        }

        #endregion

    }
}

