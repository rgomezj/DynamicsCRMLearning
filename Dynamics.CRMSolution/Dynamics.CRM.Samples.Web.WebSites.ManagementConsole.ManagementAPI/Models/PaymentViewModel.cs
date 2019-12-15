using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Models
{
    public class PaymentViewModel
    {
        public Guid SalesOrderId { get; set; }
        public string ChequeNumber { get; set; }
        public bool IsCheque { get; set; }

    }
}