using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Models
{
    public class ModifyRegistrationViewModel
    {
        public Guid RegistrationId;
        public List<CourseClass> Sessions { get; set; }
        public List<Product> EventOptions { get; set; }
        public Contact Contact { get; set; }
        public Event Event { get; set; }
        public SalesOrder SalesOrder { get; set; }
    }
}