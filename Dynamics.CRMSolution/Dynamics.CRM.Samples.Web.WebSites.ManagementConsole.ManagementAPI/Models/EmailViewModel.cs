using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Models
{
    public class EmailViewModel
    {
        public List<Contact> Contacts { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public TypeOfTransaction TypeOfTransaction { get; set; }
    }
}