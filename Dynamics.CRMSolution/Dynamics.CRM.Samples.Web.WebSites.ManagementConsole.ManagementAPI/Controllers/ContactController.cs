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
    public class ContactController : ApiController
    {
        private ContactBL _ContactBL;

        public ContactController(IContactRepository _contactRepository)
        {
            _ContactBL = new ContactBL(_contactRepository);
        }

        [System.Web.Http.HttpGet]
        public List<Contact> Contact(string name)
        {
            List<Contact> contacts = _ContactBL.GetContactsByFilter(name);
            return contacts;
        }
    }
}
