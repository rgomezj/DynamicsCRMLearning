using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pavliks.WAM.ManagementConsole.Domain;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces
{
    public interface IWaitlistRepository
    {
        List<Waitlist> GetAllTheWaistlist();

        List<Waitlist> GetWaistlists(Guid eventId);

        Guid SaveWaistlists(Waitlist waitList);

        string GenerateLink(Waitlist waitlist, bool updateSalesOrder);

        void SendEmail(Waitlist waitlist, int trigger);
    }
}
