using Pavliks.WAM.ManagementConsole.BL;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class EventController : ApiController
    {
        private EventBl _EventBL;

        public EventController(IEventRepository _EventRepository)
        {
            _EventBL = new EventBl(_EventRepository);

        }

        [System.Web.Http.HttpGet]
        public List<Event> AllEvents()
        {
            var events = _EventBL.GetAllEvents();
            return events;
        }

        [System.Web.Http.HttpGet]
        public Event GetEvent(string id)
        {
            Guid eventId = Guid.Parse(id);
            Event eventData = _EventBL.GetEventById(eventId);
            return eventData;
        }


        [System.Web.Http.HttpGet]
        public List<Event> GetEventsByName(string name)
        {
            List<Event> events = _EventBL.GetEventsByFilter(name);
            return events;
        }
    }
}
