using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavliks.WAM.ManagementConsole.BL
{


    public class SMSMessageBL
    {

        #region Attributes
        private ISMSMessageRepository _SMSMessageRepository;
        #endregion
        #region Properties
        #endregion
        #region Methods
        public SMSMessageBL(ISMSMessageRepository smsMessageRepository)
        {
            this._SMSMessageRepository = smsMessageRepository;
        }
        /// <summary>
        /// Method that allows setting the status of the sms messsage.
        /// </summary>
        /// <param name="smsMessage"></param>
        public void SetStatus(SMSMessage smsMessage)
        {
            _SMSMessageRepository.SetStatus(smsMessage);
        }

        /// <summary>
        /// Method that allows getting a sms message by messageid.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public SMSMessage GetSMSMessageByMessageId(string messageId)
        {
            return _SMSMessageRepository.GetSMSMessageByMessageId(messageId);
        }
        #endregion




    }
}
