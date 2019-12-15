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
    ///  Mapper class that converts a session entity to a session domain.
    /// </summary>
    public class CourseClassMapper
    {
        /// <summary>
        /// Mapper that converts a price list entity to a price list domain.
        /// </summary>
        /// <param name="courseClassEntity">Price list as entity.</param>
        /// <returns>Price list converts as price list domain.</returns>
        public CourseClass EntityToDomain(Entity courseClassEntity)
        {
            CourseClass session = new CourseClass();
            session.Id = courseClassEntity.Id;
            if (courseClassEntity.Contains("dm_id"))
            {
                session.dm_id = (string)courseClassEntity["dm_id"];
            }
            return session;
        }

    }
}
