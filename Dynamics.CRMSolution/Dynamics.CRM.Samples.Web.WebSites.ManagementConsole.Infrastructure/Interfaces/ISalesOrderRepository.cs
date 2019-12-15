using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces
{
    public interface ISalesOrderRepository
    {
        SalesOrder GetSalesOrderById(string id);
        Guid CreateOrder(SalesOrder order);
        void OpenOrder(Guid salesOrderId);
        void CloseOrder(Guid salesOrderId);
        void isScholarpshipOrder(Guid salesOrderId, bool isScholarpship);
        void CancelOrder(Guid salesOrderId);
    }
}
