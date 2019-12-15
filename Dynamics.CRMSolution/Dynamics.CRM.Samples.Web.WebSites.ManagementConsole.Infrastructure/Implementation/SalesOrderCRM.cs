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
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace Pavliks.WAM.ManagementConsole.Infrastructure.Implementation
{
    public class SalesOrderCRM : ISalesOrderRepository
    {
        public SalesOrder GetSalesOrderById(string id)
        { //Creates an crm connection.
            DataManager DataManager = new DataManager();
            SalesOrderMapper salesOrderMapper = new SalesOrderMapper(DataManager.ConnectionOnpremise());
            return salesOrderMapper.EntityToDomain(DataManager.Retrieve(new Guid(id), "salesorder", new ColumnSet(true)));
        }

        public Guid CreateOrder(SalesOrder order)
        {
            //Creates an crm connection.
            DataManager DataManager = new DataManager();
            SalesOrderMapper salesOrderMapper = new SalesOrderMapper(DataManager.ConnectionOnpremise());
            return DataManager.Create(salesOrderMapper.DomainToEntity(order));
        }

        public void OpenOrder(Guid salesOrderId)
        {
            DataManager DataManager = new DataManager();

            SetStateRequest stateRequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference("salesorder", salesOrderId),
                State = new OptionSetValue((int)StateOrder.Active),
                Status = new OptionSetValue((int)StatusOrder.ManageConsole)
            };
            DataManager.Execute(stateRequest);
        }

        public void CloseOrder(Guid salesOrderId)
        {
            try {
                DataManager DataManager = new DataManager();
                
                Entity orderEntity = new Entity("salesorder");
                orderEntity.Id = salesOrderId;
                orderEntity["dm_managementconsolefullfiled"] = true;
                DataManager.Update(orderEntity);

                FulfillSalesOrderRequest stateRequest = new FulfillSalesOrderRequest();
                stateRequest.OrderClose = new Entity("orderclose");
                stateRequest.OrderClose["salesorderid"] = new EntityReference("salesorder", salesOrderId);
                stateRequest.Status = new OptionSetValue((int)StatusOrder.Complete);
               
                DataManager.Execute(stateRequest);
            }catch(Exception e)
            {
                //this catch is because whe try update the order and it is fulfilled the system throw exception
            }
        }

        public void CancelOrder(Guid salesOrderId)
        {
            try
            {
                DataManager DataManager = new DataManager();
                CancelSalesOrderRequest stateRequest = new CancelSalesOrderRequest();
                stateRequest.OrderClose = new Entity("orderclose");
                stateRequest.OrderClose["salesorderid"] = new EntityReference("salesorder", salesOrderId);
                stateRequest.Status = new OptionSetValue((int)StatusOrder.NoMoney);

                DataManager.Execute(stateRequest);
            }
            catch (Exception e)
            {
                //this catch is because whe try update the order and it is fulfilled the system throw exception
            }
        }
        public void isScholarpshipOrder(Guid salesOrderId, bool isScholarpship)
        {
            try
            {
                DataManager DataManager = new DataManager();

                Entity orderEntity = new Entity("salesorder");
                orderEntity.Id = salesOrderId;
                orderEntity["dm_isscholarship"] = isScholarpship;

                DataManager.Update(orderEntity);
            }
            catch (Exception e)
            {
                //this catch is because whe try update the order and it is fulfilled the system throw exception
            }
        }
    }
}
