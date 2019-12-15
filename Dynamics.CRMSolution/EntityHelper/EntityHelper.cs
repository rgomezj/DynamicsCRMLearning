using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityHelper
{
    public class EntityHelperClass
    {
        public void ChangeEntityStatus(IOrganizationService service, EntityReference entityRef, int statecode, int statuscode)
        {
            // Create the Request Object
            SetStateRequest state = new SetStateRequest();

            // Set the Request Object's Properties
            state.State = new OptionSetValue(statecode);
            state.Status =
                new OptionSetValue(statuscode);

            // Point the Request to the case whose state is being changed
            state.EntityMoniker = entityRef;

            // Execute the Request
            SetStateResponse stateSet = (SetStateResponse)service.Execute(state);
                        
        }
    }
}
