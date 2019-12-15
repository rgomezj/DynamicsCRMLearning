using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class SalesOrderItem
    {
        #region Properties
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal ManualDiscount { get; set; }
        public decimal CourseCredit { get; set; }
        public decimal ScholarshipAmount { get; set; }
        public decimal Tax { get; set; }
        public int LineItemNumber   { get; set; }
        public Product Product { get; set; }
        public string ProductDescription { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public Registration Registration { get; set; }
        public Event Event { get; set; }
        public Contact Contact { get; set; }
        public CourseClass Session { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public Reservation Reservation { get; set; }
        public bool IsNew { get; set; }
        public LineItemType LineItemType { get; set; }
        public LineItemTypeInSalesOrder LineItemTypeInsalesOrder { get; set; }
        public bool Refunded { get; set; }
        public bool Deactivated { get; set; }

        #endregion
    }
}
