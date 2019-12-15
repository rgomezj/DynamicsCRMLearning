using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Models
{
    public class SalesOrderItemsListViewModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountRefunded { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal ManualDiscount { get; set; }
        public decimal CourseCredit { get; set; }
        public decimal ScholarshipAmount { get; set; }
        public decimal Tax { get; set; }
        public int LineItemNumber { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string RegistrationName { get; set; }
        public string RegistrationId { get; set; }
        public string EventName { get; set; }
        public string ContactFullName { get; set; }
        public string SalesOrderId { get; set; }
        public string ReservationId { get; set; }
        public string ReservationName { get; set; }
        public string EventOptionId { get; set; }
        public string ProductId { get; set; }
        public string ContactId { get; set; }
        public string OrderManagementItemId { get; set; }
        public ActionItem ActionItem { get; set; }
        public LineItemType LineItemType { get; set; }
        public bool Refunded { get; set; }
        public bool Deactivated { get; set; }
    }
}