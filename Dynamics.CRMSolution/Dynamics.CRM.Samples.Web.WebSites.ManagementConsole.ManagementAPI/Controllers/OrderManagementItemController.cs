using AutoMapper;
using Pavliks.WAM.ManagementConsole.BL;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Implementation;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using Pavliks.WAM.ManagementConsole.ManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class OrderManagementItemController : ApiController
    {
        private OrderManagementItemBL _OrderManagementItemBL;

        public OrderManagementItemController(IOrderManagementItemRepository IOrderManagementItemRepository, ISalesOrderRepository IsalesOrderRepository, ISalesOrderItemRepository IsalesOrderItemRepository,IRegistrationRepository IregistrationRepository, IOrderTransactionRepository _OrderTransactionRepository)
        {
            _OrderManagementItemBL = new OrderManagementItemBL(IOrderManagementItemRepository, IsalesOrderRepository, IsalesOrderItemRepository, IregistrationRepository,  _OrderTransactionRepository);
        }

        [System.Web.Http.HttpDelete()]
        public HttpResponseMessage DeleteCurrentProcess(string salesOrderId)
        {
            try
            {
                _OrderManagementItemBL.RemoveOrderManagementItemsByOrderId(Guid.Parse(salesOrderId));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Error when removing the existing Process." + ex.Message });
            }
        }

        [System.Web.Http.HttpDelete()]
        public HttpResponseMessage RemoveOrderManagementItemById(string id)
        {
            try
            {
                _OrderManagementItemBL.RemoveOrderManagementItemsById(Guid.Parse(id));
                return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Record Processed" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Error when removing the item." + ex.Message });
            }
        }

        [System.Web.Http.HttpPut()]
        [System.Web.Http.ActionName("ConfirmedOrderManagementItemByOrderId")]
        public HttpResponseMessage ConfirmedOrderManagementItemByOrderId(SalesOrderViewModel salesOrderId)
        {
            try
            {
                _OrderManagementItemBL.ConfirmedOrderManagementItemsByOrderId(salesOrderId.Id,salesOrderId.IsCheque,salesOrderId.ChequeNumber);
                return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Record Processed" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Error when change status the item." + ex.Message });
            }
        }

        [System.Web.Http.HttpPost()]
        public HttpResponseMessage CreateOrderManagementItem(SalesOrderItemsListViewModel item)
        {
            try
            {

                Registration registration = new Registration() { Id = Guid.Parse(item.RegistrationId) };
                SalesOrder salesOrder = new SalesOrder() { Id = Guid.Parse(item.SalesOrderId) };
                registration.SalesOrder = salesOrder;
                Guid id = _OrderManagementItemBL.CreateOrderManagementItem(registration, salesOrder, item.Id, item.Quantity, item.Amount, item.ActionItem, StatusItem.InProgress, item.LineItemType);
                return Request.CreateResponse(HttpStatusCode.OK, new { IdItem = id, Message = "Product processed" });


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Error when removing the item." + ex.Message });
            }
        }
        [System.Web.Http.HttpPut()]
        [System.Web.Http.ActionName("UpdateOrderManagementItem")]
        public HttpResponseMessage UpdateOrderManagementItem(SalesOrderItemsListViewModel item)
        {
            try
            {
                Guid orderManagementItem = Guid.Parse(item.OrderManagementItemId);
                decimal newAmount = item.AmountRefunded;
                _OrderManagementItemBL.UpdateOrderManagement(orderManagementItem,newAmount);
                return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Product processed" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Error when removing the item." + ex.Message });
            }
        }

        [System.Web.Http.HttpPut()]
        [System.Web.Http.ActionName("UpdateDeactivatedFlag")]
        public HttpResponseMessage UpdateDeactivatedFlag(SalesOrderItemsListViewModel item)
        {
            try
            {
                Guid orderManagementItem = Guid.Parse(item.OrderManagementItemId);
                _OrderManagementItemBL.UpdateOrderManagementDeactivatedFlag(orderManagementItem, item.Deactivated);
                return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Product processed" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { message = "Error when removing the item." + ex.Message });
            }
        }
    }
}