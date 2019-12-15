
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.BL
{
   public class SessiontBL
   {
       #region Attributes

       private ISessionRepository _SessionRepository;

        #endregion

        public SessiontBL(ISessionRepository _SessionRepository)
        {
            this._SessionRepository = _SessionRepository;
        }

        #region Methods

        public List<CourseClass> GetSessions(Guid eventId)
        {
            return _SessionRepository.GetSessions(eventId);
        }

        #endregion
   }
}
