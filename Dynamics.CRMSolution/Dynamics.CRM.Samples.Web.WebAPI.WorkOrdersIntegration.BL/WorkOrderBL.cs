using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcos.CUC.WorkOrdersIntegration.BL
{
    public class WorkOrderBL
    {
        WorkOrdersInfrastructure.Interfaces.IWorkOrder workOrderRepository;

        public WorkOrderBL(WorkOrdersInfrastructure.Interfaces.IWorkOrder _workOrderRepository)
        {
            this.workOrderRepository = _workOrderRepository;
        }

        public WorkOrderBL()
        {
            this.workOrderRepository = new WorkOrdersInfrastructure.Implementation.WorkOrder();
        }

        public string GetAssociatedWorkOrders(string customerid, string locationid)
        {
            return this.workOrderRepository.GetAssociatedWorkOrders(customerid, locationid);
        }

        public Core.WorkOrder GetWorkOrderDetails(string workordernumber)
        {
            return this.workOrderRepository.GetWorkOrderDetails(workordernumber);
        }
    }
}
