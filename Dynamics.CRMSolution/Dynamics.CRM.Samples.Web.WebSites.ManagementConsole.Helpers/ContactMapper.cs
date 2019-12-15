using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavliks.WAM.ManagementConsole.Helpers
{
    /// <summary>
    ///  Mapper class that converts a contact entity to a contact domain.
    /// </summary>
    public class ContactMapper
    {

           private IOrganizationService _organizationService;
           public ContactMapper(IOrganizationService service)
        {

            this._organizationService = service;
        }
        /// <summary>
        /// Mapper that converts a contact entity to a contact domain.
        /// </summary>
        /// <param name="contactEntity">Price list as entity.</param>
        /// <returns>Price list converts as price list domain.</returns>
        public Contact EntityToDomain(Entity contactEntity)
        {
            Contact contact = new Contact();
            contact.Id = contactEntity.Id;
            if (contactEntity.Contains("fullname"))
            {
                contact.FullName = (string)contactEntity["fullname"];
            }
            if (contactEntity.Contains("jobtitle"))
            {
                contact.JobTitle = (string)contactEntity["jobtitle"];
            }
            if (contactEntity.Contains("parentcustomerid"))
            {
                contact.CompanyName = getCompanyName((EntityReference)contactEntity["parentcustomerid"]);
            }
            if (contactEntity.Contains("emailaddress1"))
            {
                contact.Email = (string)contactEntity["emailaddress1"];
            }
            if (contactEntity.Contains("telephone1"))
            {
                contact.BusinessPhone = (string)contactEntity["telephone1"];
            } 
            return contact;
        }


        /// <summary>
        /// Method that gets  the name of the account associated to the contact.
        /// </summary>
        /// <param name="accountReference"></param>
        /// <returns> The price list with all its attributes. </returns>
        public string getCompanyName(EntityReference accountReference)
        {

            Entity account = _organizationService.Retrieve(accountReference.LogicalName, accountReference.Id, new ColumnSet(true));

            if (account.Contains("name"))
            {
                return (string)account["name"];

            }else
            {

                return null;
            }
          
        }
    }
}
