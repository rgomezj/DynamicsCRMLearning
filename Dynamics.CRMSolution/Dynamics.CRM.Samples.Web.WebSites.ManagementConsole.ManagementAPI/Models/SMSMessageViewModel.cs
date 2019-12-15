using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Models
{
    public class SMSMessageViewModel
    {
        public Guid Id { get; set; }
        public string MessageId { get; set; }
        public StatusReasonSMSMessage MessageStatus { get; set; }
        public StateSMSMessage State { get; set; }

    }
}