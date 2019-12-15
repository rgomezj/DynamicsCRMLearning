
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.BL
{
    public class ContactBL
    {
        private IContactRepository _ContactRepository;

        public ContactBL(IContactRepository _ContactRepository)
        {
            this._ContactRepository = _ContactRepository;
        }

        public List<Contact> GetContactsByFilter(string filter)
        {
            return _ContactRepository.GetContactsByFilter(filter);
        }
    }
}
