using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Arcos.CUC.WorkOrdersIntegration.WebAPIClient.Controllers
{
    [EnableCors(origins: "https://crmdev.cuc.ky", headers: "*", methods: "*")]
    public class WorkOrdersController : ApiController
    {
    
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetAssociatedWorkOrders(QueryWorkOrder queryData)
        {
            // Ninject should resolve this dependency of WorkOrderBL with IWorkOrder. We have not configured Ninject yet, so the default constructor will be used, which is ok
            WorkOrdersIntegration.BL.WorkOrderBL workOrderBL = new WorkOrdersIntegration.BL.WorkOrderBL();
            string workOrders = workOrderBL.GetAssociatedWorkOrders(queryData.CustomerId, queryData.LocationId);

            return Request.CreateResponse(HttpStatusCode.OK, new { workOrders = workOrders });
        }

        [System.Web.Http.HttpGet]
        public WorkOrderData GetWorkOrderDetails(string id)
        {
            WorkOrdersIntegration.BL.WorkOrderBL workOrderBL = new WorkOrdersIntegration.BL.WorkOrderBL();
            WorkOrdersIntegration.Core.WorkOrder workOrderDetails = workOrderBL.GetWorkOrderDetails(id);

            AutoMapper.Mapper.CreateMap(typeof(WorkOrdersIntegration.Core.WorkOrder), typeof(WorkOrderData));
            WorkOrderData workOrderData = AutoMapper.Mapper.Map<WorkOrdersIntegration.Core.WorkOrder, WorkOrderData>(workOrderDetails);

            return workOrderData;
        }
    }
}
