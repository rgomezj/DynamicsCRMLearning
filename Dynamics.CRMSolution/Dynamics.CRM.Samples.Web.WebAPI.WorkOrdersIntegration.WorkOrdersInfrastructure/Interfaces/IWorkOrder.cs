using System;
using System.Collections.Generic;
using System.ServiceModel;
using CoreModels = Arcos.CUC.WorkOrdersIntegration.Core;

namespace Arcos.CUC.WorkOrdersInfrastructure.Interfaces
{
    public interface IWorkOrder
    {
        // This will query the database for the ID's
        string GetAssociatedWorkOrders(string customerid, string locationid);

        // Other one that will query the External Web service for the detail of a work order
        CoreModels.WorkOrder GetWorkOrderDetails(string workordernumber);
    }
}







