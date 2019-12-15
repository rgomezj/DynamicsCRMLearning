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
    public class SalesOrderItemController : ApiController
    {
        private SalesOrderItemBL _SalesOrderItemBL;

        public SalesOrderItemController(ISalesOrderItemRepository _salesOrderItemRepository, IRegistrationRepository _registrationRepository)
        {
            _SalesOrderItemBL = new SalesOrderItemBL(_salesOrderItemRepository, _registrationRepository);
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("GetSalesOrderItemBySalesOrder")]
        public IEnumerable<SalesOrderItemsListViewModel> GetSalesOrderItemBySalesOrder(string salesOrderId)
        {
            #region convertion
            List<SalesOrderItem> salesOrderItems = _SalesOrderItemBL.GetAllSalesOrderItemByOrderId(salesOrderId);

            //It converts the Registration domain record to Registration Index ViewModel
            AutoMapper.MapperConfiguration config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SalesOrderItem, SalesOrderItemsListViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var salesOrderViewModel = mapper.Map<IEnumerable<SalesOrderItem>, IEnumerable<SalesOrderItemsListViewModel>>(salesOrderItems);
            #endregion
            return salesOrderViewModel;
        }
    }
}