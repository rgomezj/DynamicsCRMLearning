using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pavliks.WAM.ManagementConsole.Domain;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces
{
    public interface IOrderManagementItemRepository
    {
        List<OrderManagementItem> GetOrderManagementItemBySalesOrder(Guid salesOrderGuid, StatusItem status);
        void ChangeStatusOrderManagementItem(Guid orderManagementGuid, StatusItem status, StateItem state);
        void DeleteOrderManagementItem(Guid orderManagementGuid);
        Guid CreateOrderManagementItem(OrderManagementItem orderManagementItem);
        void UpdateOrderManagemertItem(OrderManagementItem orderManagementItem);
        void UpdateOrderManagemertItemDeactivatedFlag(OrderManagementItem orderManagementItem);
    }
}
