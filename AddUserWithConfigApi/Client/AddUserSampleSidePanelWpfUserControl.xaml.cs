using System;
using System.Linq;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.ConfigurationItems;

namespace AddUserSample.Client
{
    public partial class AddUserSampleSidePanelWpfUserControl : SidePanelWpfUserControl
    {
        public AddUserSampleSidePanelWpfUserControl()
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
                var roleFolder = new RoleFolder();
                var roles = roleFolder.Roles;

                rolesCombobox.DisplayMemberPath = nameof(Role.DisplayName);
                rolesCombobox.SelectedValuePath = nameof(Role.Path);
                rolesCombobox.ItemsSource = roles.ToArray();
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("The server doesn't support the specified path"))
                {
                    PrintMessage("This plugin only support XPCO 10.1a or newer.");
                }
            }
            catch (Exception ex)
            {
                PrintMessage(ex.Message);
            }
        }

        private void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rolePath = rolesCombobox.SelectedValue;
                var userName = userNameTextBox.Text;
                var password = passwordTextBox.Password.ToString();

                if (rolePath == null)
                {
                    PrintMessage("Please select a role");
                    return;
                }
                var folder = new BasicUserFolder();
                var addTask = folder.AddBasicUser(userName, "User created by AddUserWithConfigApi sample", password);
                if (addTask.State != StateEnum.Success)
                {
                    PrintMessage("Failed to create basic user with error: " + addTask.ErrorText);
                    return;
                }
                var newUser = new BasicUser(EnvironmentManager.Instance.CurrentSite.ServerId, addTask.Path);
                var role = new Role(EnvironmentManager.Instance.CurrentSite.ServerId, rolePath as string);
                var result = role.UserFolder.AddRoleMember(newUser.Sid);
                if (result.State == StateEnum.Success)
                {
                    PrintMessage(String.Format("User '{0}' is successfully created and added to role '{1}'.", userName, rolesCombobox.Text));
                }
                else
                {
                    PrintMessage("Operation failed with error: " + result.ErrorText);
                }
            }
            catch (Exception ex)
            {
                PrintMessage(ex.Message);
            }
        }

        private void PrintMessage(string message)
        {
            resultTextBox.Text = message;
        }

        private void rolesCombobox_Initialized(object sender, EventArgs e)
        {
        }
    }
}