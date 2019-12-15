using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces
{
    public interface ISMSMessageRepository
    {
       
        void SetStatus(SMSMessage smsMessage);
        SMSMessage GetSMSMessageByMessageId(string messageId);
    }
}
