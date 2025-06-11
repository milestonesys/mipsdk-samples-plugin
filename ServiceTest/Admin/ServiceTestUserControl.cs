using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VideoOS.Platform;

namespace ServiceTest.Admin
{
    /// <summary>
    /// This sample can register and unregister an arbitrary service to the system.
    /// This would normally be performed during the installation process of a given service.
    /// </summary>
    public partial class ServiceTestUserControl : UserControl
    {
        internal event EventHandler ConfigurationChangedByUser;

        /// <summary>
        /// Identification of my test service
        /// </summary>
        private Guid Instance = new Guid("D5A78B1D-0D7E-4AF0-8C16-83590737B937");

        public ServiceTestUserControl()
        {
            InitializeComponent();
        }

        private bool _ignoreChanges = false;
        internal void OnUserChange(object sender, EventArgs e)
        {
            if (ConfigurationChangedByUser != null && _ignoreChanges == false)
                ConfigurationChangedByUser(this, new EventArgs());
        }

        internal void FillContent(Item item)
        {
            _ignoreChanges = true;
            FillServiceList();
            _ignoreChanges = false;
        }


        internal void ClearContent()
        {
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            FillServiceList();
        }


        private void FillServiceList()
        {
            _listBoxServices.Items.Clear();
            try
            {
                List<Configuration.ServiceURIInfo> serviceUriInfo =
                    Configuration.Instance.GetRegisteredServiceUriInfo(Configuration.Instance.ServerFQID.ServerId);
                foreach (Configuration.ServiceURIInfo si in serviceUriInfo)
                {
                    _listBoxServices.Items.Add(si.Name + ", url=" + si.UriArray[0] + ", ServiceId=" + si.Type.ToString() + ", Data=" +
                                               si.ServiceData);
                }
            }
            catch (Exception ex)
            {
                _listBoxServices.Items.Add("Server services unavailable:" + ex.Message);
            }
        }

        private void OnRegister(object sender, EventArgs e)
        {
            try
            {
                Configuration.Instance.RegisterServiceUri(ServiceTestDefinition.ServiceTestKind,
                                                          Configuration.Instance.ServerFQID.ServerId, Instance,
                                                          new Uri("http://testserver:5555"), "A test service", "Of no use",
                                                          "Custom data content");
            }
            catch (Exception)
            {
                MessageBox.Show("You cannot register the same service twice");
            }
        }

        private void OnUnRegister(object sender, EventArgs e)
        {
            try
            {
                Configuration.Instance.UnRegisterServiceUri(Configuration.Instance.ServerFQID.ServerId, Instance);
            }
            catch (Exception)
            {
                MessageBox.Show("There was no service to Unregister");
            }
        }

    }
}
