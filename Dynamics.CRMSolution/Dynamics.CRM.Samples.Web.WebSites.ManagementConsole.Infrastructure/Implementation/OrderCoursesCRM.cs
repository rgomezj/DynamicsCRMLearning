using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pavliks.WAM.ManagementConsole.Domain;
using System.Web.Configuration;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using CrmToolkit;
using Pavliks.WAM.ManagementConsole.Helpers;
using Microsoft.Xrm.Sdk.Client;
using System.ServiceModel.Description;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class OrderCoursesCRM : IOrderCoursesRepository
    {
        DataManager DataManager = new DataManager();

        public OrderCoursesCRM()
        {

        }
       
        public Guid CreateOrderCourses(OrderCourses order)
        {
            //Creates an crm connection.
            DataManager DataManager = new DataManager();
            OrderCoursesMapper orderCoursesMapper = new OrderCoursesMapper(DataManager.ConnectionOnpremise());
            return DataManager.Create(orderCoursesMapper.DomainToEntity(order));
        }
        
    }
}
