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
    ///  Mapper class that converts a registration entity to a registration domain.
    /// </summary>
    public class RegistrationMapper
    {

        private IOrganizationService _organizationService;
        public RegistrationMapper(IOrganizationService service)
        {

            this._organizationService = service;
        }


        /// <summary>
        /// Mapper that converts a registration entity to a registration domain.
        /// </summary>
        /// <param name="registrationEntity">Registration as entity.</param>
        /// <returns>Registration converts as sales order domain.</returns>
        public Registration EntityToDomain(Entity registrationEntity)
        {
            Registration registration = new Registration();
            registration.Id = registrationEntity.Id;

            string attribute = string.Empty;
            object valueAttribute = null;

            if (registrationEntity.Contains("dm_name"))
            {
                registration.Name = (string)registrationEntity["dm_name"];

            }

            if (registrationEntity.Contains("createdon"))
            {
                registration.CreatedOn = (DateTime)registrationEntity["createdon"];

            }
            if (registrationEntity.Contains("dm_salesorderid"))
            {
                registration.SalesOrder = new SalesOrder() { Id = ((EntityReference)registrationEntity["dm_salesorderid"]).Id };

                attribute = Mapping.GetAttributeName(registrationEntity, "SalesOrder.dm_outstandingamount");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.SalesOrder.OutstandingBalance = ((Money)((AliasedValue)valueAttribute).Value).Value;
                }

                attribute = Mapping.GetAttributeName(registrationEntity, "SalesOrder.ispricelocked");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.SalesOrder.IsPriceLocked = (bool)((AliasedValue)valueAttribute).Value;
                }

                attribute = Mapping.GetAttributeName(registrationEntity, "SalesOrder.totaltax");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.SalesOrder.TotalTax = ((Money)((AliasedValue)valueAttribute).Value).Value;
                }

                attribute = Mapping.GetAttributeName(registrationEntity, "SalesOrder.totalamount");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.SalesOrder.TotalAmount = ((Money)((AliasedValue)valueAttribute).Value).Value;
                }
                attribute = Mapping.GetAttributeName(registrationEntity, "SalesOrder.dm_creditamount");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.SalesOrder.CreditAmount = ((Money)((AliasedValue)valueAttribute).Value).Value;
                }

                attribute = Mapping.GetAttributeName(registrationEntity, "SalesOrder.dm_paidamount");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.SalesOrder.PaidAmount = ((Money)((AliasedValue)valueAttribute).Value).Value;
                }

                attribute = Mapping.GetAttributeName(registrationEntity, "SalesOrder.ordernumber");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.SalesOrder.Number = ((AliasedValue)valueAttribute).Value.ToString();
                }
            }


            if (registrationEntity.Contains("dm_courseid"))
            {
                registration.Event = new Event() { Id = ((EntityReference)registrationEntity["dm_courseid"]).Id ,Name = ((EntityReference)registrationEntity["dm_courseid"]).Name };
            }


            if (registrationEntity.Contains("dm_registrantid"))
            {

                registration.Contact = new Contact() { Id = ((EntityReference)registrationEntity["dm_registrantid"]).Id };

                attribute = Mapping.GetAttributeName(registrationEntity, "Contact.fullname");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.Contact.FullName = (string)((AliasedValue)valueAttribute).Value;
                }

                attribute = Mapping.GetAttributeName(registrationEntity, "Contact.jobtitle");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.Contact.JobTitle = (string)((AliasedValue)valueAttribute).Value;
                }

                attribute = Mapping.GetAttributeName(registrationEntity, "Contact.emailaddress1");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.Contact.Email = (string)((AliasedValue)valueAttribute).Value;
                }

                attribute = Mapping.GetAttributeName(registrationEntity, "Contact.telephone1");

                if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    registration.Contact.BusinessPhone = (string)((AliasedValue)valueAttribute).Value;
                }
            }

            if (registrationEntity.Contains("dm_constituentid"))
            {
                EntityReference constituentReference = (EntityReference)registrationEntity["dm_constituentid"];
                registration.Constituent = new Contact() { Id = constituentReference.Id, FullName = constituentReference.Name };
            }

            if (registrationEntity.Contains("dm_amountpaid"))
            {
                registration.AmountPaid = ((Money)registrationEntity["dm_amountpaid"]).Value;

            }
            if (registrationEntity.Contains("dm_registrationstatus"))
            {
                registration.RegistrationStatus = (dm_registrationstatus)((OptionSetValue)registrationEntity["dm_registrationstatus"]).Value;

            }

            attribute = Mapping.GetAttributeName(registrationEntity, "statuscode");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.StatusReason = (StatusReason)((OptionSetValue)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_coursefee");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.CourseFee = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_extamvalue");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.AmCare = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_extpmvalue");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.PmCare = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_suplunvalue");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.SupervisedLunch = ((Money)valueAttribute).Value;
            }

            //
            attribute = Mapping.GetAttributeName(registrationEntity, "dm_amopaicou");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.CourseFeeGross = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_amopaiaddons");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.AddonsFeeGross = ((Money)valueAttribute).Value;
            }

            

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_coursecreditappliedtocoursefee");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.CourseCreditAppliedToCourseFee = ((Money)valueAttribute).Value;
            }


            attribute = Mapping.GetAttributeName(registrationEntity, "dm_coursecreditappliedamcare");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.CourseCreditAmCare = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_coursecreditappliedpmcare");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.CourseCreditPmCare = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_coursecreditappliedsupervisedlunch");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.CourseCreditSupervisedLunch = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_coursecreditappliedtoforcredit");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.CourseCreditAppliedToForCredit = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_extpmgrossfee");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.PMCareFeeGross = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_extamgrossfee");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.AMCareFeeGross = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_supervisedluchgrossfee");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.SupervisedLunchGross = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_forcreditfeegross");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.ForCreditFeeGross = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_amtpaidforcredit");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.ForCreditFee = ((Money)valueAttribute).Value;
            }
            attribute = Mapping.GetAttributeName(registrationEntity, "dm_scholarshipfund");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                registration.ContainsScholarpship = true;
            }
            else
            {
                registration.ContainsScholarpship = false;
            }

            attribute = Mapping.GetAttributeName(registrationEntity, "dm_refunded");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                if((bool)valueAttribute == true)
                {
                    registration.Refunded = true;
                }
            }
            else
            {
                registration.Refunded = false;
            }
            attribute = Mapping.GetAttributeName(registrationEntity, "dm_registrationdeactivated");

            if (registrationEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                if ((bool)valueAttribute == true)
                {
                    registration.Deactivated = true;
                }
            }
            else
            {
                registration.Deactivated = false;
            }



            return registration;

        }


        /// <summary>
        /// Mapper that converts a registration domain to a registration entity.
        /// </summary>
        /// <param name="registration">Registration as domain.</param>
        /// <returns>Registration converts as entity.</returns>
        public Entity DomainToEntity(Registration registration)
        {
            Entity registrationEntity = new Entity("dm_registration");
            registrationEntity.Id = registration.Id;

            registrationEntity["dm_name"] = registration.Name;
            registrationEntity["createdon"] = registration.CreatedOn;
            registrationEntity["dm_amountpaid"] = new Money(registration.AmountPaid);

            if (registration.SalesOrder != null)
            {
                registrationEntity["dm_salesorderid"] = new EntityReference("salesorder", registration.SalesOrder.Id);
            }

            if (registration.RegistrationStatus.HasValue)
            {
                registrationEntity["dm_registrationstatus"] = new OptionSetValue((int)registration.RegistrationStatus);
            }

            if (registration.Event != null)
            {
                registrationEntity["dm_courseid"] = new EntityReference("dm_course", registration.Event.Id);
            }
            if (registration.Contact!=null)
            {
                registrationEntity["dm_registrantid"] = new EntityReference("contact", registration.Contact.Id);
            }
           
            if (registration.StatusReason != StatusReason.None)
            {
                registrationEntity["statuscode"] = new OptionSetValue((int)registration.StatusReason);
            }

            return registrationEntity;

        }

        /// <summary>
        /// Method that gets the the sales order associated to the registration.
        /// </summary>
        /// <param name="salesOrderReference"></param>
        /// <returns> The sales order with all its attributes. </returns>
        public SalesOrder getSalesOrder(EntityReference salesOrderReference)
        {

            Entity salesOrder = _organizationService.Retrieve(salesOrderReference.LogicalName, salesOrderReference.Id, new ColumnSet(true));

            SalesOrderMapper salesOrderMapper = new SalesOrderMapper(_organizationService);

            return salesOrderMapper.EntityToDomain(salesOrder);
        }

        /// <summary>
        /// Method that gets the the sales order associated to the registration.
        /// </summary>
        /// <param name="contactReference"></param>
        /// <returns> The sales order with all its attributes. </returns>
        public Contact getContact(EntityReference contactReference)
        {

            Entity contact = _organizationService.Retrieve(contactReference.LogicalName, contactReference.Id, new ColumnSet(true));

            ContactMapper contactMapper = new ContactMapper(_organizationService);

            return contactMapper.EntityToDomain(contact);
        }
    }
}
