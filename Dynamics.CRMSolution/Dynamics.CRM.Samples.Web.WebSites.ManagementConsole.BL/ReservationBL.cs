
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.BL
{
    public class ReservationBL
    {
        private IReservationRepository _ReservationRepository;
        
        public List<Reservation> GetReservationsByRegistration(Guid registration)
        {
            return _ReservationRepository.GetReservationsByRegistration(registration);
        }
        
        public ReservationBL(IReservationRepository _ReservationRepository)
        {
            this._ReservationRepository = _ReservationRepository;
        }
        
    }
}
