using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pavliks.WAM.ManagementConsole.Domain;
using CrmToolkit;
using Pavliks.WAM.ManagementConsole.Helpers;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class SMSMessageCRM : ISMSMessageRepository
    {
        DataManager DataManager = new DataManager();

        /// <summary>
        /// Method that allows setting the status of the sms messsage.
        /// </summary>
        /// <param name="smsMessage"></param>
        public void SetStatus(SMSMessage smsMessage)
        {

            #region Set Status
            SetStateRequest state = new SetStateRequest();
            state.State = new OptionSetValue((int)smsMessage.State);
            state.Status = new OptionSetValue((int)smsMessage.MessageStatus);
            EntityReference smsMessageReference = new EntityReference("dm_smsmessage", smsMessage.Id);
            state.EntityMoniker = smsMessageReference;
            SetStateResponse stateSet = (SetStateResponse)DataManager.Execute(state);
            #endregion
        }
        /// <summary>
        /// Method that allows getting a sms message by messageid.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public SMSMessage GetSMSMessageByMessageId(string messageId)
        {

            QueryExpression query = new QueryExpression("dm_smsmessage")
            {
                ColumnSet = new ColumnSet(true)
            };

            query.Criteria.AddCondition("dm_messageid", ConditionOperator.Equal, messageId);

            EntityCollection smsMessageColletion = DataManager.RetrieveMultiple(query);

            if(smsMessageColletion.Entities.Count>0)
            {
                SMSMessageMapper smsMessageMapper = new SMSMessageMapper();
                return smsMessageMapper.EntityToDomain(smsMessageColletion.Entities[0]);
            }
            else
            {
                return null;
            }


        }
    }
}
