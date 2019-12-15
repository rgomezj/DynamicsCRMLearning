using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{

    /// <summary>
    ///  Mapper class that converts a event entity to a event domain.
    /// </summary>

    public class EventMapper
    {

        private IOrganizationService _organizationService;
        public EventMapper(IOrganizationService service)
        {

            this._organizationService = service;
        }

        public EventMapper()
        {
        }

        /// <summary>
        /// Mapper that converts a event entity to a event domain.
        /// </summary>
        /// <param name="eventEntity">Sales order as entity.</param>
        /// <returns>Sales order converts as sales order domain.</returns>
        public Event EntityToDomain(Entity eventEntity)
        {
            Event eventDomain = new Event();
            eventDomain.Id = eventEntity.Id;
            if (eventEntity.Contains("dm_name"))
            {
                eventDomain.Name = (string)eventEntity["dm_name"];

            }
            if (eventEntity.Contains("dm_maxregistratnts"))
            {
                eventDomain.dm_registrationlimit = (int)eventEntity["dm_maxregistratnts"];

            }

            if (eventEntity.Contains("dm_startdate"))
            {
                eventDomain.dm_startdate = (DateTime)eventEntity["dm_startdate"];

            }
            
            if (eventEntity.Contains("dm_waitlist"))
            {
                eventDomain.dm_waitlist = (int)eventEntity["dm_waitlist"];
            }
            if (eventEntity.Contains("dm_hybrid"))
            {
                eventDomain.dm_hybrid = (bool)eventEntity["dm_hybrid"];

            }

            if (eventEntity.Contains("dm_totalregistrants"))
            {
                eventDomain.dm_totalregistrants = (int)eventEntity["dm_totalregistrants"];

            }
            if (eventEntity.Contains("dm_places"))
            {
                eventDomain.SpacesAvailable = (int)eventEntity["dm_places"];

            }

            return eventDomain;
        }

        /// <summary>
        /// Mapper that converts a event entity to a event domain.
        /// </summary>
        /// <param name="salesOrderEntity">Event as domain.</param>
        /// <returns>Event converts as entity.</returns>
        public Entity DomainToEntity(Event eventDomain)
        {
            Entity eventEntity = new Entity("dm_course");
            eventEntity.Id = eventDomain.Id;

            eventEntity["dm_name"] = eventDomain.Name;
            eventEntity["dm_maxregistratnts"] = eventDomain.dm_registrationlimit;
            eventEntity["dm_startdate"] = eventDomain.dm_startdate;
            eventEntity["dm_waitlist"] = eventDomain.dm_waitlist;
            eventEntity["dm_hybrid"] = eventDomain.dm_hybrid;
            eventEntity["dm_totalregistrants"] = eventDomain.dm_totalregistrants;
            return eventEntity;
        }


    }

}

