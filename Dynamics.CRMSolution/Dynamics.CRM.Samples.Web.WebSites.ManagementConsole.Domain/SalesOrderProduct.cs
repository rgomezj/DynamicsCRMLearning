using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class SalesOrderProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Registration Registration { get; set; }
        public SalesOrder Order { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
    }
}
