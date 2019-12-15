using System;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class OrderManagementItemViewModel
    {
        public Guid Id { get; set; }

        public Guid SalesOrderItem { get; set; }
    }
}