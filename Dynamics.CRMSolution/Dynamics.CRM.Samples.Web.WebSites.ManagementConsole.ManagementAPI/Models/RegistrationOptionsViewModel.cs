using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Models
{
    public class RegistrationOptionsViewModel
    {
        public Registration Registration { get; set; }
        public Event Event { get; set; }
        public List<Product> RegistrationLevels { get; set; }
        public List<CourseClass> Classes { get; set; }
        public List<Product> EventOptions { get; set; }
    }
}