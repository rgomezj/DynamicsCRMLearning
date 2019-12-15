using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{

    public class WaitListMapper
    {

        public static Waitlist EntityToDomain(Entity waitListEntity)
        {
            Waitlist waitlist = new Waitlist();
            waitlist.Id = waitListEntity.Id;

            string attribute = string.Empty;
            object valueAttribute = null;

            attribute = Mapping.GetAttributeName(waitListEntity, "dm_contactid");
            EntityReference reference;

            if (waitListEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                reference = valueAttribute as EntityReference;
                waitlist.Contact = new Contact() { Id = reference.Id, FullName = reference.Name };
            }

            attribute = Mapping.GetAttributeName(waitListEntity, "dm_courseid");

            if (waitListEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                reference = valueAttribute as EntityReference;
                waitlist.EventCRM = new Event() { Id = reference.Id };
            }

            attribute = Mapping.GetAttributeName(waitListEntity, "dm_orderid");

            if (waitListEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                reference = valueAttribute as EntityReference;
                waitlist.SalesOrder = new SalesOrder() { Id = reference.Id };
            }

            attribute = Mapping.GetAttributeName(waitListEntity, "createdon");

            if (waitListEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                waitlist.CreatedOn = (DateTime)valueAttribute;
            }

            attribute = Mapping.GetAttributeName(waitListEntity, "dm_registrationlink");

            if (waitListEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                waitlist.RegistrationLink = valueAttribute.ToString();
            }


            return waitlist;

        }

        public static Entity DomainToEntity(Waitlist waitlistDomain)
        {
            Entity waitlist = new Entity("dm_waitlist");
            waitlist.Id = waitlistDomain.Id;

            if (waitlistDomain.Contact.Id != Guid.Empty)
            {
                waitlist["dm_contactid"] = new EntityReference("contact", waitlistDomain.Contact.Id);
            }

            if (waitlistDomain.EventCRM.Id != Guid.Empty)
            {
                waitlist["dm_courseid"] = new EntityReference("dm_courseid", waitlistDomain.EventCRM.Id);
            }

            if(waitlistDomain.SalesOrder.Id != Guid.Empty)
            {
                waitlist["dm_orderid"] = new EntityReference("salesorder", waitlistDomain.SalesOrder.Id);
            }

            waitlist["dm_eventname"] = waitlistDomain.EventName;

            return waitlist;
        }
    }

}

