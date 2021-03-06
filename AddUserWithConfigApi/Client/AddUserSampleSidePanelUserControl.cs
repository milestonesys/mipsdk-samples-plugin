using AddUserSample.ConfigApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace AddUserSample.Client
{
    /// <summary>
    /// This template code should be replaced with your code.
    /// 
    /// The UserControl to be displayed in a side panel in the Smart Client.<br/>
    /// Generated by a plug-ins SidePanelPlugin.
    /// </summary>
    public partial class AddUserSampleSidePanelUserControl : SidePanelUserControl
    {
        private UserController userController = new UserController();
        private RoleController roleController = new RoleController();

        public AddUserSampleSidePanelUserControl()
        {
            InitializeComponent();

        }

        public override void Init()
        {
            QueryRoles();
        }

        private void QueryRoles()
        {

            try
            {
                var roles = roleController.QueryRoles();

                rolesCombobox.DisplayMember = "Key";
                rolesCombobox.ValueMember = "Value";
                rolesCombobox.DataSource = roles.ToArray();
            }
            catch(ArgumentException ex)
            {
                if (ex.Message.Contains("The server doesn't support the specified path"))
                {
                    printMessage("This plugin only support XPCO 10.1a or newer.");
                }
            }
            catch (Exception ex)
            {
                printMessage(ex.Message);
            }
        }

        private void commitButton_Click(object sender, EventArgs e)
        {
            try
            {
                var rolePath = rolesCombobox.SelectedValue;
                var userName = userNameTextBox.Text;
                var password = passwordTextBox.Text;

                if (rolePath == null)
                {
                    printMessage("Please select a role");
                    return;
                }
                var success = userController.CreateAndAuthorizeUser(userName, password, rolePath.ToString());

                if (success)
                {
                    printMessage(String.Format("User '{0}' is successfully created and added to role '{1}'.", userName, rolesCombobox.Text));
                }
            }
            catch (FaultException<ArgumentNullExceptionFault> ex)
            {
                if (ex.Message.Contains("VMO62002"))
                {
                    if (ex.Message.Contains("password"))
                    {
                        printMessage("Please provide a password.");
                    }
                    else if (ex.Message.Contains("name"))
                    {
                        printMessage("Please provide a user name.");
                    }
                }
            }
            catch (FaultException<ArgumentExceptionFault> ex)
            {
                if (ex.Message.Contains("Could not create the basic user.") && ex.Message.Contains("already exist."))
                {
                    printMessage(String.Format("User '{0}' username already exists. Please choose another name.", userNameTextBox.Text));
                }
            }
            catch (Exception ex)
            {
                printMessage(ex.Message);
            }
        }

        private void printMessage(string message)
        {
            resultTextBox.Text = message;
        }
    }
}
