using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces
{
    public interface IRegistrationRepository
    {
        List<Registration> GetAllTheRegistration(string eventId);
        List<Registration> GetRegistrationsByOrder(Guid orderId);
        Registration GetRegistrationById(Guid registrationId);
        void UpdateRefundedRegistration(Registration registration);
        void UpdateDeactivatedRegistration(Registration registration);
    }
}
