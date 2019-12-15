using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pavliks.WAM.ManagementConsole.Domain;
using CrmToolkit;
using Pavliks.WAM.ManagementConsole.Helpers;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class SalesOrderItemCRM : ISalesOrderItemRepository
    {
        DataManager DataManager = new DataManager();

        public List<SalesOrderItem> GetAllSalesOrderItemByOrderId(string orderId)
        {
            //Creates an crm connection.
            SalesOrderItemMapper salesOrderItemMapper = new SalesOrderItemMapper();
            List<SalesOrderItem> salesOrderItemList = new List<SalesOrderItem>();

            //Now I will search the order  that is associated with the registration.
            QueryExpression query = new QueryExpression("salesorderdetail");

            query.ColumnSet.AddColumns("baseamount", "tax", "lineitemnumber", "productdescription", "quantity", "productid", "dm_courseid", "dm_contactid", "dm_registrationid", "salesorderid", "dm_reservationid", "dm_lineitemtype");

            //we get just the activated registration.
            ConditionExpression salesOrderCondition = new ConditionExpression("salesorderid", ConditionOperator.Equal, new Guid(orderId));
            query.Criteria.AddCondition(salesOrderCondition);
            query.LinkEntities.Add(new LinkEntity("salesorderdetail", "dm_registration", "dm_registrationid", "dm_registrationid", JoinOperator.LeftOuter));
            query.LinkEntities[0].EntityAlias = "Registration";
            query.LinkEntities[0].Columns.AddColumns("dm_registrantid", "dm_registrationstatus");

            EntityCollection salesOrderItemCollection = DataManager.RetrieveMultiple(query);

            foreach (var salesOrderItem in salesOrderItemCollection.Entities)
            {
                //Converts the registration to a registration domain.
                salesOrderItemList.Add(salesOrderItemMapper.EntityToDomain(salesOrderItem));
            }


            return salesOrderItemList;
        }
        public void UpdateSalesOrderItem(SalesOrderItem orderManagementItem)
        {
            //Creates an crm connection.
            SalesOrderItemMapper salesOrderItemMapper = new SalesOrderItemMapper();
            List<SalesOrderItem> salesOrderItemList = new List<SalesOrderItem>();

            ColumnSet oItemColumns = new ColumnSet(new string[] { "quantity" });
            Entity Item = DataManager.Retrieve(orderManagementItem.Id, "salesorderdetail", oItemColumns);
            Item.Attributes["quantity"] = orderManagementItem.Quantity;
            DataManager.Update(Item);
        }

        public void CreateSalesOrderItem(SalesOrderItem item)
        {
            Entity salesOrderItem = new Entity("salesorderdetail");
            if (item.Contact != null && Guid.Empty != item.Contact.Id)
            {
                salesOrderItem["dm_contactid"] = new EntityReference("contact", item.Contact.Id);
            }
            if (item.Event != null && Guid.Empty != item.Event.Id)
            {
                salesOrderItem["dm_courseid"] = new EntityReference("dm_course", item.Event.Id);
            }
            salesOrderItem["dm_lineitemtype"] = new OptionSetValue((int)item.LineItemTypeInsalesOrder);
            if (item.Registration != null && Guid.Empty != item.Registration.Id)
            {
                salesOrderItem["dm_registrationid"] = new EntityReference("dm_registration", item.Registration.Id);
            }
            salesOrderItem["priceperunit"] = new Money(item.Amount);
            salesOrderItem["quantity"] = item.Quantity;
            salesOrderItem["productdescription"] = item.ProductDescription;
            salesOrderItem["isproductoverridden"] = true;
            salesOrderItem["description"] = item.Description;
            if (item.SalesOrder != null && Guid.Empty != item.SalesOrder.Id)
            {
                salesOrderItem["salesorderid"] = new EntityReference("salesorder", item.SalesOrder.Id);
            }
            DataManager.Create(salesOrderItem);
        }

    }
}
