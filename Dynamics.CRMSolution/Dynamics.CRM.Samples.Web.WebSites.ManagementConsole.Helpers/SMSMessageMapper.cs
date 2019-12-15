using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{

    /// <summary>
    ///  Mapper class that converts a sales order entity to a sales order domain.
    /// </summary>

    public class SMSMessageMapper
    {

        /// <summary>
        /// Mapper that converts a sms message entity to a sms message domain.
        /// </summary>
        public Entity DomainToEntity(SMSMessage smsMessageDomain)
        {
            Entity smsMessage = new Entity("dm_smsmessage");
            smsMessage.Id = smsMessageDomain.Id;
            smsMessage["statuscode"] = new OptionSetValue((int)smsMessageDomain.MessageStatus);
            smsMessage["statecode"] = new OptionSetValue((int)smsMessageDomain.State);
            return smsMessage;
        }


        /// <summary>
        /// Mapper that converts a sms message entity to a sms message domain.
        /// </summary>
        public SMSMessage EntityToDomain(Entity smsMessageEntity)
        {
            SMSMessage smsMessage = new SMSMessage();
            smsMessage.Id = smsMessageEntity.Id;

            string attribute = string.Empty;

            if (smsMessageEntity.Contains("dm_messageid"))
            {
                smsMessage.MessageId = (string)smsMessageEntity["dm_messageid"];

            }
            if (smsMessageEntity.Contains("statuscode"))
            {
                smsMessage.MessageStatus = (StatusReasonSMSMessage)((OptionSetValue)smsMessageEntity["statuscode"]).Value;
                
            }

            if (smsMessageEntity.Contains("statecode"))
            {
                smsMessage.State = (StateSMSMessage)((OptionSetValue)smsMessageEntity["statecode"]).Value;

            }

            return smsMessage;
        }


    }

}

