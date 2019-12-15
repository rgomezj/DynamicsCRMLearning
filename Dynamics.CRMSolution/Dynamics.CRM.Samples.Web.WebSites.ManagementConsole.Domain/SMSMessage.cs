using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class SMSMessage
    {

        #region Properties
        public Guid Id { get; set; }
        public string MessageId { get; set; }
        //This one is the same status reason.
        public StatusReasonSMSMessage MessageStatus { get; set; }
        public StateSMSMessage State { get; set; }
        #endregion

    }


}
