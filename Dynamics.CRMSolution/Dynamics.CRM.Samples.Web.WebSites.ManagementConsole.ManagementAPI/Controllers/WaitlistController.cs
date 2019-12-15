using Pavliks.WAM.ManagementConsole.BL;
using Pavliks.WAM.ManagementConsole.Domain;
using Pavliks.WAM.ManagementConsole.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI.Controllers
{
    public class WaitlistController : ApiController
    {
        private WaitlistBL _WaitlistBL;

        public WaitlistController(IWaitlistRepository _WaitlistRepository, ISalesOrderRepository _SalesOrderRepository, IOrderCoursesRepository _orderCoursesRepository)
        {
            _WaitlistBL = new WaitlistBL(_WaitlistRepository, _SalesOrderRepository, _orderCoursesRepository);
        }

        [System.Web.Http.HttpGet]
        public List<Waitlist> Waitlist()
        {
            var waitlist = _WaitlistBL.GetAllTheWaistlist();
            return waitlist;
        }

        [System.Web.Http.HttpGet]
        public List<Waitlist> Waitlist(string eventId)
        {
            var waitlist = _WaitlistBL.GetWaistlists(Guid.Parse(eventId));
            return waitlist;
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage GenerateLink(Waitlist waitlistItem)
        {
            try
            {
                var waitlist = _WaitlistBL.GenerateLink(waitlistItem);
                return Request.CreateResponse(HttpStatusCode.OK, waitlist);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = e.Message });
            }
        }
        [System.Web.Http.HttpPut]
        public HttpResponseMessage SendLink(Waitlist waitlistItem)
        {
            try
            {
                var waitlist = _WaitlistBL.SendLink(waitlistItem);
                return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Email Sent" });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message = e.Message });
            }
        }
    }
}
