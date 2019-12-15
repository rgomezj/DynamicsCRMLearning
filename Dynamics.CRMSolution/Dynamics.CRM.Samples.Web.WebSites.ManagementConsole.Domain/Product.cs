using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class Product
  {

        #region Attributes

        #endregion

        #region Properties

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public PriceList PriceList { get; set; }
        public string Description { get; set; }

        #endregion
    }
}
