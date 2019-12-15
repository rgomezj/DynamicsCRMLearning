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
using AuthorizeNet;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Controllers;
using System.Configuration;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class OrderTransactionController : ApiController
    {
        private OrderTransactionBL _OrderTransactionBL;
        private PaymentBL _PaymentBL;
        private SalesOrderBL _SalesOrderBL;
        private ConfigurationBL _ConfigurationBL;

        public OrderTransactionController(IOrderTransactionRepository _IOrderTransactionRepository, IConfigurationRepository _ConfigurationRepository, ISalesOrderRepository _ISalesOrderRepository, IConfigurationRepository _IConfigurationRepository)
        {
            _OrderTransactionBL = new OrderTransactionBL(_IOrderTransactionRepository);
            _PaymentBL = new PaymentBL(_ConfigurationRepository, _IOrderTransactionRepository);
            _SalesOrderBL = new SalesOrderBL(_ISalesOrderRepository);
            _ConfigurationBL = new ConfigurationBL(_IConfigurationRepository);

        }

        [System.Web.Http.HttpGet()]
        [System.Web.Http.ActionName("GetFirsTransaction")]
        public HttpResponseMessage GetFirsTransaction(Guid salesOrderId)
        {
            OrderTransaction orderTransaction = _OrderTransactionBL.GetFirstTransaction(salesOrderId);
            return Request.CreateResponse(HttpStatusCode.OK, orderTransaction);
        }
        [System.Web.Http.HttpPost()]
        public HttpResponseMessage RefundOrderTransaction(SalesOrderViewModel salesOrder)
        {
            SalesOrder salesOrderComplete = _SalesOrderBL.GetSalesOrder(salesOrder.Id.ToString());
            //OrderTransaction orderTransaction = _OrderTransactionBL.GetFirstTransaction(salesOrderId);
            Domain.Configuration configuration = _ConfigurationBL.GetConfiguration();

            SalesOrder salesOrderRefund = new SalesOrder() { Id = salesOrder.Id, PaidAmount = salesOrder.PaidAmount };

            if (salesOrderComplete.paymentType == PaymentType.CreditCard)
            {
                PaymentResponse response = null;
                if (configuration.ManagementConsoleTestMode)
                {
                    response = RandomResult();
                }
                else
                {
                    response = _PaymentBL.Refund(salesOrderRefund,false);
                }
                if (response.OK)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, response);

                }
            }
            else
            {
                PaymentResponse response = new PaymentResponse();
                //if (salesOrderComplete.paymentType == PaymentType.Check)
                //{

                response.OK = false;
                if (string.IsNullOrEmpty(salesOrder.ChequeNumber) && !salesOrder.WillSpecifyLater)
                {
                    response.Message = "Please provide a cheque number";
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
                }
                else
                {
                    response.OK = true;
                    response.isCheque = true;
                    response.ChequeNumber = salesOrder.ChequeNumber;
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                //}
                //else
                //{
                //    response.Message = "No payment type was assigned to the original order";
                //    return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
                //}
            }
        }

        [System.Web.Http.HttpGet()]
        [System.Web.Http.ActionName("GetTransactionByOrder")]
        public HttpResponseMessage GetTransactionByOrder(Guid salesOrderId)
        {
            List<OrderTransaction> orderTransactions = _OrderTransactionBL.GetTransactionByOrder(salesOrderId);
            return Request.CreateResponse(HttpStatusCode.OK, orderTransactions);
        }

        private PaymentResponse RandomResult()
        {
            PaymentResponse paymentResponse = new PaymentResponse();

            Random gen = new Random();
            if (gen.Next(2) == 0)
            {
                paymentResponse.OK = true;
                paymentResponse.Message = "Test Mode: Successful transaction";
            }
            else
            {
                paymentResponse.OK = false;
                paymentResponse.Message = "Test Mode: an error ocurred";
            }
            return paymentResponse;
        }
    }
}