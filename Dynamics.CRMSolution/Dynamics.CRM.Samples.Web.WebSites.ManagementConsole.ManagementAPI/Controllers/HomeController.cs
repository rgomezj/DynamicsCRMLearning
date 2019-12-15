using Pavliks.WAM.ManagementConsole.BL;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class HomeController : Controller
    {
        private SalesOrderBL _salesOrderBL;

        public HomeController(ISalesOrderRepository _salesOrderRepository)
        {
            _salesOrderBL = new SalesOrderBL(_salesOrderRepository);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
