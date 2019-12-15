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
using Pavliks.WAM.ManagementConsole.Helpers;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class EventCRM : IEventRepository
    {
        public Event GetEventById(Guid id)
        {
            //Creates an crm connection.
            DataManager DataManager = new DataManager();
            //Now I will search the order  that is associated with the registration.

            ColumnSet ColumnSet = new ColumnSet(new string[] { "dm_name", "dm_maxregistratnts", "dm_startdate", "dm_waitlist", "dm_hybrid","dm_places" });

            Entity eventEntity= DataManager.Retrieve(id, "dm_course", ColumnSet);
            EventMapper eventMapper = new EventMapper();
            Event e = new Event();
            e = eventMapper.EntityToDomain(eventEntity);
            return e;
            //return DataManager.Retrieve(id, "dm_event", ColumnSet);
        }

        public List<Event> GetEventsByFilter(string filter)
        {
            List<Event> events = new List<Event>();
            
            DataManager DataManager = new DataManager();
            
            //Now I will search the order  that is associated with the registration.
            QueryExpression eventQuery = new QueryExpression("dm_course")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_name", "dm_maxregistratnts", "dm_startdate", "dm_waitlist", "dm_totalregistrants", "dm_places" })
            };

            eventQuery.Criteria.AddCondition("dm_name", ConditionOperator.Like, "%" + filter + "%");

            EntityCollection eventColletion = DataManager.RetrieveMultiple(eventQuery);
            EventMapper eventMapper = new EventMapper();

            for (int i = 0; i < eventColletion.Entities.Count; i++)
            {
                Event eventNew = new Event();
                eventNew = eventMapper.EntityToDomain(eventColletion.Entities[i]);
                events.Add(eventNew);
                

            }
            return events;
        }

        List<Event> IEventRepository.GetAllEvents()
        {
            List<Event> events = new List<Event>();
            //Creates an crm connection.
            DataManager DataManager = new DataManager();
            //Now I will search the order  that is associated with the registration.
            QueryExpression eventQuery = new QueryExpression("dm_course")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_name", "dm_maxregistratnts", "dm_startdate", "dm_waitlist", "dm_totalregistrants", "dm_places" })
            };
            // eventQuery.Criteria.AddCondition("dm_enablewaitlist", ConditionOperator.Equal, true);
            // eventQuery.Criteria.AddCondition("dm_eventstatus", ConditionOperator.Equal, 7);

            EntityCollection eventColletion = DataManager.RetrieveMultiple(eventQuery);
            EventMapper eventMapper = new EventMapper();
            for (int i = 0; i < eventColletion.Entities.Count; i++)
            {
                Event eventNew = new Event();

                //eventNew.Id = e.Id;
                //if (e.Contains("dm_name"))
                //{
                //    eventNew.Name = e["dm_name"].ToString();
                //}
                eventNew = eventMapper.EntityToDomain(eventColletion.Entities[i]);
                events.Add(eventNew);

            }

            return events;

        }

 
    }
}
