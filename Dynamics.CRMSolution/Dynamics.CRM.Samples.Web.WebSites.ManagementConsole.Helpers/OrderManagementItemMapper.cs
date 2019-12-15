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
    public class OrderManagementItemMapper
    {
        public OrderManagementItemMapper()
        {
        }

        public static OrderManagementItem EntityToDomain(Entity orderManagementItemEntity)
        {
            OrderManagementItem orderManagementItem = new OrderManagementItem();
            orderManagementItem.Id = orderManagementItemEntity.Id;

            string attribute = string.Empty;
            object valueAttribute = null;

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_quantity");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.Quantity = ((decimal)valueAttribute);
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_action");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.ActionItem = (ActionItem)((OptionSetValue)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "statuscode");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.StatusItem = (StatusItem)((OptionSetValue)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_registrationid");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.Registration = new Registration() { Id = ((EntityReference)valueAttribute).Id };

                attribute = Mapping.GetAttributeName(orderManagementItemEntity, "Registration.dm_addonsfee");

                if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
                {
                    orderManagementItem.Registration.AmountAddons = ((Money)((AliasedValue)valueAttribute).Value).Value;

                }
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_productid");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.Product = new Product() { Id = ((EntityReference)valueAttribute).Id };
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_reservationid");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.Reservation = new Reservation() { Id = ((EntityReference)valueAttribute).Id };
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_contactid");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.Contact = new Contact() { Id = ((EntityReference)valueAttribute).Id };
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_salesorder");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.SalesOrder = new SalesOrder() { Id = ((EntityReference)valueAttribute).Id };
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_extendedamount");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.Price = ((Money)valueAttribute).Value;
            }
            
             attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_salesorderitemid");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.SalesOrderItem = Guid.Parse(valueAttribute.ToString());
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_lineitemtype");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderManagementItem.LineItemType = (LineItemType)((OptionSetValue)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(orderManagementItemEntity, "dm_registrationdeactivated");

            if (orderManagementItemEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                if ((bool)valueAttribute == true)
                {
                    orderManagementItem.DeactivatedFlag = true;
                }
            }
            else
            {
                orderManagementItem.DeactivatedFlag = false;
            }

            return orderManagementItem;

        }

        public static Entity DomainToEntity(OrderManagementItem orderManagementItem)
        {
            Entity orderManagementEntity = new Entity("dm_ordermanagementitem");
            orderManagementEntity.Id = orderManagementItem.Id;

            orderManagementEntity["dm_quantity"] = orderManagementItem.Quantity;
            orderManagementEntity["dm_salesorderitemid"] = orderManagementItem.SalesOrderItem.ToString();
            orderManagementEntity["dm_extendedamount"] = new Money(orderManagementItem.Price);
            orderManagementEntity["dm_action"] = new OptionSetValue((int)orderManagementItem.ActionItem);
            orderManagementEntity["statuscode"] = new OptionSetValue((int)orderManagementItem.StatusItem);
            orderManagementEntity["dm_lineitemtype"] = new OptionSetValue((int)orderManagementItem.LineItemType);


            if (orderManagementItem.Registration != null && orderManagementItem.Registration.Id != Guid.Empty)
            {
                orderManagementEntity["dm_registrationid"] = new EntityReference("dm_registration", orderManagementItem.Registration.Id);
            }
            if (orderManagementItem.Reservation != null && orderManagementItem.Reservation.Id != Guid.Empty)
            {
                orderManagementEntity["dm_reservationid"] = new EntityReference("dm_reservation", orderManagementItem.Reservation.Id);
            }
          
            if (orderManagementItem.SalesOrder != null && orderManagementItem.SalesOrder.Id != Guid.Empty)
            {
                orderManagementEntity["dm_salesorder"] = new EntityReference("salesorder", orderManagementItem.SalesOrder.Id);
            }
            if (orderManagementItem.Product != null && orderManagementItem.Product.Id != Guid.Empty)
            {
                orderManagementEntity["dm_productid"] = new EntityReference("product", orderManagementItem.Product.Id);
            }
            if (orderManagementItem.Contact != null && orderManagementItem.Contact.Id != Guid.Empty)
            {
                orderManagementEntity["dm_contactid"] = new EntityReference("contact", orderManagementItem.Contact.Id);
            }
            return orderManagementEntity;
        }
    }
}
