using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class SalesOrder
    {
        public Guid Id { get; set; }
        public decimal? OutstandingBalance { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string ChequeNumber { get; set; }
        public PriceList PriceList   { get; set; }
        public Currency Currency { get; set; }
        public bool IsPriceLocked { get; set; }
        public Contact Customer { get; set; }
        public Waitlist Waitlist { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public StateOrder StateOrder { get; set; }
        public TypeOrder? typeOfOrder { get; set; }
        public PaymentType? paymentType { get; set; }
    }
}
