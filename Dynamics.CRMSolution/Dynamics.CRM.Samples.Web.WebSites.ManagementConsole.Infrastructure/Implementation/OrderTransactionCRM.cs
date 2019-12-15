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

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class OrderTransactionCRM : IOrderTransactionRepository
    {
        DataManager DataManager = new DataManager();

        public OrderTransactionCRM()
        {

        }

        public OrderTransaction GetFirstTransaction(Guid salesOrderId)
        {
            OrderTransaction orderTransaction = null;
            QueryExpression transactionsQuery = new QueryExpression("dm_ordertransaction")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_name", "createdon", "dm_orderid", "dm_transactionid", "dm_transactionamount", "dm_ccnumber", "dm_transactioncomments", "dm_paymenttype" })
            };

            ConditionExpression transactionTypeCondition = new ConditionExpression("dm_transactiontype", ConditionOperator.Equal, (int)TransactionType.Sale);
            ConditionExpression salesOrderCondition = new ConditionExpression("dm_orderid", ConditionOperator.Equal, salesOrderId);

            transactionsQuery.Criteria.AddCondition(transactionTypeCondition);
            transactionsQuery.Criteria.AddCondition(salesOrderCondition);
            transactionsQuery.AddOrder("createdon", OrderType.Ascending);

            EntityCollection orderTransactions = DataManager.RetrieveMultiple(transactionsQuery);

            if (orderTransactions.Entities.Count > 0)
            {
                orderTransaction = OrderTransactionMapper.EntityToDomain(orderTransactions.Entities[0]);
            }

            return orderTransaction;
        }
        public Guid CreateOrderTransaction(OrderTransaction order)
        {
            //Creates an crm connection.
            DataManager DataManager = new DataManager();
            OrderTransactionMapper orderTransactionMapper = new OrderTransactionMapper(DataManager.ConnectionOnpremise());
            return DataManager.Create(orderTransactionMapper.DomainToEntity(order));
        }
        public OrderTransaction GetLastRefundTransaction(Guid salesOrderId)
        {
            OrderTransaction orderTransaction = null;
            QueryExpression transactionsQuery = new QueryExpression("dm_ordertransaction")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_name", "createdon", "dm_orderid", "dm_transactionid", "dm_transactionamount", "dm_ccnumber", "dm_transactioncomments", "dm_paymenttype" })
            };

            ConditionExpression transactionTypeCondition = new ConditionExpression("dm_transactiontype", ConditionOperator.Equal, (int)TransactionType.Refund);
            ConditionExpression salesOrderCondition = new ConditionExpression("dm_orderid", ConditionOperator.Equal, salesOrderId);

            transactionsQuery.Criteria.AddCondition(transactionTypeCondition);
            transactionsQuery.Criteria.AddCondition(salesOrderCondition);
            transactionsQuery.AddOrder("createdon", OrderType.Descending);

            EntityCollection orderTransactions = DataManager.RetrieveMultiple(transactionsQuery);

            if (orderTransactions.Entities.Count > 0)
            {
                orderTransaction = OrderTransactionMapper.EntityToDomain(orderTransactions.Entities[0]);
            }

            return orderTransaction;
        }
        public List<OrderTransaction> GetTransactionByOrder(Guid salesOrderId)
        {
            List<OrderTransaction> orderTransactionsDomain = new List<OrderTransaction>();
            OrderTransaction orderTransaction = null;
            QueryExpression transactionsQuery = new QueryExpression("dm_ordertransaction")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_name", "createdon", "dm_orderid", "dm_transactionid", "dm_transactionamount", "dm_ccnumber", "dm_transactioncomments", "dm_paymenttype", "dm_transactiontype" })
            };

            ConditionExpression salesOrderCondition = new ConditionExpression("dm_orderid", ConditionOperator.Equal, salesOrderId);

            transactionsQuery.Criteria.AddCondition(salesOrderCondition);
            transactionsQuery.AddOrder("createdon", OrderType.Descending);

            EntityCollection orderTransactions = DataManager.RetrieveMultiple(transactionsQuery);

            foreach (Entity e in orderTransactions.Entities)
            {
                orderTransaction = OrderTransactionMapper.EntityToDomain(e);
                orderTransactionsDomain.Add(orderTransaction);
            }

            return orderTransactionsDomain;
        }

    }
}
