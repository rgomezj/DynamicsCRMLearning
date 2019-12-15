using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{
    public class OrderCoursesMapper
    {
        private IOrganizationService _organizationService;

        public OrderCoursesMapper(IOrganizationService service)
        {
            this._organizationService = service;
        }

        public Entity DomainToEntity(OrderCourses order)
        {
            Entity orderTransaction = new Entity("dm_orderevent");
            orderTransaction.Id = order.Id;
            if (order.Event != null)
            {
                orderTransaction["dm_courseid"] = new EntityReference("dm_course", order.Event.Id);
            }
            if (order.SalesOrder != null)
            {
                orderTransaction["dm_orderid"] =new EntityReference("salesorder",order.SalesOrder.Id) ;
            }
            if (order.Name != null)
            {
                orderTransaction["dm_name"] = order.Name;
            }
           
            return orderTransaction;
        }
    }
}
