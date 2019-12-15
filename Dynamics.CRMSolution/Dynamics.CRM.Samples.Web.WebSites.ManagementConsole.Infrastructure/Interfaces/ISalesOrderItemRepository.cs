using Microsoft.Xrm.Sdk;
using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces
{
    public interface ISalesOrderItemRepository
    {
        List<SalesOrderItem> GetAllSalesOrderItemByOrderId(string orderId);
        void UpdateSalesOrderItem(SalesOrderItem salesOrderItem);
        void CreateSalesOrderItem(SalesOrderItem item);
    }
}
