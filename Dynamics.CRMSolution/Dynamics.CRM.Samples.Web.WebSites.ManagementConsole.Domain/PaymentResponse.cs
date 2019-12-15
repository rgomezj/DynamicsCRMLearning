using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class PaymentResponse
    {

        #region Attributes

        #endregion

        #region Properties

        public bool OK { get; set; }
        public bool isCheque { get; set; }
        public string Message { get; set; }
        public string ChequeNumber { get; set; }

        #endregion
    }
}
