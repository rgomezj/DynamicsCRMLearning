using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pavliks.WAM.ManagementConsole.Domain;
using System.Net;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces
{
    public interface IEmailRepository
    {
        bool SendEmailReceipt(Email emailToSend);
        byte[] ReceiptReport(Configuration configuration, string salesOrderId);
    }
}
