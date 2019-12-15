using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.EnforceUniqueContact
{
    /// <summary>
    /// Custom activity that allows searching for duplicate contacts.
    /// </summary>
    public class EnforceUniqueContact : CodeActivity
    {
        #region Attributes
        [RequiredArgument]
        [Input("Contact to be validated if not exist another one with the same email."), ReferenceTarget("contact")]
        public InArgument<EntityReference> Contact { get; set; }
        private ITracingService m_tracingService;
        #endregion
        #region Methods
        /// <summary>
        /// Method that allow recording application trace.
        /// </summary>
        /// <param name="message"></param>
        private void Trace(string message)
        {
            if (this.m_tracingService != null)
            {
                this.m_tracingService.Trace(message);
            }
        }
        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext extension = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationService service = executionContext.GetExtension<IOrganizationServiceFactory>().CreateOrganizationService(new Guid?(extension.UserId));
            this.m_tracingService = executionContext.GetExtension<ITracingService>();
            //Calls the run method that will validate for a duplicate contact.
            this.Run(service, this.Contact.Get(executionContext));
        }
        /// <summary>
        /// Method that allows searching for duplicate contacts.
        /// </summary>
        /// <param name="service">Service that allow you to connect to CRM.</param>
        /// <param name="contactReference">Contact to be validated if not exist another one with the same email.</param>
        public void Run(IOrganizationService service, EntityReference contactReference)
        {

            //For trace the execution.
            this.Trace("1");
            //Gets the contact with its email address.
            Entity contact = service.Retrieve(contactReference.LogicalName, contactReference.Id, new ColumnSet(new string[] { "emailaddress1" }));
            //Now I will search on all the contact if exist another contact with the same email.
            QueryExpression query = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet(false)
            };
            //For trace the execution.
            this.Trace("2");
            //Declares conditions for the query.
            ConditionExpression emailCondition = new ConditionExpression("emailaddress1", ConditionOperator.Equal, contact["emailaddress1"]);
            ConditionExpression idCondition = new ConditionExpression("contactid", ConditionOperator.NotEqual, contact["contactid"]);
            ConditionExpression stateCondition = new ConditionExpression("statecode", ConditionOperator.Equal, 0);
            ConditionExpression statusCondition = new ConditionExpression("statuscode", ConditionOperator.Equal, 1);
            query.Criteria.AddCondition(emailCondition);
            query.Criteria.AddCondition(idCondition);
            query.Criteria.AddCondition(stateCondition);
            query.Criteria.AddCondition(statusCondition);
            //For trace the execution.
            this.Trace("3");
            //Searches for duplicate contacts.
            EntityCollection contacts = service.RetrieveMultiple(query);
            //If exist at least one contact, means that a duplicate contact exist.
            //For trace the execution.
            this.Trace("4");
            if (contacts.Entities.Count > 0)
            {
                throw new InvalidPluginExecutionException(OperationStatus.Canceled, String.Format("Duplicate Warning: this email address {0} already belongs to an existing contact.", contact["emailaddress1"].ToString()));
            }

        }
        #endregion
        #region Properties
        #endregion
    }
}
