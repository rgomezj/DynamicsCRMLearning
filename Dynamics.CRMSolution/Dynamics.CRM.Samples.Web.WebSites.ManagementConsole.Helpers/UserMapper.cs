using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{
    /// <summary>
    ///  Mapper class that converts an user entity to an user domain.
    /// </summary>
    public class UserMapper
    {


        /// <summary>
        /// Mapper that converts a user entity to a user domain.
        /// </summary>
        /// <param name="userEntity">Registration as entity.</param>
        /// <returns>Registration converts as sales order domain.</returns>
        public User EntityToDomain(Entity userEntity)
        {
            User user = new User();
            user.Id = userEntity.Id;
            if (userEntity.Contains("fullname"))
            {
                user.FullName = (string)userEntity["fullname"];
            }

            return user;
        }

    }
}
