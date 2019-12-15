using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Models
{
    public class NewRegistrationViewModel
    {
        public Product RegistrationLevel { get; set; }
        public List<CourseClass> Classes { get; set; }
        public List<Product> EventOptions { get; set; }
        public Contact Contact { get; set; }
        public Event Event { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public bool CreateSalesOrder { get; set; }
        public Waitlist Waitlist { get; set; }
        public AttendanceType AttendanceType { get; set; }
    }
}