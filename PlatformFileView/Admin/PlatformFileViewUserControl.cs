using System;
using System.Windows.Forms;
using PlatformFileView.WebService;
using VideoOS.Platform;  

namespace PlatformFileView.Admin
{
    public partial class PlatformFileViewUserControl : UserControl
    {
        internal event EventHandler ConfigurationChangedByUser;
        private Guid _objectId = Guid.NewGuid();

        public PlatformFileViewUserControl()
        {
            InitializeComponent();
        }

		internal String DisplayName
        {
            get { return textBoxName.Text; }
            set { textBoxName.Text = value; }
        }

        /// <summary>
        /// This field represent the machine we like to look at.
        /// </summary>
    	internal FQID ServerFQID
    	{
    	    get
    	    {
                Uri uri = new UriBuilder(textBox1.Text).Uri;
                return new FQID(new ServerId("FILE", uri.DnsSafeHost, 25243, _objectId), 
                    Guid.Empty,
                    _objectId,
                    FolderType.SystemDefined, 
                    PlatformFileViewDefinition.PlatformFolderKind) 
                    { ObjectIdString = "C:\\" };
    	    }
    	    set
    	    {
    	        textBox1.Text = value.ServerId.ServerHostname;
    	        _objectId = value.ObjectId;
    	    }
    	}

        internal void OnUserChange(object sender, EventArgs e)
        {
            if (ConfigurationChangedByUser != null)
                ConfigurationChangedByUser(this, new EventArgs());
        }

        internal void FillContent()
        {
            picker.Init();
            picker.KindFilter = new System.Collections.Generic.List<Guid> { 
                PlatformFileViewDefinition.PlatformFolderKind, 
                PlatformFileViewDefinition.PlatformFileKind };
			picker.GroupTabVisible = false;
        	SetPickerRoot();
        }


        private void OnClick(object sender, EventArgs e)
        {
			SetPickerRoot();
        }

		private void SetPickerRoot()
		{
			WebServiceManager.Instance.SetRemoteAddress(textBox1.Text);
            FolderItem rootFolderItem = new FolderItem(ServerFQID, DisplayName);
            picker.ItemsToSelectFrom = new System.Collections.Generic.List<Item> { rootFolderItem };
		}
    }
}
