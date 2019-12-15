using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace Dynamics.CRM.asuarez.CustomActivity1
{
    public class CA_Event_DetectDuplicateEvents : CodeActivity
    {
        #region Attributes
        [RequiredArgument]
        [Input("Event to be validated if not exist another one with the same params."), ReferenceTarget("asuarez_event")]
        public InArgument<EntityReference> Event { get; set; }
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
            this.Trace("step 0");
            //Calls the run method that will validate for a duplicate contact.
            this.Run(service, this.Event.Get(executionContext));
        }
        /// <summary>
        /// Method that allows searching for duplicate contacts.
        /// </summary>
        /// <param name="service">Service that allow you to connect to CRM.</param>
        /// <param name="eventReference">Contact to be validated if not exist another one with the same email.</param>
        public void Run(IOrganizationService service, EntityReference eventReference)
        {
            try
            {

                //For trace the execution.
                this.Trace("step 1");
                //Gets the contact with its email address.
                Entity eventEnt = service.Retrieve(eventReference.LogicalName, eventReference.Id, new ColumnSet(new string[] { "asuarez_name", "asuarez_startdate", "asuarez_enddate", "asuarez_capacity" }));
                //Now I will search on all the contact if exist another contact with the same email.
                QueryExpression query = new QueryExpression("asuarez_event")
                {
                    ColumnSet = new ColumnSet(false)
                };
                //For trace the execution.
                this.Trace("step 2");
                //Declares conditions for the query.

                if (eventEnt.Contains("asuarez_name"))
                {
                    query.Criteria.AddCondition(new ConditionExpression("asuarez_name", ConditionOperator.Equal, eventEnt["asuarez_name"]));
                }
                else
                {
                    throw new Exception("asuarez_name");
                }

                if (eventEnt.Contains("asuarez_startdate"))
                {
                    query.Criteria.AddCondition(new ConditionExpression("asuarez_startdate", ConditionOperator.Equal, eventEnt["asuarez_startdate"]));
                }
                else
                {
                    throw new Exception("asuarez_startdate");
                }

                if (eventEnt.Contains("asuarez_enddate"))
                {
                    query.Criteria.AddCondition(new ConditionExpression("asuarez_enddate", ConditionOperator.Equal, eventEnt["asuarez_enddate"]));
                }
                else
                {
                    throw new Exception("asuarez_enddate");
                }

                if (eventEnt.Contains("asuarez_capacity"))
                {
                    query.Criteria.AddCondition(new ConditionExpression("asuarez_capacity", ConditionOperator.Equal, eventEnt["asuarez_capacity"]));
                }
                else
                {
                    throw new Exception("asuarez_capacity");
                }


                query.Criteria.AddCondition(new ConditionExpression("asuarez_eventid", ConditionOperator.NotEqual, eventReference.Id));


                //For trace the execution.
                this.Trace("step 3");
                //Searches for duplicate contacts.
                EntityCollection contacts = service.RetrieveMultiple(query);
                //If exist at least one contact, means that a duplicate contact exist.
                //For trace the execution.
                this.Trace("step 4");
                if (contacts.Entities.Count > 0)
                {
                    throw new InvalidPluginExecutionException(OperationStatus.Canceled, "Duplicate Error: this Event already exists.");
                }

            }
            catch (Exception e)
            {
                this.Trace(e.Message);
                throw e;
            }
        }
        #endregion
        #region Properties
        #endregion
    }
}
