using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Newtonsoft.Json.Linq;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace CA.SMSMessage
{
    /// <summary>
    /// Custom activity that allows sending a sms message.
    /// </summary>
    public class SMSMessageSend : CodeActivity
    {
        public enum StatusReason
        {
            Unqueued = 602300004,
            Queued = 602300002
        }

        public enum State
        {
            Open = 0,
        }

        [RequiredArgument]
        [Input("SMS Auth Token.")]
        public InArgument<String> SMSAuthToken { get; set; }

        [RequiredArgument]
        [Input("Portal URL.")]
        public InArgument<String> portalURL { get; set; }

        [RequiredArgument]
        [Input("SMS Account SID.")]
        public InArgument<String> SMSAccountSID { get; set; }

        [ReferenceTarget("dm_smsmessage")]
        [RequiredArgument]
        [Input("SMS Message.")]
        public InArgument<EntityReference> SMSMessage { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory factory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            tracingService = executionContext.GetExtension<ITracingService>();
            Trace("Calls the run method.");
            this.Run(service, SMSMessage.Get(executionContext), SMSAuthToken.Get(executionContext), portalURL.Get(executionContext), SMSAccountSID.Get(executionContext));
        }

        public void Run(IOrganizationService service, EntityReference smsMessageReference, String smsAuthToken, String portalURL, String smsAccountSID)
        {
            Trace("Gets the sms message with its attributes.");
            Entity smsMessage = service.Retrieve(smsMessageReference.LogicalName, smsMessageReference.Id, new ColumnSet(new string[] { "dm_fromnumber", "dm_messagebody", "dm_tonumber" }));
            Trace("Creates the Twilio object that will allow sending the SMS message.");
            var twilio = new TwilioRestClient(smsAccountSID, smsAuthToken);
            Trace("Validates that all the parameters needed for the sending process come.");
            if (smsMessage.Contains("dm_fromnumber") && smsMessage.Contains("dm_tonumber") && smsMessage.Contains("dm_messagebody")&&!string.IsNullOrEmpty(portalURL))
            {
                Trace("Sends the sms messsage." + portalURL);
                var message = twilio.SendMessage((string)smsMessage["dm_fromnumber"], (string)smsMessage["dm_tonumber"], (string)smsMessage["dm_messagebody"], portalURL + "/callback-sms");


                if (string.IsNullOrEmpty(message.Sid))
                {
                    Trace("There was an error during the sending process.");
                    SetStateRequest state = new SetStateRequest();
                    state.State = new OptionSetValue((int)State.Open);
                    Trace("Sets the status as Failed.");
                    state.Status = new OptionSetValue((int)StatusReason.Unqueued);
                    state.EntityMoniker = smsMessageReference;
                    SetStateResponse stateSet = (SetStateResponse)service.Execute(state);
                    Entity smsMessageToUpdate = new Entity(smsMessage.LogicalName);
                    smsMessageToUpdate.Id = smsMessage.Id;
                    string errorMessage = GetMessage(message.RestException);
                    smsMessageToUpdate["dm_statusmessage"] = errorMessage;
                    service.Update(smsMessageToUpdate);
                }
                else
                {

                    Trace("Updates the message id in the SMS Message record.");

                    Entity smsMessageToUpdate = new Entity(smsMessage.LogicalName);
                    smsMessageToUpdate.Id = smsMessage.Id;
                    smsMessageToUpdate["dm_messageid"] = message.Sid;
                    smsMessageToUpdate["dm_statusmessage"] = "Message Sent to the SMS Platform with message ID:" + smsMessage.Id.ToString();
                    service.Update(smsMessageToUpdate);

                    Trace("The sending process was successful.");
                    SetStateRequest state = new SetStateRequest();
                    state.State = new OptionSetValue((int)State.Open);
                    Trace("Sets the status as Sent.");
                    state.Status = new OptionSetValue((int)StatusReason.Queued);
                    state.EntityMoniker = smsMessageReference;
                    SetStateResponse stateSet = (SetStateResponse)service.Execute(state);
                }
            }


        }

        private string GetMessage(RestException restException)
        {
            StringBuilder builder = new StringBuilder();
            if(restException != null)
            {
                builder.AppendLine("Code:" + restException.Code);
                builder.AppendLine("Message:" + restException.Message);
                builder.AppendLine("MoreInfo:" + restException.MoreInfo);
                builder.AppendLine("Status:" + restException.Status);
            }
            return builder.ToString();
        }



        #region Tracing
        private ITracingService tracingService;
        private void Trace(string message)
        {
            if (tracingService != null)
            {
                tracingService.Trace(message);
            }
        }
        #endregion Tracing
    }
}