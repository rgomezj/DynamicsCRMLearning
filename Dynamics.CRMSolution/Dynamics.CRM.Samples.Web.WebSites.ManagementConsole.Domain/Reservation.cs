using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
   public class Reservation
    {

        #region Properties

        public Guid Id { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public CourseClass CourseClass { get; set; }
        public Registration Registration { get; set; }
        public StatusReason StatusReason { get; set; }
        public Event Course { get; set; }


        #endregion
    }
}
