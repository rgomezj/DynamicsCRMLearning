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
using Microsoft.Crm.Sdk.Messages;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class OrderManagementItemCRM : IOrderManagementItemRepository
    {
        DataManager DataManager = new DataManager();

        public void DeleteOrderManagementItem(Guid orderManagementGuid)
        {
            DataManager.Delete("dm_ordermanagementitem", orderManagementGuid);
        }

        public List<OrderManagementItem> GetOrderManagementItemBySalesOrder(Guid salesOrderGuid, StatusItem status)
        {
            List<OrderManagementItem> listOrderManagementItems = new List<OrderManagementItem>();

            QueryExpression orderManagementQuery = new QueryExpression("dm_ordermanagementitem")
            {
                ColumnSet = new ColumnSet(new string[] { "dm_registrationid", "dm_quantity", "dm_action", "dm_extendedamount", "dm_reservationid", "dm_salesorder", "dm_salesorderitemid", "statuscode","dm_productid","dm_contactid", "dm_lineitemtype", "dm_registrationdeactivated" })
            };

            ConditionExpression salesOrderCondition = new ConditionExpression("dm_salesorder", ConditionOperator.Equal, salesOrderGuid);
            ConditionExpression statusCondition = new ConditionExpression("statuscode", ConditionOperator.Equal, (int)status);
            orderManagementQuery.Criteria.AddCondition(salesOrderCondition);
            orderManagementQuery.Criteria.AddCondition(statusCondition);

            orderManagementQuery.LinkEntities.Add(new LinkEntity("dm_ordermanagementitem", "salesorder", "dm_salesorder", "salesorderid", JoinOperator.Inner));
            orderManagementQuery.LinkEntities[0].EntityAlias = "SalesOrder";
            orderManagementQuery.LinkEntities[0].Columns.AddColumns("dm_outstandingamount", "ispricelocked", "pricelevelid", "totaltax", "totalamount", "dm_creditamount", "dm_paidamount");

            orderManagementQuery.LinkEntities.Add(new LinkEntity("dm_ordermanagementitem", "dm_registration", "dm_registrationid", "dm_registrationid", JoinOperator.LeftOuter));
            orderManagementQuery.LinkEntities[1].EntityAlias = "Registration";
            orderManagementQuery.LinkEntities[1].Columns.AddColumns("dm_name", "createdon", "dm_registrationstatus", "dm_amountpaid", "dm_salesorderid", "dm_registrantid", "dm_addonsfee");

            orderManagementQuery.LinkEntities.Add(new LinkEntity("dm_ordermanagementitem", "dm_reservation", "dm_reservationid", "dm_reservationid", JoinOperator.LeftOuter));
            orderManagementQuery.LinkEntities[2].EntityAlias = "Reservation";
            orderManagementQuery.LinkEntities[2].Columns.AddColumns("dm_name");

            EntityCollection orderManagementItemsCollection = DataManager.RetrieveMultiple(orderManagementQuery);

            foreach (var orderManagementItem in orderManagementItemsCollection.Entities)
            {
                listOrderManagementItems.Add(OrderManagementItemMapper.EntityToDomain(orderManagementItem));
            }

            return listOrderManagementItems;
        }

        public Guid CreateOrderManagementItem(OrderManagementItem orderManagementItem)
        {
            Entity orderManagementItemEntity = OrderManagementItemMapper.DomainToEntity(orderManagementItem);
            return DataManager.Create(orderManagementItemEntity);
        }
      
        public void ChangeStatusOrderManagementItem(Guid orderManagementGuid, StatusItem status, StateItem state)
        {
            SetStateRequest stateRequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference("dm_ordermanagementitem", orderManagementGuid),
                State = new OptionSetValue((int)state),
                Status = new OptionSetValue((int)status)
            };
            DataManager.Execute(stateRequest);
            
        }

        public void UpdateOrderManagemertItem(OrderManagementItem orderManagementItem)
        {
            ColumnSet oItemColumns = new ColumnSet(new string[] { "dm_extendedamount" });
            Entity Item = DataManager.Retrieve(orderManagementItem.Id, "dm_ordermanagementitem", oItemColumns);
            Item.Attributes["dm_extendedamount"] = new Money(orderManagementItem.Price);
            DataManager.Update(Item);
        }

        public void UpdateOrderManagemertItemDeactivatedFlag(OrderManagementItem orderManagementItem)
        {
            ColumnSet oItemColumns = new ColumnSet(new string[] { "dm_registrationdeactivated" });
            Entity Item = DataManager.Retrieve(orderManagementItem.Id, "dm_ordermanagementitem", oItemColumns);
            Item.Attributes["dm_registrationdeactivated"] = orderManagementItem.DeactivatedFlag;
            DataManager.Update(Item);
        }
    }
}
