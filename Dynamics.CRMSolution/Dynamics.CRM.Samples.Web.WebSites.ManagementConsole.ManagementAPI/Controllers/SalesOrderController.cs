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
    public class SalesOrderController : ApiController
    {
        private SalesOrderBL _SalesOrderBL;

        public SalesOrderController(ISalesOrderRepository _salesOrderRepository)
        {
            _SalesOrderBL = new SalesOrderBL(_salesOrderRepository);
        }


        [System.Web.Http.HttpGet]
        public SalesOrderViewModel GetSalesOrder(string id)
        {

            #region convertion
            //It converts the Registration domain record to Registration Index ViewModel
            AutoMapper.MapperConfiguration config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SalesOrder, SalesOrderViewModel>();
            });
            IMapper mapper = config.CreateMapper();
                var salesOrderViewModel = mapper.Map<SalesOrder, SalesOrderViewModel>(_SalesOrderBL.GetSalesOrder(id));
            #endregion
            return salesOrderViewModel;
        }

        [System.Web.Http.HttpPut]
        public HttpResponseMessage ChangeStatus(SalesOrderViewModel salesOrder)
        {
            SalesOrder salesOrderChange = new SalesOrder() { Id = salesOrder.Id, StatusOrder = salesOrder.StatusOrder, StateOrder = salesOrder.StateOrder };
            _SalesOrderBL.ChangeStatus(salesOrderChange);
            return Request.CreateResponse(HttpStatusCode.OK, new { SalesOrderId = salesOrder.Id });
        }

    }
}