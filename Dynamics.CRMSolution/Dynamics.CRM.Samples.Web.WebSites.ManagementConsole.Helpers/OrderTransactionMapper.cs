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
    public class OrderTransactionMapper
    {
        private IOrganizationService _organizationService;

        public OrderTransactionMapper(IOrganizationService service)
        {
            this._organizationService = service;
        }

        public static OrderTransaction EntityToDomain(Entity orderTransactionEntity)
        {
            OrderTransaction orderTransaction = new OrderTransaction();
            orderTransaction.Id = orderTransactionEntity.Id;

            string attribute = string.Empty;
            object valueAttribute = null;


            attribute = Mapping.GetAttributeName(orderTransactionEntity, "dm_name");

            if (orderTransactionEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderTransaction.Name = valueAttribute.ToString();
            }

            attribute = Mapping.GetAttributeName(orderTransactionEntity, "dm_orderid");

            if (orderTransactionEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                EntityReference reference = (EntityReference)valueAttribute;
                orderTransaction.SalesOrder = new SalesOrder() { Id = reference.Id, Name = reference.Name };
            }

            attribute = Mapping.GetAttributeName(orderTransactionEntity, "dm_transactionid");

            if (orderTransactionEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderTransaction.TransactionId = valueAttribute.ToString();
            }
            attribute = Mapping.GetAttributeName(orderTransactionEntity, "dm_transactionamount");

            if (orderTransactionEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderTransaction.TransactionAmount = ((Money)valueAttribute).Value;
            }
            attribute = Mapping.GetAttributeName(orderTransactionEntity, "dm_ccnumber");

            if (orderTransactionEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderTransaction.CCNumber = valueAttribute.ToString();
            }
            attribute = Mapping.GetAttributeName(orderTransactionEntity, "dm_transactioncomments");

            if (orderTransactionEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderTransaction.TransactionComments = valueAttribute.ToString();
            }
            
            attribute = Mapping.GetAttributeName(orderTransactionEntity, "dm_transactiontype");

            if (orderTransactionEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderTransaction.TransactionType = (TransactionType)((OptionSetValue)valueAttribute).Value;
            }
            attribute = Mapping.GetAttributeName(orderTransactionEntity, "dm_paymenttype");

            if (orderTransactionEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderTransaction.PaymentType = (PaymentTypeOrderTransaction)((OptionSetValue)valueAttribute).Value;
            }

            attribute = Mapping.GetAttributeName(orderTransactionEntity, "createdon");

            if (orderTransactionEntity.Attributes.TryGetValue(attribute, out valueAttribute))
            {
                orderTransaction.Date = (DateTime)valueAttribute;
            }

            return orderTransaction;
        }


        public Entity DomainToEntity(OrderTransaction order)
        {
            Entity orderTransaction = new Entity("dm_ordertransaction");
            orderTransaction.Id = order.Id;

            if (order.CCNumber != null)
            {
                orderTransaction["dm_ccnumber"] = order.CCNumber;
            }
            if (order.TransactionType.HasValue)
            {
                orderTransaction["dm_transactiontype"] = new OptionSetValue((int)order.TransactionType.Value);
            }
            if (order.TransactionAmount != null)
            {
                orderTransaction["dm_transactionamount"] = new Money(order.TransactionAmount.Value);
            }
            if (order.TransactionId != null)
            {
                orderTransaction["dm_transactionid"] = order.TransactionId;
            }
            if (order.SalesOrder != null)
            {
                orderTransaction["dm_orderid"] =new EntityReference("salesorder",order.SalesOrder.Id) ;
            }
            if (order.Name != null)
            {
                orderTransaction["dm_name"] = order.Name;
            }
            if (order.TransactionComments != null)
            {
                orderTransaction["dm_transactioncomments"] = order.TransactionComments;
            }
            if (order.PaymentType.HasValue)
            {
                orderTransaction["dm_paymenttype"] = new OptionSetValue((int)order.PaymentType.Value);
            }
            return orderTransaction;
        }
    }
}
