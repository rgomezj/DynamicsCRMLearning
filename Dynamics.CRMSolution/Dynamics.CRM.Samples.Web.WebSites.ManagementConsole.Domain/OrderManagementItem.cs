using CrmToolkit.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class OrderManagementItem
    {
        #region Attributes

        public Guid Id { get; set; }

        public Guid SalesOrderItem { get; set; }

        public ActionItem ActionItem { get; set; }

        public StatusItem StatusItem { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public SalesOrder SalesOrder { get; set; }

        public Registration Registration { get; set; }

        public Reservation Reservation { get; set; }

        public Product Product { get; set; }

        public Contact Contact { get; set; }

        public LineItemType LineItemType { get; set; }

        public bool DeactivatedFlag { get; set; }


        #endregion
    }
}
