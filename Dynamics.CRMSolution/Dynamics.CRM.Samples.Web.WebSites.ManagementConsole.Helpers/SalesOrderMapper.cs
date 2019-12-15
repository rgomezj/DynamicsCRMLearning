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

    /// <summary>
    ///  Mapper class that converts a sales order entity to a sales order domain.
    /// </summary>

    public class SalesOrderMapper
    {

        private IOrganizationService _organizationService;
        public SalesOrderMapper(IOrganizationService service)
        {

            this._organizationService = service;
        }
        /// <summary>
        /// Mapper that converts a sales order entity to a sales order domain.
        /// </summary>
        /// <param name="salesOrderEntity">Sales order as entity.</param>
        /// <returns>Sales order converts as sales order domain.</returns>
        public SalesOrder EntityToDomain(Entity salesOrderEntity)
        {
            SalesOrder salesOrder = new SalesOrder();
            salesOrder.Id = salesOrderEntity.Id;

            string attribute = string.Empty;
            object valueAttribute;

            if (salesOrderEntity.Contains("dm_outstandingamount"))
            {
                salesOrder.OutstandingBalance = ((Money)salesOrderEntity["dm_outstandingamount"]).Value;

            }
            if (salesOrderEntity.Contains("ispricelocked"))
            {
                salesOrder.IsPriceLocked = (bool)salesOrderEntity["ispricelocked"];

            }

            if (salesOrderEntity.Contains("pricelevelid"))
            {
                salesOrder.PriceList = getPriceList((EntityReference)salesOrderEntity["pricelevelid"]);

            }
            if (salesOrderEntity.Contains("transactioncurrencyid"))
            {
                salesOrder.Currency = getCurrency((EntityReference)salesOrderEntity["transactioncurrencyid"]);

            }
            if (salesOrderEntity.Contains("customerid"))
            {
                salesOrder.Customer = getContact((EntityReference)salesOrderEntity["customerid"]);

            }
            if (salesOrderEntity.Contains("totaltax"))
            {
                salesOrder.TotalTax = ((Money)salesOrderEntity["totaltax"]).Value;

            }
            if (salesOrderEntity.Contains("totalamount"))
            {
                salesOrder.TotalAmount = ((Money)salesOrderEntity["totalamount"]).Value;

            }
            if (salesOrderEntity.Contains("dm_creditamount"))
            {
                salesOrder.CreditAmount = ((Money)salesOrderEntity["dm_creditamount"]).Value;

            }
            if (salesOrderEntity.Contains("dm_paidamount"))
            {
                salesOrder.PaidAmount = ((Money)salesOrderEntity["dm_paidamount"]).Value;

            }

            attribute = Mapping.GetAttributeName(salesOrderEntity, "dm_waitlist");

            if (salesOrderEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                EntityReference waitlist = (EntityReference)valueAttribute;
                salesOrder.Waitlist = new Waitlist() {  Id = waitlist.Id };
            }

            attribute = Mapping.GetAttributeName(salesOrderEntity, "dm_typeorder");

            if (salesOrderEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrder.typeOfOrder = (TypeOrder)((OptionSetValue)valueAttribute).Value;
            }
            attribute = Mapping.GetAttributeName(salesOrderEntity, "dm_paymenttype");

            if (salesOrderEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrder.paymentType = (PaymentType)((OptionSetValue)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(salesOrderEntity, "ordernumber");

            if (salesOrderEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrder.Number = valueAttribute.ToString();
            }

            return salesOrder;
        }

        /// <summary>
        /// Mapper that converts a sales order entity to a sales order domain.
        /// </summary>
        /// <param name="salesOrderEntity">Sales order as entity.</param>
        /// <returns>Sales order converts as sales order domain.</returns>
        public Entity DomainToEntity(SalesOrder salesOrderDomain)
        {
            Entity salesOrder = new Entity("salesorder");
            salesOrder.Id = salesOrderDomain.Id;

           
            salesOrder["ispricelocked"] = salesOrderDomain.IsPriceLocked;
            if(salesOrderDomain.PriceList!=null)
            {
            salesOrder["pricelevelid"] = new EntityReference("pricelevel", salesOrderDomain.PriceList.Id);
            }
            if (salesOrderDomain.Currency != null)
            {
                salesOrder["transactioncurrencyid"] = new EntityReference("transactioncurrency", salesOrderDomain.Currency.Id);
            }
            if (salesOrderDomain.Customer!=null)
            {
                salesOrder["customerid"] = new EntityReference("contact", salesOrderDomain.Customer.Id);
            }

            if (salesOrderDomain.TotalTax.HasValue)
            {

                salesOrder["totaltax"] = new Money(salesOrderDomain.TotalTax.Value);
            }
            if (salesOrderDomain.TotalAmount.HasValue)
            {

                salesOrder["totalamount"] = new Money(salesOrderDomain.TotalAmount.Value);
            }

            if (salesOrderDomain.CreditAmount.HasValue)
            {

                salesOrder["dm_creditamount"] = new Money(salesOrderDomain.CreditAmount.Value);
            }
            if (salesOrderDomain.PaidAmount.HasValue)
            {

                salesOrder["dm_paidamount"] = new Money(salesOrderDomain.PaidAmount.Value);
            }
            if (salesOrderDomain.OutstandingBalance.HasValue)
            {

                salesOrder["dm_outstandingamount"] = new Money(salesOrderDomain.OutstandingBalance.Value);

            }

            if (!string.IsNullOrEmpty(salesOrderDomain.Name))
            {

                salesOrder["name"] = salesOrderDomain.Name;
            }

            if (salesOrderDomain.Waitlist != null)
            {

                salesOrder["dm_waitlist"] = new EntityReference("dm_waitlist", salesOrderDomain.Waitlist.Id);
            }
            if (salesOrderDomain.typeOfOrder != null)
            {

                salesOrder["dm_typeorder"] = new OptionSetValue((int)salesOrderDomain.typeOfOrder);
            }
            if (salesOrderDomain.paymentType.HasValue)
            {

                salesOrder["dm_paymenttype"] = new OptionSetValue((int)salesOrderDomain.paymentType);
            }
            if (salesOrderDomain.ChequeNumber != null)
            {

                salesOrder["dm_checknumber"] = salesOrderDomain.ChequeNumber;
            }

            return salesOrder;
        }

        /// <summary>
        /// Method that gets  the price list associated to the product.
        /// </summary>
        /// <param name="priceListReference"></param>
        /// <returns> The price list with all its attributes. </returns>
        public PriceList getPriceList(EntityReference priceListReference)
        {

            Entity priceList = _organizationService.Retrieve(priceListReference.LogicalName, priceListReference.Id, new ColumnSet(true));

            PriceListMapper priceListMapper = new PriceListMapper();

            return priceListMapper.EntityToDomain(priceList);
        }
        /// <summary>
        /// Method that gets  the price list associated to the product.
        /// </summary>
        /// <param name="priceListReference"></param>
        /// <returns> The price list with all its attributes. </returns>
        public Currency getCurrency(EntityReference currencyReference)
        {

            Entity currency = _organizationService.Retrieve(currencyReference.LogicalName, currencyReference.Id, new ColumnSet(true));

            CurrencyMapper currencyMapper = new CurrencyMapper();

            return currencyMapper.EntityToDomain(currency);
        }

        /// <summary>
        /// Method that gets the contact associated to the order.
        /// </summary>
        /// <param name="contactReference"></param>
        /// <returns> The price list with all its attributes. </returns>
        public Contact getContact(EntityReference contactReference)
        {

            Entity contact = _organizationService.Retrieve(contactReference.LogicalName, contactReference.Id, new ColumnSet(true));

            ContactMapper contactMapper = new ContactMapper(_organizationService);

            return contactMapper.EntityToDomain(contact);
        }


    }

}

