using CrmToolkit.Attributes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Domain
{
    public class Configuration
    {
        #region Properties
       
        public Guid Id { get; set; }

        public string Name { get; set; }

        public User Emailsender { get; set; }
        
        public string EventCalendarURL { get; set; }

        public string ManagementConsoleURL { get; set; }

        public string PaymentURL { get; set; }

        public string ReportServerURL { get; set; }

        public string OrderReceiptReportPath { get; set; }

        public string UserNameReportServer { get; set; }

        public string PasswordReportServer { get; set; }

        public string ApiLoginID { get; set; }

        public string ApiTransactionKey { get; set; }

        public bool ManagementConsoleTestMode { get; set; }

        #endregion
    }
}
