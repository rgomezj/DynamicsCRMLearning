using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using EntityHelper;

namespace Dynamics.CRM.asuarez.CustomActivity1
{
    public class CA_Event_ChangeEventStatus : CodeActivity
    {
        [ReferenceTarget("asuarez_event")]
        [RequiredArgument]
        [Input("Event")]
        public InArgument<EntityReference> EventRef { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory factory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            tracingService = executionContext.GetExtension<ITracingService>();

            this.Run(service, EventRef.Get(executionContext));
        }

        public void Run(IOrganizationService service, EntityReference eventRef)
        {
            var entity = service.Retrieve(eventRef.LogicalName, eventRef.Id, new ColumnSet("asuarez_capacity", "asuarez_contactsregistered"));
            int capacity = 0, registered = 0;
            if (entity.Contains("asuarez_capacity"))
                capacity = Int32.Parse(entity["asuarez_capacity"].ToString());
            if (entity.Contains("asuarez_contactsregistered"))
                registered = Int32.Parse(entity["asuarez_contactsregistered"].ToString());
            if (registered >= capacity)//Validating if the registered value exceeds capacity is out of the scope of this excercise.
            {
                var helper = new EntityHelperClass();
                helper.ChangeEntityStatus(service, eventRef, 1, 115490000);
            }
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