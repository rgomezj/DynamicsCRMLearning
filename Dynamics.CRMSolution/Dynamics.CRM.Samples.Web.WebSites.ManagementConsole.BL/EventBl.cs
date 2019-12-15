
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.BL
{
    public class EventBl
    {
        #region Attributes

        private IEventRepository _EventRepository;

        #endregion

        #region Properties

        #endregion

        #region Methods

        public List<Event> GetAllEvents()
        {
            return _EventRepository.GetAllEvents();
        }

        #endregion

        #region Constructor

        public EventBl(IEventRepository _EventRepository)
        {
            this._EventRepository = _EventRepository;
        }

        public Event GetEventById(Guid id)
        {
            Event eventInfo = _EventRepository.GetEventById(id);
            //eventInfo.SpacesAvailable = eventInfo.dm_registrationlimit - eventInfo.dm_totalregistrants; 
            return eventInfo;
        }

        public List<Event> GetEventsByFilter(string filter)
        {
            List<Event> events = _EventRepository.GetEventsByFilter(filter);
            return events;
        }

        #endregion

    }
}
