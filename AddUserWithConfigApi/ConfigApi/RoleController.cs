using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoOS.ConfigurationAPI;

namespace AddUserSample.ConfigApi
{
    public class RoleController
    {
        /// <summary>
        /// Queries all the roles in the system using the configuration API
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> QueryRoles()
        {
            var configurationApiClient = ConfigurationApiClient.CreateClient();

            // The existing roles are children under of the RoleFolder
            string roleFolderPath = String.Format("/{0}", ItemTypes.RoleFolder);
            var roleConfigItems = configurationApiClient.GetChildItems(roleFolderPath);

            var roles = new Dictionary<string, string>();
            foreach (var role in roleConfigItems)
            {
                roles.Add(role.DisplayName, role.Path);
            }

            return roles;
        }
    }
}
