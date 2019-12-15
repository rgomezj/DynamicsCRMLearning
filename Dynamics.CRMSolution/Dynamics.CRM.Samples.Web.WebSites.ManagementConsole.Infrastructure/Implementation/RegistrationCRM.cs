using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pavliks.WAM.ManagementConsole.Domain;
using System.Web.Configuration;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using CrmToolkit;
using Pavliks.WAM.ManagementConsole.Helpers;
using Microsoft.Xrm.Sdk.Client;
using System.ServiceModel.Description;
using Pavliks.WAM.ManagementConsole.Helpers;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class RegistrationCRM : IRegistrationRepository
    {
        DataManager DataManager = new DataManager();

        public RegistrationCRM()
        {
            //Constructor
        }

        public List<Registration> GetAllTheRegistration(string eventId)
        {
            RegistrationMapper registrationMapper = new RegistrationMapper(DataManager.ConnectionOnpremise());
            List<Registration> listRegistrations = new List<Registration>();
            //Creates an crm connection.
            
            //Now I will search the order  that is associated with the registration.
            QueryExpression regQuery = new QueryExpression("dm_registration")
            {
                ColumnSet = new ColumnSet(true)
            };
            //we get just the activated registration.
            ConditionExpression statusCondition = new ConditionExpression("statuscode", ConditionOperator.Equal, 1);
            ConditionExpression eventCondition = new ConditionExpression("dm_courseid", ConditionOperator.Equal, new Guid(eventId));
            ConditionExpression salesOrderCondition = new ConditionExpression("dm_salesorderid", ConditionOperator.NotNull, null);

            regQuery.Criteria.AddCondition(statusCondition);
            regQuery.Criteria.AddCondition(eventCondition);
            regQuery.Criteria.AddCondition(salesOrderCondition);
            regQuery.LinkEntities.Add(new LinkEntity("dm_registration", "salesorder", "dm_salesorderid", "salesorderid", JoinOperator.LeftOuter));
            regQuery.LinkEntities[0].EntityAlias = "SalesOrder";
            regQuery.LinkEntities[0].Columns.AddColumns("dm_outstandingamount", "ispricelocked", "pricelevelid", "totaltax", "totalamount", "dm_creditamount", "dm_paidamount", "ordernumber");
            regQuery.LinkEntities.Add(new LinkEntity("dm_registration", "contact", "dm_registrantid", "contactid", JoinOperator.LeftOuter));
            regQuery.LinkEntities[1].EntityAlias = "Contact";
            regQuery.LinkEntities[1].Columns.AddColumns("fullname", "jobtitle", "emailaddress1", "telephone1");

            EntityCollection registrationCollection = DataManager.RetrieveMultiple(regQuery);

            foreach (var registration in registrationCollection.Entities)
            {
                
                //Converts the registration to a registration domain.
                listRegistrations.Add(registrationMapper.EntityToDomain(registration));
            }

            return listRegistrations;
        }

        public Registration GetRegistrationById(Guid registrationId)
        {
            RegistrationMapper registrationMapper=new RegistrationMapper(DataManager.ConnectionOnpremise());

            //Now I will search the order  that is associated with the registration.
            QueryExpression regQuery = new QueryExpression("dm_registration")
            {
                ColumnSet = new ColumnSet(true)
            };

            ConditionExpression idCondition = new ConditionExpression("dm_registrationid", ConditionOperator.Equal, registrationId);
            regQuery.Criteria.AddCondition(idCondition);
            regQuery.LinkEntities.Add(new LinkEntity("dm_registration", "salesorder", "dm_salesorderid", "salesorderid", JoinOperator.LeftOuter));
            regQuery.LinkEntities[0].EntityAlias = "SalesOrder";
            regQuery.LinkEntities[0].Columns.AddColumns("dm_outstandingamount", "ispricelocked", "pricelevelid", "totaltax", "totalamount", "dm_creditamount", "dm_paidamount", "ordernumber");
            regQuery.LinkEntities.Add(new LinkEntity("dm_registration", "contact", "dm_registrantid", "contactid", JoinOperator.LeftOuter));
            regQuery.LinkEntities[1].EntityAlias = "Contact";
            regQuery.LinkEntities[1].Columns.AddColumns("fullname", "jobtitle", "emailaddress1", "telephone1");

            EntityCollection registration = DataManager.RetrieveMultiple(regQuery);


            return registrationMapper.EntityToDomain(registration.Entities[0]);
        }

        public List<Registration> GetRegistrationsByOrder(Guid orderId)
        {
            RegistrationMapper registrationMapper = new RegistrationMapper(DataManager.ConnectionOnpremise());
            List<Registration> listRegistrations = new List<Registration>();
            //Creates an crm connection.

            //Now I will search the order  that is associated with the registration.
            QueryExpression regQuery = new QueryExpression("dm_registration")
            {
                ColumnSet = new ColumnSet(true)

            };
            //we get just the activated registration.
            ConditionExpression statusCondition = new ConditionExpression("statuscode", ConditionOperator.Equal, 1);
            ConditionExpression salesOrderCondition = new ConditionExpression("dm_salesorderid", ConditionOperator.Equal, orderId);

            regQuery.Criteria.AddCondition(statusCondition);
            regQuery.Criteria.AddCondition(salesOrderCondition);
            regQuery.LinkEntities.Add(new LinkEntity("dm_registration", "salesorder", "dm_salesorderid", "salesorderid", JoinOperator.LeftOuter));
            regQuery.LinkEntities[0].EntityAlias = "SalesOrder";
            regQuery.LinkEntities[0].Columns.AddColumns("dm_outstandingamount", "ispricelocked", "pricelevelid", "totaltax", "totalamount", "dm_creditamount", "dm_paidamount", "ordernumber");
            regQuery.LinkEntities.Add(new LinkEntity("dm_registration", "contact", "dm_registrantid", "contactid", JoinOperator.LeftOuter));
            regQuery.LinkEntities[1].EntityAlias = "Contact";
            regQuery.LinkEntities[1].Columns.AddColumns("fullname", "jobtitle", "emailaddress1", "telephone1");

            EntityCollection registrationCollection = DataManager.RetrieveMultiple(regQuery);

            foreach (var registration in registrationCollection.Entities)
            {
                //Converts the registration to a registration domain.
                listRegistrations.Add(registrationMapper.EntityToDomain(registration));
            }

            return listRegistrations;
        }

        public void UpdateRefundedRegistration(Registration registration)
        {
            ColumnSet oItemColumns = new ColumnSet(new string[] { "dm_refunded" });
            Entity Item = DataManager.Retrieve(registration.Id, "dm_registration", oItemColumns);
            Item.Attributes["dm_refunded"] = registration.Refunded;
            DataManager.Update(Item);
        }
        public void UpdateDeactivatedRegistration(Registration registration)
        {
            ColumnSet oItemColumns = new ColumnSet(new string[] { "dm_registrationdeactivated" });
            Entity Item = DataManager.Retrieve(registration.Id, "dm_registration", oItemColumns);
            Item.Attributes["dm_registrationdeactivated"] = registration.Deactivated;
            DataManager.Update(Item);
        }
    }
}
