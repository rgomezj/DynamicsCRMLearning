
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.BL
{
    public class EmailBL
    {
        private IEmailRepository _EmailRepository;
        private IConfigurationRepository _ConfigurationRepository;

        public EmailBL(IEmailRepository _EmailRepository, IConfigurationRepository _ConfigurationRepository)
        {
            this._EmailRepository = _EmailRepository;
            this._ConfigurationRepository = _ConfigurationRepository;
        }
        /// <summary>
        /// Method that sends an email to a group of contacts.
        /// </summary>
        public bool SendEmailReceipt(Email emailToSend)
        {
            Configuration configuration = _ConfigurationRepository.GetConfiguration();
            emailToSend.Configuration = configuration;
            return _EmailRepository.SendEmailReceipt(emailToSend);
        }

        /// <summary>
        /// Method that allows you to generate the receipt report
        /// </summary>
        /// <param name="salesOrderId">sales order id</param>
        /// <returns>The report as web reponse.</returns>
        public byte[] ReceiptReport(string salesOrderId)
        {
            Configuration configuration = _ConfigurationRepository.GetConfiguration();
            return _EmailRepository.ReceiptReport(configuration, salesOrderId);
        }
    }
}
