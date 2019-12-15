using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using CrmToolkit;
using Pavliks.WAM.ManagementConsole.Helpers;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class ContactCRM : IContactRepository
    {
        public List<Contact> GetContactsByFilter(string filter)
        {
            List<Contact> contacts = new List<Contact>();

            DataManager DataManager = new DataManager();

            QueryExpression contactQuery = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet(new string[] { "fullname", "jobtitle", "parentcustomerid", "emailaddress1", "telephone1" })
            };

            if(filter == null)
            {
                filter = string.Empty;
            }

            contactQuery.TopCount = 100;

            FilterExpression filterExpression = contactQuery.Criteria.AddFilter(LogicalOperator.Or);

            filterExpression.AddCondition("fullname", ConditionOperator.Like, "%" + filter + "%");
            filterExpression.AddCondition("emailaddress1", ConditionOperator.Like, "%" + filter + "%");
            filterExpression.AddCondition("telephone1", ConditionOperator.Like, "%" + filter + "%");
            
            EntityCollection contactsCollection = DataManager.RetrieveMultiple(contactQuery);

            Contact contact;
            ContactMapper contactMapper = new ContactMapper(DataManager.ConnectionOnpremise());
            foreach (var contactItem in contactsCollection.Entities)
            {
                contact = contactMapper.EntityToDomain(contactItem);
                contacts.Add(contact);
            }
            return contacts;
        }
    }
}
