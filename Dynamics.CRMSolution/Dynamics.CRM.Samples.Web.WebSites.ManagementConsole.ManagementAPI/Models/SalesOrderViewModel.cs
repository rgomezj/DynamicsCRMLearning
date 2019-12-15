using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Models
{
    public class SalesOrderViewModel
    {
        public Guid Id { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OutStandingBalance { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public StateOrder StateOrder { get; set; }
        public PaymentType paymentType { get; set; }
        public string ChequeNumber { get; set; }
        public bool WillSpecifyLater { get; set; }
        public bool IsCheque { get; set; }

    }
}