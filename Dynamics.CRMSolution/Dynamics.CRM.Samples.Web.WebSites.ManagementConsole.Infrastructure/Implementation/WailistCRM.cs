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
    public class WailistCRM : IWaitlistRepository
    {


        public List<Waitlist> GetWaistlist(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public Guid SaveWaistlists(Waitlist waitList)
        {
            DataManager DataManager = new DataManager();
            Entity waitlistSave = WaitListMapper.DomainToEntity(waitList);
            if (waitList.Id == Guid.Empty)
            {
                return DataManager.Create(waitlistSave);
            }
            else
            {
                DataManager.Update(waitlistSave);
                return waitList.Id;
            }
        }

        public string GenerateLink(Waitlist waitlist, bool updateSalesOrder)
        {
            DataManager service = new DataManager();
            if (updateSalesOrder)
            {
                Entity waitlisToChange = new Entity("dm_waitlist");
                waitlisToChange.Id = waitlist.Id;
                waitlisToChange["dm_orderid"] = new EntityReference("salesorder", waitlist.SalesOrder.Id);
                service.Update(waitlisToChange);
            }
            else
            {
                Entity waitlisToChange = new Entity("dm_waitlist");
                waitlisToChange.Id = waitlist.Id;
                waitlisToChange["dm_triggergeneratelink"] = new Random().Next(100);
                service.Update(waitlisToChange);
            }

            Entity generatedlink = service.Retrieve(waitlist.Id, "dm_waitlist", new ColumnSet("dm_registrationlink"));
            if (generatedlink.Contains("dm_registrationlink"))
            {
                return generatedlink["dm_registrationlink"].ToString();
            }
            else
            {
                return null;
            }

        }

        List<Waitlist> IWaitlistRepository.GetAllTheWaistlist()
        {
            List<Waitlist> waitlists = new List<Waitlist>();
            //Creates an crm connection.
            DataManager DataManager = new DataManager();
            //Now I will search the order  that is associated with the registration.
            QueryExpression waitlistQuery = new QueryExpression("dm_waitlist")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_courseid", "dm_contactid", "createdon", "dm_orderid", "dm_registrationlink" })
            };
            EntityCollection waitlistCollection = DataManager.RetrieveMultiple(waitlistQuery);

            EntityReference reference;
            foreach (var waitlistItem in waitlistCollection.Entities)
            {
                Waitlist waitlist = new Waitlist() { Id = waitlistItem.Id };

                if (waitlistItem.Contains("dm_contactid"))
                {
                    reference = waitlistItem["dm_contactid"] as EntityReference;

                    Entity contact = DataManager.Retrieve(reference.Id, reference.LogicalName, new ColumnSet("fullname", "preferredcontactmethodcode"));
                    waitlist.Contact = new Contact() { Id = contact.Id, FullName = contact["fullname"].ToString() };
                    if (contact.Contains("preferredcontactmethodcode"))
                    {
                        waitlist.Contact.PreferedMethod = contact.FormattedValues["preferredcontactmethodcode"].ToString();
                    }

                }
                if (waitlistItem.Contains("dm_courseid"))
                {
                    reference = waitlistItem["dm_courseid"] as EntityReference;
                    waitlist.EventCRM = new Event() { Id = reference.Id };

                }
                if (waitlistItem.Contains("createdon"))
                {
                    waitlist.CreatedOn = (DateTime)waitlistItem["createdon"];
                }
                waitlists.Add(waitlist);

            }

            return waitlists;

        }

        List<Waitlist> IWaitlistRepository.GetWaistlists(Guid eventId)
        {
            List<Waitlist> waitlists = new List<Waitlist>();

            //Creates an crm connection.
            DataManager DataManager = new DataManager();

            //Now I will search the order  that is associated with the registration.
            QueryExpression waitlistQuery = new QueryExpression("dm_waitlist")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_courseid", "dm_contactid", "createdon", "dm_orderid", "dm_registrationlink" })
            };

            waitlistQuery.Criteria.AddCondition("dm_courseid", ConditionOperator.Equal, eventId);
            waitlistQuery.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (int)StatusReason.Active);

            //waitlistQuery.LinkEntities.Add(new LinkEntity("dm_waitlist", "salesorder", "dm_waitlistid", "dm_waitlist", JoinOperator.LeftOuter));
            //waitlistQuery.LinkEntities[0].LinkCriteria.AddCondition("statuscode", ConditionOperator.NotEqual, (int)StatusOrder.NoMoney);
            //waitlistQuery.LinkEntities[0].EntityAlias = "SalesOrder";
            //waitlistQuery.LinkEntities[0].Columns.AddColumns("name", "statuscode");

            // Complete with a condition to verify that the sales order has not been paid (when those custom statuses for the order are defined)
            // waitlistQuery.Criteria.AddCondition("dm_eventid", ConditionOperator.Equal, eventId);

            EntityCollection waitlistCollection = DataManager.RetrieveMultiple(waitlistQuery);
            foreach (var waitlistItem in waitlistCollection.Entities)
            {
                // If it is in the order, is because a sales order to convert to registration has been already created

                Waitlist waitlistMapping = WaitListMapper.EntityToDomain(waitlistItem);
                if (waitlistItem.Contains("dm_contactid"))
                {
                    EntityReference reference = waitlistItem["dm_contactid"] as EntityReference;

                    Entity contact = DataManager.Retrieve(reference.Id, reference.LogicalName, new ColumnSet("fullname", "preferredcontactmethodcode"));
                    waitlistMapping.Contact = new Contact() { Id = contact.Id, FullName = contact["fullname"].ToString() };
                    if (contact.Contains("preferredcontactmethodcode"))
                    {
                        waitlistMapping.Contact.PreferedMethod = contact.FormattedValues["preferredcontactmethodcode"].ToString();
                    }

                }
                waitlists.Add(waitlistMapping);
            }

            return waitlists;

        }

        public void SendEmail(Waitlist waitlist, int trigger)
        {
            DataManager service = new DataManager();
            Entity waitlisToChange = new Entity("dm_waitlist");
            waitlisToChange.Id = waitlist.Id;
            waitlisToChange["dm_triggersendemail"] = trigger;
            service.Update(waitlisToChange);
        }
    }
}
