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
    ///  Mapper class that converts a sales order item entity to a sales order item domain.
    /// </summary>

    public class SalesOrderItemMapper
    {
        /// <summary>
        /// Mapper that converts a sales order item entity to a sales order item domain.
        /// </summary>
        /// <param name="salesOrderItemEntity">Sales order item as entity.</param>
        /// <returns>Sales order item converted as sales order domain.</returns>
        public SalesOrderItem EntityToDomain(Entity salesOrderItemEntity)
        {
            SalesOrderItem salesOrderItem = new SalesOrderItem() { Contact = new Contact(), Event = new Event() };
            salesOrderItem.Id = salesOrderItemEntity.Id;
            string attribute = Mapping.GetAttributeName(salesOrderItemEntity, "baseamount");
            object valueAttribute;

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrderItem.Amount = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "tax");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrderItem.Tax = ((Money)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "lineitemnumber");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrderItem.LineItemNumber = ((int)valueAttribute);
            }


            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "productdescription");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrderItem.Description = ((string)valueAttribute);
            }

            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "quantity");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrderItem.Quantity = ((decimal)valueAttribute);
            }

            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "dm_courseid");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                EntityReference eventSalesOrderItem = (EntityReference)valueAttribute;
                salesOrderItem.Event = new Event() { Id = eventSalesOrderItem.Id, Name = eventSalesOrderItem.Name };
            }
            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "dm_reservationid");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                EntityReference reservationSalesOrderItem = (EntityReference)valueAttribute;
                salesOrderItem.Reservation = new Reservation() { Id = reservationSalesOrderItem.Id, Name = reservationSalesOrderItem.Name };
            }
           

            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "dm_contactid");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                EntityReference contactSalesOrderItem = (EntityReference)valueAttribute;
                salesOrderItem.Contact = new Contact() { Id = contactSalesOrderItem.Id, FullName = contactSalesOrderItem.Name };
            }

            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "salesorderid");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                EntityReference salesOrder = (EntityReference)valueAttribute;
                salesOrderItem.SalesOrder = new SalesOrder() { Id = salesOrder.Id };
            }

            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "dm_registrationid");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrderItem.Registration = new Registration() { Id = ((EntityReference)valueAttribute).Id };

                attribute = Mapping.GetAttributeName(salesOrderItemEntity, "Registration.dm_registrationstatus");

                if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    OptionSetValue optionSet = ((AliasedValue)valueAttribute).Value as OptionSetValue;
                    salesOrderItem.Registration.RegistrationStatus = (dm_registrationstatus)optionSet.Value;
                }

                attribute = Mapping.GetAttributeName(salesOrderItemEntity, "Registration.dm_registrantid");

                if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    salesOrderItem.Registration.Name = ((EntityReference)((AliasedValue)valueAttribute).Value).Name;
                }
            }

            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "productid");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrderItem.Product = new Product() { Id = ((EntityReference)valueAttribute).Id };

                attribute = Mapping.GetAttributeName(salesOrderItemEntity, "Products.name");

                if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    salesOrderItem.Product.Name = ((AliasedValue)valueAttribute).Value.ToString();
                }

                attribute = Mapping.GetAttributeName(salesOrderItemEntity, "Products.price");

                if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    salesOrderItem.Product.UnitPrice = (decimal)(((Microsoft.Xrm.Sdk.Money)((((AliasedValue)valueAttribute).Value))).Value);
                    
                }

                attribute = Mapping.GetAttributeName(salesOrderItemEntity, "Products.description");

                if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    salesOrderItem.Product.Description = (string)((AliasedValue)valueAttribute).Value;
                }

                attribute = Mapping.GetAttributeName(salesOrderItemEntity, "Products.pricelevelid");

                if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    salesOrderItem.Product.PriceList = new PriceList() { Id = ((EntityReference)((AliasedValue)valueAttribute).Value).Id };

                    attribute = Mapping.GetAttributeName(salesOrderItemEntity, "PriceList.name");

                    if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                    {
                        salesOrderItem.Product.PriceList.Name = (string)((AliasedValue)valueAttribute).Value;
                    }
                }
            }

            attribute = Mapping.GetAttributeName(salesOrderItemEntity, "dm_lineitemtype");

            if (salesOrderItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                salesOrderItem.LineItemType = (LineItemType)((OptionSetValue)valueAttribute).Value;
            }

            return salesOrderItem;
        }

        /// <summary>
        /// Mapper that converts a sales order item entity to    a sales order item domain.
        /// </summary>
        /// <param name="salesOrderEntity">Sales order as entity.</param>
        /// <returns>Sales order converts as sales order domain.</returns>
        public Entity DomainToEntity(SalesOrderItem salesOrderItemDomain)
        {
            Entity salesOrderItem = new Entity("salesorderdetail");
            salesOrderItem.Id = salesOrderItemDomain.Id;

            salesOrderItem["baseamount"] = new Money(salesOrderItemDomain.Amount);
            salesOrderItem["tax"] = new Money(salesOrderItemDomain.Tax);
            salesOrderItem["lineitemnumber"] = salesOrderItemDomain.LineItemNumber;
            salesOrderItem["description"] = salesOrderItemDomain.Description;
            salesOrderItem["quantity"] = salesOrderItemDomain.Quantity;
            if (salesOrderItemDomain.Product != null)
            {
                salesOrderItem["productid"] = new EntityReference("product", salesOrderItemDomain.Product.Id);
            }

            return salesOrderItem;
        }
    }

}

