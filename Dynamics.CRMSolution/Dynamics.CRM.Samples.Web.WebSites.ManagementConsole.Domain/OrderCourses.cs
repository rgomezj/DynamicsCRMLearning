using CrmToolkit.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class OrderCourses
    {
        #region Properties
       
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public SalesOrder SalesOrder { get; set; }

        public Event Event { get; set; }

        #endregion
    }
}
