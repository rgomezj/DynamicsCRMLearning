using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI
{
    public partial class CallBackSMSMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            log4net.LogManager.GetLogger(this.GetType()).Error("enter");

            //Request.Params["fdss"]
            string MessageSid = null;
            string MessageStatus = null;
            string ErrorCode = null;
            try
            {
                MessageSid = Request.Params["MessageSid"];
                MessageStatus = Request.Params["MessageStatus"];
                ErrorCode = Request.Params["ErrorCode"];
                log4net.LogManager.GetLogger(this.GetType()).Error("pre CRM");

                UpdateSMSMessage(GetCRMConnection(), MessageSid, MessageStatus, ErrorCode);

                log4net.LogManager.GetLogger(this.GetType()).Error("Message Sid: " + MessageSid);
                log4net.LogManager.GetLogger(this.GetType()).Error("Message Status: " + MessageStatus);
                log4net.LogManager.GetLogger(this.GetType()).Error("Error Code: " + ErrorCode);
                log4net.LogManager.GetLogger(this.GetType()).Error("post CRM");

            }
            catch (Exception exception)
            {
                log4net.LogManager.GetLogger(this.GetType()).Error("///////////////Error Starts////////////");
                log4net.LogManager.GetLogger(this.GetType()).Error("Method name: Page_Load");
                log4net.LogManager.GetLogger(this.GetType()).Error("Source: " + exception.Source);
                log4net.LogManager.GetLogger(this.GetType()).Error("/////////////////////////////");
                log4net.LogManager.GetLogger(this.GetType()).Error("StackTrace: " + exception.StackTrace);
                log4net.LogManager.GetLogger(this.GetType()).Error("/////////////////////////////");
                log4net.LogManager.GetLogger(this.GetType()).Error("InnerException: " + exception.InnerException);
                log4net.LogManager.GetLogger(this.GetType()).Error("/////////////////////////////");
                log4net.LogManager.GetLogger(this.GetType()).Error("Message: " + exception.Message);
                log4net.LogManager.GetLogger(this.GetType()).Error("/////////////////////////////");
                log4net.LogManager.GetLogger(this.GetType()).Error("Message Sid: " + MessageSid);
                log4net.LogManager.GetLogger(this.GetType()).Error("Message Status: " + MessageStatus);
                log4net.LogManager.GetLogger(this.GetType()).Error("Error Code: " + ErrorCode);
                log4net.LogManager.GetLogger(this.GetType()).Error("///////////////Error Ends////////////");
            }

        }

        /// <summary>
        /// Method that allows you to get the CRM service.
        /// </summary>
        /// <returns>CRM service.</returns>
        public static OrganizationServiceProxy GetCRMConnection()
        {
            string crmInstance = ConfigurationManager.AppSettings["OrganizationServiceCRM"];
            string user = ConfigurationManager.AppSettings["userNameCRM"];
            string password = ConfigurationManager.AppSettings["passwordCRM"];
            OrganizationServiceProxy service;
            ClientCredentials credentials = new ClientCredentials();
            credentials.UserName.UserName = user;
            credentials.UserName.Password = password;
            service = new OrganizationServiceProxy(new Uri(crmInstance), null, credentials, null);
            service.Authenticate();
            return service;
        }
        /// <summary>
        /// Method that allows you to update a case with certain information.
        /// </summary>
        /// <param name="service">service that allows you to connect to the CRM instance.</param>
        public void UpdateSMSMessage(IOrganizationService service, string MessageSid, string currentMessageStatus, string ErrorCode)
        {
            QueryExpression configurationQuery = new QueryExpression("dm_smsmessage")
            {
                ColumnSet = new ColumnSet("dm_statusmessage")
            };
            configurationQuery.Criteria.AddCondition("dm_messageid", ConditionOperator.Equal, MessageSid);
            EntityCollection configurations = service.RetrieveMultiple(configurationQuery);
            if (configurations != null && configurations.Entities.Count > 0)
            {
                Entity smsMessage = null;
                smsMessage = configurations.Entities.First();
                string messagePre = "";
                if (smsMessage.Contains("dm_statusmessage"))
                {
                    messagePre = smsMessage["dm_statusmessage"].ToString();
                }
                smsMessage["dm_statusmessage"] = messagePre + "\nDate:" + DateTime.Now.ToString() + " \nMessage Status:" + currentMessageStatus + ". Message Code:" + ErrorCode;
                if (currentMessageStatus == MessageStatus.delivered.ToString().ToUpper() || currentMessageStatus == MessageStatus.sent.ToString().ToUpper())
                {
                    log4net.LogManager.GetLogger(this.GetType()).Error("This the Date");
                    smsMessage["dm_datesent"] = DateTime.Now;
                }
                service.Update(smsMessage);

                SetStateRequest state = new SetStateRequest();
                state.State = new OptionSetValue((int)State.open);
                if (currentMessageStatus == MessageStatus.delivered.ToString().ToUpper() || currentMessageStatus == MessageStatus.sent.ToString().ToUpper())
                {
                    state.Status = new OptionSetValue((int)MessageStatus.sent);
                }
                if (currentMessageStatus == MessageStatus.failed.ToString().ToUpper() || currentMessageStatus == MessageStatus.undelivered.ToString().ToUpper())
                {
                    state.Status = new OptionSetValue((int)MessageStatus.failed);
                }
                if (currentMessageStatus == MessageStatus.queued.ToString().ToUpper())
                {
                    state.Status = new OptionSetValue((int)MessageStatus.queued);
                }
                EntityReference smsMessageReference = new EntityReference("dm_smsmessage", smsMessage.Id);
                state.EntityMoniker = smsMessageReference;
                service.Execute(state);
            }
        }

        public enum MessageStatus
        {
            sent = 602300001,
            failed = 602300003,
            queued = 602300002,
            delivered = 1, // not in CRM, equivalent to Sent
            undelivered = 2 // not in CRM, equivalent to failed

        }
        public enum State
        {
            open = 0
        }
    }
}