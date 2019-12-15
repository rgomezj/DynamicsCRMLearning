using Pavliks.WAM.ManagementConsole.BL;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class PagesController : Controller
    {
        public PagesController ()
        {
            

        }

        public ActionResult Waitlist()
        {
            return View();
        }

        public ActionResult Events()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistrationOptions()
        {
            return View();
        }

        public ActionResult SwapRegistration()
        {
            return View();
        }

        public ActionResult SalesOrderItemsList()
        {
            return View();
        }

        public ActionResult SuccessfulTransaction()
        {
            return View();
        }

        public ActionResult CancelledTransaction()
        {
            return View();
        }

        public ActionResult ModifySessions()
        {
            return View();
        }

        public ActionResult TransferRegistration()
        {
            return View();
        }
    }
}
