using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pavliks.WAM.ManagementConsole.Domain;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces
{
    public interface IReservationRepository
    {
        Guid CreateReservation(Reservation reservation);

        List<Reservation> GetReservationsByRegistration(Guid registrationId);
    }
}
