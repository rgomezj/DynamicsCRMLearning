using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces
{
    public interface IOrderTransactionRepository
    {
        OrderTransaction GetFirstTransaction(Guid salesOrderId);
        OrderTransaction GetLastRefundTransaction(Guid salesOrderId);
        Guid CreateOrderTransaction(OrderTransaction order);
        List<OrderTransaction> GetTransactionByOrder(Guid salesOrderId);

    }
}
