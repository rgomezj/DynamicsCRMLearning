using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using CrmToolkit;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
   public class SessionCRM : ISessionRepository
    {
        public List<CourseClass> GetSessions(Guid eventId)
        {
            List<CourseClass> sessions = new List<CourseClass>();
            //Creates an crm connection.
            DataManager DataManager = new DataManager();
            //Now I will search the order  that is associated with the registration.
            QueryExpression eventQuery = new QueryExpression("dm_class")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_subject", "dm_id" })
            };
            eventQuery.Criteria.AddCondition("dm_courseid", ConditionOperator.Equal, eventId);
            
            EntityCollection sessionCollection = DataManager.RetrieveMultiple(eventQuery);

            for (int i = 0; i < sessionCollection.Entities.Count; i++)
            {
                CourseClass e = new CourseClass();
                
                e = sessionCollection.Entities[i].EntityToDto(e);
                sessions.Add(e);

            }

            return sessions;

        }
    }
}
