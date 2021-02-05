using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;

namespace AddUserSample.ConfigApi
{
    internal class UserController
    {
        public bool CreateAndAuthorizeUser(string userName, string password, string rolePath)
        {
            var configurationApiClient = ConfigurationApiClient.CreateClient();
            var newUserPath = CreateUser(configurationApiClient, userName, password);
            var success = AuthorizeUser(configurationApiClient, newUserPath, rolePath);

            return success;
        }

        /// <summary>
        /// Creates the user using the configuration API
        /// </summary>
        /// <param name="configurationApiClient"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string CreateUser(IConfigurationService configurationApiClient, string userName, string password)
        {
            // The create user functionality is a method on BasicUserFolder
            string userFolderPath = string.Format("/{0}", ItemTypes.BasicUserFolder);

            var userFolder = configurationApiClient.GetItem(userFolderPath);
            var addUserInvokeInfo = configurationApiClient.InvokeMethod(userFolder, "AddBasicUser");

            // Changing the properties of the invokeInfo object to the provided parameters
            var nameProperty = addUserInvokeInfo.Properties.Where(property => property.Key == "Name").FirstOrDefault();
            nameProperty.Value = userName;

            var passwordProperty = addUserInvokeInfo.Properties.Where(property => property.Key == "Password").FirstOrDefault();
            passwordProperty.Value = password;

            var descriptionProperty = addUserInvokeInfo.Properties.Where(property => property.Key == "Description").FirstOrDefault();
            descriptionProperty.Value = "Added test user";

            var canChangePasswordProperty = addUserInvokeInfo.Properties.Where(property => property.Key == "CanChangePassword").FirstOrDefault();
            canChangePasswordProperty.Value = false.ToString();

            // Calling the AddBasicUser method on BasicUserFolder again with the prepared invokeInfo object
            var addUserInvokeResult = configurationApiClient.InvokeMethod(addUserInvokeInfo, "AddBasicUser");

            return addUserInvokeResult.Path;
        }

        /// <summary>
        /// Adds a given user to a given role as a member
        /// </summary>
        /// <param name="configurationApiClient"></param>
        /// <param name="userPath"></param>
        /// <param name="rolePath"></param>
        /// <returns></returns>
        private bool AuthorizeUser(IConfigurationService configurationApiClient, string userPath, string rolePath)
        {
            // Gets the given user and stores the SID of the user
            var newUser = configurationApiClient.GetItem(userPath);
            var sidProperty = newUser.Properties.Where(property => property.Key == "Sid").FirstOrDefault();

            // Adding role members is possible with the AddRoleMember method on the UserFolder of a specific Role
            string userFolderPath = string.Format("{0}/{1}", rolePath, ItemTypes.UserFolder);
            var userFolder = configurationApiClient.GetItem(userFolderPath);

            ConfigurationItem authorizeUserInfokeInfo = configurationApiClient.InvokeMethod(userFolder, "AddRoleMember");

            // Changing the properties of the invokeInfo object to the SID of the user
            var nameProperty = authorizeUserInfokeInfo.Properties.Where(property => property.Key == "Sid").FirstOrDefault();
            nameProperty.Value = sidProperty.Value;

            ConfigurationItem invokeResultItem = configurationApiClient.InvokeMethod(authorizeUserInfokeInfo, "AddRoleMember");

            return invokeResultItem.Properties.FirstOrDefault(property => property.Key == "State").Value == "Success";
        }
    }
}
